using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Jammit.Audio;
using Jammit.Controls;
using Jammit.Model;

namespace Jammit
{
  
  public partial class SongWindow : Form
  {
    private Song s;
    private ISongPlayer player;
    private Timer updateTimer;

    public SongWindow(SongMeta t)
    {
      s = new Song(t);
      InitializeComponent();
      Text = $"Score: {t.Artist} - {t.Name} [{t.Instrument}]";

      using (var x = s.OpenZip())
      {
        using (var stream = x.GetEntry($"{s.Metadata.GuidString}.jcf/cover.jpg").Open())
        using (var ms = new MemoryStream())
        {
          stream.CopyTo(ms);
          albumArtwork.Image = Image.FromStream(ms);
        }

        if (s.Tracks[0].HasNotation)
        {
          using (var stream = x.GetEntry($"{s.Metadata.GuidString}.jcf/{s.Tracks[0].Id}_jcfn_00").Open())
          using (var ms = new MemoryStream())
          {
            stream.CopyTo(ms);
            score1.Image = Image.FromStream(ms);
          }
        }
      }

      player = new JammitZipSongPlayer(s);
      for (int x = 0; x < player.Channels; x++)
        AddFader(x);
      
      seekBar.Maximum = (int)(player.Length.TotalSeconds + 1);
      updateTimer = new Timer();
      updateTimer.Interval = 30;
      updateTimer.Tick += UpdateTimer_Tick;
      waveform1.WaveData = s.GetWaveform();
    }

    private void AddFader(int channel)
    {
      var fader = new Fader();
      fader.Text = player.GetChannelName(channel);
      fader.OnFaderChange += value => player.SetChannelVolume(channel, value / 100.0f);
      mixerFlowPanel.Controls.Add(fader);
    }

    private void UpdateTimer_Tick(object sender, EventArgs e)
    {
      timePos.Text = player.Position.ToString("mm\\:ss");
      timeRemain.Text = "-" + player.Length.Subtract(player.Position).ToString("mm\\:ss");
      seekBar.Value = (int)player.Position.TotalSeconds;
      waveform1.PositionSamples = player.PositionSamples;
      waveform1.Invalidate();
      score1.TimeSeconds = player.Position.TotalSeconds;
    }

    private void SongWindow_FormClosing(object sender, FormClosingEventArgs e)
    {
      updateTimer.Stop();
      updateTimer.Dispose();
      player.Stop();
    }

    private void seekBar_MouseUp(object sender, MouseEventArgs e)
    {
      player.Position = TimeSpan.FromSeconds(seekBar.Value);
    }

    private void playPauseButton_Click(object sender, EventArgs e)
    {
      if (player.State == NAudio.Wave.PlaybackState.Playing)
      {
        player.Pause();
        updateTimer.Stop();
        button1.Text = "Play";
      }
      else
      {
        player.Play();
        updateTimer.Start();
        button1.Text = "Pause";
      }
    }
  }
}
