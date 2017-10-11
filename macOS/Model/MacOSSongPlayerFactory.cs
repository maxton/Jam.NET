using System;

using Jammit.Audio;
using Jammit.Audio.macOS;
using Jammit.Model;

namespace Jammit.Model.macOS
{
  class MacOSSongPlayerFactory : ISongPlayerFactory
  {
    public ISongPlayer CreateSongPlayer(ISong song)
    {
      return new MacOSSongPlayer(song);
    }
  }
}
