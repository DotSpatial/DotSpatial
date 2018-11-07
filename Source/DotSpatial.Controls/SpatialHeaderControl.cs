// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using DotSpatial.Controls.Header;

namespace DotSpatial.Controls
{
    /// <summary>
    /// The spatial header control.
    /// </summary>
    [PartNotDiscoverable]
    public class SpatialHeaderControl : Component, IHeaderControl, ISupportInitialize
    {
        #region Fields

        private readonly MenuBarHeaderControl _menuBar;
        private DefaultMenuBars _defaultMenuBar;
        private AppManager _applicationManager;
        private bool _isInitializing;
        private MenuStrip _menuStrip;
        private ToolStripPanel _toolbarsContainer;
        private CultureInfo _spatialHeaderCulture;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpatialHeaderControl"/> class.
        /// </summary>
        public SpatialHeaderControl()
        {
            _menuBar = new MenuBarHeaderControl();
            _menuBar.RootItemSelected += (sender, args) =>
                {
                    RootItemSelected?.Invoke(this, args);
                };
        }

        #endregion

        #region Events

        /// <inheritdoc />
        public event EventHandler<RootItemEventArgs> RootItemSelected;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the application manager.
        /// </summary>
        [Description("Gets or sets the application manager.")]
        public AppManager ApplicationManager
        {
            get
            {
                return _applicationManager;
            }

            set
            {
                if (value == _applicationManager) return;
                if (_applicationManager != null)
                {
                    _applicationManager.HeaderControlChanged -= ApplicationManagerOnHeaderControlChanged;
                    _applicationManager.ExtensionsActivated -= ApplicationManagerOnExtensionsActivated;
                    _applicationManager.AppCultureChanged -= OnAppCultureChanged;
                }

                _applicationManager = value;

                if (_applicationManager != null)
                {
                    _applicationManager.HeaderControlChanged += ApplicationManagerOnHeaderControlChanged;
                    _applicationManager.ExtensionsActivated += ApplicationManagerOnExtensionsActivated;
                    _applicationManager.AppCultureChanged += OnAppCultureChanged;
                }

                InitHeaderControl();
            }
        }

        /// <summary>
        /// Gets or sets Menu strip for header control menus.
        /// </summary>
        [Description("Gets or sets Menu strip for header control menus.")]
        public MenuStrip MenuStrip
        {
            get
            {
                return _menuStrip;
            }

            set
            {
                if (value == _menuStrip) return;
                _menuStrip = value;
                InitHeaderControl();
            }
        }

        /// <summary>
        /// Gets or sets Toolbars container for header control buttons.
        /// </summary>
        [Description("Gets or sets Toolbars container for header control buttons")]
        public ToolStripPanel ToolbarsContainer
        {
            get
            {
                return _toolbarsContainer;
            }

            set
            {
                if (_toolbarsContainer == value) return;
                _toolbarsContainer = value;
                InitHeaderControl();
            }
        }

        #endregion

        /// <summary>
        /// sets a value indicating the culture to use for resources.
        /// </summary>
        public CultureInfo SpatialHeaderCulture
        {
            set
            {
                if (_spatialHeaderCulture == value) return;

                _spatialHeaderCulture = value;

                if (_spatialHeaderCulture == null) _spatialHeaderCulture = new CultureInfo(string.Empty);

                Thread.CurrentThread.CurrentCulture = _spatialHeaderCulture;
                Thread.CurrentThread.CurrentUICulture = _spatialHeaderCulture;

                UpdateSpatialHeaderItems();
            }
        }

        #region Methods

        /// <inheritdoc />
        public object Add(HeaderItem item)
        {
            return _menuBar.Add(item);
        }

        /// <inheritdoc />
        public void BeginInit()
        {
            _isInitializing = true;
        }

        /// <inheritdoc />
        public void EndInit()
        {
            _isInitializing = false;
            InitHeaderControl();
        }

        /// <inheritdoc />
        public void Remove(string key)
        {
            _menuBar.Remove(key);
        }

        /// <inheritdoc />
        public void RemoveAll()
        {
            _menuBar.RemoveAll();
        }

        /// <inheritdoc />
        public void SelectRoot(string key)
        {
            _menuBar.SelectRoot(key);
        }

        private void ApplicationManagerOnExtensionsActivated(object sender, EventArgs eventArgs)
        {
            _menuBar.LoadToolstrips();
        }

        private void ApplicationManagerOnHeaderControlChanged(object sender, EventArgs eventArgs)
        {
            InitHeaderControl();
        }

        private void InitHeaderControl()
        {
            if (_isInitializing) return;
            if (ToolbarsContainer == null || MenuStrip == null) return;

            // Add default menus/buttons
            if (ApplicationManager?.HeaderControl != null)
            {
                _menuBar.IgnoreToolstripPositionSaving = DesignMode;
                _menuBar.Initialize(ToolbarsContainer, MenuStrip);

                _defaultMenuBar = new DefaultMenuBars(ApplicationManager);
                _defaultMenuBar.Initialize(ApplicationManager.HeaderControl);

                // load here in DesignMode, because _applicationManager.ExtensionsActivated doesn't get raised
                if (DesignMode)
                {
                    _menuBar.LoadToolstrips();
                }
            }
        }

        private void OnAppCultureChanged(object sender, CultureInfo appCulture)
        {
            SpatialHeaderCulture = appCulture;
        }

        private void UpdateSpatialHeaderItems()
        {
        }

        #endregion
    }
}