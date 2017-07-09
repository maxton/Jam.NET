using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using PCLStorage;
using Jammit.Model;

namespace Jammit.Mobile
{
  public partial class App : Application
  {
    private static Client.IClient client;

    //TODO: Deprecate, or research how to initialize resources (FileSystem, etc).
    public App()
    {
      InitializeComponent();

      MainPage = new Jammit.Mobile.MainPage();
    }

    public App(IFileSystem fileSystem)
    {
      InitializeComponent();

      Library.FileSystem = fileSystem;
      client = new Client.RestClient();
      MainPage = new Jammit.Mobile.MainPage();
    }

    #region Properties

    public static Client.IClient Client => client;

    #endregion

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
