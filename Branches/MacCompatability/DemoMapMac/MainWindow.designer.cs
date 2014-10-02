// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;
using System.CodeDom.Compiler;

namespace MacDemoMap
{
	[Register ("MainWindow")]
	partial class MainWindow
	{
		[Outlet]
		MonoMac.AppKit.NSButton deselectButton { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton panButton { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton selectButton { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton zoomInButton { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton zoomOutButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (deselectButton != null) {
				deselectButton.Dispose ();
				deselectButton = null;
			}

			if (panButton != null) {
				panButton.Dispose ();
				panButton = null;
			}

			if (selectButton != null) {
				selectButton.Dispose ();
				selectButton = null;
			}

			if (zoomInButton != null) {
				zoomInButton.Dispose ();
				zoomInButton = null;
			}

			if (zoomOutButton != null) {
				zoomOutButton.Dispose ();
				zoomOutButton = null;
			}
		}
	}

	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
