// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

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
    /// It shows how to create your own dock manager as extension.
    /// You may delete this class in your application. In this case the default dock manager control will be used.
    /// </summary>
    internal class MdiDocking : IDockManager, IPartImportsSatisfiedNotification
    {
        #region Fields

        [Import("Shell")]
        private ContainerControl Shell { get; set; }

        private readonly List<Form> _forms = new();

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
            Form form = GetFormByKey(key);
            if (form != null)
            {
                form.Close();
                _forms.Remove(form);

                form.Activated -= Form_Activated;
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
            Form form = GetFormByKey(key);
            form?.Focus();
        }

        /// <summary>
        /// Adds the specified panel.
        /// </summary>
        /// <param name="panel"></param>
        public void Add(DockablePanel panel)
        {
            Add(panel.Key, panel.InnerControl);
            OnPanelAdded(new DockablePanelEventArgs(panel.Key));
        }

        public void ResetLayout()
        {
            _forms.ForEach(_ => _.Show(Shell));
        }

        #endregion

        protected virtual void OnPanelRemoved(DockablePanelEventArgs ea)
        {
            PanelRemoved?.Invoke(this, ea);
        }

        protected virtual void OnPanelAdded(DockablePanelEventArgs ea)
        {
            PanelAdded?.Invoke(this, ea);
        }

        protected virtual void OnPanelClosed(DockablePanelEventArgs ea)
        {
            PanelClosed?.Invoke(this, ea);
        }

        protected virtual void OnActivePanelChanged(DockablePanelEventArgs ea)
        {
            ActivePanelChanged?.Invoke(this, ea);
        }

        protected virtual void OnPanelHidden(DockablePanelEventArgs ea)
        {
            PanelHidden?.Invoke(this, ea);
        }

        public void Add(string key, Control panel)
        {
            if (panel == null)
            {
                throw new ArgumentNullException(nameof(panel));
            }

            panel.Dock = DockStyle.Fill;
            Form form = new()
            {
                Name = key,
                Text = panel.Text,
                Width = panel.Width,
                Height = panel.Height,
                MdiParent = Shell as Form,
            };
            form.Controls.Add(panel);

            form.Activated += Form_Activated;
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
            Form form = (Form)sender;
            if (!form.Visible)
            {
                OnPanelHidden(new DockablePanelEventArgs(form.Name));
            }
        }

        private void Form_Activated(object sender, EventArgs e)
        {
            OnActivePanelChanged(new DockablePanelEventArgs(((Form)sender).Name));
        }

        public void HidePanel(string key)
        {
            Form form = GetFormByKey(key);
            form?.Hide();
        }

        public void ShowPanel(string key)
        {
            Form form = GetFormByKey(key);
            form?.Show();
        }

        private Form GetFormByKey(string key)
        {
            return _forms.FirstOrDefault(_ => _.Name == key);
        }

        public void OnImportsSatisfied()
        {
            if (Shell is Form form)
            {
                form.IsMdiContainer = true;
            }
        }
    }
}