using System.ComponentModel.Composition;
using System.Linq;
using DotSpatial.Controls.DefaultRequiredImports;
using DotSpatial.Controls.Header;
using MenuBarHeaderControl = DotSpatial.Controls.MonoMac.Header.MenuBarHeaderControl;
using DotSpatial.Extensions;
using MonoMac.AppKit;

namespace DotSpatial.Controls.MonoMac.DefaultRequiredImports
{
    /// <summary>
    /// Default Header control. It will used when no custom implementation of IHeaderControl where found.
    /// </summary>
    [Export(typeof(IHeaderControl))]
    [DefaultRequiredImport]
    internal class HeaderControl : MenuBarHeaderControl, ISatisfyImportsExtension
    {
        private bool _isActivated;

        [Import]
        private AppManager App { get; set; }

        [Import("Shell", typeof(NSWindow))]
        private NSWindow Shell { get; set; }

        public int Priority { get { return 1; } }

        public void Activate()
        {
            if (_isActivated) return;

            var headerControls = App.CompositionContainer.GetExportedValues<IHeaderControl>().ToList();

            // Activate only if there are no other IHeaderControl implementations and
            // custom HeaderControl not yet set
            if (App.HeaderControl == null &&
                headerControls.Count == 1 && headerControls[0].GetType() == GetType())
            {
                _isActivated = true;

//                // NSToolbar
//                var container = new ToolStripPanel {Dock = DockStyle.Top};
//                Shell.AddSubview(container);
//
//                // NSMenu
//                var menuStrip = new MenuStrip { Name = DEFAULT_GROUP_NAME, Dock = DockStyle.Top };
//                Shell.AddSubview(menuStrip);
//               
//                Initialize(container, menuStrip);
//                App.ExtensionsActivated += delegate { LoadToolstrips(); };
//
//                // Add default buttons
//                new DefaultMenuBars(App).Initialize(this);
            }
        }
    }
}
