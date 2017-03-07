using System;
using System.Windows.Forms;
using Jammit.Model;

namespace Jammit.Controls
{
  public partial class SeekBar : UserControl, ISynchronizable
  {
    public delegate void SliderMoveEvent(int value);
    public delegate void SliderDropEvent(int value);

    public event SliderMoveEvent OnSliderMove;
    public event SliderDropEvent OnSliderDrop;

    public bool Moving { get; private set; }
    public int Value => trackBar1.Value;
    private long _samplePos;
    public long SamplePosition
    {
      get { return _samplePos; }
      set
      {
        _samplePos = value;
        var time = TimeSpan.FromSeconds(_samplePos / 44100.0);
        var len = TimeSpan.FromSeconds(_samples / 44100.0);
        timePos.Text = time.ToString("mm\\:ss");
        timeRemain.Text = "-" + len.Subtract(time).ToString("mm\\:ss");
        if (!Moving)
          trackBar1.Value = _samples == 0 ? 1 : (int)(trackBar1.Maximum * _samplePos / _samples);
      }
    }

    private long _samples;
    public long Samples
    {
      get { return _samples; }
      set
      {
        _samples = value;
        trackBar1.Minimum = 0;
        trackBar1.Maximum = (int) (value/44100.0 + 0.5);
        Invalidate();
      }
    }

    public SeekBar()
    {
      InitializeComponent();
      trackBar1.MouseDown += (o, e) => Moving = true;
      trackBar1.MouseUp += (o, e) =>
      {
        OnSliderDrop?.Invoke(trackBar1.Value);
        Moving = false;
      };
      trackBar1.Scroll += (o, e) => OnSliderMove?.Invoke(trackBar1.Value);
    }

  }
}
