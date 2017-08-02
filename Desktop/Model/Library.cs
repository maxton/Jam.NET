using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Jammit.Model
{
  public static class Library
  {
    private static readonly string CacheFileName = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Jam.NET", // Must match root  namespace
        "contentCache.xml");

    private static Dictionary<Guid, SongMeta> _cache;

    private static Dictionary<Guid, SongMeta> InitCache()
    {
      var cache = new Dictionary<Guid, SongMeta>();
      if (!File.Exists(CacheFileName)) return cache;

      try
      {
        using (var stream = File.OpenRead(CacheFileName))
        {
          var doc = XDocument.Load(stream);
          foreach (var t in doc.Element("songs").Elements())
          {
            var track = SongMeta.FromXml(t);
            cache[track.ContentGuid] = track;
          }
        }
      }
      catch (Exception e)
      {
        // Do nothing. We'll just rebuild the cache file if this fails.
        Console.WriteLine(e.Message);
        Console.WriteLine(e.StackTrace);
      }
      return cache;
    }

    public static void ResetCache()
    {
      _cache = new Dictionary<Guid, SongMeta>();
      UpdateCache();
    }

    /// <summary>
    /// Checks the configured track folder for new tracks, and removes tracks from the cache
    /// that no longer exist.
    /// </summary>
    public static void UpdateCache()
    {
      if (_cache == null) _cache = InitCache();


      if (!Directory.Exists(Properties.Settings.Default.TrackPath))
      {
        System.Windows.Forms.MessageBox.Show("Error! You have chosen an invalid content folder.\r\n" +
                        "Please select a valid content folder.");
        return;
      }

      var foundTracks = new Dictionary<Guid, SongMeta>();
      XDocument cacheDoc = new XDocument(new XElement("songs"));
      var tracksEl = cacheDoc.Element("songs");

      foreach (var t in _cache)
      {
        var exists = t.Value.SongPath.StartsWith(Properties.Settings.Default.TrackPath)
          && ((t.Value.Type == "Zip" && File.Exists(t.Value.SongPath))
                     || (t.Value.Type == "Folder" && Directory.Exists(t.Value.SongPath)));
        if (!exists) continue;

        foundTracks[t.Key] = t.Value;
        tracksEl.Add(t.Value.ToXml());
      }

      // Search for ZIP files.
      var files = Directory.GetFiles(Properties.Settings.Default.TrackPath, "*.zip");
      foreach (var file in files)
      {
        // Ensure filename is a GUID string
        if (Path.GetFileName(file)?.Length != 40) continue;
        try
        {
          var guid = Guid.Parse(Path.GetFileName(file).Substring(0, 36));
          if (foundTracks.ContainsKey(guid)) continue;
          var newTrack = SongMeta.FromZip(file);
          foundTracks[newTrack.ContentGuid] = newTrack;
          tracksEl.Add(newTrack.ToXml());
        }
        catch (Exception e)
        {
          Console.WriteLine(file);
          Console.WriteLine(e.Message);
        }
      }

      // Search for JCF directories.
      var dirs = Directory.GetDirectories(Properties.Settings.Default.TrackPath, "*-*-*-*-*");
      foreach (var dir in dirs)
      {

        if (Path.GetFileName(dir)?.Length < 36) continue;
        try
        {
          var guid = Guid.Parse(Path.GetFileName(dir).Substring(0, 36));
          if (foundTracks.ContainsKey(guid)) continue;
          if (!File.Exists(Path.Combine(dir, "info.plist"))) continue;
          using (var reader = new StreamReader(new FileStream(Path.Combine(dir, "info.plist"), FileMode.Open)))
          {
            var newTrack = SongMeta.FromPlist(XDocument.Parse(reader.ReadToEnd()), guid, "Folder", dir);
            foundTracks[newTrack.ContentGuid] = newTrack;
            tracksEl.Add(newTrack.ToXml());
          }
        }
        catch (Exception e)
        {
          Console.WriteLine(dirs);
          Console.WriteLine(e.Message);
        }
      }

      string cacheDir = Path.GetDirectoryName(CacheFileName);
      if (!Directory.Exists(cacheDir))
        Directory.CreateDirectory(cacheDir);

      using (var s = File.OpenWrite(CacheFileName))
      {
        s.SetLength(0);
        cacheDoc.Save(s);
      }
      _cache = foundTracks;
    }

    /// <summary>
    /// Get all thet songs in the cache.
    /// </summary>
    /// <returns></returns>
    public static List<SongMeta> GetSongs()
    {
      if (_cache == null) _cache = InitCache();

      return _cache.Values.ToList();
    }

    public static bool SongExists(Guid guid)
    {
      if (_cache == null) _cache = InitCache();
      return _cache.ContainsKey(guid);
    }
  }
}
