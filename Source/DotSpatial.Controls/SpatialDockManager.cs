// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows.Forms;
using DotSpatial.Controls.Docking;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Simple dock manager implementation. It can be used in design time.
    /// </summary>
    [PartNotDiscoverable] // Do not allow discover this class by MEF
    public class SpatialDockManager : SplitContainer, IDockManager
    {
        #region Fields

        private readonly Dictionary<string, TabPage> _allTabs = new Dictionary<string, TabPage>();

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpatialDockManager"/> class.
        /// </summary>
        public SpatialDockManager()
        {
            Panel1.ControlAdded += (sender, args) =>
                {
                    if (TabControl1 != null) return;
                    TabControl1 = args.Control as TabControl;
                };
            Panel2.ControlAdded += (sender, args) =>
                {
                    if (TabControl2 != null) return;
                    TabControl2 = args.Control as TabControl;
                };
            Panel1.ControlRemoved += (sender, args) =>
                {
                    if (args.Control == TabControl1) TabControl1 = null;
                };
            Panel2.ControlRemoved += (sender, args) =>
                {
                    if (args.Control == TabControl2) TabControl2 = null;
                };
        }

        #endregion

        #region Events

        /// <inheritdoc />
        public event EventHandler<DockablePanelEventArgs> ActivePanelChanged;

        /// <inheritdoc />
        public event EventHandler<DockablePanelEventArgs> PanelAdded;

        /// <inheritdoc />
        public event EventHandler<DockablePanelEventArgs> PanelClosed;

        /// <inheritdoc />
        public event EventHandler<DockablePanelEventArgs> PanelHidden;

        /// <inheritdoc />
        public event EventHandler<DockablePanelEventArgs> PanelRemoved;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets TabControl For Panel1. It used for storing Left and Top panels.
        /// </summary>
        [Description("Gets or sets TabControl For Panel1. It used for storing Left and Top panels.")]
        public TabControl TabControl1 { get; set; }

        /// <summary>
        /// Gets or sets TabControl For Panel2. It used for storing Right and Bottom panels.
        /// </summary>
        [Description("Gets or sets TabControl For Panel2. It used for storing Right and Bottom panels.")]
        public TabControl TabControl2 { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Add(DockablePanel panel)
        {
            if (panel == null) throw new ArgumentNullException(nameof(panel));

            TabControl tabControl;
            if (panel.Dock == DockStyle.Left || panel.Dock == DockStyle.Top)
            {
                tabControl = TabControl1;
            }
            else
            {
                tabControl = TabControl2;
            }

            if (tabControl == null) return;

            var tabPage = new TabPage
            {
                Name = panel.Key,
                Text = panel.Caption,
            };
            tabPage.VisibleChanged += TabPageOnVisibleChanged;

            panel.InnerControl.Dock = DockStyle.Fill;
            panel.PropertyChanged += TabPageOnPropertyChanged;
            tabPage.Controls.Add(panel.InnerControl);

            _allTabs.Add(panel.Key, tabPage);
            tabControl.TabPages.Add(tabPage);
            OnPanelAdded(new DockablePanelEventArgs(panel.Key));
            OnActivePanelChanged(new DockablePanelEventArgs(panel.Key));
        }

        /// <summary>
        /// Add default tab controls to the dock manager
        /// </summary>
        public void AddDefaultTabControls()
        {
            if (TabControl1 == null)
            {
                Panel1.Controls.Add(new TabControl
                {
                    Dock = DockStyle.Fill
                });
                Debug.Assert(TabControl1 != null, "TabControl1 may not be null");
            }

            if (TabControl2 == null)
            {
                Panel2.Controls.Add(new TabControl
                {
                    Dock = DockStyle.Fill
                });
                Debug.Assert(TabControl2 != null, "TabControl2 may not be null");
            }
        }

        /// <inheritdoc />
        public void HidePanel(string key)
        {
            var tabPage = GetByKey(key);
            if (tabPage == null) return;

            var tabControl = (TabControl)tabPage.Parent;

            // Select another tab (if any)
            var i = 0;
            while (tabControl.TabPages.Count > i)
            {
                var current = tabControl.TabPages[i++];
                if (current != tabPage)
                {
                    SelectPanel(current.Name);
                    break;
                }
            }
        }

        /// <inheritdoc />
        public void Remove(string key)
        {
            var tabPage = GetByKey(key);
            if (tabPage == null) return;

            ((TabControl)tabPage.Parent).TabPages.Remove(tabPage);
            _allTabs.Remove(key);
            tabPage.VisibleChanged -= TabPageOnVisibleChanged;
            OnPanelRemoved(new DockablePanelEventArgs(key));
        }

        /// <inheritdoc />
        public void ResetLayout()
        {
            foreach (var tabPage in _allTabs)
            {
                tabPage.Value.Show();
            }
        }

        /// <inheritdoc />
        public void SelectPanel(string key)
        {
            var tabPage = GetByKey(key);
            if (tabPage == null) return;

            ((TabControl)tabPage.Parent).SelectTab(tabPage);
            OnActivePanelChanged(new DockablePanelEventArgs(key));
        }

        /// <inheritdoc />
        public void ShowPanel(string key)
        {
            var tabPage = GetByKey(key);
            tabPage?.Show();
        }

        /// <summary>
        /// Raises <see cref="RaiseDockableEvent"/> event.
        /// </summary>
        /// <param name="ea">The event args.</param>
        protected virtual void OnActivePanelChanged(DockablePanelEventArgs ea)
        {
            RaiseDockableEvent(ActivePanelChanged, ea);
        }

        /// <summary>
        /// Raises <see cref="RaiseDockableEvent"/> event.
        /// </summary>
        /// <param name="ea">The event args.</param>
        protected virtual void OnPanelAdded(DockablePanelEventArgs ea)
        {
            RaiseDockableEvent(PanelAdded, ea);
        }

        /// <summary>
        /// Raises <see cref="RaiseDockableEvent"/> event.
        /// </summary>
        /// <param name="ea">The event args.</param>
        protected virtual void OnPanelClosed(DockablePanelEventArgs ea)
        {
            RaiseDockableEvent(PanelClosed, ea);
        }

        /// <summary>
        /// Raises <see cref="RaiseDockableEvent"/> event.
        /// </summary>
        /// <param name="ea">The event args.</param>
        protected virtual void OnPanelHidden(DockablePanelEventArgs ea)
        {
            RaiseDockableEvent(PanelHidden, ea);
        }

        /// <summary>
        /// Raises <see cref="RaiseDockableEvent"/> event.
        /// </summary>
        /// <param name="ea">The event args.</param>
        protected virtual void OnPanelRemoved(DockablePanelEventArgs ea)
        {
            RaiseDockableEvent(PanelRemoved, ea);
        }

        private TabPage GetByKey(string key)
        {
            TabPage tabPage;
            if (_allTabs.TryGetValue(key, out tabPage)) return tabPage;
            return null;
        }

        private void RaiseDockableEvent(EventHandler<DockablePanelEventArgs> handler, DockablePanelEventArgs ea)
        {
            handler?.Invoke(this, ea);
        }

        private void TabPageOnVisibleChanged(object sender, EventArgs eventArgs)
        {
            var tabPage = (TabPage)sender;
            if (!tabPage.Visible)
            {
                OnPanelHidden(new DockablePanelEventArgs(tabPage.Name));
            }
        }

        private void TabPageOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = (DockablePanel)sender;
            var guiItem = GetByKey(item.Key);

            switch (e.PropertyName)
            {
                case "Caption":
                    guiItem.Text = item.Caption;
                    break;
            }
        }

        #endregion
    }
}