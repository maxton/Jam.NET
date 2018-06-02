﻿using AppKit;
using Foundation;

using PCLStorage;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

using Jammit.Audio.macOS;

namespace Jammit.macOS
{
  [Register("AppDelegate")]
  public class AppDelegate : FormsApplicationDelegate
  {
    NSWindow _window;

    public AppDelegate()
    {
      var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

      var rect = new CoreGraphics.CGRect(200, 1000, 1024, 768);
      _window = new NSWindow(rect, style, NSBackingStore.Buffered, false);
      _window.Title = "Xamarin.Forms on Mac!";
      _window.TitleVisibility = NSWindowTitleVisibility.Hidden;
    }

    public override NSWindow MainWindow
    {
      get
      {
        return _window;
      }
    }

    public override void DidFinishLaunching(NSNotification notification)
    {
      Forms.Init();
      LoadApplication(new Jammit.Portable.App(FileSystem.Current, (s) => { return new MacOSSongPlayer(s); }));
      base.DidFinishLaunching(notification);
    }

    public override void WillTerminate(NSNotification notification)
    {
      // Insert code here to tear down your application
    }
  }
}
