// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Controls.Docking;
using DotSpatial.Extensions;

namespace DotSpatial.Controls.DefaultRequiredImports
{
    /// <summary>
    /// Default Dock Manager. It will be used when no custom implementation of IDockManager was found.
    /// </summary>
    [DefaultRequiredImport]
    internal class DockManager : IDockManager, ISatisfyImportsExtension
    {
        #region Fields

        private SpatialDockManager _dockManager;
        private bool _isActivated;

        #endregion

        #region Events

        /// <summary>
        /// Event that is rased after the active panel changes.
        /// </summary>
        public event EventHandler<DockablePanelEventArgs> ActivePanelChanged;

        /// <summary>
        /// Event that is rased after a panel was added.
        /// </summary>
        public event EventHandler<DockablePanelEventArgs> PanelAdded;

        /// <summary>
        /// Event that is rased after a panel was closed.
        /// </summary>
        public event EventHandler<DockablePanelEventArgs> PanelClosed;

        /// <summary>
        /// Event that is rased after a panel was hidden.
        /// </summary>
        public event EventHandler<DockablePanelEventArgs> PanelHidden;

        /// <summary>
        /// Event that is rased after a panel was removed.
        /// </summary>
        public event EventHandler<DockablePanelEventArgs> PanelRemoved;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the priority.
        /// </summary>
        public int Priority => 0;

        [Import]
        private AppManager App { get; set; }

        [Import("Shell", typeof(ContainerControl))]
        private ContainerControl Shell { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Activate()
        {
            if (_isActivated) return;
            var dockManagers = App.CompositionContainer.GetExportedValues<IDockManager>().ToList();

            // Activate only if there are no other IDockManager implementations and
            // custom DockManager not yet set
            if (App.DockManager == null && dockManagers.Count == 1 && dockManagers[0].GetType() == GetType())
            {
                _isActivated = true;

                _dockManager = new SpatialDockManager
                {
                    Dock = DockStyle.Fill
                };
                _dockManager.AddDefaultTabControls();
                _dockManager.ActivePanelChanged += (sender, args) => RaiseDockableEvent(ActivePanelChanged, args);
                _dockManager.PanelClosed += (sender, args) => RaiseDockableEvent(PanelClosed, args);
                _dockManager.PanelAdded += (sender, args) => RaiseDockableEvent(PanelAdded, args);
                _dockManager.PanelRemoved += (sender, args) => RaiseDockableEvent(PanelRemoved, args);
                _dockManager.PanelHidden += (sender, args) => RaiseDockableEvent(PanelHidden, args);

                Shell.Controls.Add(_dockManager);
            }
        }

        /// <summary>
        /// Adds the given panel .
        /// </summary>
        /// <param name="panel">Panel that should be added.</param>
        public void Add(DockablePanel panel)
        {
            if (!_isActivated) return;
            _dockManager.Add(panel);
        }

        /// <summary>
        /// Hides the panel with the given key.
        /// </summary>
        /// <param name="key">Key of the panel that should be hidden.</param>
        public void HidePanel(string key)
        {
            if (!_isActivated) return;
            _dockManager.HidePanel(key);
        }

        /// <summary>
        /// Removes the item with the given key.
        /// </summary>
        /// <param name="key">Key of the item that should be removed.</param>
        public void Remove(string key)
        {
            if (!_isActivated) return;
            _dockManager.Remove(key);
        }

        /// <summary>
        /// Resets the layout.
        /// </summary>
        public void ResetLayout()
        {
            if (!_isActivated) return;
            _dockManager.ResetLayout();
        }

        /// <summary>
        /// Selects the panel with the given key.
        /// </summary>
        /// <param name="key">Key of the panel that should be selected.</param>
        public void SelectPanel(string key)
        {
            if (!_isActivated) return;
            _dockManager.SelectPanel(key);
        }

        /// <summary>
        /// Shows the panel with the given key.
        /// </summary>
        /// <param name="key">Key of the panel that should be shown.</param>
        public void ShowPanel(string key)
        {
            if (!_isActivated) return;
            _dockManager.ShowPanel(key);
        }

        private void RaiseDockableEvent(EventHandler<DockablePanelEventArgs> handler, DockablePanelEventArgs ea)
        {
            handler?.Invoke(this, ea);
        }

        #endregion
    }
}