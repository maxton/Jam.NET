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

    public FolderSong(SongMeta metadata)
    {
      Metadata = metadata;
      Tracks = InitTracks();
      Beats = InitBeats();
      Sections = InitSections();
    }

    public sbyte[] GetWaveform()
    {
      var path = string.Format("{0}/{1}.jcf/music.waveform", Properties.Settings.Default.TrackPath, Metadata.GuidString);
      using (var fs = new FileStream(path, FileMode.Open))
      using (var ms = new MemoryStream())
      {
        fs.CopyTo(ms);
        return new UnionArray { Bytes = ms.ToArray() }.Sbytes;
      }
    }

    public Image GetCover()
    {
      var path = string.Format("{0}/{1}.jcf/cover.jpg", Properties.Settings.Default.TrackPath, Metadata.GuidString);
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
        ret.Add(Image.FromFile($"{Properties.Settings.Default.TrackPath}/{Metadata.GuidString}.jcf/{t.Id}_jcfn_{i:D2}"));

      return ret;
    }

    public List<Image> GetTablature(Track t)
    {
      if (!t.HasTablature) return null;
      var ret = new List<Image>();
      for (var i = 0; i < t.NotationPages; i++)
        ret.Add(Image.FromFile($"{Properties.Settings.Default.TrackPath}/{Metadata.GuidString}.jcf/{t.Id}_jcft_{i:D2}"));

      return ret;
    }

    public ISongPlayer GetSongPlayer()
    {
      return new JammitFolderSongPlayer(this);
    }

    private List<Track> InitTracks()
    {
      var tracksArray = (NSArray)PropertyListParser.Parse($"{Properties.Settings.Default.TrackPath}/{Metadata.GuidString}.jcf/tracks.plist");
      return Track.FromNSArray(tracksArray, path => File.Exists($"{Properties.Settings.Default.TrackPath}/{Metadata.GuidString}.jcf/{path}"));
    }

    private List<Beat> InitBeats()
    {
      var beatArray = (NSArray)PropertyListParser.Parse($"{Properties.Settings.Default.TrackPath}/{Metadata.GuidString}.jcf/beats.plist");
      var ghostArray = (NSArray)PropertyListParser.Parse($"{Properties.Settings.Default.TrackPath}/{Metadata.GuidString}.jcf/ghost.plist");
      return Beat.FromNSArrays(beatArray, ghostArray);
    }

    private List<Section> InitSections()
    {
      var sectionArray = (NSArray)PropertyListParser.Parse($"{Properties.Settings.Default.TrackPath}/{Metadata.GuidString}.jcf/sections.plist");
      return sectionArray.GetArray().OfType<NSDictionary>().Select(dict => new Section
      {
        BeatIdx = dict.Int("beat") ?? 0,
        Beat = Beats[dict.Int("beat") ?? 0],
        Number = dict.Int("number") ?? 0,
        Type = dict.Int("type") ?? 0
      }).ToList();
    }
  }
}
