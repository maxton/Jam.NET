using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using libVLCX;
using Jammit.Model;
using Windows.Media.Devices;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.System.Profile;

namespace Jammit.Audio
{
  public class VlcSongPlayer : ISongPlayer
  {
    #region private members

    VLC.MediaElement mediaElement;
    string token;

    #endregion // private members

    public VlcSongPlayer(ISong s, VLC.MediaElement mediaElement)
    {
      string trackFile = "";
      foreach (var t in s.Tracks)
      {
        if (t.Class == "JMFileTrack")
        {
          trackFile = $"{t.Identifier}_jcfx.aifc";//TODO: REMOVE AIFC EXTENSION!
          token = "{" + t.Identifier.ToString().ToUpper() + "}";

          string trackPath = $"Tracks\\{s.Metadata.Id}.jcf\\{trackFile}";
          var fileTask = Task.Run(async() => await ApplicationData.Current.LocalFolder.GetFileAsync(trackPath));
          fileTask.Wait();
         StorageApplicationPermissions.FutureAccessList.AddOrReplace(token, fileTask.Result);

          break;//UNDO.
        }
        else
        {
          //TODO: Click track
        }
      }
      //var uri = $"ms-appdata:///local/Tracks/{s.Metadata.Id}.jcf/{trackPath}";
      var uri = $"winrt://{token}";

      this.mediaElement = mediaElement;
      //this.mediaElement.MediaSource = VLC.MediaSource.CreateFromUri(uri);
      this.mediaElement.Source = uri;
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
      var player = mediaElement.MediaPlayer;
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
