using System;

using Jammit.Audio;

namespace Jammit.Model
{
  public class MockSongPlayerFactory : ISongPlayerFactory
  {
    public MockSongPlayerFactory()
    {
    }

    public ISongPlayer CreateSongPlayer(ISong song)
    {
      return new MockSongPlayer(song);
    }
  }
}
