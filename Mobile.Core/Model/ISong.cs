using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jammit.Audio;
using Xamarin.Forms;

namespace Jammit.Model
{
  public interface ISong
  {
    SongInfo Metadata { get; }

    IReadOnlyList<Track> Tracks { get; }

    IReadOnlyList<Beat> Beats { get; }

    IReadOnlyList<Section> Sections { get; }

    sbyte[] GetWaveForm();

    Image GetCover();

    List<Image> GetNotation(Track t);

    List<Image> GetTablature(Track t);

    ScoreNodes GetNotationData(string trackName, string notationType);

    Stream GetContentStream(string path);

    Stream GetSeekableContentStream(string path);

    ISongPlayer2 GetSongPlayer();
  }
}
