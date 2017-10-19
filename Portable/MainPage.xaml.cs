using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Jammit.Model;
using PCLStorage;

namespace Jammit.Portable
{
  public partial class MainPage : ContentPage
  {
    public MainPage()
    {
      InitializeComponent();

      this.FilesPath.Text = App.FileSystem.LocalStorage.Path;
      this.LibraryView.ItemsSource = App.Library.Songs;
    }

    [Obsolete]
    public static string DeviceId => Plugin.DeviceInfo.CrossDeviceInfo.Current.Id;

    [Obsolete]
    public static string DevicePlatform => Plugin.DeviceInfo.CrossDeviceInfo.Current.Platform.ToString();

    private void UpdateListView()
    {
      //Songs.Sort((t1, t2) => t1.Artist.CompareTo(t2.Artist) * 10 + t1.Title.CompareTo(t2.Title));
    }

    private void LibraryView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
      Navigation.PushModalAsync(new SongPage(e.Item as SongInfo));
    }

    private void CatalogButton_Clicked(object sender, EventArgs e)
    {
      Navigation.PushModalAsync(new CatalogPage());
    }

    private void LibraryItem_Delete(object sender, EventArgs e)
    {
      var song = (sender as MenuItem).BindingContext as SongInfo;
      App.Library.RemoveSong(song.Id);
      UpdateListView();
    }

    private void SettingsButton_Clicked(object sender, EventArgs e)
    {
      Navigation.PushModalAsync(new SettingsPage());
    }

    #region Events

    protected override void OnAppearing()
    {
      base.OnAppearing();
      UpdateListView();
    }

    #endregion
  }
}
