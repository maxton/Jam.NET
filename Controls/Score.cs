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

    protected override void OnPaint(PaintEventArgs pe)
    {
      if (_imgs != null && _nodes != null && _beats != null)
      {
        UpdateCurrentBeat();
        var xOffset = (Width - _imgs[0].Width)/2;
        var x = _nodes[_beatNum].X + xOffset;
        var y = _track.ScoreSystemInterval * _nodes[_beatNum].Row;
        var img = y/_imgs[0].Height;

        if (_imgs.Count > img)
        {
          pe.Graphics.DrawImageUnscaled(_imgs[img], xOffset, -y + 1024*img);
          if(_imgs.Count > img + 1)
            pe.Graphics.DrawImageUnscaled(_imgs[img+1], xOffset, -y + _imgs[img].Height + 1024*img);
        }
        pe.Graphics.DrawLine(Pens.Green, x, 0, x, _track.ScoreSystemHeight);
      }

      base.OnPaint(pe);
    }
  }
}