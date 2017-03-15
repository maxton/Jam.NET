using System;
using Jammit.Model;
using NAudio.Wave;

namespace Jammit.Audio
{
  public class MockSongPlayer : ISongPlayer
  {
    private TimeSpan _position;
    private PlaybackState _playbackState;
    private readonly WaveMixerStream32 _mixer;

    public MockSongPlayer(ISong s)
    {
      _mixer = new WaveMixerStream32();
      _mixer.AddInputStream(new WaveChannel32(new ClickTrackStream(s.Beats)));
    }

    #region ISongPlayer members

    public void Play()
    {
      _playbackState = PlaybackState.Playing;
    }

    public void Pause()
    {
      _playbackState = PlaybackState.Paused;
    }

    public void Stop()
    {
      _playbackState = PlaybackState.Stopped;
    }

    public long PositionSamples => _position.Ticks / 8;

    public TimeSpan Position
    {
      get { return _position; }
      set { _position = value; }
    }

    public TimeSpan Length => _mixer.TotalTime;

    public PlaybackState State => _playbackState;

    public int Channels => 0;

    public string GetChannelName(int channel) => $"Mock Channel[{channel}]";

    public void SetChannelVolume(int channel, float volume) { }

    public float GetChannelVolume(int channel) => 0.0f;

    #endregion
  }
}
