using System;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Xml.Linq;
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

      return new SongMeta
      {
        Name = plist.XPathSelectElement("/plist/dict/key[.=\'title\']/following-sibling::string[1]").Value,
        Artist = plist.XPathSelectElement("/plist/dict/key[.=\'artist\']/following-sibling::string[1]").Value,
        Album = plist.XPathSelectElement("/plist/dict/key[.=\'album\']/following-sibling::string[1]").Value,
        Instrument = InstrumentCode(plist.XPathSelectElement("/plist/dict/key[.=\'instrument\']/following-sibling::integer[1]").Value),
        ContentGuid = id,
        Type = type,
        SongPath = path
      };
    }

    public static SongMeta FromZip(string zipFileName)
    {
      using (var a = ZipFile.OpenRead(zipFileName))
      {
        Guid id = Guid.Empty;
        string entryPath = null;
        foreach (var e in a.Entries)
        {
          if (e.FullName.EndsWith(".jcf/") || Regex.IsMatch(e.FullName, @".*-.*-.*-.*/"))
          {
            id = Guid.Parse(e.FullName.Substring(0, 36));
            entryPath = e.FullName.Substring(0, e.FullName.IndexOf('/') + 1);
            break;
          }
        }
        using (var reader = new StreamReader(a.GetEntry(entryPath + "info.plist").Open()))
        {
          return FromPlist(XDocument.Parse(reader.ReadToEnd()), id, "Zip", zipFileName);
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
