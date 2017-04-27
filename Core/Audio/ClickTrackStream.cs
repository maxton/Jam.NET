using System;
using System.Collections.Generic;
using Jammit.Model;
using NAudio.Wave;

namespace Jammit.Audio
{
  public class ClickTrackStream : WaveStream
  {
    private readonly IReadOnlyList<Beat> _beats;

    private readonly short[] _click;

    public ClickTrackStream(IReadOnlyList<Beat> beats)
    {
      _beats = beats;
      WaveFormat = new WaveFormat(44100, 16, 2);
      Length = (long)(_beats[_beats.Count - 1].Time*44100*4);
      var stick = Properties.Resources.stick;
      _click = new short[stick.Length/2];
      Buffer.BlockCopy(stick, 0, _click, 0, stick.Length);
    }

    public override long Length { get; }

    /// <summary>
    /// Mono Samples
    /// </summary>
    private int _samplePos;
    /// <summary>
    /// Index into array.
    /// </summary>
    private int _currentBeat;
    public override long Position
    {
      get { return _samplePos*4; }

      set
      {
        _samplePos = (int)value/4;
        var time = (_samplePos + _click.Length)/44100.0;
        for (var i = 0; i < _beats.Count; i++)
        {
          _currentBeat = i;
          if (_beats[i].Time >= time) break;
        }
      }
    }

    public override WaveFormat WaveFormat { get; }

    public override int Read(byte[] buffer, int offset, int count)
    {
      int bytesRead = 0;
      while (count > 0 && _beats.Count > _currentBeat)
      {
        var sampleOffset = _samplePos - (int)((_beats[_currentBeat].Time - 0.005) * 44100);
        // empty space before click
        while (sampleOffset < 0 && count > 0)
        {
          buffer[offset++] = 0;buffer[offset++] = 0;
          buffer[offset++] = 0;buffer[offset++] = 0;
          _samplePos++;
          sampleOffset++;
          count -= 4;
          bytesRead += 4;
        }
        // click
        while (sampleOffset >= 0 && sampleOffset < _click.Length && count > 0)
        {
          buffer[offset++] = (byte)(_click[sampleOffset] & 0xFF);
          buffer[offset++] = (byte)((_click[sampleOffset] >> 8) & 0xFF);
          buffer[offset++] = (byte)(_click[sampleOffset] & 0xFF);
          buffer[offset++] = (byte)((_click[sampleOffset] >> 8) & 0xFF);
          _samplePos++;
          sampleOffset++;
          count -= 4;
          bytesRead += 4;
        }
        // if click is done set the next click.
        if (sampleOffset >= _click.Length)
        {
          _currentBeat++;
        }
      }
      return bytesRead;
    }
  }
}