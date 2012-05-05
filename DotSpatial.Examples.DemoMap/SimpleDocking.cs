// -----------------------------------------------------------------------
// <copyright file="SimpleDocking.cs" company="DotSpatial Team">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DotSpatial.Controls.Docking;

namespace DemoMap
{
    /// <summary>
    ///
    /// </summary>
    public class SimpleDocking // Add this interface to use this docking manager : IDockManager
    {
        private List<Form> forms = new List<Form>();

        #region IDockManager Members

        /// <summary>
        /// Removes the specified panel.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(string key)
        {
            foreach (Form form in forms)
            {
                if (form.Name == key)
                {
                    form.Close();
                    forms.Remove(form);
                    break;
                }
            }
        }

        /// <summary>
        /// Occurs when the active panel is changed.
        /// </summary>
        public event EventHandler<DockablePanelEventArgs> ActivePanelChanged;

        /// <summary>
        /// Selects the panel.
        /// </summary>
        /// <param name="key">The key.</param>
        public void SelectPanel(string key)
        {
        }

        /// <summary>
        /// Adds the specified panel.
        /// </summary>
        /// <param name="panel"></param>
        public void Add(DockablePanel panel)
        {
            Add(panel.Key, panel.Caption, panel.InnerControl, panel.Dock);
        }

        /// <summary>
        /// Resets the layout of the dock panels to a developer specified location.
        /// </summary>
        public void ResetLayout()
        {
        }

        public event EventHandler<DockablePanelEventArgs> PanelClosed;

        public event EventHandler<DockablePanelEventArgs> PanelAdded;

        public event EventHandler<DockablePanelEventArgs> PanelRemoved;

        #endregion

        #region OnPanelRemoved

        /// <summary>
        /// Triggers the PanelRemoved event.
        /// </summary>
        public virtual void OnPanelRemoved(DockablePanelEventArgs ea)
        {
            if (PanelRemoved != null)
                PanelRemoved(null/*this*/, ea);
        }

        #endregion

        #region OnPanelAdded

        /// <summary>
        /// Triggers the PanelAdded event.
        /// </summary>
        public virtual void OnPanelAdded(DockablePanelEventArgs ea)
        {
            if (PanelAdded != null)
                PanelAdded(null/*this*/, ea);
        }

        #endregion

        #region OnPanelClosed

        /// <summary>
        /// Triggers the PanelClosed event.
        /// </summary>
        public virtual void OnPanelClosed(DockablePanelEventArgs ea)
        {
            if (PanelClosed != null)
                PanelClosed(null/*this*/, ea);
        }

        #endregion

        #region OnActivePanelChanged

        /// <summary>
        /// Triggers the ActivePanelChanged event.
        /// </summary>
        public virtual void OnActivePanelChanged(DockablePanelEventArgs ea)
        {
            if (ActivePanelChanged != null)
                ActivePanelChanged(null/*this*/, ea);
        }

        #endregion

        /// <summary>
        /// Adds the specified panel.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="caption">The caption of the panel and any tab button.</param>
        /// <param name="panel">The panel.</param>
        /// <param name="dockStyle">The dock location.</param>
        public void Add(string key, string caption, Control panel, DockStyle dockStyle)
        {
            panel.Dock = DockStyle.Fill;

            var form = new Form();
            form.Controls.Add(panel);
            form.Name = key;
            form.Width = panel.Width;
            form.Height = panel.Height;
            form.Show();
            form.Activated += form_Activated;
            forms.Add(form);
        }

        private void form_Activated(object sender, EventArgs e)
        {
            OnActivePanelChanged(new DockablePanelEventArgs((sender as Form).Name));
        }
    }
}