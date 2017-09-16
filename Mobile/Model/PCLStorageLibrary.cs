using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using PCLStorage;

namespace Jammit.Model
{
  class PCLStorageLibrary : ILibrary
  {
    private const string LibraryFilePath = "library.xml";

    private IFolder storage;

    private IFile libraryFile;

    private IDictionary<Guid, SongInfo> cache;

    public PCLStorageLibrary(IFileSystem fileSystem)
    {
      storage = fileSystem.LocalStorage;

      // If library file doesn't exist, initialize.
      var fileExistsTask = Task.Run(async () => await storage.CheckExistsAsync(LibraryFilePath));
      fileExistsTask.Wait();

      if (ExistenceCheckResult.FileExists != fileExistsTask.Result)
      {
        libraryFile = storage.CreateFileAsync(LibraryFilePath, CreationCollisionOption.FailIfExists).Result;
        using (var stream = libraryFile.OpenAsync(FileAccess.ReadAndWrite).Result)
        {
          var xdoc = new XDocument(new XElement("songs"));
          xdoc.Save(stream);
        }
      }
      else
      {
        libraryFile = storage.GetFileAsync(LibraryFilePath).Result;
      }// library file exists?

      InitCache();
    }

    private void InitCache()
    {
      cache = new Dictionary<Guid, SongInfo>();

      using (var libraryStream = libraryFile.OpenAsync(FileAccess.Read).Result)
      {
        var xdoc = XDocument.Load(libraryStream);
        foreach (var element in xdoc.Element("songs").Elements())
        {
          var song = FromXml(element);
          cache[song.Id] = song;
        }
      }
    }

    private SongInfo FromXml(XElement e)
    {
      return new SongInfo
      (
        Guid.Parse(e.Attribute("id").Value),
        e.Element("artist").Value,
        e.Element("album").Value,
        e.Element("title").Value,
        e.Element("instrument").Value,
        e.Element("genre").Value
      );
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
      libraryFile.WriteAllTextAsync("").Wait();
      using (var stream = libraryFile.OpenAsync(FileAccess.ReadAndWrite).Result)
      {
        //Hack: clear file
        //TODO: Remove
        var xdoc = new XDocument(new XElement("songs"));
        foreach (var item in cache)
        {
          var element = ToXml(item.Value);
          xdoc.Element("songs").Add(element);
        }

        xdoc.Save(stream);
      }
    }

    #region ILibrary methods

    public void AddSong(SongInfo song)
    {
      if (Mobile.Settings.SkipDownload)
      {
        cache[song.Id] = song;
        Save();
      }
      else
      {
        // Download the file.
        //TODO: Uncomment once a way to unzip (PCL) is found.
        var downloadTask = Task.Run(async () => await Mobile.App.Client.DownloadSong(song.Id));
        downloadTask.Wait();

        var downloadsDir = storage.CreateFolderAsync("Downloads", CreationCollisionOption.OpenIfExists).Result;
        var fileTask = downloadsDir.CreateFileAsync($"{song.Id}.zip", CreationCollisionOption.ReplaceExisting);
        using (var stream = fileTask.Result.OpenAsync(FileAccess.ReadAndWrite).Result)
        {
          downloadTask.Result.CopyTo(stream);
        }
      }
    }

    public List<SongInfo> GetSongs()
    {
      return cache.Values.ToList();
    }

    public void RemoveSong(Guid id)
    {
      cache.Remove(id);
      Save();
    }

    #endregion // ILibrary members
  }
}
