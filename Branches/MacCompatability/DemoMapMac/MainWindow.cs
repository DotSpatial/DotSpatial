using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;
using DotSpatial.Controls.MonoMac;
using DotSpatial.Topology;
using MonoMac.AppKit;
using MonoMac.Foundation;

namespace MacDemoMap
{
	public partial class MainWindow : MonoMac.AppKit.NSWindow
	{
		private AppManager appManager;
        private DotSpatial.Controls.MonoMac.Map map = new DotSpatial.Controls.MonoMac.Map();
        [System.ComponentModel.Composition.Export("Shell", typeof(NSView))]
        private static NSView Shell;

		#region Constructors

		// Called when created from unmanaged code
		public MainWindow (IntPtr handle) : base (handle)
		{
			Initialize ();
		}

        // Called when created directly from a XIB file
        [MonoMac.Foundation.Export ("initWithCoder:")]
        public MainWindow (NSCoder coder) : base (coder)
        {
            Initialize ();
        }

		// Shared initialization code
		void Initialize ()
		{
            MinSize = new SizeF(0, 90);
		}

		public override void AwakeFromNib()
        {
            // Set up DotSpatial
            Shell = ContentView;
            appManager = new AppManager();
            appManager.LoadExtensions ();

            // Add map to DockManager
            appManager.Map = map;
            appManager.DockManager.Add(new DockablePanel("kMap", "Map", map, DockStyle.Fill));

            // Add ToolBar
            NSToolbar toolBar = new NSToolbar ("SpatialToolStrip");
            Toolbar = toolBar;
            toolBar.SizeMode = NSToolbarSizeMode.Small;
            //toolBar.DisplayMode = NSToolbarDisplayMode.Icon;
            //toolBar.ShowsBaselineSeparator = false;
            toolBar.Delegate = new ToolBarDelegate (map);

            // Add Pan
            toolBar.InsertItem("Pan", 0);

            // Create Main Menu
            Menu = new NSMenu ("MainMenu");
            NSApplication.SharedApplication.MainMenu = Menu;

            // Add AppMenu
            NSMenuItem menuItem = new NSMenuItem ("Application Menu");
            Menu.AddItem (menuItem);
            NSMenu AppMenu = new NSMenu ("Application Menu");
            menuItem.Submenu = AppMenu;

            // Add Open...
            NSMenuItem openMenuItem = new NSMenuItem ("Open...");
            AppMenu.AddItem (openMenuItem);
            openMenuItem.Activated += Load_Project;

            // Add Quit to AppMenu
            NSMenuItem quitMenuItem = new NSMenuItem ("Quit");
            AppMenu.AddItem(quitMenuItem);
            quitMenuItem.Activated += Quit_Clicked;

            // Map Events
            panButton.Activated += ChangeFunctionMode;
            zoomInButton.Activated += ChangeFunctionMode;
            zoomOutButton.Activated += ChangeFunctionMode;
            selectButton.Activated += ChangeFunctionMode;
            deselectButton.Activated += ChangeFunctionMode;

            // Move buttons to Map
            panButton.RemoveFromSuperview ();
            zoomInButton.RemoveFromSuperview ();
            zoomOutButton.RemoveFromSuperview ();
            selectButton.RemoveFromSuperview ();
            deselectButton.RemoveFromSuperview ();
            map.AddSubview (panButton);
            map.AddSubview (zoomInButton);
            map.AddSubview (zoomOutButton);
            map.AddSubview (selectButton);
            map.AddSubview (deselectButton);
            panButton.Frame = new RectangleF (0, map.Height - 30 - 5, 35, 35);
            zoomInButton.Frame = new RectangleF (0, map.Height - 30*2 - 5, 35, 35);
            zoomOutButton.Frame = new RectangleF (0, map.Height - 30*3 - 5, 35, 35);
            selectButton.Frame = new RectangleF (0, map.Height - 30*4 - 5, 35, 35);
            deselectButton.Frame = new RectangleF (0, map.Height - 30*5 - 5, 35, 35);
		}

        public NSToolbarItem WillBeInsertedIntoToolbar(NSToolbar toolbar, string itemIdentifier, bool flag)
        {
            var toolBarItemPan = new NSToolbarItem ("Pan") {Label = "Pan", ToolTip = "Pan", 
                        PaletteLabel = "Pan", Image = NSImage.ImageNamed ("hand_32x32.png")};
            toolBarItemPan.Activated += (Object sender, EventArgs e) => {map.FunctionMode = FunctionMode.Pan;};
            return toolBarItemPan;
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
            // Deselect
            if (sender == deselectButton)
            {
                IEnvelope env;
                map.MapFrame.ClearSelection(out env);
                return;
            }
            // Pan
			if (sender == panButton)
    			map.FunctionMode = FunctionMode.Pan;
            else
                panButton.State = NSCellStateValue.Off;
            // Zoom In
			if (sender == zoomInButton)
    			map.FunctionMode = FunctionMode.ZoomIn;
            else
                zoomInButton.State = NSCellStateValue.Off;
            // Zoom Out
			if (sender == zoomOutButton)
    			map.FunctionMode = FunctionMode.ZoomOut;
            else
                zoomOutButton.State = NSCellStateValue.Off;
            // Select
			if (sender == selectButton)
    			map.FunctionMode = FunctionMode.Select;
            else
                selectButton.State = NSCellStateValue.Off;
		}

        public void Quit_Clicked(Object sender, EventArgs e)
        {
            NSApplication.SharedApplication.Terminate(this);
        }

		#endregion
	}
}

