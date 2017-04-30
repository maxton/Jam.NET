using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Jammit.Model;

namespace Jammit.Mobile
{
  public partial class MainPage : ContentPage
  {
    public MainPage()
    {
      InitializeComponent();

      UpdateListView();
    }

    //public static List<SongMeta> Songs => Library.GetSongs();
    public static SongMeta[] Songs =
    {
      new SongMeta { Artist = "Artist 1" , Album = "Album 1", Name = "Song 1", Instrument = "Instrument 1", Type = "Type 0", ContentGuid = Guid.NewGuid() },
      new SongMeta { Artist = "Artist 1" , Album = "Album 1", Name = "Song 1", Instrument = "Instrument 2", Type = "Type 1", ContentGuid = Guid.NewGuid() },
      new SongMeta { Artist = "Artist 1" , Album = "Album 1", Name = "Song 1", Instrument = "Instrument 3", Type = "Type 2", ContentGuid = Guid.NewGuid() },
      new SongMeta { Artist = "Artist 1" , Album = "Album 2", Name = "Song 2", Instrument = "Instrument 1", Type = "Type 0", ContentGuid = Guid.NewGuid() },
      new SongMeta { Artist = "Artist 1" , Album = "Album 2", Name = "Song 2", Instrument = "Instrument 2", Type = "Type 1", ContentGuid = Guid.NewGuid() },
      new SongMeta { Artist = "Artist 1" , Album = "Album 2", Name = "Song 2", Instrument = "Instrument 3", Type = "Type 2", ContentGuid = Guid.NewGuid() },
      new SongMeta { Artist = "Artist 2" , Album = "Album 3", Name = "Song 1", Instrument = "Instrument 1", Type = "Type 0", ContentGuid = Guid.NewGuid() },
      new SongMeta { Artist = "Artist 2" , Album = "Album 3", Name = "Song 1", Instrument = "Instrument 2", Type = "Type 1", ContentGuid = Guid.NewGuid() },
      new SongMeta { Artist = "Artist 2" , Album = "Album 3", Name = "Song 1", Instrument = "Instrument 3", Type = "Type 2", ContentGuid = Guid.NewGuid() },
      new SongMeta { Artist = "Artist 2" , Album = "Album 4", Name = "Song 2", Instrument = "Instrument 1", Type = "Type 0", ContentGuid = Guid.NewGuid() },
      new SongMeta { Artist = "Artist 2" , Album = "Album 4", Name = "Song 2", Instrument = "Instrument 2", Type = "Type 1", ContentGuid = Guid.NewGuid() },
      new SongMeta { Artist = "Artist 2" , Album = "Album 4", Name = "Song 2", Instrument = "Instrument 3", Type = "Type 2", ContentGuid = Guid.NewGuid() }
    };

    private void UpdateListView()
    {
      var tracks = Library.GetSongs();
      tracks.Sort((t1, t2) => t1.Artist.CompareTo(t2.Artist) * 10 + t1.Name.CompareTo(t2.Name));
    }
  }
}
