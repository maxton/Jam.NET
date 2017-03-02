using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Jammit.Model;

namespace Jammit.Controls
{
  public class Score : UserControl, ISynchronizable
  {
    public long SamplePosition
    {
      get { return _samplePosition; }
      set
      {
        _samplePosition = value;
        pictureBox1.Invalidate();
      }
    }

    public long Samples { get; set; }

    // State data for selected notation
    private List<Image> _imgs;
    private IReadOnlyList<BeatInfo> _nodes;
    private IReadOnlyList<Beat> _beats;
    private int _beatNum;
    private Track _track;

    private ISong _song;
    private ComboBox dropdown;
    private PictureBox pictureBox1;
    private Pen _line;
    private long _samplePosition;

    public Score()
    {
      InitializeComponent();
      pictureBox1.Paint += (o, e) => PaintScore(e);
    }

    protected override void OnResize(EventArgs e)
    {
      pictureBox1.Invalidate();
      base.OnResize(e);
    }

    struct ScoreInfo
    {
      public string TrackName;
      public string Type;
      public Track Track;
      public override string ToString() => TrackName + " - " + Type;
    }

    public void SetSong(ISong s)
    {
      _song = s;
      _beats = s.Beats;
      foreach (var t in s.Tracks)
      {
        if (t.HasNotation)
          dropdown.Items.Add(new ScoreInfo {TrackName = t.Title, Type = "Score", Track = t});
        if (t.HasTablature)
          dropdown.Items.Add(new ScoreInfo {TrackName = t.Title, Type = "Tablature", Track = t});
      }
      dropdown.SelectedIndex = 0;
    }

    private void SetNotation(List<Image> imgs, ScoreNodes nodes, Track track)
    {
      DoubleBuffered = true;
      if (imgs == null) return;
      _imgs = imgs;
      if (imgs.Count == 0) return;
      _nodes = nodes.Nodes;
      _track = track;
      _line = new Pen(Color.FromArgb(0x7F00FF00), 3);
      pictureBox1.Invalidate();
    }

    private void UpdateCurrentBeat()
    {
      _beatNum = 0;
      for (int i = 0; i < _beats.Count - 1; i++)
      {
        if (_beats[i + 1].Time > SamplePosition/44100.0)
        {
          _beatNum = i;
          break;
        }
      }
    }

    /// <summary>
    /// Return the x offset to draw.
    /// </summary>
    /// <returns></returns>
    private int Interpolate()
    {
      if(_nodes.Count > _beatNum+1)
      {
        if (_nodes[_beatNum].Row != _nodes[_beatNum + 1].Row && _beatNum != 0)
        {
          // Case 1: the next beat is on the next row
          var timeDiff = _beats[_beatNum + 1].Time - _beats[_beatNum].Time;
          var xDiff = _nodes[_beatNum].X - _nodes[_beatNum - 1].X;
          var timeFromBeat = SamplePosition / 44100.0 - _beats[_beatNum].Time;
          // What we do here is guess how "wide" the last beat is, because they don't include that in the data :(
          // 0.75 times the last beat width tends to be good enough without sliding off the score too often.
          return (int)(xDiff * timeFromBeat / timeDiff * 0.75);
        }
        else
        {
          // Case 2: the next beat is on the same row
          var timeDiff = _beats[_beatNum + 1].Time - _beats[_beatNum].Time;
          var xDiff = _nodes[_beatNum + 1].X - _nodes[_beatNum].X;
          var timeFromBeat = SamplePosition/44100.0 - _beats[_beatNum].Time;
          return (int) (xDiff*timeFromBeat/timeDiff);
        }
      }
      return 0;
    }

    private void PaintScore(PaintEventArgs pe)
    {
      if (_imgs != null && _nodes != null && _beats != null)
      {
        UpdateCurrentBeat();
        var xOffset = (Width - _imgs[0].Width)/2;
        var x = _nodes[_beatNum].X + xOffset + Interpolate();
        var y = _nodes[_beatNum].Row == 65535 ? 0 : _track.ScoreSystemInterval * _nodes[_beatNum].Row;
        var img = _nodes[_beatNum].Row == 65535 ? 0 : y/_imgs[0].Height;

        if (_imgs.Count > img)
        {
          var rect = Rectangle.FromLTRB(xOffset, -y + 1024*img, xOffset+724, 1024 - y + 1024 * img);
          pe.Graphics.DrawImage(_imgs[img], rect);
          if (_imgs.Count > img + 1)
            pe.Graphics.DrawImageUnscaled(_imgs[img + 1], Rectangle.FromLTRB(xOffset, 1024 - y + 1024*img, xOffset + 724, 2048 - y + 1024 * img));
        }
        pe.Graphics.DrawLine(_line, x, 0, x, _track.ScoreSystemHeight);
      }

      base.OnPaint(pe);
    }

    private void InitializeComponent()
    {
      this.dropdown = new System.Windows.Forms.ComboBox();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.SuspendLayout();
      // 
      // dropdown
      // 
      this.dropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.dropdown.FormattingEnabled = true;
      this.dropdown.Location = new System.Drawing.Point(0, 0);
      this.dropdown.Name = "dropdown";
      this.dropdown.Size = new System.Drawing.Size(200, 21);
      this.dropdown.TabIndex = 0;
      this.dropdown.SelectedIndexChanged += new System.EventHandler(this.dropdown_SelectedIndexChanged);
      // 
      // pictureBox1
      // 
      this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pictureBox1.BackColor = System.Drawing.Color.White;
      this.pictureBox1.Location = new System.Drawing.Point(0, 25);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(870, 325);
      this.pictureBox1.TabIndex = 1;
      this.pictureBox1.TabStop = false;
      // 
      // Score
      // 
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.dropdown);
      this.Controls.Add(this.pictureBox1);
      this.Name = "Score";
      this.Size = new System.Drawing.Size(870, 350);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.ResumeLayout(false);

    }

    private void dropdown_SelectedIndexChanged(object sender, EventArgs e)
    {
      var info = (ScoreInfo)dropdown.SelectedItem;
      if (info.Type == "Tablature")
        SetNotation(_song.GetTablature(info.Track), _song.GetNotationData(info.TrackName, info.Type), info.Track);
      else
        SetNotation(_song.GetNotation(info.Track), _song.GetNotationData(info.TrackName, info.Type), info.Track);
    }
  }
}