using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using PCLStorage;

namespace Jammit.Model
{
  //TODO: Deprecate in favor of ILibrary.
  public static class Library
  {
    private static readonly string CacheFileName = "contentCache.xml";

    private static Dictionary<Guid, SongMeta> _cache;

    public static IFileSystem FileSystem;

    private static Dictionary<Guid, SongMeta> InitCache()
    {
      var cache = new Dictionary<Guid, SongMeta>();

      SongMeta[] songs =
      {
        new SongMeta { Artist = "Artist 2" , Album = "Album 4", Name = "Song 2", Instrument = "Instrument 2", Type = "Type 1", ContentGuid = Guid.NewGuid() },
        new SongMeta { Artist = "Artist 2" , Album = "Album 4", Name = "Song 2", Instrument = "Instrument 3", Type = "Type 2", ContentGuid = Guid.NewGuid() }
      };

      foreach (var song in songs)
      {
        cache.Add(song.ContentGuid, song);
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

      var foundTracks = new Dictionary<Guid, SongMeta>();

      var tasksDir = FileSystem.LocalStorage.CreateFolderAsync("Tracks", CreationCollisionOption.OpenIfExists).Result;
      var dirsTask = tasksDir.GetFoldersAsync();
      var dirs = dirsTask.Result;
      foreach (var dir in dirs)
      {
        try
        {
          var guid = Guid.Parse(dir.Name);
          if (foundTracks.ContainsKey(guid))
            continue;

          var plistTask = dir.GetFileAsync("info.plist");
          var plist = plistTask.Result;
          using (var reader = new StreamReader(plist.OpenAsync(FileAccess.Read).Result))
          {
            var xml = XDocument.Parse(reader.ReadToEnd());
            SongMeta track = SongMeta.FromPlist(xml, guid, "Folder", dir.Path);

            foundTracks[track.ContentGuid] = track;
            _cache.Add(guid, track);
          } // using xml reader
        }
        catch (Exception)
        {
          throw;
        }
      } // for each dir
    }

    /// <summary>
    /// Get all thet songs in the cache.
    /// </summary>
    /// <returns></returns>
    public static List<SongMeta> GetSongs()
    {
      if (_cache == null)
      {
        _cache = InitCache();
        UpdateCache();
      }

      return _cache.Values.ToList();
    }

    public static void RemoveSong(Guid id)
    {
      _cache.Remove(id);
    }
  }
}
