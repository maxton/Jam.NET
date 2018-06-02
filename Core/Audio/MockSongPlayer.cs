using System;

namespace Jammit.Audio
{
  public class MockSongPlayer : ISongPlayer
  {
    private TimeSpan _position;
    private PlaybackStatus _playbackState;

    public MockSongPlayer(Model.ISong song) { }

    #region ISongPlayer members

    public void Play()
    {
      _playbackState = PlaybackStatus.Playing;
    }

    public void Pause()
    {
      _playbackState = PlaybackStatus.Paused;
    }

    public void Stop()
    {
      _playbackState = PlaybackStatus.Stopped;
    }

    public long PositionSamples => _position.Ticks / 8;

    public TimeSpan Position
    {
      get { return _position; }
      set { _position = value; }
    }

    public TimeSpan Length => TimeSpan.Zero;

    public PlaybackStatus State => _playbackState;

    public int Channels => 0;

    public string GetChannelName(int channel) => $"Mock Channel[{channel}]";

    public void SetChannelVolume(int channel, float volume) { }

    public float GetChannelVolume(int channel) => 0.0f;

    #endregion
  }
}
