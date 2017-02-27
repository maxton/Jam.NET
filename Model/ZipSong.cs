using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Claunia.PropertyList;
using Jammit.Audio;

namespace Jammit.Model
{
  /// <summary>
  /// Represents a song backed with a standard .zip content file.
  /// </summary>
  class ZipSong : ISong
  {
    public SongMeta Metadata { get; }

    public IReadOnlyList<Track> Tracks { get; }
    public IReadOnlyList<Beat> Beats { get; }
    public IReadOnlyList<Section> Sections { get; }

    private List<ScoreNodes> _notationData;

    public ZipSong(SongMeta metadata)
    {
      Metadata = metadata;
      Tracks = InitTracks();
      Beats = InitBeats();
      Sections = InitSections();
      _notationData = InitScoreNodes();
    }

    public sbyte[] GetWaveform()
    {
      using (var a = OpenZip())
      using (var s = a.GetEntry($"{Metadata.GuidString}.jcf/music.waveform").Open())
      using (var ms = new MemoryStream())
      {
        s.CopyTo(ms);
        return new UnionArray { Bytes = ms.ToArray() }.Sbytes;
      }
    }

    public Image GetCover()
    {
      using (var x = OpenZip())
      using (var stream = x.GetEntry($"{Metadata.GuidString}.jcf/cover.jpg").Open())
      {
        return Image.FromStream(stream);
      }
    }

    public List<Image> GetNotation(Track t)
    {
      var ret = new List<Image>();
      if (!t.HasNotation) return null;
      using (var arc = OpenZip())
        for (var i = 0; i < t.NotationPages; i++)
          using (var img = arc.GetEntry($"{Metadata.GuidString}.jcf/{t.Id}_jcfn_{i:D2}").Open())
            ret.Add(Image.FromStream(img));

      return ret;
    }

    public List<Image> GetTablature(Track t)
    {
      var ret = new List<Image>();
      if (!t.HasTablature) return null;
      using (var arc = OpenZip())
        for (var i = 0; i < t.NotationPages; i++)
          using (var img = arc.GetEntry($"{Metadata.GuidString}.jcf/{t.Id}_jcft_{i:D2}").Open())
            ret.Add(Image.FromStream(img));

      return ret;
    }

    public ISongPlayer GetSongPlayer()
    {
      return new JammitNAudioSongPlayer(this);
    }

    public Stream GetContentStream(string s)
    {
      var zip = OpenZip();
      var stream = new OnDisposeStream(zip.GetEntry(Metadata.GuidString + ".jcf/" + s).Open());
      stream.OnDispose += () => zip.Dispose();
      return stream;
    }

    public Stream GetSeekableContentStream(string s)
    {
      using (var x = OpenZip())
      using (var file = x.GetEntry(Metadata.GuidString + ".jcf/" + s).Open())
      {
        var ms = new MemoryStream();
        file.CopyTo(ms);
        return ms;
      }
    }
    public ScoreNodes GetNotationData(string trackName, string notationType)
    {
      return _notationData.FirstOrDefault(score => trackName == score.Title && notationType == score.Type);
    }

    private List<Track> InitTracks()
    {
      using (var x = OpenZip())
      using (var s = x.GetEntry(Metadata.GuidString + ".jcf/tracks.plist").Open())
      {
        var tracksArray = (NSArray)PropertyListParser.Parse(s);
        return Track.FromNSArray(tracksArray, path => x.GetEntry($"{Metadata.GuidString}.jcf/" + path) != null);
      }
    }

    private List<Beat> InitBeats()
    {
      NSArray beatArray, ghostArray;
      using (var arc = OpenZip())
      {
        using (var stream = arc.GetEntry($"{Metadata.GuidString}.jcf/beats.plist").Open())
          beatArray = (NSArray)PropertyListParser.Parse(stream);
        using (var stream = arc.GetEntry($"{Metadata.GuidString}.jcf/ghost.plist").Open())
          ghostArray = (NSArray) PropertyListParser.Parse(stream);
      }
      return Beat.FromNSArrays(beatArray, ghostArray);
    }

    private List<Section> InitSections()
    {
      NSArray sectionArray;
      using (var arc = OpenZip())
      using (var stream = arc.GetEntry($"{Metadata.GuidString}.jcf/sections.plist").Open())
        sectionArray = (NSArray)PropertyListParser.Parse(stream);
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

    public ZipArchive OpenZip() => ZipFile.OpenRead(Metadata.SongPath);
  }
}
