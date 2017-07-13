using System;

namespace Jammit.Audio
{
  public interface ISongPlayer2
  {
    void Play();
    void Pause();
    void Stop();
    long PositionSamples { get; }
    TimeSpan Position { get; set; }
    TimeSpan Length { get; }
    PlaybackStatus State { get; }

    int Channels { get; }
    string GetChannelName(int channel);
    void SetChannelVolume(int channel, float volume);
    float GetChannelVolume(int channel);
  }
}
