// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A Brian Marchioni original toolstrip... preloaded with content.
    /// </summary>
    // This control will no longer be visible
    [ToolboxItem(false)]
    public partial class LayoutMapToolStrip : ToolStrip
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutMapToolStrip"/> class.
        /// </summary>
        public LayoutMapToolStrip()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the layout control associated with this toolstrip.
        /// </summary>
        [Browsable(false)]
        public LayoutControl LayoutControl { get; set; }

        #endregion

        #region Methods

        // Fires when the user clicks the pan button
        private void BtnPanClick(object sender, EventArgs e)
        {
            LayoutControl.MapPanMode = _btnPan.Checked;
        }

        // Fires when the user clicks the zoom to full extent button
        private void BtnZoomFullExtentClick(object sender, EventArgs e)
        {
            LayoutControl.ZoomFullExtentMap(LayoutControl.SelectedLayoutElements[0] as LayoutMap);
        }

        // Fires the zoom in control on the modeler
        private void BtnZoomInClick(object sender, EventArgs e)
        {
            LayoutControl.ZoomInMap(LayoutControl.SelectedLayoutElements[0] as LayoutMap);
        }

        // Fires the zoom out control on the modeler
        private void BtnZoomOutClick(object sender, EventArgs e)
        {
            LayoutControl.ZoomOutMap(LayoutControl.SelectedLayoutElements[0] as LayoutMap);
        }

        // Zoom the map to the extent of the layout
        private void BtnZoomViewExtentClick(object sender, EventArgs e)
        {
            LayoutControl.ZoomFullViewExtentMap(LayoutControl.SelectedLayoutElements[0] as LayoutMap);
        }

        // If the toolbar is disabled we disable the pan checked button state
        private void LayoutMapToolStripEnabledChanged(object sender, EventArgs e)
        {
            _btnPan.Checked = false;
        }

        #endregion
    }
}