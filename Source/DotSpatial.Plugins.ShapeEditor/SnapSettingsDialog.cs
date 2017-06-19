// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Windows.Forms;

namespace DotSpatial.Plugins.ShapeEditor
{
    /// <summary>
    /// This dialog is used for de-/activing snapping.
    /// </summary>
    public partial class SnapSettingsDialog : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SnapSettingsDialog"/> class.
        /// </summary>
        public SnapSettingsDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets a value indicating whether snapping is active.
        /// </summary>
        public bool DoSnapping
        {
            get { return cbPerformSnap.Checked; }
            set { cbPerformSnap.Checked = value; }
        }
    }
}
