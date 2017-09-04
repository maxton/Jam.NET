using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jammit.Audio;
using Jammit.Model;

namespace Jammit.UWP.Audio
{
  class NAudioUniversalSongPlayer : ISongPlayer2
  {
    /// <summary>
    /// Creates a song player for the given song using the NAudio backend.
    /// </summary>
    /// <param name="s">ISong to play.</param>
    internal NAudioUniversalSongPlayer(ISong s)
    {
      throw new NotImplementedException();
    }

    ~NAudioUniversalSongPlayer()
    {
      throw new NotImplementedException();
    }

    public void Play()
    {
      throw new NotImplementedException();
    }

    public void Stop()
    {
      throw new NotImplementedException();
    }

    public void Pause()
    {
      throw new NotImplementedException();
    }

    public PlaybackStatus State => throw new NotImplementedException();

    public TimeSpan Position
    {
      get { throw new NotImplementedException(); }
      set { throw new NotImplementedException(); }
    }

    // 32-bit samples, 2 channels = 8 bytes per sample frame
    public long PositionSamples => throw new NotImplementedException();

    public TimeSpan Length => throw new NotImplementedException();
    public int Channels => throw new NotImplementedException();
    public string GetChannelName(int channel) => throw new NotImplementedException();

    public void SetChannelVolume(int channel, float volume)
    {
      throw new NotImplementedException();
    }

    public float GetChannelVolume(int channel) => throw new NotImplementedException();
  }
}
