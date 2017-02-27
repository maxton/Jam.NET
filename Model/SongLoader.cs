using System;

namespace Jammit.Model
{
  public static class SongLoader
  {
    public static ISong Load(SongMeta meta)
    {
      switch (meta.Type)
      {
        case "Zip":
          return new ZipSong(meta);
        case "Folder":
          return new FolderSong(meta);
        default:
          throw new Exception("Unhandled song type: " + meta.Type);
      }
    }
  }
}