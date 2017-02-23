using System;
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

    public ZipSong(SongMeta metadata)
    {
      Metadata = metadata;
      var tracks = new List<Track>();
      InitTracks(tracks);
      Tracks = tracks;
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
      using (var ms = new MemoryStream())
      {
        stream.CopyTo(ms);
        return Image.FromStream(ms);
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
      return new JammitZipSongPlayer(this);
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

    public ZipArchive OpenZip() => ZipFile.OpenRead(Metadata.ZipFileName);
  }
}
