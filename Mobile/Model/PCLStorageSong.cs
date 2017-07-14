using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jammit.Audio;
using PCLStorage;
using Xamarin.Forms;

namespace Jammit.Model
{
  public class PCLStorageSong : ISong
  {
    private List<ScoreNodes> _notationData;
    private IFolder _songDir;

    public PCLStorageSong(SongInfo metadata)
    {
      Metadata = metadata;
      Tracks = InitTracks();
      Beats = InitBeats();
      Sections = InitSections();
      _notationData = InitScoreNodes();
      _songDir = Mobile.App.FileSystem.LocalStorage
        .GetFolderAsync(Path.Combine("Tracks", $"{metadata.Id}.jcf")).Result;
    }

    private List<Track> InitTracks()
    {
      throw new NotImplementedException();
    }

    private List<Beat> InitBeats()
    {
      throw new NotImplementedException();
    }

    private List<Section> InitSections()
    {
      throw new NotImplementedException();
    }

    private List<ScoreNodes> InitScoreNodes()
    {
      throw new NotImplementedException();
    }

    #region ISong members

    public SongInfo Metadata { get; }

    public IReadOnlyList<Track> Tracks { get; }

    public IReadOnlyList<Beat> Beats { get; }

    public IReadOnlyList<Section> Sections { get; }

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

    public ScoreNodes GetNotationData(string trackName, string notation)
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
