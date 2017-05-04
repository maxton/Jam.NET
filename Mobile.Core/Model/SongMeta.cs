using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Jammit.Model
{
  /// <summary>
  /// Metadata about a song
  /// </summary>
  public class SongMeta
  {
    //TODO: Made this fields properties for Xaml binding. Reconcile with 'Desktop Core'.
    public string Name;
    public string Title => Name;
    public string Artist { get; set; }
    public string Album { get; set; }
    public string Instrument { get; set; }
    public Guid ContentGuid;

    /// <summary>
    /// One of: Zip, Folder
    /// </summary>
    public string Type;

    /// <summary>
    /// Full path to Zip or Folder
    /// </summary>
    public string SongPath;
    public string GuidString => ContentGuid.ToString().ToUpper();

    private static string InstrumentCode(string code)
    {
      switch (code)
      {
        case "0":
          return "Guitar";
        case "1":
          return "Bass";
        case "2":
          return "Drums";
        case "3":
          return "Keyboard";
        case "4":
          return "Vocals";
      }
      return "Unknown";
    }

    public static SongMeta FromPlist(XDocument plist, Guid id, string type, string path)
    {
      var dict =  (plist.Elements().First().FirstNode as XElement);
      var keys = dict.Elements("key");
      SongMeta track = new SongMeta
      {
        ContentGuid = id,
        Type = type,
        SongPath = path,
        Instrument = InstrumentCode(dict.Element("dict").Element("integer").Value)
    };

      int attributeCount = 0;
      foreach (var key in keys)
      {
        switch (key.Value)
        {
          case "title":
            track.Name = (key.NextNode as XElement).Value; attributeCount++; break;
          case "artist":
            track.Artist = (key.NextNode as XElement).Value; attributeCount++; break;
          case "album":
            track.Album = (key.NextNode as XElement).Value; attributeCount++; break;

          default:
            break;
        }
      }

      return track;
    }

    public static SongMeta FromZip(string zipFileName)
    {
      //using (var a = ZipFile.OpenRead(zipFileName))
      //{
      //  Guid id = Guid.Empty;
      //  string entryPath = null;
      //  foreach (var e in a.Entries)
      //  {
      //    if (e.FullName.EndsWith(".jcf/") || Regex.IsMatch(e.FullName, @".*-.*-.*-.*/"))
      //    {
      //      id = Guid.Parse(e.FullName.Substring(0, 36));
      //      entryPath = e.FullName;
      //      break;
      //    }
      //  }
      //  using (var reader = new StreamReader(a.GetEntry(entryPath + "info.plist").Open()))
      //  {
      //    return FromPlist(XDocument.Parse(reader.ReadToEnd()), id, "Zip", zipFileName);
      //  }
      //}
      return default(SongMeta);
    }

    public static SongMeta FromXml(XElement n)
    {
      return new SongMeta
      {
        ContentGuid = Guid.Parse(n.Attribute("id").Value),
        Name = n.Element("name").Value,
        Artist = n.Element("artist").Value,
        Album = n.Element("album").Value,
        Instrument = n.Element("instrument").Value,
        Type = n.Attribute("type").Value,
        SongPath = n.Element("path").Value
      };
    }

    public XElement ToXml()
    {
      return new XElement("song",
        new XAttribute("id", ContentGuid.ToString()),
        new XAttribute("type", Type),
        new XElement("name", Name),
        new XElement("artist", Artist),
        new XElement("album", Album),
        new XElement("instrument", Instrument),
        new XElement("path", SongPath));
    }
  }
}
