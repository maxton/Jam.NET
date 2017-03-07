using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Jammit.Model;

namespace Jammit.Controls
{
  class Waveform : UserControl, ISynchronizable
  {
    private long _samplePos;

    public long SamplePosition
    {
      get
      {
        return _samplePos;
      }
      set
      {
        _samplePos = value;
        Looper.SamplePosition = value;
      }
    }

    public long Samples { get; set; }

    public sbyte[] WaveData;
    public IReadOnlyList<Section> Sections;
    public IReadOnlyList<Beat> Beats;
    public readonly Looper Looper;

    private readonly Pen _nowPen = new Pen(Color.FromArgb(0x80, 0xFF, 0xFF, 0xFF), 2.0f);
    private readonly Pen _sectionPen = Pens.White;
    private readonly Pen _loopPen = Pens.Red;
    const double SecondsPerPixel = 1024 / 44100.0;

    private MouseState _mouse;

    public Waveform()
    {
      Looper = new Looper();
      MouseDown += OnMouseDown;
      DoubleBuffered = true;
    }

    private float TimeToX(double time) => (float)(time / SecondsPerPixel) - (int)(SamplePosition/1024) + Width/2;
    private double XToTime(int x) => (x + (int) (SamplePosition/1024 - Width/2))*SecondsPerPixel;

    private double BeatSnap(double time)
    {
      return Beats.OrderBy(b => Math.Abs(b.Time - time)).First().Time;
    }

    private enum MouseState
    {
      None,
      MovingLoopStart,
      MovingLoopEnd,
      MovingWaveform
    }

    private void OnMouseDown(object target, MouseEventArgs me)
    {
      MouseMove += OnMouseMove;
      MouseUp += OnMouseUp;
      if (Looper.Enabled && Math.Abs(me.X - TimeToX(Looper.LoopStart)) < 6)
      {
        _mouse = MouseState.MovingLoopStart;
      }
      else if (Looper.Enabled && Math.Abs(me.X - TimeToX(Looper.LoopEnd)) < 6)
      {
        _mouse = MouseState.MovingLoopEnd;
      }
      else
      {
        _mouse = MouseState.None;
      }
    }

    private void OnMouseUp(object target, MouseEventArgs me)
    {
      MouseMove -= OnMouseMove;
      MouseUp -= OnMouseUp;
      _mouse = MouseState.None;
    }

    private void OnMouseMove(object target, MouseEventArgs me)
    {
      switch (_mouse)
      {
        case MouseState.MovingLoopStart:
          Looper.LoopStart = BeatSnap(XToTime(me.X));
          break;
        case MouseState.MovingLoopEnd:
          Looper.LoopEnd = BeatSnap(XToTime(me.X));
          break;
      }
    }

    protected override void OnPaint(PaintEventArgs pe)
    {
      var ratio = Height/256.0f;
      var halfWidth = Width/2;
      var halfHeight = Height/2;

      // This is the center point.
      // 1 pixel = 1 waveform entry
      var currentSampleIdx = (int) (SamplePosition/1024);

      // Draw Wave Data
      if (WaveData != null)
      {
        pe.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
        var pen = new Pen(ForeColor);

        // Start drawing at 0, unless the current sample is less than halfWidth
        var waveStart = Math.Max(0, halfWidth - currentSampleIdx);
        var waveEnd = Math.Min(Width, WaveData.Length/2 - currentSampleIdx + halfWidth);

        // Draw zero line
        pe.Graphics.DrawLine(pen, waveStart, halfHeight, waveEnd, halfHeight);
        for (var x = waveStart; x < waveEnd; x++)
        {
          var sample = 2*(currentSampleIdx - halfWidth + x);
          pe.Graphics.DrawLine(pen, x, (128 + WaveData[sample])*ratio, x, (128 + WaveData[sample + 1])*ratio);
        }
      }

      // Draw section markers
      if (Sections != null)
      {
        var minTime = (currentSampleIdx - Width)*SecondsPerPixel;
        var maxTime = (currentSampleIdx + halfWidth)*SecondsPerPixel;
        foreach (var section in Sections)
        {
          if (section.Beat.Time > minTime && section.Beat.Time < maxTime)
          {
            var x = TimeToX(section.Beat.Time);
            pe.Graphics.DrawLine(_sectionPen, x, 0, x, Height);
            pe.Graphics.DrawString(section.Name, Font, Brushes.White, x, 0);
          }
        }
      }

      // Draw loop points
      if (Looper.Enabled)
      {
        var x = TimeToX(Looper.LoopStart);
        pe.Graphics.DrawLine(_loopPen, x, 0, x, Height);
        pe.Graphics.DrawString("Start", Font, Brushes.Red, x, Height - 20);
        x = TimeToX(Looper.LoopEnd);
        pe.Graphics.DrawLine(_loopPen, x, 0, x, Height);
        pe.Graphics.DrawString("End", Font, Brushes.Red, x, Height - 20);
      }

      // Draw cursor
      pe.Graphics.DrawLine(_nowPen, Width/2.0f, 0, Width/2.0f, Height);
      base.OnPaint(pe);
    }
  }
}
