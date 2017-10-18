using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jammit.Audio;
using Jammit.Model;

namespace Jammit.UWP.Model
{
  class NAudioUniversalSongPlayerFactory : ISongPlayerFactory
  {
    public ISongPlayer CreateSongPlayer(ISong song)
    {
      //return new NAudioUniversalSongPlayer(song);
      return new AudioGraphSongPlayer(song);
    }
  }
}
