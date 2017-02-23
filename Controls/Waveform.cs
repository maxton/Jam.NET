using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.MediaFoundation;

namespace Jammit.Controls
{
  class Waveform : UserControl
  {
    public double TimeSeconds { get; set; } = 0.0;
    public long PositionSamples { get; set; } = 0;
    public sbyte[] WaveData;

    private readonly Pen _nowPen = new Pen(Color.FromArgb(0x80, 0xFF, 0xFF, 0xFF), 2.0f);

    public Waveform()
    {
      DoubleBuffered = true;
    }

    protected override void OnPaint(PaintEventArgs pe)
    {
      if (WaveData != null)
      {
        pe.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
        var pen = new Pen(ForeColor);
        var ratio = Height / 256.0f;
        var halfWidth = Width/2;
        var halfHeight = Height/2;

        // This is the center point.
        // 1 pixel = 1 waveform entry
        var currentSampleIdx = (int)(PositionSamples/2048);

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

      // Draw cursor
      pe.Graphics.DrawLine(_nowPen, Width/2.0f, 0, Width/2.0f, Height);
      base.OnPaint(pe);
    }
  }
}
