using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using DotSpatial.Controls;
using DotSpatial.MacControls;
using DotSpatial.Topology;

namespace MacDemoMap
{
	public partial class MainWindow : MonoMac.AppKit.NSWindow
	{
		private AppManager appManager;
		private MacMap map = new MacMap();

		#region Constructors

		// Called when created from unmanaged code
		public MainWindow (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public MainWindow (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		// Shared initialization code
		void Initialize ()
		{
		}

		public override void AwakeFromNib()
        {
            //Map map = new Map ();
            NSTabView tabView = new NSTabView (new RectangleF (21, 5, 680, 451));
            tabView.AutoresizingMask = NSViewResizingMask.HeightSizable | NSViewResizingMask.WidthSizable;
            ContentView.AddSubview (tabView);
			NSTabViewItem item = new NSTabViewItem ();
			item.Label = "Map";
            item.View = (NSView)map;
			tabView.Add(item);
			appManager = new DotSpatial.Controls.AppManager();
			appManager.Map = map;
			LoadProject.Activated += Load_Project;
			panButton.Activated += ChangeFunctionMode;
			zoomInButton.Activated += ChangeFunctionMode;
			zoomOutButton.Activated += ChangeFunctionMode;
			selectButton.Activated += ChangeFunctionMode;
			deselectButton.Activated += ChangeFunctionMode;

            Menu = new NSMenu ("MainMenu");
            NSMenuItem menuItem = new NSMenuItem ("Application Menu");
            Menu.AddItem (menuItem);
            NSMenu BlaBla = new NSMenu ("Application Menu");
            menuItem.Submenu = BlaBla;
            BlaBla.AddItem (new NSMenuItem ("Quit"));
            NSApplication.SharedApplication.MainMenu = Menu;
		}

		public void Load_Project(Object sender, EventArgs e)
		{
            NSOpenPanel openDlg = new NSOpenPanel ();
            openDlg.ReleasedWhenClosed = true;
            openDlg.Prompt = "Select Project File";
            String[] files = { "dspx" };
            openDlg.AllowedFileTypes = files;
            if(openDlg.RunModal() == 1)
                appManager.SerializationManager.OpenProject (openDlg.Url.RelativePath);
		}

		public void ChangeFunctionMode(Object sender, EventArgs e)
		{
			if (sender == panButton)
			{
				map.FunctionMode = FunctionMode.Pan;
			}
			if (sender == zoomInButton)
			{
				map.FunctionMode = FunctionMode.ZoomIn;
			}
			if (sender == zoomOutButton)
			{
				map.FunctionMode = FunctionMode.ZoomOut;
			}
			if (sender == selectButton)
			{
				map.FunctionMode = FunctionMode.Select;
			}
			if (sender == deselectButton)
			{
				IEnvelope env;
				map.MapFrame.ClearSelection(out env);
			}
		}

		#endregion
	}
}

