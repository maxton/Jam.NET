using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jammit.Model
{
  public class SongMeta2
  {
    public SongMeta2(Guid id, string artist, string album, string title, string instrument, string genre)
    {
      Id = id;
      Artist = artist;
      Album = album;
      Title = title;
      Instrument = instrument;
      Genre = genre;
    }

    public Guid Id { get; private set; }
    public string Artist { get; private set; }
    public string Album { get; private set; }
    public string Title { get; private set; }
    public string Instrument { get; private set; }
    public string Genre { get; private set; }
  }
}
