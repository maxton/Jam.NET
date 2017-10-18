using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Audio;
using Windows.Media.Render;
using Windows.Storage;

using Jammit.Model;

namespace Jammit.Audio
{
  public class AudioGraphSongPlayer : ISongPlayer
  {
    #region Private members

    private AudioGraph audioGraph;
    private DeviceInformation selectedDevice;
    private AudioDeviceOutputNode deviceOutputNode;
    private PlaybackStatus status = PlaybackStatus.Stopped;

    private string trackPath;

    #endregion

    public AudioGraphSongPlayer(ISong s)
    {
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

      var devices = Task.Run(async () => await DeviceInformation.FindAllAsync(DeviceClass.AudioRender)).Result;
      //foreach (var device in devices)
      //{

      //}
      selectedDevice = devices.FirstOrDefault(d => d.IsDefault);

      // Init graph
      var settings = new AudioGraphSettings(AudioRenderCategory.Media);
      settings.PrimaryRenderDevice = selectedDevice;
      var createResult = Task.Run(async () => await AudioGraph.CreateAsync(settings)).Result;
      if (AudioGraphCreationStatus.Success != createResult.Status)
        return;
      audioGraph = createResult.Graph;
      //audioGraph.UnrecoverableErrorOccurred += (AudioGraph sender, AudioGraphUnrecoverableErrorOccurredEventArgs args) => { };

      var deviceResult = Task.Run(async () => await audioGraph.CreateDeviceOutputNodeAsync()).Result;
      if (AudioDeviceNodeCreationStatus.Success != deviceResult.Status)
        return;
      deviceOutputNode = deviceResult.DeviceOutputNode;

      var uri = new Uri($"ms-appdata:///local/Tracks/{s.Metadata.Id}.jcf/{trackPath}");
      var file = Task.Run(async () => await StorageFile.GetFileFromApplicationUriAsync(uri)).Result;

      var createInput = Task.Run(async () => await audioGraph.CreateFileInputNodeAsync(file)).Result;
      if (AudioFileNodeCreationStatus.Success != createInput.Status)
        return;

      var fileInputNode = createInput.FileInputNode;
      fileInputNode.LoopCount = null;
      fileInputNode.OutgoingGain = 1;
      fileInputNode.AddOutgoingConnection(deviceOutputNode);
    }

    #region ISongPlayer members

    public void Play()
    {
      audioGraph.Start();
      status = PlaybackStatus.Playing;
    }

    public void Pause()
    {
      audioGraph.Stop();
      status = PlaybackStatus.Paused;
    }

    public void Stop()
    {
      audioGraph.Stop();
      status = PlaybackStatus.Stopped;
    }

    public PlaybackStatus State => status;

    public TimeSpan Position { get; set; }

    public long PositionSamples => 0;

    public TimeSpan Length => TimeSpan.Zero;

    public int Channels => 0;

    public string GetChannelName(int channel) => "";

    public void SetChannelVolume(int channel, float volume)
    {

    }

    public float GetChannelVolume(int channel) => 0f;

    #endregion // ISongPlayer members
  }
}
