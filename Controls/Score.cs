using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Jammit.Model;

namespace Jammit.Controls
{
  public class Score : UserControl, ISynchronizable
  {
    public long SamplePosition { get; set; }
    public long Samples { get; set; }

    private List<Image> _imgs;
    private IReadOnlyList<BeatInfo> _nodes;
    private IReadOnlyList<Beat> _beats;
    private int _beatNum;
    private Track _track;
    private Pen _line;

    protected override void OnResize(EventArgs e)
    {
      Invalidate();
      base.OnResize(e);
    }

    public void SetNotation(List<Image> imgs, ScoreNodes nodes, IReadOnlyList<Beat> beats, Track track)
    {
      DoubleBuffered = true;
      if (imgs == null) return;
      _imgs = imgs;
      if (imgs.Count == 0) return;
      _nodes = nodes.Nodes;
      _beats = beats;
      _track = track;
      _line = new Pen(Color.FromArgb(0x7F00FF00), 3);
      Invalidate();
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
        if (_nodes[_beatNum].Row != _nodes[_beatNum + 1].Row)
        {
          var timeDiff = _beats[_beatNum + 1].Time - _beats[_beatNum].Time;
          var xDiff = _nodes[_beatNum].X - _nodes[_beatNum - 1].X;
          var timeFromBeat = SamplePosition / 44100.0 - _beats[_beatNum].Time;
          return (int)(xDiff * timeFromBeat / timeDiff / 1.5);
        }
        else
        {
          var timeDiff = _beats[_beatNum + 1].Time - _beats[_beatNum].Time;
          var xDiff = _nodes[_beatNum + 1].X - _nodes[_beatNum].X;
          var timeFromBeat = SamplePosition/44100.0 - _beats[_beatNum].Time;
          return (int) (xDiff*timeFromBeat/timeDiff);
        }
      }
      return 0;
    }

    protected override void OnPaint(PaintEventArgs pe)
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
          pe.Graphics.DrawImageUnscaled(_imgs[img], xOffset, -y + 1024*img);
          if(_imgs.Count > img + 1)
            pe.Graphics.DrawImageUnscaled(_imgs[img+1], xOffset, -y + _imgs[img].Height + 1024*img);
        }
        pe.Graphics.DrawLine(_line, x, 0, x, _track.ScoreSystemHeight);
      }

      base.OnPaint(pe);
    }
  }
}