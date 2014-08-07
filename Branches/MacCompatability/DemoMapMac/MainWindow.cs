using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using DotSpatial.Controls;
using DotSpatial.Controls.MonoMac;
using DotSpatial.Topology;

namespace MacDemoMap
{
	public partial class MainWindow : MonoMac.AppKit.NSWindow
	{
		private AppManager appManager;
        private DotSpatial.Controls.MonoMac.Map map = new DotSpatial.Controls.MonoMac.Map();

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
            //Add table view for map
            NSTabView tabView = new NSTabView (new RectangleF (21, 5, 680, 451));
            tabView.AutoresizingMask = NSViewResizingMask.HeightSizable | NSViewResizingMask.WidthSizable;
            ContentView.AddSubview (tabView);

            //Add map
			NSTabViewItem item = new NSTabViewItem ();
			item.Label = "Map";
            item.View = (NSView)map;
			tabView.Add(item);
			appManager = new DotSpatial.Controls.AppManager();
			appManager.Map = map;

            //Create Main Menu
            Menu = new NSMenu ("MainMenu");
            NSApplication.SharedApplication.MainMenu = Menu;

            //Add AppMenu
            NSMenuItem menuItem = new NSMenuItem ("Application Menu");
            Menu.AddItem (menuItem);
            NSMenu AppMenu = new NSMenu ("Application Menu");
            menuItem.Submenu = AppMenu;
            NSMenuItem quitMenuItem = new NSMenuItem ("Quit");
            AppMenu.AddItem(quitMenuItem);

            //Events
            LoadProject.Activated += Load_Project;
            panButton.Activated += ChangeFunctionMode;
            zoomInButton.Activated += ChangeFunctionMode;
            zoomOutButton.Activated += ChangeFunctionMode;
            selectButton.Activated += ChangeFunctionMode;
            deselectButton.Activated += ChangeFunctionMode;
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

        public void Quit_Clicked(Object sender, EventArgs e)
        {
            NSApplication.SharedApplication.Terminate(this);
        }

		#endregion
	}
}

