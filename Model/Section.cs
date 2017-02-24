namespace Jammit.Model
{
  public class Section
  {
    public int BeatIdx;
    public Beat Beat;
    public int Number;
    public int Type;
    public string Name => TypeToString(Type) + (Number > 0 ? " " + Number : "");

    public static string TypeToString(int type)
    {
      if (type >= 0 && type < TypeStrings.Length)
        return TypeStrings[type];
      return "Unknown";
    }

    private static readonly string[] TypeStrings =
    {
      "Count In", "Intro", "Verse", "Pre-Chorus", "Chorus",
      "Solo", "Bridge", "Outro", "Break", "End", "Post-Chorus",
      "Instrumental", "Re Intro", "B-Section", "Riff", "Breakdown",
      "Pre-Intro", "Interlude", "Refrain", "Pre-Verse", "Free Time",
      "Part", "Reprise", "C-Section", "Ending", "Guitar Solo", "Keyboard Solo",
      "Bass Solo", "Drum Solo", "Fill", "Fade Out", "Fade In", "Post Fade",
      "Post Solo", "Post Break", "Count Off", "Transition", "Modulation",
      "Prelude", "Ad Lib", "Middle Eight", "Vamp", "Theme", "Hook", "Figure",
      "Motif", "Piece", "Cycle", "Section"
    };
  }
}