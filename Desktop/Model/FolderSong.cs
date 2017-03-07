using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;

using Claunia.PropertyList;
using Jammit.Audio;

namespace Jammit.Model
{
  /// <summary>
  /// Represents a song in a raw file sistem folder.
  /// </summary>
  class FolderSong : ISong
  {
    public SongMeta Metadata { get; }

    public IReadOnlyList<Track> Tracks { get; }

    public IReadOnlyList<Beat> Beats { get; }

    public IReadOnlyList<Section> Sections { get; }

    private List<ScoreNodes> _notationData;

    public FolderSong(SongMeta metadata)
    {
      Metadata = metadata;
      Tracks = InitTracks();
      Beats = InitBeats();
      Sections = InitSections();
      _notationData = InitScoreNodes();
    }

    public sbyte[] GetWaveform()
    {
      var path = Path.Combine(Metadata.SongPath, "music.waveform");
      using (var fs = new FileStream(path, FileMode.Open))
      using (var ms = new MemoryStream())
      {
        fs.CopyTo(ms);
        return new UnionArray { Bytes = ms.ToArray() }.Sbytes;
      }
    }

    public Image GetCover()
    {
      var path = Path.Combine(Metadata.SongPath, "cover.jpg");
      using (var fs = new FileStream(path, FileMode.Open))
      using (var ms = new MemoryStream())
      {
        fs.CopyTo(ms);
        return Image.FromStream(ms);
      }
    }

    public List<Image> GetNotation(Track t)
    {
      if (!t.HasNotation) return null;
      var ret = new List<Image>();
      for (var i = 0; i < t.NotationPages; i++)
        ret.Add(Image.FromFile(Path.Combine(Metadata.SongPath, $"{t.Id}_jcfn_{i:D2}")));

      return ret;
    }

    public List<Image> GetTablature(Track t)
    {
      if (!t.HasTablature) return null;
      var ret = new List<Image>();
      for (var i = 0; i < t.NotationPages; i++)
        ret.Add(Image.FromFile(Path.Combine(Metadata.SongPath, $"{t.Id}_jcft_{i:D2}")));

      return ret;
    }

    public ScoreNodes GetNotationData(string trackName, string notationType)
    {
      return _notationData.FirstOrDefault(score => trackName == score.Title && notationType == score.Type);
    }

    public ISongPlayer GetSongPlayer()
    {
      return new JammitNAudioSongPlayer(this);
    }

    public Stream GetContentStream(string s) => GetSeekableContentStream(s);
    public Stream GetSeekableContentStream(string s)
    {
      return File.OpenRead(Path.Combine(Metadata.SongPath, s));
    }

    private List<Track> InitTracks()
    {
      var tracksArray = (NSArray)PropertyListParser.Parse(Path.Combine(Metadata.SongPath, "tracks.plist"));
      return Track.FromNSArray(tracksArray, path => File.Exists(Path.Combine(Metadata.SongPath, path)));
    }

    private List<Beat> InitBeats()
    {
      var beatArray = (NSArray)PropertyListParser.Parse(Path.Combine(Metadata.SongPath, "beats.plist"));
      var ghostArray = (NSArray)PropertyListParser.Parse(Path.Combine(Metadata.SongPath, "ghost.plist"));
      return Beat.FromNSArrays(beatArray, ghostArray);
    }

    private List<Section> InitSections()
    {
      var sectionArray = (NSArray)PropertyListParser.Parse(Path.Combine(Metadata.SongPath, "sections.plist"));
      return sectionArray.GetArray().OfType<NSDictionary>().Select(dict => new Section
      {
        BeatIdx = dict.Int("beat") ?? 0,
        Beat = Beats[dict.Int("beat") ?? 0],
        Number = dict.Int("number") ?? 0,
        Type = dict.Int("type") ?? 0
      }).ToList();
    }

    private List<ScoreNodes> InitScoreNodes()
    {
      using (var nodes = GetContentStream("nowline.nodes"))
      {
        return ScoreNodes.FromStream(nodes);
      }
    }
  }
}
