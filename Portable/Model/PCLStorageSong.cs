using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using PCLStorage;
using Xamarin.Forms;

using Jammit.Audio;

/// <summary>
/// Represents a song in an App's local storage folder.
/// </summary>
namespace Jammit.Model
{
  public class PCLStorageSong : ISong
  {
    private List<ScoreNodes> _notationData;
    private IFolder _songDir;

    public PCLStorageSong(SongInfo metadata)
    {
      Metadata = metadata;

      _songDir = Portable.App.FileSystem.LocalStorage
        .GetFolderAsync(Path.Combine("Tracks", $"{metadata.Id}.jcf")).Result;

      Tracks = InitTracks();
      Beats = InitBeats();
      Sections = InitSections();
      _notationData = InitScoreNodes();
    }

    List<TrackInfo> LoadTracks(SongInfo song, XDocument xml)
    {
      // plist/array
      var array = xml.Elements().First().FirstNode as XElement;
      var dicts = array.Elements("dict");

      List<TrackInfo> result = new List<TrackInfo>();
      foreach (var dict in dicts)
      {
        var map = new Dictionary<string, string>();
        var keys = dict.Elements("key");
        foreach (var key in keys)
          map[key.Value] = (key.NextNode as XElement).Value;

        //TODO: Define ITrackLoader?
        if (map.Count == 3)
          result.Add(new ConcreteTrackInfo
          {
            Class = map["class"],
            Identifier = Guid.Parse(map["identifier"]),
            Title = map["title"]
          });
        else if (map.Count == 5)
        {
          var source = new FileTrackInfo
          {
            Identifier = Guid.Parse(map["identifier"]),
            Title = map["title"],
            ScoreSystemHeight = uint.Parse(map["scoreSystemHeight"]),
            ScoreSystemInterval = uint.Parse(map["scoreSystemInterval"])
          };

          uint notationPages = 0;
          uint tablaturePages = 0;
          var files = _songDir.GetFilesAsync().Result;
          foreach (var file in files)
          {
            if (Regex.IsMatch(file.Name, $"{source.Identifier.ToString().ToUpper()}_jcfn_\\d\\d"))
              notationPages++;
            else if (Regex.IsMatch(file.Name, $"{source.Identifier.ToString().ToUpper()}_jcft_\\d\\d"))
              tablaturePages++;
          }

          if (notationPages + tablaturePages > 0)
            result.Add(new NotatedTrackInfo(source)
            {
              NotationPages = notationPages,
              TablaturePages = tablaturePages
            });
          else //TODO:throw?
            result.Add(source);
        }
        else if (map.Count == 2)
          result.Add(new EmptyTrackInfo { Identifier = Guid.Parse(map["identifier"]) });
        else
          throw new Exception("Irregular number of track fields. Expected 3 or 5.");
      } // foreach dict

      return result;
    }

    private List<Beat> LoadBeats(SongInfo song, XDocument beatsXml, XDocument ghostXml)
    {
      List<Beat> result = new List<Beat>();
     
      // plist/array
      var beatsArray = beatsXml.Elements().First().FirstNode as XElement;
      var beatsDicts = beatsArray.Elements("dict");
      var ghostArray = ghostXml.Elements().First().FirstNode as XElement;
      var ghostDicts = ghostArray.Elements("dict");

      using (var beatsEnum = beatsDicts.GetEnumerator())
      using (var ghostEnum = ghostDicts.GetEnumerator())
      {
        while (beatsEnum.MoveNext() && ghostEnum.MoveNext())
        {
          var beatKeys = beatsEnum.Current.Elements("key");
          Dictionary<string, string> map = new Dictionary<string, string>();
          foreach (var beatKey in beatKeys)
          {

          }

          using (var beatKeysEnum = beatsEnum.Current.Elements("key").GetEnumerator())
          using (var ghostKeysEnum = ghostEnum.Current.Elements("key").GetEnumerator())
          {
            var beatMap = new Dictionary<string, string>();
            while (beatKeysEnum.MoveNext())
              beatMap[beatKeysEnum.Current.Value] = (beatKeysEnum.Current.NextNode as XElement).Value;

            var ghostMap = new Dictionary<string, string>();
            while (ghostKeysEnum.MoveNext())
              ghostMap[ghostKeysEnum.Current.Value] = (ghostKeysEnum.Current.NextNode as XElement).Value;

            result.Add(new Beat(
              double.Parse(beatMap["position"]),
              beatMap.ContainsKey("isDownbeat"),  // Assume true if declared
              ghostMap.ContainsKey("isGhostBeat") // Assume true if declared
            ));
          }
        } // while beatsEnum && ghostEnum iterate
      }

      return result;
    }

    private List<Section> LoadSections(XDocument xml)
    {
      // plist/array
      var array = xml.Elements().First().FirstNode as XElement;
      var dicts = array.Elements("dict");

      List<Section> result = new List<Section>();
      foreach (var dict in dicts)
      {
        var map = new Dictionary<string, string>();
        var keys = dict.Elements("key");
        foreach (var key in keys)
          map[key.Value] = (key.NextNode as XElement).Value;

        result.Add(new Section
        {
          BeatIdx = int.Parse(map["beat"]),
          Beat = Beats[int.Parse(map["beat"])],
          Number = int.Parse(map["number"]),
          Type = int.Parse(map["type"])
        });
      }

      return result;
    }

    private IReadOnlyList<TrackInfo> InitTracks()
    {
      var plistFile = _songDir.GetFileAsync("tracks.plist").Result;
      using (var stream = plistFile.OpenAsync(PCLStorage.FileAccess.Read).Result)
      using (var reader = new StreamReader(stream))
      {
        //PropertyListParser.Parse(fs); //meh! FileInfo depends on mscorlib.dll
        return LoadTracks(Metadata, XDocument.Parse(reader.ReadToEnd()));
      }
    }

    private List<Beat> InitBeats()
    {
      var ghostFile = _songDir.GetFileAsync("ghost.plist").Result;
      var beatsFile = _songDir.GetFileAsync("beats.plist").Result;
      using (var beatsStream = beatsFile.OpenAsync(PCLStorage.FileAccess.Read).Result)
      using (var beatsReader = new StreamReader(beatsStream))
      using (var ghostStream = ghostFile.OpenAsync(PCLStorage.FileAccess.Read).Result)
      using (var ghostReader = new StreamReader(ghostStream))
      {
        return LoadBeats(Metadata, XDocument.Parse(beatsReader.ReadToEnd()), XDocument.Parse(ghostReader.ReadToEnd()));
      }
    }

    private List<Section> InitSections()
    {
      var plistFile = _songDir.GetFileAsync("sections.plist").Result;
      using (var stream = plistFile.OpenAsync(PCLStorage.FileAccess.Read).Result)
      using (var reader = new StreamReader(stream))
      {
        return LoadSections(XDocument.Parse(reader.ReadToEnd()));
      }
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

    public Stream GetSeekableContentStream(string path)
    {
      return _songDir.GetFileAsync(path).Result.OpenAsync(PCLStorage.FileAccess.Read).Result;
    }

    public ISongPlayer GetSongPlayer()
    {
      return new MockSongPlayer(this);
    }

    public ImageSource GetCover()
    {
      var stream = _songDir.GetFileAsync("cover.jpg").Result.OpenAsync(PCLStorage.FileAccess.Read).Result;
      return FileImageSource.FromStream(() => { return stream; });
    }

    public List<ImageSource> GetNotation(TrackInfo t)
    {
      var ret = new List<ImageSource>();
      var notated = t as NotatedTrackInfo;

      for (int i = 0; i < notated.NotationPages; i++)
      {
        var imageFile = _songDir.GetFileAsync($"{notated.Identifier}_jcfn_{i:D2}").Result;
        var imageStream = imageFile.OpenAsync(PCLStorage.FileAccess.Read);
        ret.Add(ImageSource.FromStream(() => { return imageStream.Result; }));
      }

      return ret;
    }

    public List<ImageSource> GetTablature(TrackInfo t)
    {
      var ret = new List<ImageSource>();
      var notated = t as NotatedTrackInfo;

      for (int i = 0; i < notated.NotationPages; i++)
      {
        var imageFile = _songDir.GetFileAsync($"{notated.Identifier}_jcft_{i:D2}").Result;
        var imageStream = imageFile.OpenAsync(PCLStorage.FileAccess.Read);
        ret.Add(ImageSource.FromStream(() => { return imageStream.Result; }));
      }

      return ret;
    }

    public ScoreNodes GetNotationData(string trackName, string notationType)
    {
      return _notationData.FirstOrDefault(score => trackName == score.Title && notationType == score.Type);
    }

    public sbyte[] GetWaveForm()
    {
      var fsTask = _songDir.GetFileAsync("music.waveform").Result.OpenAsync(PCLStorage.FileAccess.Read);
      using (var ms = new MemoryStream())
      {
        fsTask.Result.CopyTo(ms);

        return new UnionArray { Bytes = ms.ToArray() }.Sbytes;
      }
    }

    #endregion
  }
}
