using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Jammit.Mobile
{
  public partial class App : Application
  {
    public App()
    {
      InitializeComponent();

      MainPage = new Jammit.Mobile.MainPage();
    }

    protected override void OnStart()
    {
      // Handle when your app starts
      MainPage.BackgroundColor = Color.FromHex(Helpers.Settings.Dummy);
    }

    protected override void OnSleep()
    {
      // Handle when your app sleeps
    }

    protected override void OnResume()
    {
      // Handle when your app resumes
    }
  }
}
