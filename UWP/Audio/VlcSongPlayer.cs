using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using libVLCX;
using Jammit.Model;
using Windows.Media.Devices;
using Windows.System.Profile;

namespace Jammit.Audio
{
  public class VlcSongPlayer : ISongPlayer
  {
    #region private members

    VLC.MediaElement mediaElement;

    #endregion // private members

    public VlcSongPlayer(ISong s, VLC.MediaElement mediaElement)
    {
      string trackPath = "";
      foreach (var t in s.Tracks)
      {
        if (t.Class == "JMFileTrack")
        {
          trackPath = $"{t.Identifier}_jcfx";
        }
        else
        {
          //TODO: Click track
        }
      }
      var uri = $"ms-appdata:///local/Tracks/{s.Metadata.Id}.jcf/{trackPath}";

      this.mediaElement = mediaElement;
      this.mediaElement.MediaSource = VLC.MediaSource.CreateFromUri(uri);
    }

    #region ISongPlayer methods

    public long PositionSamples => throw new NotImplementedException();

    public TimeSpan Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public TimeSpan Length => throw new NotImplementedException();

    public PlaybackStatus State { get; private set; }

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
      this.mediaElement.Pause();
      this.State = PlaybackStatus.Paused;
    }

    public void Play()
    {
      this.mediaElement.Play();
      this.State = PlaybackStatus.Playing;
    }

    public void SetChannelVolume(int channel, float volume)
    {
      throw new NotImplementedException();
    }

    public void Stop()
    {
      this.mediaElement.Stop();
      this.State = PlaybackStatus.Stopped;
    }

    #endregion
  }
}
