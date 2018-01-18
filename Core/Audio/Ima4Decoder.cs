using System;
using System.IO;

namespace Jammit.Audio
{
  public class Ima4Decoder
  {
    // File variables
    private readonly Stream _stream;
    private readonly long _bitstreamOffset;
    private readonly uint _sampleFrames;
    private readonly long _numPackets;

    // Decoder state

    /// <summary>
    /// The index of the current packet (whose samples are in _sampleData).
    /// </summary>
    private long _currentPacket;

    /// <summary>
    /// The offset into the current packet (in sample-frames).
    /// </summary>
    private int _frameOffset;

    private readonly short[] _sampleData = new short[128];

    /// <summary>
    /// The index of the first sample that would be returned on a call to GetSamples.
    /// </summary>
    public long CurrentSample { get; private set; }

    /// <summary>
    /// Number of sample-frames in the stream.
    /// </summary>
    public uint Samples => _sampleFrames;

    /// <summary>
    /// The maximum sample value in the last decoded buffer.
    /// </summary>
    public short MaximumValue { get; private set; }

    public Ima4Decoder(Stream inputStream)
    {
      if (!inputStream.CanSeek)
        throw new Exception("Must be able to seek.");
      inputStream.Seek(0, SeekOrigin.Begin);
      if (inputStream.ReadASCIINullTerminated(4) != "FORM")
        throw new Exception("Unrecognized filetype.");

      long aifSize = inputStream.ReadUInt32BE();

      if (aifSize > inputStream.Length)
        throw new Exception("File is cut off");
      if (inputStream.ReadASCIINullTerminated(4) != "AIFC")
        throw new Exception("Input file is not AIFC");
      _stream = inputStream;

      while (_stream.Position < aifSize + 8)
      {
        var chunkType = _stream.ReadASCIINullTerminated(4);
        var chunkLength = _stream.ReadUInt32BE();
        switch (chunkType)
        {
          case "FVER":
            if (_stream.ReadUInt32BE() != 0xA2805140)
              throw new Exception("Unsupported AIFC FVER");
            break;
          case "COMM":
            var pos = _stream.Position;
            if (_stream.ReadUInt16BE() != 2)
              throw new Exception("Only 2-channel files are supported");
            _sampleFrames = _stream.ReadUInt32BE() * 64;
            var bitsPerSample = _stream.ReadUInt16BE();
            if (_stream.ReadExtended() != 44100.0)
              throw new Exception("Only 44.1kHz is supported");
            if (_stream.ReadASCIINullTerminated(4) != "ima4")
              throw new Exception("Not IMA4");
            _stream.Position = pos + chunkLength;
            break;
          case "FLLR":
            goto default;
          case "SSND":
            _bitstreamOffset = _stream.Position + 8;
            _numPackets = (chunkLength - 8)/34;
            goto default;
          default:
            _stream.Seek(chunkLength, SeekOrigin.Current);
            break;
        }
      }

      _stream.Position = _bitstreamOffset;
      Seek(0);
    }

    public void Seek(long sample)
    {
      var packet = sample/64;
      if (packet >= 0 && packet < _numPackets)
      {
        _currentPacket = packet;
        _sampleData[126] = 0;
        _sampleData[127] = 0;
        ReadTwoPackets(_currentPacket);
        _frameOffset = (int)(sample % 64);
        CurrentSample = sample;
      }
    }

    private static short ClampStepIdx(short index) => (short)(index < 0 ? 0 : (index > 88 ? 88 : index));

    private static void DecodePacket(int predictor, short index, byte[] bitstream, int chan, short[] samples)
    {
      for (int k = 0; k < 64; k++)
      {
        int step = ImaStepSize[index];

        int bytecode = k%2 == 0 ? bitstream[k/2] & 0xF : (bitstream[k/2] >> 4) & 0xF;

        index += ImaIndxAdjust[bytecode];
        index = ClampStepIdx(index);

        int diff = step >> 3;
        if ((bytecode & 1) != 0) diff += step >> 2;
        if ((bytecode & 2) != 0) diff += step >> 1;
        if ((bytecode & 4) != 0) diff += step;
        if ((bytecode & 8) != 0) diff = -diff;

        predictor += diff;
        if (predictor < -32768)
          predictor = -32768;
        else if (predictor > 32767)
          predictor = 32767;

        samples[2 * k + chan] = (short)predictor;
      }
    }
    
    /// <summary>
    /// Read 2 packets; one stereo frame with sample-interleave.
    /// </summary>
    /// <returns></returns>
    private void ReadTwoPackets(long packetFrame)
    {
      _stream.Position = _bitstreamOffset + 68*packetFrame;

      int leftPredictor, rightPredictor;
      byte leftIndex, rightIndex;
      var left = new byte[32];
      var right = new byte[32];

      leftPredictor = _stream.ReadInt16BE();
      leftIndex = (byte)(leftPredictor & 0x7f);
      leftPredictor &= (short)(leftPredictor & -0x80);
      leftPredictor |= _sampleData[126] & 0x7f;
      _stream.Read(left, 0, 32);

      rightPredictor = _stream.ReadInt16BE();
      rightIndex = (byte)(rightPredictor & 0x7f);
      rightPredictor = (short)(rightPredictor & -128);
      rightPredictor |= _sampleData[127] & 0x7f;
      _stream.Read(right, 0, 32);

      DecodePacket(leftPredictor, leftIndex, left, 0, _sampleData);
      DecodePacket(rightPredictor, rightIndex, right, 1, _sampleData);
    }

    /// <summary>
    /// Returns the number of sample-frames read from the stream.
    /// </summary>
    /// <param name="samples">Buffer of sample-interleaved stuff</param>
    /// <param name="count">Number of sample-frames to read.</param>
    /// <returns>Number of sample-frames read.</returns>
    public int GetSamples(short[] samples, int count)
    {
      // Read samples left in decoded packet.
      // Decode new packets until we reach count or run out of packets.
      // Update current sample, return number of samples read.
      int framesRead = 0;
      MaximumValue = 0;
      while (_frameOffset < 64 && framesRead < count)
      {
        samples[framesRead*2] = _sampleData[_frameOffset*2];
        samples[framesRead*2 + 1] = _sampleData[_frameOffset*2 + 1];
        if (samples[framesRead*2] > MaximumValue) MaximumValue = samples[framesRead*2];
        if (samples[framesRead*2+1] > MaximumValue) MaximumValue = samples[framesRead*2+1];
        _frameOffset++;
        framesRead++;
      }

      _currentPacket++;
      while (framesRead < count && _currentPacket*2 < _numPackets)
      {
        ReadTwoPackets(_currentPacket++);
        _frameOffset = 0;
        while (_frameOffset < 64 && framesRead < count)
        {
          samples[framesRead * 2] = _sampleData[_frameOffset * 2];
          samples[framesRead * 2 + 1] = _sampleData[_frameOffset * 2 + 1];
          if (samples[framesRead * 2] > MaximumValue) MaximumValue = samples[framesRead * 2];
          if (samples[framesRead * 2 + 1] > MaximumValue) MaximumValue = samples[framesRead * 2 + 1];
          _frameOffset++;
          framesRead++;
        }
      }
      _currentPacket--;
      CurrentSample = _currentPacket*64 + _frameOffset;
      return framesRead;
    }

    static readonly short[] ImaIndxAdjust =
    { -1, -1, -1, -1,
	    +2, +4, +6, +8,
	    -1, -1, -1, -1,
	    +2, +4, +6, +8
    };

    static readonly short[] ImaStepSize =
    { 7, 8, 9, 10, 11, 12, 13, 14, 16, 17, 19, 21, 23, 25, 28, 31, 34, 37, 41, 45,
      50, 55, 60, 66, 73, 80, 88, 97, 107, 118, 130, 143, 157, 173, 190, 209, 230,
      253, 279, 307, 337, 371, 408, 449, 494, 544, 598, 658, 724, 796, 876, 963,
      1060, 1166, 1282, 1411, 1552, 1707, 1878, 2066, 2272, 2499, 2749, 3024, 3327,
      3660, 4026, 4428, 4871, 5358, 5894, 6484, 7132, 7845, 8630, 9493, 10442,
      11487, 12635, 13899, 15289, 16818, 18500, 20350, 22385, 24623, 27086, 29794,
      32767
    };
  }
}
