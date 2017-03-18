using Claunia.PropertyList;

namespace Jammit
{
  /// <summary>
  /// Convenience extensions for Plist classes.
  /// </summary>
  public static class PlistExtensions
  {
    public static double? Double(this NSDictionary d, string key)
      => d.ContainsKey(key) ? d[key].ToObject() as double? : null;

    public static int? Int(this NSDictionary d, string key)
      => d.ContainsKey(key) ? d[key].ToObject() as int? : null;

    public static string String(this NSDictionary d, string key)
      => d.ContainsKey(key) ? d[key].ToObject() as string : null;

    public static bool? Bool(this NSDictionary d, string key)
      => d.ContainsKey(key) ? d[key].ToObject() as bool? : null;
  }
}