using System.Collections.Generic;
using System.IO;

using Jammit.Audio;
using Xamarin.Forms;

namespace Jammit.Model
{
  public interface ISong
  {
    /// <summary>
    /// The metadata for this song.
    /// </summary>
    SongInfo Metadata { get; }

    /// <summary>
    /// A list of all the tracks in this song.
    /// </summary>
    IReadOnlyList<TrackInfo> Tracks { get; }

    /// <summary>
    /// A list of the beats in this song, in order.
    /// </summary>
    IReadOnlyList<Beat> Beats { get; }

    /// <summary>
    /// A list of the sections in this song, in order.
    /// </summary>
    IReadOnlyList<Section> Sections { get; }

    /// <summary>
    /// Returns the waveform data for this song.
    /// </summary>
    sbyte[] GetWaveForm();

    /// <summary>
    /// Returns the cover artwork for this song.
    /// </summary>
    Image GetCover();

    /// <summary>
    /// Returns the notation images for this song, in order.
    /// </summary>
    /// <param name="t">Track to get notation for.</param>
    List<Image> GetNotation(TrackInfo t);

    /// <summary>
    /// Returns the tablature images for this song, in order.
    /// </summary>
    /// <param name="t">Track to get tablature for.</param>
    List<Image> GetTablature(TrackInfo t);

    ScoreNodes GetNotationData(string trackName, string notationType);

    /// <summary>
    /// Returns a stream to the file at the given path in this song's content folder.
    /// The returned stream is readable, but not necessarily writable or seekable.
    /// </summary>
    /// <param name="path">Path within the content file. i.e.: info.plist</param>
    Stream GetContentStream(string path);

    /// <summary>
    /// Returns a stream to the file at the given path in this song's content folder.
    /// The returned stream is seekable and readable, but not necessarily writable.
    /// </summary>
    /// <param name="path">Path within the content file. i.e.: info.plist</param>
    Stream GetSeekableContentStream(string path);

    /// <summary>
    /// Returns a Song Player that can play back the audio for this song.
    /// </summary>
    ISongPlayer2 GetSongPlayer();
  }
}
