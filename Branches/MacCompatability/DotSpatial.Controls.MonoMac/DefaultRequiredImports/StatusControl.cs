using System.ComponentModel.Composition;
using System.Linq;
using DotSpatial.Controls.DefaultRequiredImports;
using DotSpatial.Controls.Header;
using DotSpatial.Extensions;
using MonoMac.AppKit;

namespace DotSpatial.Controls.MonoMac.DefaultRequiredImports
{
    /// <summary>
    /// Default Status Control. It will used when no custom implementation of IStatusControl where found.
    /// </summary>
    [DefaultRequiredImport]
    internal class StatusControl : IStatusControl, ISatisfyImportsExtension
    {
        #region Fields
        
        private SpatialStatusStrip _statusStrip;
        private bool _isActivated;

        #endregion

        [Import]
        private AppManager App { get; set; }

        [Import("Shell", typeof(NSView))]
        private NSView Shell { get; set; }

        #region IStatusControl Members
        
        public void Add(StatusPanel panel)
        {
            if (!_isActivated) return;
            _statusStrip.Add(panel);
        }

        public void Progress(string key, int percent, string message)
        {
            if (!_isActivated) return;
            _statusStrip.Progress(key, percent, message);
        }

        public void Remove(StatusPanel panel)
        {
            if (!_isActivated) return;
            _statusStrip.Remove(panel);
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
                _statusStrip = new SpatialStatusStrip();
                Shell.AddSubview(_statusStrip);

                // adding initial status panel to the status strip control
                Add(new ProgressStatusPanel());
            }
        }
    }
}