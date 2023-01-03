using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;
using DotSpatial.Controls.Header;

namespace DotSpatial.Examples.AppManagerCustomizationRuntime.AppManagerRequirements
{
    /// <summary>
    /// Simple implmenentation of IStatusControl.
    /// It shows a technique how to create own status control as extension.
    /// You may delete this class in your application. In this case default status control will be used.
    /// </summary>
    internal class StatusControl: IStatusControl, IPartImportsSatisfiedNotification
    {
        private StatusPanel defaultStatusPanel;
        private StatusStrip statusStrip;

        [Import("Shell", typeof(ContainerControl))]
        private ContainerControl Shell { get; set; }

        #region IPartImportsSatisfiedNotification Members

        public void OnImportsSatisfied()
        {
            statusStrip = new StatusStrip
                          {
                              ForeColor = Color.Blue
                          };

            // adding the status strip control
            Shell.Controls.Add(statusStrip);

            // adding one initial status panel to the status strip control
            defaultStatusPanel = new StatusPanel();
            Add(defaultStatusPanel);
        }

        #endregion

        #region IStatusControl Members

        /// <summary>
        /// Adds a status panel to the status strip
        /// </summary>
        /// <param name="panel">the user-specified status panel</param>
        public void Add(StatusPanel panel)
        {
            var myLabel = new ToolStripStatusLabel
                          {
                              Name = panel.Key,
                              Text = panel.Caption,
                              Width = panel.Width,
                              Spring = (panel.Width == 0),
                              TextAlign = ContentAlignment.MiddleLeft
                          };

            panel.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
                {
                    var item = (StatusPanel)sender;

                    myLabel.Text = item.Caption;
                    myLabel.Width = item.Width;
                };

            statusStrip.Items.Add(myLabel);
        }

        public void Progress(string key, int percent, string message)
        {
            defaultStatusPanel.Caption = percent == 0 ? message : String.Format("{0}... {1}%", message, percent);

            if (!statusStrip.InvokeRequired)
            {
                // most actions happen on one thread and the status bar never repaints itself until the end of a process unless
                // we call Application.DoEvents() or refresh the control.
                statusStrip.Refresh();
            }
        }

        public void Remove(StatusPanel panel)
        {
            statusStrip.Items.RemoveByKey(panel.Key);
        }

        #endregion
    }
}