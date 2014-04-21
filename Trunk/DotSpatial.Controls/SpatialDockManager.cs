using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
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
        private readonly Dictionary<string, TabPage> _allTabs = new Dictionary<string, TabPage>();

        public SpatialDockManager()
        {
            Panel1.Controls.Add(TabControl1 = new TabControl { Dock = DockStyle.Fill, });
            Panel2.Controls.Add(TabControl2 = new TabControl { Dock = DockStyle.Fill });

            if (IsInDesignMode)
            {
                TabControl1.TabPages.Add(new TabPage("Page1"));
                TabControl1.TabPages.Add(new TabPage("Page2"));

                TabControl2.TabPages.Add(new TabPage("Page1"));
                TabControl2.TabPages.Add(new TabPage("Page2"));
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TabControl TabControl1 { get; private set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TabControl TabControl2 { get; private set; }


        protected bool IsInDesignMode
        {
            get
            {
                return DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            }
        }

        public event EventHandler<DockablePanelEventArgs> ActivePanelChanged;
        public event EventHandler<DockablePanelEventArgs> PanelClosed;
        public event EventHandler<DockablePanelEventArgs> PanelAdded;
        public event EventHandler<DockablePanelEventArgs> PanelRemoved;
        public event EventHandler<DockablePanelEventArgs> PanelHidden;

        public void Add(DockablePanel panel)
        {
            if (panel == null) throw new ArgumentNullException("panel");

            TabControl tabControl;
            if (panel.Dock == DockStyle.Left || panel.Dock == DockStyle.Top)
            {
                tabControl = TabControl1;
            }
            else
            {
                tabControl = TabControl2;
            }

            var tabPage = new TabPage
                          {
                              Name = panel.Key,
                              Text = panel.Caption,
                          };
            tabPage.VisibleChanged += TabPageOnVisibleChanged;

            panel.InnerControl.Dock = DockStyle.Fill;
            tabPage.Controls.Add(panel.InnerControl);

            _allTabs.Add(panel.Key, tabPage);
            tabControl.TabPages.Add(tabPage);
            OnPanelAdded(new DockablePanelEventArgs(panel.Key));
            OnActivePanelChanged(new DockablePanelEventArgs(panel.Key));
        }

        private void TabPageOnVisibleChanged(object sender, EventArgs eventArgs)
        {
            var tabPage = (TabPage)sender;
            if (!tabPage.Visible)
            {
                OnPanelHidden(new DockablePanelEventArgs(tabPage.Name));
            }
        }

        public void Remove(string key)
        {
            var tabPage = GetByKey(key);
            if (tabPage == null) return;

            ((TabControl)tabPage.Parent).TabPages.Remove(tabPage);
            tabPage.VisibleChanged -= TabPageOnVisibleChanged;
            OnPanelRemoved(new DockablePanelEventArgs(key));
        }

        public void ResetLayout()
        {
            foreach (var tabPage in _allTabs)
            {
                tabPage.Value.Show();
            }
        }

        public void SelectPanel(string key)
        {
            var tabPage = GetByKey(key);
            if (tabPage == null) return;

            tabPage.Show();
            tabPage.Focus();
            OnActivePanelChanged(new DockablePanelEventArgs(key));
        }

        public void HidePanel(string key)
        {
            var tabPage = GetByKey(key);
            if (tabPage == null) return;

            tabPage.Hide();
        }

        public void ShowPanel(string key)
        {
            var tabPage = GetByKey(key);
            if (tabPage == null) return;

            tabPage.Show();
        }

        protected virtual void OnPanelRemoved(DockablePanelEventArgs ea)
        {
            RaiseDockableEvent(PanelRemoved, ea);
        }

        protected virtual void OnPanelAdded(DockablePanelEventArgs ea)
        {
            RaiseDockableEvent(PanelAdded, ea);
        }

        protected virtual void OnPanelClosed(DockablePanelEventArgs ea)
        {
            RaiseDockableEvent(PanelClosed, ea);
        }

        protected virtual void OnActivePanelChanged(DockablePanelEventArgs ea)
        {
            RaiseDockableEvent(ActivePanelChanged, ea);
        }

        protected virtual void OnPanelHidden(DockablePanelEventArgs ea)
        {
            RaiseDockableEvent(PanelHidden, ea);
        }

        private void RaiseDockableEvent(EventHandler<DockablePanelEventArgs> handler, DockablePanelEventArgs ea)
        {
            if (handler != null)
                handler(this, ea);
        }

        private TabPage GetByKey(string key)
        {
            TabPage tabPage;
            if (_allTabs.TryGetValue(key, out tabPage)) return tabPage;
            return null;
        }
    }
}
