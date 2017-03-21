using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Jammit.Helpers
{
  /// <summary>
  /// This is the Settings static class that can be used in your Core solution or in any
  /// of your client applications. All settings are laid out the same exact way with getters
  /// and setters. 
  /// </summary>
  public static class Settings
  {
    private static ISettings AppSettings
    {
      get
      {
        return CrossSettings.Current;
      }
    }

    private static string[] COLORS = { "FF0000", "00FF00", "0000FF" };

    #region Setting Constants

    private const string SettingsKey = "settings_key";
    private static readonly string SettingsDefault = string.Empty;

    private const string TrackPathKey = "trackpath_key";
    private static readonly string TrackPathDefault = ".";

    private const string DummyIndexKey = "dummyindex_key";
    private static readonly int DummyIndexDefault = 0;

    #endregion

    public static string GeneralSettings
    {
      get
      {
        return AppSettings.GetValueOrDefault<string>(SettingsKey, SettingsDefault);
      }
      set
      {
        AppSettings.AddOrUpdateValue<string>(SettingsKey, value);
      }
    }

    public static string TrackPath
    {
      get { return AppSettings.GetValueOrDefault<string>(TrackPathKey, TrackPathDefault); }
      set { AppSettings.AddOrUpdateValue<string>(TrackPathKey, value); }
    }

    public static string Dummy
    {
      get
      {
        int index = AppSettings.GetValueOrDefault<int>(DummyIndexKey);
        AppSettings.AddOrUpdateValue<int>(DummyIndexKey, (index + 1) % COLORS.Length);

        return COLORS[index];
      }
    }
  }
}