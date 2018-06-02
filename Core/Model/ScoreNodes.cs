using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Jammit.Model
{
  public class ScoreNodes
  {
    public readonly string Title;
    public readonly string Type;
    public readonly IReadOnlyList<BeatInfo> Nodes;

    private ScoreNodes(string t, string y, IReadOnlyList<BeatInfo> nodes)
    {
      Title = t;
      Type = y;
      Nodes = nodes;
    }

    public static List<ScoreNodes> FromStream(Stream s)
    {
      var list = new List<ScoreNodes>();
      var numTracks = s.ReadInt32LE();
      byte[] temp = new byte[32];
      for (var i = 0; i < numTracks; i++)
      {
        s.Read(temp, 0, 32);
        var title = Encoding.UTF8.GetString(temp, 0, 32);
        title = title.Remove(title.IndexOf('\0'));
        s.Read(temp, 0, 32);
        var type = Encoding.UTF8.GetString(temp, 0, 32);
        type = type.Remove(type.IndexOf('\0'));
        var nodes = BeatInfo.FromStream(s);
        list.Add(new ScoreNodes(title, type, nodes));
      }
      return list;
    }
  }
  public class BeatInfo
  {
    public readonly ushort Measure;
    public readonly ushort Row;
    public readonly float X;

    private BeatInfo(ushort measure, ushort row, float x)
    {
      Measure = measure;
      Row = row;
      X = x;
    }

    /// <summary>
    /// Reads nowline nodes.
    /// </summary>
    /// <param name="s">Stream seeked to numBeats</param>
    /// <returns>List of nowline nodes</returns>
    public static List<BeatInfo> FromStream(Stream s)
    {
      var list = new List<BeatInfo>();
      int numNodes = s.ReadInt32LE();
      for (int i = 0; i < numNodes; i++)
      {
        list.Add(new BeatInfo(s.ReadUInt16LE(), s.ReadUInt16LE(), s.ReadFloat()));
      }
      return list;
    }
  }
}