using System;
using System.Windows.Forms;

namespace Jammit.Controls
{
  public class Score : PictureBox
  {
    public double TimeSeconds { get; set; } = 0.0;
    protected override void OnResize(EventArgs e)
    {
      Invalidate();
      base.OnResize(e);
    }

    protected override void OnPaint(PaintEventArgs pe)
    {
      if(Image != null)
        pe.Graphics.TranslateTransform((Width-Image.Width)/2, 0);
      base.OnPaint(pe);
    }
  }
}