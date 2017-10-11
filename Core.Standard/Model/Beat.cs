using System.Collections.Generic;
using Claunia.PropertyList;

namespace Jammit.Model
{
  public class Beat
  {
    public readonly double Time;
    public readonly bool IsDownBeat;
    public readonly bool IsGhostBeat;

    public Beat(double time, bool isDb, bool isGb)
    {
      Time = time;
      IsDownBeat = isDb;
      IsGhostBeat = isGb;
    }

    public static List<Beat> FromNSArrays(NSArray beatArray, NSArray ghostArray)
    {
      var beats = new List<Beat>();
      for (var i = 0; i < beatArray.Count; i++)
      {
        var dict = beatArray.GetArray()[i] as NSDictionary;
        beats.Add(new Beat(dict.Double("position") ?? 0,
          dict.Bool("isDownbeat") ?? false,
          (ghostArray.GetArray()[i] as NSDictionary).Bool("isGhostBeat") ?? false));
      }
      return beats;
    }
  }
}
