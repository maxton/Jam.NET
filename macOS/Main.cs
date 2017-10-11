using AppKit;

namespace Jammit.macOS
{
  static class MainClass
  {
    static void Main(string[] args)
    {
      NSApplication.Init();
      //https://blog.xamarin.com/preview-bringing-macos-to-xamarin-forms/
      NSApplication.SharedApplication.Delegate = new AppDelegate();
      NSApplication.Main(args);
    }
  }
}
