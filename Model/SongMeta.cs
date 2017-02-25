﻿using System;
using System.Xml.Linq;
using System.IO;
using System.IO.Compression;
using System.Xml.XPath;

namespace Jammit.Model
{
  /// <summary>
  /// Metadata about a song
  /// </summary>
  public class SongMeta
  {
    public string Name;
    public string Artist;
    public string Album;
    public string Instrument;
    public Guid ContentGuid;
    public string GuidString => ContentGuid.ToString().ToUpper();
    public string ZipFileName => Path.Combine(Properties.Settings.Default.TrackPath, GuidString + ".zip");
    public string DirName => Path.Combine(Properties.Settings.Default.TrackPath, GuidString + ".jcf");

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

    public static SongMeta FromPlist(XDocument plist, Guid id)
    {

      return new SongMeta
      {
        Name = plist.XPathSelectElement("/plist/dict/key[.=\'title\']/following-sibling::string[1]").Value,
        Artist = plist.XPathSelectElement("/plist/dict/key[.=\'artist\']/following-sibling::string[1]").Value,
        Album = plist.XPathSelectElement("/plist/dict/key[.=\'album\']/following-sibling::string[1]").Value,
        Instrument = InstrumentCode(plist.XPathSelectElement("/plist/dict/key[.=\'instrument\']/following-sibling::integer[1]").Value),
        ContentGuid = id
      };
    }

    public static SongMeta FromZip(string zipFileName)
    {
      using (var a = ZipFile.OpenRead(zipFileName))
      {
        Guid id = Guid.Empty;
        foreach (var e in a.Entries)
          if (e.FullName.EndsWith(".jcf/"))
          {
            id = Guid.Parse(e.FullName.Substring(0, 36));
            break;
          }
        using (var reader = new StreamReader(a.GetEntry(id.ToString("D").ToUpper() + ".jcf/info.plist").Open()))
        {
          return FromPlist(XDocument.Parse(reader.ReadToEnd()), id);
        }
      }
    }
    public static SongMeta FromXml(XElement n)
    {
      return new SongMeta
      {
        ContentGuid = Guid.Parse(n.Attribute("id").Value),
        Name = n.Element("name").Value,
        Artist = n.Element("artist").Value,
        Album = n.Element("album").Value,
        Instrument = n.Element("instrument").Value
      };
    }

    public XElement ToXml()
    {
      return new XElement("song",
        new XAttribute("id", ContentGuid.ToString()),
        new XElement("name", Name),
        new XElement("artist", Artist),
        new XElement("album", Album),
        new XElement("instrument", Instrument));
    }
  }
}
