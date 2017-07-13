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
  public class PCLStorageSong : ISong
  {
    // list scorenodes notationData

    public PCLStorageSong(SongInfo metadata)
    {
      Metadata = metadata;
      Tracks = InitTracks();
      // Beats
      // Sections
      // notationData
    }

    private List<Track> InitTracks()
    {
      throw new NotImplementedException();
    }

    //InitBeats

    // InitSections

    // InitScoreNodes

    #region ISong members

    public SongInfo Metadata { get; }

    public IReadOnlyList<Track> Tracks { get; }

    public Stream GetContentStream(string path)
    {
      throw new NotImplementedException();
    }

    public Image GetCover()
    {
      throw new NotImplementedException();
    }

    public List<Image> GetNotation(Track t)
    {
      throw new NotImplementedException();
    }

    public Stream GetSeekableContentStream(string path)
    {
      throw new NotImplementedException();
    }

    public ISongPlayer2 GetSongPlayer()
    {
      throw new NotImplementedException();
    }

    public List<Image> GetTablature(Track t)
    {
      throw new NotImplementedException();
    }

    public sbyte[] GetWaveForm()
    {
      throw new NotImplementedException();
    }

    #endregion

  }
}
