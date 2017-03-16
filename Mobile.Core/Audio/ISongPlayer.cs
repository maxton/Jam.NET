using System;
using NAudio.Wave;

namespace Jammit.Audio
{
  /// <summary>
  /// A song player plays a song, which is consists of one or more channels.
  /// Channels could represent different instruments.
  /// </summary>
  public interface ISongPlayer
  {
    void Play();
    void Pause();
    void Stop();
    long PositionSamples { get; }
    TimeSpan Position { get; set; }
    TimeSpan Length { get; }
    PlaybackState State { get; }

    int Channels { get; }
    string GetChannelName(int channel);
    void SetChannelVolume(int channel, float volume);
    float GetChannelVolume(int channel);
  }
}