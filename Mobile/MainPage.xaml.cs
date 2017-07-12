using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Jammit.Model;
using PCLStorage;

namespace Jammit.Mobile
{
  public partial class MainPage : ContentPage
  {
    public MainPage()
    {
      InitializeComponent();

      this.FilesPath.Text = Library.FileSystem.LocalStorage.Path;
    }

    public static List<SongInfo> Songs => App.Library.GetSongs();

    private void UpdateListView()
    {
      LibraryView.ItemsSource = null;
      Songs.Sort((t1, t2) => t1.Artist.CompareTo(t2.Artist) * 10 + t1.Title.CompareTo(t2.Title));
      LibraryView.ItemsSource = Songs;
    }

    private void LibraryView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
      Navigation.PushModalAsync(new SongPage(e.Item as SongMeta));
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
      UpdateListView();
    }

    #endregion
  }
}
