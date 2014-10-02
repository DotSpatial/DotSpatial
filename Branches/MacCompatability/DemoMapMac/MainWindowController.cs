using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace MacDemoMap
{
    public partial class MainWindowController : NSWindowController
	{
		#region Constructors

		// Called when created from unmanaged code
		public MainWindowController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public MainWindowController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		// Call to load from the XIB/NIB file
		public MainWindowController () : base ("MainWindow")
		{
			Initialize ();
		}
		// Shared initialization code
		void Initialize ()
		{
		}

		#endregion

        public override void AwakeFromNib()
        {
            ShouldCascadeWindows = false;
            WindowFrameAutosaveName = Window.FrameAutosaveName;
            Window.WeakDelegate = this;
        }

        [Export("windowShouldClose:")]
        public bool WindowShouldClose (NSObject sender)
        {
            NSApplication.SharedApplication.Hide (this);
            return false;
        }

		//strongly typed window accessor
		public new MainWindow Window {
			get {
				return (MainWindow)base.Window;
			}
		}
	}
}

