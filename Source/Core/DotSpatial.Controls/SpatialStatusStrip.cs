// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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
    /// A pre-configured status strip with a thread safe Progress function.
    /// </summary>
    [ToolboxBitmap(typeof(SpatialStatusStrip), "SpatialStatusStrip.ico")]
    [PartNotDiscoverable] // Do not allow discover this class by MEF
    public partial class SpatialStatusStrip : StatusStrip, IStatusControl
    {
        #region Fields

        private readonly Dictionary<StatusPanel, PanelGuiElements> _panels = new();

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpatialStatusStrip"/> class which has a built in, thread safe Progress handler.
        /// </summary>
        public SpatialStatusStrip()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

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

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Add(StatusPanel panel)
        {
            if (panel == null) throw new ArgumentNullException(nameof(panel));

            ToolStripProgressBar pb = null;
            if (panel is ProgressStatusPanel psp)
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
                Spring = panel.Width == 0,
                TextAlign = ContentAlignment.MiddleLeft
            };
            Items.Add(sl);

            _panels.Add(panel, new PanelGuiElements
            {
                Caption = sl,
                Progress = pb
            });

            panel.PropertyChanged += PanelOnPropertyChanged;
        }

        /// <summary>
        /// This method is thread safe so that people calling this method don't cause a cross-thread violation
        /// by updating the progress indicator from a different thread.
        /// </summary>
        /// <param name="percent">The integer percent from 0 to 100.</param>
        /// <param name="message">A message including the percent information if wanted.</param>
        public void Progress(int percent, string message)
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

        /// <summary>
        /// Resets the progress. This method is thread safe so that people calling this method don't cause a cross-thread violation
        /// by updating the progress indicator from a different thread.
        /// </summary>
        public void Reset()
        {
            Progress(0, string.Empty);
        }

        /// <inheritdoc />
        public void Remove(StatusPanel panel)
        {
            if (panel == null) throw new ArgumentNullException(nameof(panel));

            panel.PropertyChanged -= PanelOnPropertyChanged;

            PanelGuiElements panelDesc;
            if (!_panels.TryGetValue(panel, out panelDesc)) return;

            if (panelDesc.Caption != null)
                Items.Remove(panelDesc.Caption);
            if (panelDesc.Progress != null)
                Items.Remove(panelDesc.Progress);
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
                if (e.Item is ToolStripProgressBar pb)
                {
                    ProgressBar = pb;
                }
            }

            if (ProgressLabel == null)
            {
                if (e.Item is ToolStripStatusLabel sl)
                {
                    ProgressLabel = sl;
                }
            }
        }

        /// <inheritdoc />
        protected override void OnItemRemoved(ToolStripItemEventArgs e)
        {
            base.OnItemRemoved(e);

            if (ProgressBar == e.Item) ProgressBar = null;
            if (ProgressLabel == e.Item) ProgressLabel = null;
        }

        private static string GetKeyName<T>(string key)
        {
            return typeof(T).Name + key;
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

        private void UpdateProgress(int percent, string message)
        {
            if (ProgressBar != null)
                ProgressBar.Value = percent;
            if (ProgressLabel != null)
                ProgressLabel.Text = message;
            Refresh();
        }

        #endregion

        #region Classes

        /// <summary>
        /// PanelGuiElements.
        /// </summary>
        internal class PanelGuiElements
        {
            #region Properties

            /// <summary>
            /// Gets or sets the caption.
            /// </summary>
            public ToolStripStatusLabel Caption { get; set; }

            /// <summary>
            /// Gets or sets the progress bar.
            /// </summary>
            public ToolStripProgressBar Progress { get; set; }

            #endregion
        }

        #endregion
    }
}