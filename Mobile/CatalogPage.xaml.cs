using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Jammit.Model;

namespace Jammit.Mobile
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class CatalogPage : ContentPage
  {
    public static List<SongMeta2> Catalog { get; private set; }

    public CatalogPage()
    {
      if (Catalog == null)
        LoadCatalog();

      InitializeComponent();
    }

    private void LoadCatalog()
    {
      // Schedule catalog loading and wait synchronously.
      var catalogTask = Task.Run(async () => await App.Client.LoadCatalog());
      catalogTask.Wait();

      Catalog = catalogTask.Result;
    }

    private void LoadButton_Clicked(object sender, EventArgs e)
    {
      LoadCatalog();
    }

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
      Navigation.PopModalAsync();
    }

    private void DownloadButton_Clicked(object sender, EventArgs e)
    {
      if (null == CatalogView.SelectedItem)
        return;

      App.Library.AddSong(CatalogView.SelectedItem as SongMeta2);
    }
  }
}