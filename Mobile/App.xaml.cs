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

    private static ILibrary library;

    private static IFileSystem fileSystem;

    private static ISongPlayerFactory playerFactory;

    //TODO: Deprecate, or research how to initialize resources (FileSystem, etc).
    public App()
    {
      InitializeComponent();

      MainPage = new Jammit.Mobile.MainPage();
    }

    public App(IFileSystem fileSystem)
    {
      InitializeComponent();

      Jammit.Model.Library.FileSystem = fileSystem;
      client = new Client.RestClient();
      library = new PCLStorageLibrary(fileSystem);
      App.fileSystem = fileSystem;
      MainPage = new Jammit.Mobile.MainPage();
    }

    public App(IFileSystem fileSystem, ISongPlayerFactory playerFactory) : this(fileSystem)
    {
      App.playerFactory = playerFactory;
    }

    #region Properties

    public static Client.IClient Client => client;

    public static ILibrary Library => library;

    public static IFileSystem FileSystem => fileSystem;

    public static ISongPlayerFactory SongPlayerFactory => playerFactory;

    #endregion

    protected override void OnStart()
    {
      // Handle when your app starts
      MainPage.BackgroundColor = Color.FromHex(Settings.Dummy);
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
