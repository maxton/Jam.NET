using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jammit.Model
{
  public interface ILibrary
  {
    List<SongMeta2> GetSongs();

    void AddSong(SongMeta2 song);

    void RemoveSong(Guid id);
  }
}
