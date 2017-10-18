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
    public App(IFileSystem fileSystem, Func<ISong, ISongPlayer> songPlayerFactory)
    {
      InitializeComponent();

      App.Client = new Client.RestClient();
      App.Library = new FolderLibrary(fileSystem.LocalStorage.Path);
      App.FileSystem = fileSystem;
      App.SongPlayerFactory = songPlayerFactory;

      MainPage = new Jammit.Portable.MainPage();
    }

    public App(IFileSystem fileSystem) : this(fileSystem, (s) => { return new MockSongPlayer(s); }) {}

    #region Properties

    public static Client.IClient Client { get; private set; }

    public static ILibrary Library { get; private set; }

    public static IFileSystem FileSystem { get; private set; }

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
