using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jammit.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Jammit.Mobile
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class SongPage : ContentPage
  {
    private SongMeta _song;

    public SongPage(SongMeta song)
    {
      InitializeComponent();
    }

    private void SongPage_Close(object sender, EventArgs e)
    {
      Navigation.PopModalAsync();
    }
  }
}
