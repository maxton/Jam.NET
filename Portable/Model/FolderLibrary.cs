using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using PCLStorage;
using Xamarin.Forms;

namespace Jammit.Model
{
  class FolderLibrary : BindableObject, ILibrary, System.ComponentModel.INotifyPropertyChanged
  {
    
    public static readonly BindableProperty SongsProperty =
      BindableProperty.Create(
        "Songs",
        typeof(List<SongInfo>),
        typeof(List<SongInfo>),
        new List<SongInfo>() {
          new SongInfo() { Album = "Album1", Artist = "Artist1", Instrument = "Instrument1", Title = "Title1" } },
        BindingMode.TwoWay);

    private const string LibraryFileName = "library.xml";

    private string _storagePath;
    private string _libraryPath;
    private IDictionary<Guid, SongInfo> _cache;

    public FolderLibrary(string storagePath)
    {
      _storagePath = storagePath;
      _libraryPath = Path.Combine(_storagePath, LibraryFileName);

      // If library doesn't exist, initialize.
      if (!File.Exists(_libraryPath))
        new XDocument(new XElement("songs")).Save(_libraryPath);

      InitCache();
    }

    private void InitCache()
    {
      _cache = new Dictionary<Guid, SongInfo>();

      using (var stream = File.OpenRead(_libraryPath))
      {
        var doc = XDocument.Load(stream);
        foreach (var xe in doc.Element("songs").Elements())
        {
          var song = FromXml(xe);
          _cache[song.Id] = song;
        }
      }

      Songs = _cache.Values.ToList();
    }

    private SongInfo FromXml(XElement xe)
    {
      return new SongInfo
      {
        Id = Guid.Parse(xe.Attribute("id").Value),
        Artist = xe.Element("artist").Value,
        Album = xe.Element("album").Value,
        Title = xe.Element("title").Value,
        Instrument = xe.Element("instrument").Value,
        Genre = xe.Element("genre").Value
      };
    }

    public XElement ToXml(SongInfo song)
    {
      return new XElement("song",
        new XAttribute("id", song.Id.ToString().ToUpper()),
        new XElement("artist", song.Artist),
        new XElement("album", song.Album),
        new XElement("title", song.Title),
        new XElement("instrument", song.Instrument),
        new XElement("genre", song.Genre));
    }

    private void Save()
    {
      File.WriteAllText(_libraryPath, string.Empty);
      using (var stream = File.OpenWrite(_libraryPath))
      {
        //Hack: clear file
        //TODO: Remove???
        var xdoc = new XDocument(new XElement("songs"));
        foreach (var song in _cache.Values)
        {
          xdoc.Element("songs").Add(ToXml(song));
        }

        xdoc.Save(stream);
      }
    }

    #region ILibrary methods

    public void AddSong(SongInfo song)
    {
      if (!Portable.Settings.SkipDownload)
      {
        // Download th efile.
        var downloadTask = Task.Run(async () => await Portable.App.Client.DownloadSong(song.Id));
        downloadTask.Wait();

        // Make sure Tracks and Downloads dirs exist.
        var downloadsDir = Directory.CreateDirectory(Path.Combine(_storagePath, "Downloads"));
        var tracksDir = Directory.CreateDirectory(Path.Combine(_storagePath, "Tracks"));

        // Extract downloaded ZIP contents.
        var zipPath = Path.Combine(downloadsDir.FullName, $"{song.Id}.zip");
        using (var zipStream = File.Create(zipPath))
        {
          downloadTask.Result.CopyTo(zipStream);
        }
        ZipFile.ExtractToDirectory(zipPath, tracksDir.FullName);

        _cache[song.Id] = song;
        Songs = _cache.Values.ToList();

        Save();

        // Cleanup
        if (!Portable.Settings.SkipDownload)
          File.Delete(zipPath);
      }
      else
      {
        _cache[song.Id] = song;
        Songs = _cache.Values.ToList();

        Save();
      }
    }

    public List<SongInfo> GetSongs()
    {
      return _cache.Values.ToList();
    }

    public List<SongInfo> Songs
    {
      get
      {
        return (List<SongInfo>)GetValue(SongsProperty);
      }

      private set
      {
        value.Sort( (s1, s2) => s1.Artist.CompareTo(s2.Artist) * 10 + s1.Title.CompareTo(s2.Title) );
        SetValue(SongsProperty, value);
      }
    }

    public void RemoveSong(Guid id)
    {
      try
      {
        //File.Delete(Path.Combine(_storagePath, "Tracks", $"{id}.jcf"));
        // TODO: Work around System.IO file access issues on delete.
        //       Then, remove dependency on PCLStorage.
        var songTask = FileSystem.Current.LocalStorage.GetFolderAsync("Tracks").Result.GetFolderAsync($"{id}.jcf");
        songTask.Result.DeleteAsync();
      }
      catch (Exception)
      {
        //TODO: Handle exception.
      }

      _cache.Remove(id);
      Songs = _cache.Values.ToList();

      Save();
    }

    #endregion
  }
}
