// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A Brian Marchioni original toolstrip... preloaded with content.
    /// </summary>
    // This control will no longer be visible
    [ToolboxItem(false)]
    public partial class LayoutMapToolStrip : LayoutToolStrip
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

        #region Methods

        // Fires when the user clicks the pan button.
        private void BtnPanClick(object sender, EventArgs e)
        {
            LayoutControl.MouseMode = MouseMode.StartPanMap;
            SetChecked(_btnPan);
        }

        // Fires when the user clicks the zoom to full extent button
        private void BtnZoomFullExtentClick(object sender, EventArgs e)
        {
            (LayoutControl.SelectedLayoutElements[0] as LayoutMap)?.ZoomToFullExtent();
        }

        // Fires the zoom in control on the modeler
        private void BtnZoomInClick(object sender, EventArgs e)
        {
            (LayoutControl.SelectedLayoutElements[0] as LayoutMap)?.ZoomInMap();
        }

        // Fires the zoom out control on the modeler
        private void BtnZoomOutClick(object sender, EventArgs e)
        {
            (LayoutControl.SelectedLayoutElements[0] as LayoutMap)?.ZoomOutMap();
        }

        // Zoom the map to the extent of the layout
        private void BtnZoomViewExtentClick(object sender, EventArgs e)
        {
            (LayoutControl.SelectedLayoutElements[0] as LayoutMap)?.ZoomViewExtent();
        }

        // If the toolbar is disabled we disable the pan checked button state
        private void LayoutMapToolStripEnabledChanged(object sender, EventArgs e)
        {
            _btnPan.Checked = false;
        }

        #endregion
    }
}