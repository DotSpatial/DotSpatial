using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;
using DotSpatial.Controls.Header;

namespace DemoMap
{
    public class SimpleStatusControl : IStatusControl, IPartImportsSatisfiedNotification
    {
        private StatusPanel defaultStatusPanel;
        private StatusStrip statusStrip;

        [Import("Shell", typeof(ContainerControl))]
        private ContainerControl Shell { get; set; }

        #region IPartImportsSatisfiedNotification Members

        public void OnImportsSatisfied()
        {
            statusStrip = new StatusStrip();

            statusStrip.Location = new Point(0, 285);
            statusStrip.Name = "statusStrip1";
            statusStrip.Size = new Size(508, 22);
            statusStrip.TabIndex = 0;
            statusStrip.Text = String.Empty;

            //adding the status strip control
            Shell.Controls.Add(this.statusStrip);

            //adding one initial status panel to the status strip control
            defaultStatusPanel = new StatusPanel();
            this.Add(defaultStatusPanel);
        }

        #endregion

        #region IStatusControl Members

        /// <summary>
        /// Adds a status panel to the status strip
        /// </summary>
        /// <param name="panel">the user-specified status panel</param>
        public void Add(StatusPanel panel)
        {
            ToolStripStatusLabel myLabel = new ToolStripStatusLabel();
            myLabel.Name = panel.Key;
            myLabel.Text = panel.Caption;
            myLabel.Width = panel.Width;

            panel.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
                {
                    var item = sender as StatusPanel;

                    myLabel.Text = item.Caption;
                    myLabel.Width = item.Width;
                };

            statusStrip.Items.Add(myLabel);
        }

        public void Progress(string key, int percent, string message)
        {
            defaultStatusPanel.Caption = message;
            if (!statusStrip.InvokeRequired)
            {
                // most actions happen on one thread and the status bar never repaints itself until the end of a process unless
                // we call Application.DoEvents() or refresh the control.
                statusStrip.Refresh();
            }
        }

        private bool _cancel;
        public bool Cancel
        {
            set { _cancel = value; }
            get { return _cancel; }
        }

        public void Remove(StatusPanel panel)
        {
            statusStrip.Items.RemoveByKey(panel.Key);
        }

        #endregion
    }
}