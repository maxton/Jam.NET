using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using PCLStorage;

namespace Jammit.Model
{
  class DefaultLibrary : ILibrary
  {
    private const string LibraryFilePath = "library.xml";

    private IFolder storage;

    private IFile libraryFile;

    private IDictionary<Guid, SongMeta2> cache;

    public DefaultLibrary(IFileSystem fileSystem)
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
          var doc = new XDocument(new XElement("songs"));
          doc.Save(stream);
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
      cache = new Dictionary<Guid, SongMeta2>();

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

    private SongMeta2 FromXml(XElement e)
    {
      return new SongMeta2
      (
        Guid.Parse(e.Attribute("id").Value),
        e.Element("artist").Value,
        e.Element("album").Value,
        e.Element("title").Value,
        e.Element("instrument").Value,
        e.Element("genre").Value
      );
    }

    public XElement ToXml(SongMeta2 song)
    {
      return new XElement("song",
        new XAttribute("id", song.Id.ToString().ToUpper()),
        new XElement("artist", song.Artist),
        new XElement("album", song.Album),
        new XElement("title", song.Title),
        new XElement("instrument", song.Instrument),
        new XElement("genre", song.Genre));
    }

    #region ILibrary methods

    public void AddSong(SongMeta2 song)
    {
      using (var stream = libraryFile.OpenAsync(FileAccess.ReadAndWrite).Result)
      {
        var xdoc = XDocument.Load(stream);
        var element = ToXml(song);
        xdoc.Element("songs").Add(element);
        xdoc.Save(stream);
      }

      cache[song.Id] = song;
    }

    public List<SongMeta2> GetSongs()
    {
      return cache.Values.ToList();
    }

    public void RemoveSong(Guid id)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
