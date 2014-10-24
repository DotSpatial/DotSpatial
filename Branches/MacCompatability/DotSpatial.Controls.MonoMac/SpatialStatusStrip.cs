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
using DotSpatial.Controls.Header;
using MonoMac.AppKit;
using MonoMac.Foundation;

namespace DotSpatial.Controls.MonoMac
{
    /// <summary>
    /// A pre-configured status strip with a thread safe Progress function
    /// </summary>
    [ToolboxBitmap(typeof(SpatialStatusStrip), "SpatialStatusStrip.ico")]
    [PartNotDiscoverable] // Do not allow discover this class by MEF
    public partial class SpatialStatusStrip : NSView, IStatusControl
    {
        private readonly Dictionary<StatusPanel, PanelGuiElements> _panels = new Dictionary<StatusPanel, PanelGuiElements>();

        /// <summary>
        /// Creates a new instance of the StatusStrip which has a built in, thread safe Progress handler
        /// </summary>
        public SpatialStatusStrip()
        {
            AutoresizingMask = NSViewResizingMask.WidthSizable;
        }

        public override void ViewDidMoveToSuperview ()
        {
            Frame = new System.Drawing.RectangleF(10, 5, Superview.Bounds.Width, 20);
        }

        /// <summary>
        /// Gets or sets the progress bar. By default, the first ToolStripProgressBar that is added to the tool strip.
        /// </summary>
        /// <value>
        /// The progress bar.
        /// </value>
        [Description("Gets or sets the progress bar. By default, the first ToolStripProgressBar that is added to the tool strip.")]
        public NSProgressIndicator ProgressBar { get; set; }

        /// <summary>
        /// Gets or sets the progress label. By default, the first ToolStripStatusLabel that is added to the tool strip.
        /// </summary>
        /// <value>
        /// The progress label.
        /// </value>
        [Description("Gets or sets the progress label. By default, the first ToolStripStatusLabel that is added to the tool strip.")]
        public NSTextField ProgressLabel { get; set; }

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
            UpdateProgress(percent, message);
        }

        #endregion

        private void UpdateProgress(int percent, string message)
        {
            if (ProgressBar != null)
            {
                ProgressBar.DoubleValue = percent;
            }
            if (ProgressLabel != null)
            {
                ProgressLabel.StringValue = message != null ? message : "";
                ProgressLabel.SizeToFit();
            }
        }

        #region IStatusControl implementation

        public void Add(StatusPanel panel)
        {
            if (panel == null) throw new ArgumentNullException("panel");

            NSProgressIndicator pb = null;
            var psp = panel as ProgressStatusPanel;
            if (psp != null)
            {
                pb = new NSProgressIndicator
                {
                    ControlSize = NSControlSize.Regular,
                    Identifier = GetKeyName<NSProgressIndicator>(panel.Key),
                    Frame = new RectangleF(0, 0, 100, 0),
                    Indeterminate = false
                };
                pb.SizeToFit ();
                AddSubview(pb);
            }

            var caption = panel.Caption != null ? panel.Caption : "";
            var sl = new NSTextField
            {
                Identifier = GetKeyName<NSTextField>(panel.Key),
                StringValue = caption,
                Frame = new RectangleF(105, 0, 0, 0),
                Bezeled = false,
                DrawsBackground = false,
                Editable = false,
                Selectable = false
            };
            sl.SizeToFit();
            AddSubview(sl);

            _panels.Add(panel, new PanelGuiElements { Caption = sl, Progress = pb });
            ProgressBar = pb;
            ProgressLabel = sl;

            panel.PropertyChanged += PanelOnPropertyChanged;
        }

        private void PanelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var panel = (StatusPanel)sender;
            UpdatePanelGuiProps(panel, e.PropertyName);
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
                        panelDesc.Caption.StringValue = sender.Caption != null ? sender.Caption : "";
                        panelDesc.Caption.SizeToFit();
                    }
                    break;
                case "Percent":
                    if (panelDesc.Progress != null && sender is ProgressStatusPanel)
                    {
                        panelDesc.Progress.DoubleValue = ((ProgressStatusPanel)sender).Percent;
                    }
                    break;
            }
        }

        public void Remove(StatusPanel panel)
        {
            if (panel == null) throw new ArgumentNullException("panel");

            panel.PropertyChanged -= PanelOnPropertyChanged;

            PanelGuiElements panelDesc;
            if (!_panels.TryGetValue(panel, out panelDesc)) return;

            if (panelDesc.Caption != null) panelDesc.Caption.RemoveFromSuperview();
            if (panelDesc.Progress != null) panelDesc.Progress.RemoveFromSuperview();

            if (ProgressBar == panelDesc.Progress) ProgressBar = null;
            if (ProgressLabel == panelDesc.Caption) ProgressLabel = null;
        }

        private static string GetKeyName<T>(string key)
        {
            return typeof(T).Name + key;
        }

        #endregion

        internal class PanelGuiElements
        {
            public NSTextField Caption { get; set; }
            public NSProgressIndicator Progress { get; set; }
        }
    }
}