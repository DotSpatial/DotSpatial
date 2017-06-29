using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using DotSpatial.Controls.Header;

namespace DotSpatial.Controls
{
    [PartNotDiscoverable]
    public class SpatialHeaderControl : Component, IHeaderControl, ISupportInitialize
    {
        private readonly MenuBarHeaderControl _menuBar;
        private ToolStripPanel _toolbarsContainer;
        private MenuStrip _menuStrip;
        private bool _isInitializing;
        private AppManager _applicationManager;

        public SpatialHeaderControl()
        {
            _menuBar = new MenuBarHeaderControl();
            _menuBar.RootItemSelected += delegate(object sender, RootItemEventArgs args)
            {
                var h = RootItemSelected;
                if (h != null) h(this, args);
            };
        }

        /// <summary>
        /// Gets or sets the application manager.
        /// </summary>
        [Description("Gets or sets the application manager.")]
        public AppManager ApplicationManager
        {
            get { return _applicationManager; }
            set
            {
                if (value == _applicationManager) return;
                if (_applicationManager != null)
                {
                    _applicationManager.HeaderControlChanged -= ApplicationManagerOnHeaderControlChanged;
                    _applicationManager.ExtensionsActivated -= ApplicationManagerOnExtensionsActivated;
                }

                _applicationManager = value;

                if (_applicationManager != null)
                {
                    _applicationManager.HeaderControlChanged += ApplicationManagerOnHeaderControlChanged;
                    _applicationManager.ExtensionsActivated += ApplicationManagerOnExtensionsActivated;
                }
                
                InitHeaderControl();
            }
        }

        private void ApplicationManagerOnExtensionsActivated(object sender, EventArgs eventArgs)
        {
            _menuBar.LoadToolstrips();
        }

        private void ApplicationManagerOnHeaderControlChanged(object sender, EventArgs eventArgs)
        {
            InitHeaderControl();
        }

        /// <summary>
        /// Gets or sets Toolbars container for header control buttons.
        /// </summary>
        [Description("Gets or sets Toolbars container for header control buttons")]
        public ToolStripPanel ToolbarsContainer
        {
            get { return _toolbarsContainer; }
            set
            {
                if (_toolbarsContainer == value) return;
                _toolbarsContainer = value;
                InitHeaderControl();
            }
        }

        /// <summary>
        /// Gets or sets Menu strip for header control menus.
        /// </summary>
        [Description("Gets or sets Menu strip for header control menus.")]
        public MenuStrip MenuStrip
        {
            get { return _menuStrip; }
            set
            {
                if (value == _menuStrip) return;
                _menuStrip = value;
                InitHeaderControl();
            }
        }

        public void Add(HeaderItem item)
        {
            _menuBar.Add(item);
        }

        public void Remove(string key)
        {
            _menuBar.Remove(key);
        }

        public void RemoveAll()
        {
            _menuBar.RemoveAll();
        }

        public void SelectRoot(string key)
        {
            _menuBar.SelectRoot(key);
        }

        public event EventHandler<RootItemEventArgs> RootItemSelected;

        private void InitHeaderControl()
        {
            if (_isInitializing) return;
            if (ToolbarsContainer == null || MenuStrip == null) return;

            // Add default menus/buttons
            if (ApplicationManager != null && ApplicationManager.HeaderControl != null)
            {
                _menuBar.IgnoreToolstripPositionSaving = DesignMode;
                _menuBar.Initialize(ToolbarsContainer, MenuStrip);
                new DefaultMenuBars(ApplicationManager).Initialize(ApplicationManager.HeaderControl);

                // load here in DesignMode, because _applicationManager.ExtensionsActivated doesn't get raised
                if (DesignMode)
                {
                    _menuBar.LoadToolstrips();
                }
            }
        }

        public void BeginInit()
        {
            _isInitializing = true;
        }

        public void EndInit()
        {
            _isInitializing = false;
            InitHeaderControl();
        }
    }
}