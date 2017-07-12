using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// JMClickTrack, JMInputTrack
/// </summary>
namespace Jammit.Model
{
  public class TrackInfo
  {
    #region Properties

    public virtual string Class { get; set; }

    public Guid Identifier { get; set; }

    public string Title { get; set; }

    #endregion
  }

  /// <summary>
  /// JMFileTrack 
  /// </summary>
  public class FileTrackInfo : TrackInfo
  {
    public override string Class => "JMFileTrack";

    public uint ScoreSystemHeight { get; set; }

    public uint ScoreSystemInterval { get; set; }
  }
}
