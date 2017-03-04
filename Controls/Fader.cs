using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jammit.Controls
{
  public partial class Fader : UserControl
  {

    public delegate void FaderChangeDelegate(int value);

    public event FaderChangeDelegate OnFaderChange;

    public override string Text
    {
      get { return label1.Text; }
      set { label1.Text = value; }
    }

    public bool Mute
    {
      get { return muteButton.Checked; }
      set { muteButton.Checked = value; }
    }

    public Fader()
    {
      InitializeComponent();
      trackBar1.Scroll += (sender, args) => OnFaderChange?.Invoke(muteButton.Checked ? 0 : trackBar1.Value);
      muteButton.CheckedChanged += (sender, args) => OnFaderChange?.Invoke(muteButton.Checked ? 0 : trackBar1.Value);
    }
  }
}
