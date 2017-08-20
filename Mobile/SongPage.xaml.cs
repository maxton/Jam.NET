using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jammit.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Jammit.Mobile
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class SongPage : ContentPage
  {
    /// <summary>
    /// Score information (Instrument track name and type).
    /// </summary>
    public struct ScoreInfo
    {
      public string TrackName;
      public string Type;
      public TrackInfo Track;
      public override string ToString() => $"{TrackName} - {Type}";// TrackName + " - " + Type;
    }

    public SongInfo Song { get; set; }

    public ISong SongContents { get; set; }

    public List<ScoreInfo> ScoresInfo { get; private set; }

    public SongPage(SongInfo song)
    {
      BindingContext = this; // Needed to actually bind local properties.
      Song = song;
      SongContents = new PCLStorageSong(song);

      ScoresInfo = new List<ScoreInfo>();
      var tracks = SongContents.Tracks.Where(t => t is NotatedTrackInfo);
      foreach (var track in tracks)
      {
        var notated = track as NotatedTrackInfo;
        if (notated.NotationPages > 0)
          ScoresInfo.Add(new ScoreInfo { TrackName = notated.Title, Type = "Score", Track = notated });
        if (notated.TablaturePages > 0)
          ScoresInfo.Add(new ScoreInfo { TrackName = notated.Title, Type = "Tablature", Track = notated });
      }

      InitializeComponent();
    }

    private void SongPage_Close(object sender, EventArgs e)
    {
      Navigation.PopModalAsync();
    }
  }
}
