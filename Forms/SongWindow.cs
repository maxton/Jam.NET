using System;
using System.Windows.Forms;
using Jammit.Audio;
using Jammit.Controls;
using Jammit.Model;

namespace Jammit
{
  
  public partial class SongWindow : Form
  {
    private ISong _song;
    private ISongPlayer _player;
    private Timer _timer;

    public SongWindow(SongMeta t)
    {
      _song = SongLoader.Load(t);
      InitializeComponent();
      Text = $"Score: {t.Artist} - {t.Name} [{t.Instrument}]";
      albumArtwork.Image = _song.GetCover();
      if (_song.Tracks[0].HasNotation)
      {
        score1.SetImages(_song.GetNotation(_song.Tracks[0]));
      }

      _player = _song.GetSongPlayer();
      for (int x = 0; x < _player.Channels; x++)
        AddFader(x);
      
      seekBar.Maximum = (int)(_player.Length.TotalSeconds + 1);
      _timer = new Timer();
      _timer.Interval = 30;
      _timer.Tick += TimerTick;
      waveform1.WaveData = _song.GetWaveform();
      waveform1.Sections = _song.Sections;
    }

    private void AddFader(int channel)
    {
      var fader = new Fader();
      fader.Text = _player.GetChannelName(channel);
      fader.OnFaderChange += value => _player.SetChannelVolume(channel, value / 100.0f);
      mixerFlowPanel.Controls.Add(fader);
    }

    private void TimerTick(object sender, EventArgs e)
    {
      timePos.Text = _player.Position.ToString("mm\\:ss");
      timeRemain.Text = "-" + _player.Length.Subtract(_player.Position).ToString("mm\\:ss");
      seekBar.Value = (int)_player.Position.TotalSeconds;
      waveform1.PositionSamples = _player.PositionSamples;
      waveform1.Invalidate();
      score1.TimeSeconds = _player.Position.TotalSeconds;
    }

    private void SongWindow_FormClosing(object sender, FormClosingEventArgs e)
    {
      _timer.Stop();
      _timer.Dispose();
      _player.Stop();
    }

    private void seekBar_MouseUp(object sender, MouseEventArgs e)
    {
      _player.Position = TimeSpan.FromSeconds(seekBar.Value);
    }

    private void playPauseButton_Click(object sender, EventArgs e)
    {
      if (_player.State == NAudio.Wave.PlaybackState.Playing)
      {
        _player.Pause();
        _timer.Stop();
        button1.Text = "Play";
      }
      else
      {
        _player.Play();
        _timer.Start();
        button1.Text = "Pause";
      }
    }

    private void closeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Close();
    }
  }
}
