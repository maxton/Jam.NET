using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Jammit.Model;

namespace Jammit.Controls
{
  class Waveform : UserControl
  {
    public double TimeSeconds { get; set; } = 0.0;
    public long PositionSamples { get; set; } = 0;
    public sbyte[] WaveData;
    public IReadOnlyList<Section> Sections;

    private readonly Pen _nowPen = new Pen(Color.FromArgb(0x80, 0xFF, 0xFF, 0xFF), 2.0f);
    private readonly Pen _sectionPen = Pens.White;

    public Waveform()
    {
      DoubleBuffered = true;
    }

    protected override void OnPaint(PaintEventArgs pe)
    {
      var ratio = Height / 256.0f;
      var halfWidth = Width / 2;
      var halfHeight = Height / 2;

      // This is the center point.
      // 1 pixel = 1 waveform entry
      var currentSampleIdx = (int)(PositionSamples / 1024);

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
          pe.Graphics.DrawLine(pen, x, (128+WaveData[sample])*ratio, x, (128+WaveData[sample + 1])*ratio);
        }
      }

      // Draw section markers
      if (Sections != null)
      {
        var secondsPerPixel = 1024 / 44100.0;
        var minTime = (currentSampleIdx - Width) * secondsPerPixel;
        var maxTime = (currentSampleIdx + halfWidth) * secondsPerPixel;
        foreach (var section in Sections)
        {
          if (section.Beat.Time > minTime && section.Beat.Time < maxTime)
          {
            var x = (float)(section.Beat.Time / secondsPerPixel) - currentSampleIdx + halfWidth;
            pe.Graphics.DrawLine(_sectionPen, x, 0, x, Height);
            pe.Graphics.DrawString(section.Name, Font, Brushes.White, x, 0);
          }
        }
      }

      // Draw cursor
      pe.Graphics.DrawLine(_nowPen, Width/2.0f, 0, Width/2.0f, Height);
      base.OnPaint(pe);
    }
  }
}
