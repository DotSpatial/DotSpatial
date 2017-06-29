// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/12/2009 10:46:15 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;
using DotSpatial.Controls.Header;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A pre-configured status strip with a thread safe Progress function
    /// </summary>
    [ToolboxBitmap(typeof(SpatialStatusStrip), "SpatialStatusStrip.ico")]
    [PartNotDiscoverable] // Do not allow discover this class by MEF
    public partial class SpatialStatusStrip : StatusStrip, IStatusControl
    {
        private readonly Dictionary<StatusPanel, PanelGuiElements> _panels = new Dictionary<StatusPanel, PanelGuiElements>();

        /// <summary>
        /// Creates a new instance of the StatusStrip which has a built in, thread safe Progress handler
        /// </summary>
        public SpatialStatusStrip()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the progress bar. By default, the first ToolStripProgressBar that is added to the tool strip.
        /// </summary>
        /// <value>
        /// The progress bar.
        /// </value>
        [Description("Gets or sets the progress bar. By default, the first ToolStripProgressBar that is added to the tool strip.")]
        public ToolStripProgressBar ProgressBar { get; set; }

        /// <summary>
        /// Gets or sets the progress label. By default, the first ToolStripStatusLabel that is added to the tool strip.
        /// </summary>
        /// <value>
        /// The progress label.
        /// </value>
        [Description("Gets or sets the progress label. By default, the first ToolStripStatusLabel that is added to the tool strip.")]
        public ToolStripStatusLabel ProgressLabel { get; set; }

        #region IProgressHandler Members

        /// <summary>
        /// This method is thread safe so that people calling this method don't cause a cross-thread violation
        /// by updating the progress indicator from a different thread
        /// </summary>
        /// <param name="key">A string message with just a description of what is happening, but no percent completion information</param>
        /// <param name="percent">The integer percent from 0 to 100</param>
        /// <param name="message">A message</param>
        public void Progress(string key, int percent, string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action<int, string>)UpdateProgress, percent, message);
            }
            else
            {
                UpdateProgress(percent, message);
            }
        }

        #endregion

        private void UpdateProgress(int percent, string message)
        {
            if (ProgressBar != null)
                ProgressBar.Value = percent;
            if (ProgressLabel != null)
                ProgressLabel.Text = message;
            Refresh();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ToolStrip.ItemAdded"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemEventArgs"/> that contains the event data.</param>
        protected override void OnItemAdded(ToolStripItemEventArgs e)
        {
            base.OnItemAdded(e);

            if (ProgressBar == null)
            {
                var pb = e.Item as ToolStripProgressBar;
                if (pb != null)
                {
                    ProgressBar = pb;
                }
            }

            if (ProgressLabel == null)
            {
                var sl = e.Item as ToolStripStatusLabel;
                if (sl != null)
                {
                    ProgressLabel = sl;
                }
            }
        }

        protected override void OnItemRemoved(ToolStripItemEventArgs e)
        {
            base.OnItemRemoved(e);

            if (ProgressBar == e.Item) ProgressBar = null;
            if (ProgressLabel == e.Item) ProgressLabel = null;
        }

        #region IStatusControl implementation

        public void Add(StatusPanel panel)
        {
            if (panel == null) throw new ArgumentNullException("panel");

            ToolStripProgressBar pb = null;
            var psp = panel as ProgressStatusPanel;
            if (psp != null)
            {
                pb = new ToolStripProgressBar
                {
                    Name = GetKeyName<ToolStripProgressBar>(panel.Key),
                    Width = 100,
                    Alignment = ToolStripItemAlignment.Left
                };
                Items.Add(pb);
            }

            var sl = new ToolStripStatusLabel
            {
                Name = GetKeyName<ToolStripStatusLabel>(panel.Key),
                Text = panel.Caption,
                Spring = (panel.Width == 0),
                TextAlign = ContentAlignment.MiddleLeft
            };
            Items.Add(sl);

            _panels.Add(panel, new PanelGuiElements { Caption = sl, Progress = pb });

            panel.PropertyChanged += PanelOnPropertyChanged;
        }

        private void PanelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var panel = (StatusPanel)sender;
            if (InvokeRequired)
            {
                BeginInvoke((Action<StatusPanel, string>)UpdatePanelGuiProps, panel, e.PropertyName);
            }
            else
            {
                UpdatePanelGuiProps(panel, e.PropertyName);
            }
        }

        private void UpdatePanelGuiProps(StatusPanel sender, string propertyName)
        {
            PanelGuiElements panelDesc;
            if (!_panels.TryGetValue(sender, out panelDesc)) return;

            switch (propertyName)
            {
                case "Caption":
                    if (panelDesc.Caption != null)
                    {
                        panelDesc.Caption.Text = sender.Caption;
                    }
                    break;
                case "Percent":
                    if (panelDesc.Progress != null && sender is ProgressStatusPanel)
                    {
                        panelDesc.Progress.Value = ((ProgressStatusPanel)sender).Percent;
                    }
                    break;
            }
            Refresh();
        }

        public void Remove(StatusPanel panel)
        {
            if (panel == null) throw new ArgumentNullException("panel");

            panel.PropertyChanged -= PanelOnPropertyChanged;

            PanelGuiElements panelDesc;
            if (!_panels.TryGetValue(panel, out panelDesc)) return;

            if (panelDesc.Caption != null)
                Items.Remove(panelDesc.Caption);
            if (panelDesc.Progress != null)
                Items.Remove(panelDesc.Progress);
        }

        private static string GetKeyName<T>(string key)
        {
            return typeof(T).Name + key;
        }

        #endregion

        internal class PanelGuiElements
        {
            public ToolStripStatusLabel Caption { get; set; }
            public ToolStripProgressBar Progress { get; set; }
        }
    }
}