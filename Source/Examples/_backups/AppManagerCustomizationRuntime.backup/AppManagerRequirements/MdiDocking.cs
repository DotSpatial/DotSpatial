// -----------------------------------------------------------------------
// <copyright file="SimpleMdiDocking.cs" company="DotSpatial Team">
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Controls.Docking;

namespace DotSpatial.Examples.AppManagerCustomizationRuntime.AppManagerRequirements
{
    /// <summary>
    /// Simple implmenentation of IDockManager.
    /// It shows a technique how to create own dock manager as extension.
    /// You may delete this class in your application. In this case default dock manager control will be used.
    /// </summary>
    internal class MdiDocking : IDockManager, IPartImportsSatisfiedNotification
    {
        #region Fields

        [Import("Shell")]
        private ContainerControl Shell { get; set; }

        private readonly List<Form> _forms = new List<Form>();

        #endregion

        #region IDockManager Members

        /// <summary>
        /// Occurs when the active panel is changed, meaning a difference panel is activated.
        /// </summary>
        public event EventHandler<DockablePanelEventArgs> ActivePanelChanged;

        /// <summary>
        /// Occurs when a panel is closed, which means the panel can still be activated or removed.
        /// </summary>
        public event EventHandler<DockablePanelEventArgs> PanelClosed;

        /// <summary>
        /// Occurs after a panel is added.
        /// </summary>
        public event EventHandler<DockablePanelEventArgs> PanelAdded;

        /// <summary>
        /// Occurs after a panel is removed.
        /// </summary>
        public event EventHandler<DockablePanelEventArgs> PanelRemoved;

        /// <summary>
        /// Occurs when a panel is hidden.
        /// </summary>
        public event EventHandler<DockablePanelEventArgs> PanelHidden;


        /// <summary>
        /// Removes the specified panel.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(string key)
        {
            var form = GetFormByKey(key);
            if (form != null)
            {
                form.Close();
                _forms.Remove(form);

                form.Activated -= form_Activated;
                form.VisibleChanged -= FormOnVisibleChanged;
                form.Closed -= FormOnClosed;
                OnPanelRemoved(new DockablePanelEventArgs(key));
            }
        }

        /// <summary>
        /// Selects the panel.
        /// </summary>
        /// <param name="key">The key.</param>
        public void SelectPanel(string key)
        {
            var form = GetFormByKey(key);
            if (form != null) form.Focus();
        }

        /// <summary>
        /// Adds the specified panel.
        /// </summary>
        /// <param name="panel"></param>
        public void Add(DockablePanel panel)
        {
            Add(panel.Key, panel.Caption, panel.InnerControl, panel.Dock);
            OnPanelAdded(new DockablePanelEventArgs(panel.Key));
        }
       
        public void ResetLayout()
        {
            _forms.ForEach(_ => _.Show(Shell));
        }

        #endregion
       
        protected virtual void OnPanelRemoved(DockablePanelEventArgs ea)
        {
            if (PanelRemoved != null)
                PanelRemoved(this, ea);
        }

        protected virtual void OnPanelAdded(DockablePanelEventArgs ea)
        {
            if (PanelAdded != null)
                PanelAdded(this, ea);
        }
        
        protected virtual void OnPanelClosed(DockablePanelEventArgs ea)
        {
            if (PanelClosed != null)
                PanelClosed(this, ea);
        }
       
        protected virtual void OnActivePanelChanged(DockablePanelEventArgs ea)
        {
            if (ActivePanelChanged != null)
                ActivePanelChanged(this, ea);
        }

        protected virtual void OnPanelHidden(DockablePanelEventArgs ea)
        {
            if (PanelHidden != null)
                PanelHidden(this, ea);
        }
      
        public void Add(string key, string caption, Control panel, DockStyle dockStyle)
        {
            if (panel == null) throw new ArgumentNullException("panel");
            
            panel.Dock = DockStyle.Fill;
            var form = new Form
                       {
                           Name = key,
                           Text = panel.Text,
                           Width = panel.Width,
                           Height = panel.Height,
                           MdiParent = Shell as Form,
                       };
            form.Controls.Add(panel);
            
            form.Activated += form_Activated;
            form.VisibleChanged += FormOnVisibleChanged;
            form.Closed += FormOnClosed;

            _forms.Add(form);
            form.Show();
        }

        private void FormOnClosed(object sender, EventArgs eventArgs)
        {
            OnPanelClosed(new DockablePanelEventArgs(((Form)sender).Name));
        }

        private void FormOnVisibleChanged(object sender, EventArgs eventArgs)
        {
            var form = (Form) sender;
            if (!form.Visible)
            {
                OnPanelHidden(new DockablePanelEventArgs(form.Name));
            }
        }

        private void form_Activated(object sender, EventArgs e)
        {
            OnActivePanelChanged(new DockablePanelEventArgs(((Form)sender).Name));
        }

        public void HidePanel(string key)
        {
            var form = GetFormByKey(key);
            if (form != null)
            {
                form.Hide();
            }
        }

        public void ShowPanel(string key)
        {
            var form = GetFormByKey(key);
            if (form != null)
            {
                form.Show();
            }
        }

        private Form GetFormByKey(string key)
        {
            return _forms.FirstOrDefault(_ => _.Name == key);
        }

        public void OnImportsSatisfied()
        {
            var form = Shell as Form;
            if (form != null) form.IsMdiContainer = true;
        }
    }
}