using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Controls.Docking;
using DotSpatial.Extensions;

namespace DotSpatial.Controls.DefaultRequiredImports
{
    /// <summary>
    /// Default Dock Manager. It will used when no custom implementation of IDockManager where found.
    /// </summary>
    [DefaultRequiredImport]
    internal class DockManager : IDockManager, ISatisfyImportsExtension
    {
        private SpatialDockManager _dockManager;
        private bool _isActivated;

        [Import]
        private AppManager App { get; set; }

        [Import("Shell", typeof(ContainerControl))]
        private ContainerControl Shell { get; set; }

        public int Priority { get { return 0; } }

        public void Activate()
        {
            if (_isActivated) return;
            var dockManagers = App.CompositionContainer.GetExportedValues<IDockManager>().ToList();

            // Activate only if there are no other IDockManager implementations and
            // custom DockManager not yet set
            if (App.DockManager == null &&
                dockManagers.Count == 1 && dockManagers[0].GetType() == GetType())
            {
                _isActivated = true;

                _dockManager = new SpatialDockManager {Dock = DockStyle.Fill};
                _dockManager.AddDefaultTabControls();
                _dockManager.ActivePanelChanged += (sender, args) => RaiseDockableEvent(ActivePanelChanged, args);
                _dockManager.PanelClosed += (sender, args) => RaiseDockableEvent(PanelClosed, args);
                _dockManager.PanelAdded += (sender, args) => RaiseDockableEvent(PanelAdded, args);
                _dockManager.PanelRemoved += (sender, args) => RaiseDockableEvent(PanelRemoved, args);
                _dockManager.PanelHidden += (sender, args) => RaiseDockableEvent(PanelHidden, args);
                
                Shell.Controls.Add(_dockManager);
            }
        }

        private void RaiseDockableEvent(EventHandler<DockablePanelEventArgs> handler, DockablePanelEventArgs ea)
        {
            if (handler != null)
                handler(this, ea);
        }

        #region IDockManager implementation

        public event EventHandler<DockablePanelEventArgs> ActivePanelChanged;
        public event EventHandler<DockablePanelEventArgs> PanelClosed;
        public event EventHandler<DockablePanelEventArgs> PanelAdded;
        public event EventHandler<DockablePanelEventArgs> PanelRemoved;
        public event EventHandler<DockablePanelEventArgs> PanelHidden;

        public void Add(DockablePanel panel)
        {
            if (!_isActivated) return;
            _dockManager.Add(panel);
        }

        public void Remove(string key)
        {
            if (!_isActivated) return;
            _dockManager.Remove(key);
        }

        public void ResetLayout()
        {
            if (!_isActivated) return;
            _dockManager.ResetLayout();
        }

        public void SelectPanel(string key)
        {
            if (!_isActivated) return;
            _dockManager.SelectPanel(key);
        }

        public void HidePanel(string key)
        {
            if (!_isActivated) return;
            _dockManager.HidePanel(key);
        }

        public void ShowPanel(string key)
        {
            if (!_isActivated) return;
            _dockManager.ShowPanel(key);
        }

        #endregion
    }
}