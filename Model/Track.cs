using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jammit.Model
{
  class Track
  {
    public string Title;
    public string ClassName;
    public string Id;
    public int ScoreSystemHeight = 0;
    public int ScoreSystemInterval = 0;
    public bool HasNotation = false;
    public bool HasTablature = false;
    public int NotationPages = 0;
    public int TablaturePages = 0;
  }
}
