using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Jammit.Mobile
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

    private static string[] COLORS = { "774444", "447744", "444477" };

    #region Setting Constants

    private const string SettingsKey = "settings_key";
    private static readonly string SettingsDefault = string.Empty;

    private const string TrackPathKey = "trackpath_key";
    private static readonly string TrackPathDefault = ".";

    private const string DummyIndexKey = "dummyindex_key";
    private static readonly int DummyIndexDefault = 0;

    private const string SkipDownloadKey = "skipdownload_key";
    private static readonly bool SkipDownloadDefault = false;

    private const string ServiceUriKey = "serviceuri_key";
    private static readonly string ServiceUriDefault = string.Empty;

    #endregion

    public static string GeneralSettings
    {
      get
      {
        return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
      }
      set
      {
        AppSettings.AddOrUpdateValue(SettingsKey, value);
      }
    }

    public static string TrackPath
    {
      get { return AppSettings.GetValueOrDefault(TrackPathKey, TrackPathDefault); }
      set { AppSettings.AddOrUpdateValue(TrackPathKey, value); }
    }

    public static string Dummy
    {
      get
      {
        int index = AppSettings.GetValueOrDefault(DummyIndexKey, DummyIndexDefault);
        AppSettings.AddOrUpdateValue(DummyIndexKey, (index + 1) % COLORS.Length);

        return COLORS[index];
      }
    }

    public static bool SkipDownload
    {
      get { return AppSettings.GetValueOrDefault(SkipDownloadKey, SkipDownloadDefault); }
      set { AppSettings.AddOrUpdateValue(SkipDownloadKey, value); }
    }

    public static string ServiceUri
    {
      get { return AppSettings.GetValueOrDefault(ServiceUriKey, ServiceUriDefault); }
      set { AppSettings.AddOrUpdateValue(ServiceUriKey, value); }
    }
  }
}