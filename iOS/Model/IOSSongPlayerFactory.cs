using System;

using Jammit.Audio;
using Jammit.Audio.iOS;
using Jammit.Model;

namespace Jammit.Model.iOS
{
  class IOSSongPlayerFactory : ISongPlayerFactory
  {
    public ISongPlayer CreateSongPlayer(ISong song)
    {
      return new IOSSongPlayer(song);
    }
  }
}
