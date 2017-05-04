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
      UpdateListView();

      this.FilesPath.Text = Library.FileSystem.LocalStorage.Path;
    }

    public static List<SongMeta> Songs => Library.GetSongs();

    private void UpdateListView()
    {
      Songs.Sort((t1, t2) => t1.Artist.CompareTo(t2.Artist) * 10 + t1.Name.CompareTo(t2.Name));
    }

    private void LibraryView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
      Navigation.PushModalAsync(new SongPage(e.Item as SongMeta));
    }
  }
}
