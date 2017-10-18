using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jammit.Audio;
using Jammit.Model;
using NAudio.Wave;
using NAudio.Win8.Wave.WaveOutputs;

namespace Jammit.Audio
{
  class NAudioUniversalSongPlayer : ISongPlayer
  {
    private readonly List<WaveChannel32> _channels;
    private readonly WaveMixerStream32 _mixer;
    private readonly WasapiOutRT _waveOut;
    private readonly List<string> _chanNames;

    /// <summary>
    /// Creates a song player for the given song using the NAudio backend.
    /// </summary>
    /// <param name="s">ISong to play.</param>
    internal NAudioUniversalSongPlayer(ISong s)
    {
      _waveOut = new WasapiOutRT(NAudio.CoreAudioApi.AudioClientShareMode.Shared, 60);
      _mixer = new WaveMixerStream32();
      _channels = new List<WaveChannel32>();
      _chanNames = new List<string>();

      foreach (var t in s.Tracks)
      {
        if (t.Class == "JMFileTrack")
        {
          var stream = s.GetSeekableContentStream($"{t.Identifier}_jcfx");
          _channels.Add(new WaveChannel32(new ImaWaveStream(stream)));
          _chanNames.Add((t as ConcreteTrackInfo).Title);
        }
        else if (t.Class == "JMClickTrack")
        {
          //TODO: Fix exception throwing.
          //_channels.Add(new WaveChannel32(new ClickTrackStream(s.Beats)));
          //_chanNames.Add((t as ConcreteTrackInfo).Title);
        }
      }

      foreach (var d in _channels)
      {
        _mixer.AddInputStream(d);
        d.Volume = 0.75f;
      }

      _waveOut.PlaybackStopped += (sender, args) => { Position = TimeSpan.Zero; };
      //_waveOut.DesiredLatency = 60;
      //_waveOut.NumberOfBuffers = 2;
      _waveOut.Init(() => { return _mixer; });
    }

    ~NAudioUniversalSongPlayer()
    {
      if (_waveOut.PlaybackState == PlaybackState.Playing)
        _waveOut.Stop();

      _waveOut.Dispose();
    }

    #region ISongPlayer

    public void Play()
    {
      _waveOut.Play();
    }

    public void Stop()
    {
      _waveOut.Stop();
    }

    public void Pause()
    {
      _waveOut.Pause();
    }

    public PlaybackStatus State
    {
      get
      {
        switch (_waveOut.PlaybackState)
        {
          case PlaybackState.Stopped:
            return PlaybackStatus.Stopped;
          case PlaybackState.Playing:
            return PlaybackStatus.Playing;
          case PlaybackState.Paused:
            return PlaybackStatus.Paused;

          default:
            throw new Exception("Unknown playback state: " + _waveOut.PlaybackState);
        }
      }
    }

    public TimeSpan Position
    {
      get { return _mixer.CurrentTime; }
      set { _mixer.CurrentTime = value; }
    }

    // 32-bit samples, 2 channels = 8 bytes per sample frame
    public long PositionSamples => _mixer.Position / 8;

    public TimeSpan Length => _mixer.TotalTime;

    public int Channels => _channels.Count;

    public string GetChannelName(int channel) => _chanNames[channel];

    public void SetChannelVolume(int channel, float volume)
    {
      _channels[channel].Volume = volume;
    }

    public float GetChannelVolume(int channel) => _channels[channel].Volume;

    #endregion //ISongPlayer2 members
  }
}
