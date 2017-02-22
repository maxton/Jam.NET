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

    public Waveform()
    {
      DoubleBuffered = true;
    }

    protected override void OnPaint(PaintEventArgs pe)
    {
      if (WaveData != null)
      {
        var offset = (int)(PositionSamples/2048) * 2;
        pe.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
        var pen = new Pen(ForeColor);
        var ratio = Height/256.0f;
        var i = 0;
        for (var x = offset; x < offset + 2*Width && x < WaveData.Length; x++)
        {
          pe.Graphics.DrawLine(pen, i, (128+WaveData[x])*ratio, i, (128+WaveData[++x])*ratio);
          i++;
        }
      }
      base.OnPaint(pe);
    }
  }
}
