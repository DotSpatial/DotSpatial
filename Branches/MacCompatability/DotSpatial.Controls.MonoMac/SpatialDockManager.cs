using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows.Forms;
using DotSpatial.Controls.Docking;
using MonoMac.AppKit;
using MonoMac.Foundation;

namespace DotSpatial.Controls.MonoMac
{
    /// <summary>
    /// Simple dock manager implementation. It can be used in design time.
    /// </summary>
    [PartNotDiscoverable] // Do not allow discover this class by MEF
    public class SpatialDockManager : NSSplitView, IDockManager
    {
        private readonly Dictionary<string, NSTabViewItem> _allTabs = new Dictionary<string, NSTabViewItem>();
        private NSTabView TabControl1;
        private NSTabView TabControl2;

        public SpatialDockManager()
        {
            IsVertical = true;
            DividerStyle = NSSplitViewDividerStyle.Thin;
            AutoresizingMask = NSViewResizingMask.HeightSizable | NSViewResizingMask.WidthSizable;

            //Add tabview for legend
            TabControl1 = new NSTabView();
            AddSubview (TabControl1);

            //Add tabview for map
            TabControl2= new NSTabView();
            AddSubview (TabControl2);
        }

        public override void ViewDidMoveToSuperview ()
        {
            Frame = new System.Drawing.RectangleF(0, 25, Superview.Bounds.Width, Superview.Bounds.Height - 60);
            AdjustSubviews();
            SetPositionOfDivider(Bounds.Width/3, 0);
            base.ViewDidMoveToSuperview ();
        }

        public event EventHandler<DockablePanelEventArgs> ActivePanelChanged;
        public event EventHandler<DockablePanelEventArgs> PanelClosed;
        public event EventHandler<DockablePanelEventArgs> PanelAdded;
        public event EventHandler<DockablePanelEventArgs> PanelRemoved;
        public event EventHandler<DockablePanelEventArgs> PanelHidden;

        public void Add(DockablePanel panel)
        {
            if (panel == null) throw new ArgumentNullException("panel");

            NSTabView tabControl;
            if (panel.Dock == DockStyle.Left || panel.Dock == DockStyle.Top)
            {
                tabControl = TabControl1;
            }
            else
            {
                tabControl = TabControl2;
            }
            if (tabControl == null) return;

            var tabPage = new NSTabViewItem();
            tabPage.View = ((NSView)panel.InnerControl);
            tabPage.Identifier = NSObject.FromObject(panel.Key);
            tabPage.Label = panel.Caption;
            //tabPage.d += TabPageOnVisibleChanged;

            _allTabs.Add(panel.Key, tabPage);
            tabControl.Add(tabPage);
            OnPanelAdded(new DockablePanelEventArgs(panel.Key));
            OnActivePanelChanged(new DockablePanelEventArgs(panel.Key));
        }

        private void TabPageOnVisibleChanged(object sender, EventArgs eventArgs)
        {
//            var tabPage = (TabPage)sender;
//            if (!tabPage.Visible)
//            {
//                OnPanelHidden(new DockablePanelEventArgs(tabPage.Name));
//            }
        }

        public void Remove(string key)
        {
            var tabPage = GetByKey(key);
            if (tabPage == null) return;

            tabPage.TabView.Remove(tabPage);
            //tabPage.VisibleChanged -= TabPageOnVisibleChanged;
            OnPanelRemoved(new DockablePanelEventArgs(key));
        }

        public void ResetLayout()
        {
//            foreach (var tabPage in _allTabs)
//            {
//                tabPage.Value.Show();
//            }
        }

        public void SelectPanel(string key)
        {
            var tabPage = GetByKey(key);
            if (tabPage == null) return;

            tabPage.TabView.Select(tabPage);
            OnActivePanelChanged(new DockablePanelEventArgs(key));
        }

        public void HidePanel(string key)
        {
            var tabPage = GetByKey(key);
            if (tabPage == null) return;

            var tabControl = tabPage.TabView;
            // Select another tab (if any)
            var i = 0;
            while (tabControl.Items.Length > i)
            {
                var current = tabControl.Items[i++];
                if (current != tabPage)
                {
                    SelectPanel((NSString)current.Identifier);
                    break;
                }
            }
        }

        public void ShowPanel(string key)
        {
//            var tabPage = GetByKey(key);
//            if (tabPage == null) return;
//
//            tabPage.Show();
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

        private NSTabViewItem GetByKey(string key)
        {
            NSTabViewItem tabPage;
            if (_allTabs.TryGetValue(key, out tabPage)) return tabPage;
            return null;
        }
    }
}
