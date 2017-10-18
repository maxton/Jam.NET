using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using PCLStorage;
using Jammit.Audio;
using Jammit.Model;

namespace Jammit.Portable
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

      MainPage = new Jammit.Portable.MainPage();
    }

    public App(IFileSystem fileSystem, Func<ISong, ISongPlayer> songPlayerFactory)
    {
      InitializeComponent();

      client = new Client.RestClient();
      library = new FolderLibrary(fileSystem.LocalStorage.Path);
      App.fileSystem = fileSystem;
      App.SongPlayerFactory = songPlayerFactory;

      MainPage = new Jammit.Portable.MainPage();
    }

    public App(IFileSystem fileSystem) : this(fileSystem, (s) => { return new MockSongPlayer(s); }) {}

    public App(IFileSystem fileSystem, ISongPlayerFactory playerFactory)
    {
      InitializeComponent();

      client = new Client.RestClient();
      library = new FolderLibrary(fileSystem.LocalStorage.Path);
      App.fileSystem = fileSystem;
      App.playerFactory = playerFactory;

      MainPage = new Jammit.Portable.MainPage();
    }

    #region Properties

    public static Client.IClient Client => client;

    public static ILibrary Library => library;

    public static IFileSystem FileSystem => fileSystem;

    public static Func<ISong, ISongPlayer> SongPlayerFactory { get; private set; }

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
