using System;
using System.Collections.Generic;
using System.IO;
using Jammit.Model;
using NAudio.Wave;

namespace Jammit.Audio
{
  /// <summary>
  /// Handles playing the audio for a track.
  /// </summary>
  public class JammitZipSongPlayer : ISongPlayer
  {
    private List<WaveChannel32> channels;
    private WaveMixerStream32 mixer;
    private WaveOutEvent waveOut;
    private List<string> chanNames;

    /// <summary>
    /// Creates a song player for the given song.
    /// </summary>
    /// <param name="s">Song to play.</param>
    internal JammitZipSongPlayer(Song s)
    {
      waveOut = new WaveOutEvent();
      mixer = new WaveMixerStream32();
      channels = new List<WaveChannel32>();
      chanNames = new List<string>();

      using (var x = s.OpenZip())
      {
        foreach (var t in s.Tracks)
        {
          if (t.ClassName != "JMFileTrack") continue;
          var e = x.GetEntry($"{s.Metadata.GuidString}.jcf/{t.Id}_jcfx");
          using (var st = e.Open())
          {
            var ms = new MemoryStream();
            st.CopyTo(ms);
            channels.Add(new WaveChannel32(new ImaWaveStream(ms)));
            chanNames.Add(t.Title);
          }
        }
      }

      int i = 1;
      foreach (var d in channels)
      {
        mixer.AddInputStream(d);
        d.Volume = 0.75f;
      }
      waveOut.PlaybackStopped += (sender, args) => {Position = TimeSpan.Zero;};
      waveOut.DesiredLatency = 60;
      waveOut.NumberOfBuffers = 2;
      waveOut.Init(mixer);
    }

    ~JammitZipSongPlayer()
    {
      if(waveOut.PlaybackState == PlaybackState.Playing)
        waveOut.Stop();
      waveOut.Dispose();
    }

    public void Play()
    {
      waveOut.Play();
    }

    public void Stop()
    {
      waveOut.Stop();
    }

    public void Pause()
    {
      waveOut.Pause();
    }

    public PlaybackState State => waveOut.PlaybackState;

    public TimeSpan Position
    {
      get { return mixer.CurrentTime; }
      set { mixer.CurrentTime = value; }
    }

    public long PositionSamples => mixer.Position/4;

    public TimeSpan Length => mixer.TotalTime;
    public int Channels => channels.Count;
    public string GetChannelName(int channel) => chanNames[channel];

    public void SetChannelVolume(int channel, float volume)
    {
      channels[channel].Volume = volume;
    }

    public float GetChannelVolume(int channel) => channels[channel].Volume;
  }
}