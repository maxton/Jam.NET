using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Claunia.PropertyList;

namespace Jammit.Model
{
  /// <summary>
  /// Represents a song (backed by a content file).
  /// </summary>
  class Song
  {
    public readonly SongMeta Metadata;

    public readonly IReadOnlyList<Track> Tracks;

    public Song(SongMeta metadata)
    {
      Metadata = metadata;
      var tracks = new List<Track>();
      InitTracks(tracks);
      Tracks = tracks;
    }

    private void InitTracks(List<Track> tracks)
    {
      using (var x = OpenZip())
      {
        using (var s = x.GetEntry(Metadata.GuidString + ".jcf/tracks.plist").Open())
        {
          var tracksArray = (NSArray)PropertyListParser.Parse(s);
          foreach (var track in tracksArray.GetArray())
          {
            var dict = track as NSDictionary;
            if (dict == null) continue;
            var t = new Track
            {
              ClassName = dict["class"].ToString(),
              Title = dict.ContainsKey("title") ? dict["title"].ToString() : "",
              Id = dict["identifier"].ToString()
            };
            if (t.ClassName == "JMFileTrack")
            {
              t.ScoreSystemHeight = dict["scoreSystemHeight"].ToObject() as int? ?? 0;
              t.ScoreSystemInterval = dict["scoreSystemInterval"].ToObject() as int? ?? 0;
              if (x.GetEntry($"{Metadata.GuidString}.jcf/{t.Id}_jcfn_00") != null)
              {
                t.HasNotation = true;
                t.NotationPages = 1;
                while (x.GetEntry($"{Metadata.GuidString}.jcf/{t.Id}_jcfn_{t.NotationPages:D2}") != null) t.NotationPages++;
              }
              if (x.GetEntry($"{Metadata.GuidString}.jcf/{t.Id}_jcft_00") != null)
              {
                t.HasTablature = true;
                t.TablaturePages = 1;
                while (x.GetEntry($"{Metadata.GuidString}.jcf/{t.Id}_jcft_{t.TablaturePages:D2}") != null) t.TablaturePages++;
              }
            }
            tracks.Add(t);
          }
        }
      }
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

    public ZipArchive OpenZip() => ZipFile.OpenRead(Metadata.ZipFileName);
  }
}
