// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel.Composition;
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

        private bool _isActivated;

        private SpatialStatusStrip _statusStrip;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the priority.
        /// </summary>
        public int Priority { get; } = 2;

        [Import]
        private AppManager App { get; set; }

        [Import("Shell", typeof(ContainerControl))]
        private ContainerControl Shell { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Activate()
        {
            if (_isActivated) return;

            var statusControls = App.CompositionContainer.GetExportedValues<IStatusControl>().ToList();

            // Activate only if there are no other IStatusControl implementations and
            // custom ProgressHandler not yet set
            if (App.ProgressHandler == null && statusControls.Count == 1 && statusControls[0].GetType() == GetType())
            {
                _isActivated = true;

                // adding the status strip control
                _statusStrip = new SpatialStatusStrip();
                Shell.Controls.Add(_statusStrip);

                // adding initial status panel to the status strip control
                Add(new ProgressStatusPanel());
            }
        }

        /// <summary>
        /// Adds the given panel to the status strip.
        /// </summary>
        /// <param name="panel">Panel that gets added.</param>
        public void Add(StatusPanel panel)
        {
            if (!_isActivated) return;
            _statusStrip.Add(panel);
        }

        /// <summary>
        /// Shows the progress with the given message.
        /// </summary>
        /// <param name="key">A string message with just a description of what is happening, but no percent completion information.</param>
        /// <param name="percent">The integer percent from 0 to 100.</param>
        /// <param name="message">A message.</param>
        public void Progress(string key, int percent, string message)
        {
            if (!_isActivated) return;
            _statusStrip.Progress(key, percent, message);
        }

        /// <summary>
        /// Removes the given panel from the status strip.
        /// </summary>
        /// <param name="panel">Panel that gets removed.</param>
        public void Remove(StatusPanel panel)
        {
            if (!_isActivated) return;
            _statusStrip.Remove(panel);
        }

        #endregion
    }
}