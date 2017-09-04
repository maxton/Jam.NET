using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jammit.Audio;

namespace Jammit.Model
{
  public interface ISongPlayerFactory
  {
    ISongPlayer2 CreateSongPlayer(ISong song);
  }
}
