using System;
using System.IO;
using NAudio.Wave;

namespace Jammit.Audio
{
  public class ImaWaveStream : WaveStream
  {
    private readonly Ima4Decoder _decoder;

    public ImaWaveStream(Ima4Decoder d)
    {
      _decoder = d;
      WaveFormat = new WaveFormat(44100, 16, 2);
    }

    /// <summary>
    /// Wrap the array of ADPCM data.
    /// </summary>
    /// <param name="data">ADPCM packets</param>
    public ImaWaveStream(Stream data) : this(new Ima4Decoder(data)) { }

    public override WaveFormat WaveFormat { get; } 

    /// <summary>
    /// Reads given number of bytes.
    /// </summary>
    /// <returns>Number of sample bytes read (4 per sample frame).</returns>
    public override int Read(byte[] buffer, int offset, int count)
    {
      short[] buf = new short[buffer.Length/2];
      int frames = _decoder.GetSamples(buf, buf.Length/2);
      Buffer.BlockCopy(buf, 0, buffer, 0, buf.Length * 2);
      return frames * 4;
    }

    /// <summary>
    /// Length in bytes (1 sample frame = 2 bytes * 2 channels = 4)
    /// </summary>
    public override long Length => 4*(long) _decoder.Samples;

    public override long Position
    {
      get { return _decoder.CurrentSample*4; }
      set { _decoder.Seek(value/4); }
    }
  }
}
