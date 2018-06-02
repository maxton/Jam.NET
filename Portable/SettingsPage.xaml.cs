using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Jammit.Portable
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class SettingsPage : ContentPage
  {
    public SettingsPage()
    {
      InitializeComponent();
    }

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
      //Hack: Manually flushing settings.
      //TODO: Replace with tow-way binding.
      Settings.ServiceUri = ServiceUriEntry.Text;

      Navigation.PopModalAsync();
    }

    private void AuthorizeButton_Clicked(object sender, EventArgs e)
    {
      App.Client.RequestAuthorization().Wait();
    }

    private void SkipDownloadSwitch_Toggled(object sender, ToggledEventArgs e)
    {
      //TODO: Fix Two-way binding and remove statement below.
      Settings.SkipDownload = SkipDownloadSwitch.IsToggled;
    }
  }
}