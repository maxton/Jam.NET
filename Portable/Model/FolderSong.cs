using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Claunia.PropertyList;

using Jammit.Audio;

namespace Jammit.Model
{
  public class FolderSong : ISong
  {
    private string _tracksPath = Path.Combine(Library.FileSystem.LocalStorage.Path, "Tracks");
    private string _songPath;
    private List<ScoreNodes> _notationData;

    public FolderSong(SongInfo metadata)
    {
      Metadata = metadata;

      _songPath = Path.Combine(_tracksPath, $"{metadata.Id.ToString().ToUpper()}.jcf");

      Tracks = InitTracks();
      Beats = InitBeats();
      Sections = InitSections();
      _notationData = InitScoreNodes();
    }

    private List<TrackInfo> InitTracks()
    {
      var result = new List<TrackInfo>();
      var tracksArray = PropertyListParser.Parse(Path.Combine(_songPath, "tracks.plist")) as NSArray;
      foreach (var track in tracksArray)
      {
        var dict = track as NSDictionary;
        if (dict == null)
          continue;
       
        Guid id = Guid.Parse(dict.String("identifier"));
        string classs = dict.String("class");

        switch (classs)
        {
          case "JMEmptyTrack":
            result.Add(new EmptyTrackInfo()
            {
              Identifier = id
            });
            break;

          case "JMFileTrack":
            var source = new FileTrackInfo
            {
              Identifier = id,
              Title = dict.String("title"),
              ScoreSystemHeight = (uint)dict.Int("scoreSystemHeight"),
              ScoreSystemInterval = (uint)dict.Int("scoreSystemInterval")
            };
            var notationPages = Directory.GetFiles(_songPath, $"{id.ToString().ToUpper()}_jcfn_??").Length;
            var tablaturePages = Directory.GetFiles(_songPath, $"{id.ToString().ToUpper()}_jcft_??").Length;
            if (notationPages + tablaturePages > 0)
            {
              result.Add(new NotatedTrackInfo(source)
              {
                NotationPages = (uint)notationPages,
                TablaturePages = (uint)tablaturePages
              });
            }
            else
            {
              result.Add(source);
            }
            break;

          default:
            if (dict.Count == 3)
              result.Add(new ConcreteTrackInfo()
              {
                Class = classs,
                Identifier = id,
                Title = dict.String("title")
              });
            else if (dict.Count == 2)
              result.Add(new EmptyTrackInfo
              {
                Identifier = id
              });
            break;
        }
      }

      return result;
    }

    private List<Beat> InitBeats()
    {
      var result = new List<Beat>();
      var ghostArray = PropertyListParser.Parse(Path.Combine(_songPath, "ghost.plist")) as NSArray;
      var beatArray = PropertyListParser.Parse(Path.Combine(_songPath, "beats.plist")) as NSArray;

      for (int i = 0; i < beatArray.Count(); i++)
      {
        var beatDict = beatArray[i] as NSDictionary;
        var ghostDict = ghostArray[i] as NSDictionary;
        result.Add(new Beat(
          beatDict.Double("position") ?? 0,
          beatDict.Bool("isDownBeat") ?? false,
          ghostDict.Bool("isGhostBeat") ?? false
        ));
      }

      return result;
    }

    private List<Section> InitSections()
    {
      var sectionArray = (NSArray)PropertyListParser.Parse(Path.Combine(_songPath, "sections.plist"));
      return sectionArray.OfType<NSDictionary>().Select(dict => new Section
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

    #region ISong members

    public SongInfo Metadata { get; }

    public IReadOnlyList<TrackInfo> Tracks { get; }

    public IReadOnlyList<Beat> Beats { get; }

    public IReadOnlyList<Section> Sections { get; }

    public Stream GetContentStream(string path) => GetSeekableContentStream(path);

    public Stream GetSeekableContentStream(string s)
    {
      return File.OpenRead(Path.Combine(_songPath, s));
    }

    public ISongPlayer GetSongPlayer()
    {
      return new MockSongPlayer(this);
    }

    public Stream GetCover()
    {
      return File.OpenRead(Path.Combine(_songPath, "cover.jpg"));
    }

    public List<Stream> GetNotation(TrackInfo t)
    {
      var result = new List<Stream>();
      var notated = t as NotatedTrackInfo;

      foreach (var file in Directory.GetFiles(_songPath, $"{t.Identifier.ToString().ToUpper()}_jcfn_??"))
      {
        result.Add(File.OpenRead(file));
      }

      return result;
    }

    public List<Stream> GetTablature(TrackInfo t)
    {
      var result = new List<Stream>();
      var notated = t as TrackInfo;

      foreach (var file in Directory.GetFiles(_songPath, $"{t.Identifier.ToString().ToUpper()}_jcft_??"))
      {
        result.Add(File.OpenRead(file));
      }

      return result;
    }

    public ScoreNodes GetNotationData(string trackName, string notationType)
    {
      return _notationData.FirstOrDefault(score => trackName == score.Title && notationType == score.Type);
    }

    public sbyte[] GetWaveForm()
    {
      var path = Path.Combine(_songPath, "music.waveform");
      using (var fs = new FileStream(path, FileMode.Open))
      using (var ms = new MemoryStream())
      {
        fs.CopyTo(ms);
        return new UnionArray { Bytes = ms.ToArray() }.Sbytes;
      }
    }

    #endregion
  }
}
