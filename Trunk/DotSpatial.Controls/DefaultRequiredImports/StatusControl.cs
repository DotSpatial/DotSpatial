using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Controls.Header;
using DotSpatial.Extensions;

namespace DotSpatial.Controls.DefaultRequiredImports
{
    /// <summary>
    /// Default Status Control. It will used when no custom implementation of IStatusControl where found.
    /// </summary>
    [DefaultRequiredImport]
    internal class StatusControl : IStatusControl, ISatisfyImportsExtension
    {
        #region Fields

        private ProgressStatusPanel _defaultStatusPanel;
        private StatusStrip _statusStrip;
        private bool _isActivated;
        private readonly Dictionary<StatusPanel, PanelGuiElements> _panels = new Dictionary<StatusPanel, PanelGuiElements>();

        #endregion

        [Import]
        private AppManager App { get; set; }

        [Import("Shell", typeof(ContainerControl))]
        private ContainerControl Shell { get; set; }

        #region IStatusControl Members
        
        public void Add(StatusPanel panel)
        {
            if (panel == null) throw new ArgumentNullException("panel");
            if (!_isActivated) return;

            ToolStripProgressBar pb = null;
            var psp = panel as ProgressStatusPanel;
            if (psp != null)
            {
                if (_defaultStatusPanel == null) _defaultStatusPanel = psp;

                pb = new ToolStripProgressBar
                              {
                                  Name = PanelGuiElements.GetKeyName<ToolStripProgressBar>(panel.Key),
                                  Width = 100,
                                  Alignment = ToolStripItemAlignment.Left
                              };
                _statusStrip.Items.Add(pb);
            }

            var sl = new ToolStripStatusLabel
            {
                Name = PanelGuiElements.GetKeyName<ToolStripStatusLabel>(panel.Key),
                Text = panel.Caption,
                Spring = (panel.Width == 0),
                TextAlign = ContentAlignment.MiddleLeft
            };
            _statusStrip.Items.Add(sl);
            
            _panels.Add(panel, new PanelGuiElements{Caption = sl, Progress = pb});

            panel.PropertyChanged += PanelOnPropertyChanged;
        }

        private void PanelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var panel = (StatusPanel)sender;
            if (_statusStrip.InvokeRequired)
            {
                _statusStrip.BeginInvoke((Action<StatusPanel, string>)UpdatePanelGuiProps, panel, e.PropertyName);
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
                        panelDesc.Progress.Value = ((ProgressStatusPanel) sender).Percent;
                    }
                    break;
            }
            _statusStrip.Refresh();
        }

        public void Progress(string key, int percent, string message)
        {
            if (!_isActivated) return;

            if (_defaultStatusPanel == null) return;
            _defaultStatusPanel.Caption = message;
            _defaultStatusPanel.Percent = percent;
        }

        public void Remove(StatusPanel panel)
        {
            if (panel == null) throw new ArgumentNullException("panel");
            if (!_isActivated) return;

            panel.PropertyChanged -= PanelOnPropertyChanged;
            if (_defaultStatusPanel == panel) _defaultStatusPanel = null;

            PanelGuiElements panelDesc;
            if (!_panels.TryGetValue(panel, out panelDesc)) return;

            if (panelDesc.Caption != null)
                _statusStrip.Items.Remove(panelDesc.Caption);
            if (panelDesc.Progress != null)
                _statusStrip.Items.Remove(panelDesc.Progress);
        }

        #endregion

        public int Priority { get { return 2; } }

        public void Activate()
        {
            if (_isActivated) return;

            var statusControls = App.CompositionContainer.GetExportedValues<IStatusControl>().ToList();

            // Activate only if there are no other IStatusControl implementations and
            // custom ProgressHandler not yet set
            if (App.ProgressHandler == null &&
                statusControls.Count == 1 && statusControls[0].GetType() == GetType())
            {
                _isActivated = true;

                // adding the status strip control
                _statusStrip = new StatusStrip();
                Shell.Controls.Add(_statusStrip);

                // adding initial status panel to the status strip control
                Add(new ProgressStatusPanel());
            }
        }
    }
}