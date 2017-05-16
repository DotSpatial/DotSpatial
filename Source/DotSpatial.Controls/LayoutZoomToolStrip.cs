// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A Brian Marchioni original toolstrip... preloaded with content.
    /// </summary>
    // This control will no longer be visible
    [ToolboxItem(false)]
    public partial class LayoutZoomToolStrip : ToolStrip
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutZoomToolStrip"/> class.
        /// </summary>
        public LayoutZoomToolStrip()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the layout control associated with this toolstrip.
        /// </summary>
        [Browsable(false)]
        public LayoutControl LayoutControl
        {
            get
            {
                return _layoutControl;
            }

            set
            {
                _layoutControl = value;
                if (_layoutControl == null) return;
                _layoutControl.ZoomChanged += LayoutControlZoomChanged;
                LayoutControlZoomChanged(null, null);
            }
        }

        #endregion

        #region Methods

        // Fires when the user clicks the zoom to full extent button
        private void BtnZoomFullExtentClick(object sender, EventArgs e)
        {
            _layoutControl.ZoomFitToScreen();
        }

        // Fires the zoom in control on the modeler
        private void BtnZoomInClick(object sender, EventArgs e)
        {
            _layoutControl.ZoomIn();
        }

        // Fires the zoom out control on the modeler
        private void BtnZoomOutClick(object sender, EventArgs e)
        {
            _layoutControl.ZoomOut();
        }

        private void ComboZoomKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                try
                {
                    string input = _comboZoom.Text.Replace("%", string.Empty);
                    float newZoom = Convert.ToInt32(input) / 100F;
                    _layoutControl.Zoom = newZoom;
                }
                catch
                {
                    _comboZoom.Text = (_layoutControl.Zoom * 100).ToString(CultureInfo.InvariantCulture) + "%";
                }
            }
        }

        private void ComboZoomSelectedIndexChanged(object sender, EventArgs e)
        {
            string input = _comboZoom.Text.Replace("%", string.Empty);
            _layoutControl.Zoom = Convert.ToInt32(input) / 100F;
        }

        private void LayoutControlZoomChanged(object sender, EventArgs e)
        {
            _comboZoom.Text = string.Format("{0:0}", _layoutControl.Zoom * 100) + "%";
        }

        #endregion
    }
}