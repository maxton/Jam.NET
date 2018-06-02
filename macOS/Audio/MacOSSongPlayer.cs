using System;

using AVFoundation;
using Foundation;
using Jammit.Model;

namespace Jammit.Audio.macOS
{
  public class MacOSSongPlayer : ISongPlayer
  {
    #region private members

    private ISong song;
    private AVAudioPlayer player;
    private string trackPath;

    #endregion

    public MacOSSongPlayer(ISong songContents)
    {
      ActivateAudioSession();

      this.song = songContents;
      foreach (var t in song.Tracks)
      {
        if (t.Class == "JMFileTrack")
        {
          trackPath = $"{t.Identifier}_jcfx";

          break; //TODO: Add all file channel tracks.
        }
        else if (t.Class == "JMClickTrack")
        {
          //TODO
        }
      }
    }

    //TODO: Find macOS equivalent for AVAudioSession.
    private void ActivateAudioSession()
    {
      //var session = AVAudioSession.SharedInstance();
      //session.SetCategory(AVAudioSessionCategory.Ambient);// Play in the background
      //session.SetActive(true);
    }

    private void DeactivateAudioSession()
    {
      //AVAudioSession.SharedInstance().SetActive(false);
    }

    private void ReactivateAudioSession()
    {
      //AVAudioSession.SharedInstance().SetActive(true);
    }

    #region ISongPlayer members

    public void Play()
    {
      // Dispose any existing playback.
      if (player != null)
      {
        player.Stop();
        player.Dispose();
      }

      NSError err;
      var stream = song.GetSeekableContentStream(trackPath);
      player = AVAudioPlayer.FromData(NSData.FromStream(stream), out err);

      player.Volume = 0.75f;
      player.FinishedPlaying += delegate {
        player.Dispose();
        player = null;
      };
      player.NumberOfLoops = 1;
      player.Play();
    }

    public void Pause()
    {
      Stop();
    }

    public void Stop()
    {
      if (player != null)
      {
        player.Stop();
        player.Dispose();
      }
    }

    public PlaybackStatus State { get; }

    public TimeSpan Position { get; set; }

    public long PositionSamples => 0;

    public TimeSpan Length => TimeSpan.Zero;

    public int Channels => 1;

    public string GetChannelName(int channel) => "UNIQUE";

    public float GetChannelVolume(int channel) => player.Volume;

    public void SetChannelVolume(int channel, float volume)
    {
      player.Volume = volume;
    }

    #endregion
  }
}
