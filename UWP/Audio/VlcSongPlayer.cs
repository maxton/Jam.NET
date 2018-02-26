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
  class VlcControl : Windows.UI.Xaml.Controls.Control
  {
    public Windows.UI.Xaml.Controls.SwapChainPanel SwapChainPanel { get; private set; }

    protected override void OnApplyTemplate()
    {
      base.OnApplyTemplate();

      var swapChainPannel = (Windows.UI.Xaml.Controls.SwapChainPanel)this.GetTemplateChild("SwapChainPanel");
      SwapChainPanel = swapChainPannel;
      swapChainPannel.CompositionScaleChanged += (sender, e) => { };
      swapChainPannel.SizeChanged += (sender, e) => { };
    }
  }

  public class VlcSongPlayer : ISongPlayer
  {
    #region private members

    VlcControl swapChainProvider;
    MediaPlayer player;
    string audioDeviceId;

    #region VLC Dialog handlers

    void OnError(string title, string text)
    {

    }

    void OnLogin(Dialog dialog, string title, string text, string defaultUserName, bool askToStore)
    {

    }

    void OnQuestion(Dialog dialog, string title, string text, Question qType, string cancel, string action1, string action2)
    {

    }

    void OnDisplayProgress(Dialog dialog, string title, string text, bool intermediate, float position, string cancel)
    {

    }

    void OnCancel(Dialog dialog)
    {

    }

    void OnUpdateProgress(Dialog dialog, float position, string text)
    {

    }

    #endregion // VLC Dialog handlers

    #endregion // private members

    public VlcSongPlayer(ISong s)
    {
      // VLC setup
      // Initialize SwapChainPanel
      swapChainProvider = new VlcControl();
      swapChainProvider.ApplyTemplate();
      Instance instance = new Instance
      (
        new List<string>
        {
          "-I",
          "dummy",
          "--no-osd",
          //"--verbose=3",
          "--no-stats",
          "--avcodec-fast",
          "--subsdec-encoding",
          string.Empty,
          AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile" ? "--deinterlace-mode=bob" : string.Empty,
          "--aout=winstore"
          //,$"--keystore-file={System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "VLC_MediaElement_KeyStore")}"
        },
        swapChainProvider.SwapChainPanel
      );
      instance.setDialogHandlers(OnError, OnLogin, OnQuestion, OnDisplayProgress, OnCancel, OnUpdateProgress);

      audioDeviceId = MediaDevice.GetDefaultAudioRenderId(AudioDeviceRole.Default);
      MediaDevice.DefaultAudioCaptureDeviceChanged += (sender, e) =>
      {
        if (AudioDeviceRole.Default == e.Role)
          audioDeviceId = e.Id;
      };
    }

    #region ISongPlayer methods

    public long PositionSamples => throw new NotImplementedException();

    public TimeSpan Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public TimeSpan Length => throw new NotImplementedException();

    public PlaybackStatus State => throw new NotImplementedException();

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
      throw new NotImplementedException();
    }

    public void SetChannelVolume(int channel, float volume)
    {
      throw new NotImplementedException();
    }

    public void Stop()
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
