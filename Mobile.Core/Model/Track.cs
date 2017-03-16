using System;
using System.Collections.Generic;
using Claunia.PropertyList;

namespace Jammit.Model
{
  public class Track
  {
    public readonly string Title;
    public readonly string ClassName;
    public readonly string Id;
    public readonly int ScoreSystemHeight;
    public readonly int ScoreSystemInterval;
    public readonly bool HasNotation;
    public readonly bool HasTablature;
    public readonly int NotationPages;
    public readonly int TablaturePages;

    private Track(NSDictionary dict, Func<string,bool> filePredicate)
    {
      ClassName = dict.String("class");
      Title = dict.String("title") ?? "";
      Id = dict.String("identifier") ?? "";

      if (ClassName != "JMFileTrack") return;
      ScoreSystemHeight = dict.Int("scoreSystemHeight") ?? 0;
      ScoreSystemInterval = dict.Int("scoreSystemInterval") ?? 0;
      if (filePredicate($"{Id}_jcfn_00"))
      {
        HasNotation = true;
        NotationPages = 1;
        while (filePredicate($"{Id}_jcfn_{NotationPages:D2}")) NotationPages++;
      }
      if (filePredicate($"{Id}_jcft_00"))
      {
        HasTablature = true;
        TablaturePages = 1;
        while (filePredicate($"{Id}_jcft_{TablaturePages:D2}")) TablaturePages++;
      }
    }

    public static List<Track> FromNSArray(NSArray a, Func<string,bool> filePredicate)
    {
      var tracks = new List<Track>();
      foreach (var track in a.GetArray())
      {
        var dict = track as NSDictionary;
        if (dict == null) continue;
        tracks.Add(new Track(dict, filePredicate));
      }
      return tracks;
    }
  }
}
