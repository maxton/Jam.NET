using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Jammit.Controls
{
  public class Score : PictureBox
  {
    public double TimeSeconds { get; set; } = 0.0;
    private List<Image> _imgs;

    protected override void OnResize(EventArgs e)
    {
      Invalidate();
      base.OnResize(e);
    }

    public void SetImages(List<Image> imgs)
    {
      if (imgs == null) return;
      _imgs = imgs;
      if (imgs.Count == 0) return;
      Image = _imgs[0];
      Invalidate();
    }

    protected override void OnPaint(PaintEventArgs pe)
    {
      if(Image != null)
        pe.Graphics.TranslateTransform((Width-Image.Width)/2, 0);
      base.OnPaint(pe);
    }
  }
}