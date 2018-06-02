using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jammit.Model;
using Plugin.SimpleAudioPlayer;

namespace Jammit.Audio
{
  class SimpleSongPlayer : ISongPlayer
  {
    private ISimpleAudioPlayer _player = CrossSimpleAudioPlayer.Current;
    private System.IO.Stream _stream;

    internal SimpleSongPlayer(ISong s)
    {
      foreach (var t in s.Tracks)
      {
        if (t.Class == "JMFileTrack")
        {
          _stream = s.GetSeekableContentStream($"{t.Identifier}_jcfx");
          break;
        }
      }
    }

    #region ISongPlayer

    public long PositionSamples => throw new NotImplementedException();

    public TimeSpan Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public TimeSpan Length => throw new NotImplementedException();

    public PlaybackStatus State => _player.IsPlaying ? PlaybackStatus.Playing : PlaybackStatus.Stopped;
    //{
    //  get
    //  {
    //    if (_player.IsPlaying)
    //      return PlaybackStatus.Playing;

    //    return PlaybackStatus.Stopped;
    //  }
    //}

    public int Channels => throw new NotImplementedException();

    public string GetChannelName(int channel)
    {
      throw new NotImplementedException();
    }

    public float GetChannelVolume(int channel)
    {
      throw new NotImplementedException();
    }

    public void Pause()
    {
      throw new NotImplementedException();
    }

    public void Play()
    {
      _player.Load(_stream);
      _player.Play();
    }

    public void SetChannelVolume(int channel, float volume)
    {
      throw new NotImplementedException();
    }

    public void Stop()
    {
      _player.Stop();
    }

    #endregion
  }
}
