using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

namespace MacDemoMap
{
	class MainClass
	{
		static void Main (string[] args)
        {
			NSApplication.Init ();
            var application = NSApplication.SharedApplication;

            application.Delegate = new AppDelegate ();
            application.Run ();
		}
	}
}

