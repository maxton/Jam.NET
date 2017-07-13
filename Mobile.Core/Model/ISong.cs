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

    //TODO: Beats

    //TODO: Sections

    sbyte[] GetWaveForm();

    Image GetCover();

    List<Image> GetNotation(Track t);

    List<Image> GetTablature(Track t);

    // GetNotationData

    Stream GetContentStream(string path);

    Stream GetSeekableContentStream(string path);

    ISongPlayer2 GetSongPlayer();
  }
}
