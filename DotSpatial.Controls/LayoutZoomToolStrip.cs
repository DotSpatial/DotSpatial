// ********************************************************************************************************
// Product Name: DotSpatial.Layout.LayoutToolStrip
// Description:  A tool strip designed to work along with the layout engine
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll Version 6.0
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Jul, 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A Brian Marchioni original toolstrip... preloaded with content.
    /// </summary>
    [ToolboxItem(true)]
    public class LayoutZoomToolStrip : ToolStrip
    {
        #region "Private Variables"

        private ToolStripButton _btnZoomFullExtent;
        private ToolStripButton _btnZoomIn;
        private ToolStripButton _btnZoomOut;
        private ToolStripComboBox _comboZoom;
        private LayoutControl _layoutControl;

        #endregion

        #region "Constructor"

        /// <summary>
        /// Creates an instance of the toolstrip
        /// </summary>
        public LayoutZoomToolStrip()
        {
            InitializeComponent();
        }

        #endregion

        #region "properties"

        /// <summary>
        /// The layout control associated with this toolstrip
        /// </summary>
        [Browsable(false)]
        public LayoutControl LayoutControl
        {
            get { return _layoutControl; }
            set
            {
                _layoutControl = value;
                if (_layoutControl == null) return;
                _layoutControl.ZoomChanged += _layoutControl_ZoomChanged;
                _layoutControl_ZoomChanged(null, null);
            }
        }

        #endregion

        private void InitializeComponent()
        {
            this._btnZoomIn = new ToolStripButton();
            this._btnZoomOut = new ToolStripButton();
            this._btnZoomFullExtent = new ToolStripButton();
            this._comboZoom = new ToolStripComboBox();
            this.SuspendLayout();
            //
            // _btnZoomIn
            //
            this._btnZoomIn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnZoomIn.Image = Images.layout_zoom_in.ToBitmap();
            this._btnZoomIn.ImageTransparentColor = Color.Magenta;
            this._btnZoomIn.Name = "_btnZoomIn";
            this._btnZoomIn.Size = new Size(23, 22);
            this._btnZoomIn.Text = MessageStrings.LayoutToolStripZoomIn;
            this._btnZoomIn.Click += this._btnZoomIn_Click;
            //
            // _btnZoomOut
            //
            this._btnZoomOut.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnZoomOut.Image = Images.layout_zoom_out.ToBitmap();
            this._btnZoomOut.ImageTransparentColor = Color.Magenta;
            this._btnZoomOut.Name = "_btnZoomOut";
            this._btnZoomOut.Size = new Size(23, 22);
            this._btnZoomOut.Text = MessageStrings.LayoutToolStripZoomOut;
            this._btnZoomOut.Click += this._btnZoomOut_Click;
            //
            // _btnZoomFullExtent
            //
            this._btnZoomFullExtent.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnZoomFullExtent.Image = Images.layout_zoom_full_extent.ToBitmap();
            this._btnZoomFullExtent.ImageTransparentColor = Color.Magenta;
            this._btnZoomFullExtent.Name = "_btnZoomFullExtent";
            this._btnZoomFullExtent.Size = new Size(23, 22);
            this._btnZoomFullExtent.Text = MessageStrings.LayoutToolStripZoomFull;
            this._btnZoomFullExtent.Click += this._btnZoomFullExtent_Click;
            //
            // _comboZoom
            //
            this._comboZoom.Items.AddRange(new object[] {
            "50%",
            "75%",
            "100%",
            "150%",
            "200%",
            "300%"});
            this._comboZoom.Name = "_comboZoom";
            this._comboZoom.Size = new Size(75, 21);
            this._comboZoom.SelectedIndexChanged += this._comboZoom_SelectedIndexChanged;
            this._comboZoom.KeyPress += this._comboZoom_KeyPress;
            //
            // LayoutToolStrip
            //
            this.Items.AddRange(new ToolStripItem[] {
            this._btnZoomIn,
            this._btnZoomOut,
            this._btnZoomFullExtent,
            this._comboZoom});
            this.ResumeLayout(false);
        }

        #region "Envent Handlers"

        private void _comboZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            string input = _comboZoom.Text.Replace("%", string.Empty);
            _layoutControl.Zoom = Convert.ToInt32(input) / 100F;
        }

        private void _comboZoom_KeyPress(object sender, KeyPressEventArgs e)
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

        private void _layoutControl_ZoomChanged(object sender, EventArgs e)
        {
            _comboZoom.Text = String.Format("{0:0}", _layoutControl.Zoom * 100) + "%";
        }

        //Fires when the user clicks the zoom to full extent button
        private void _btnZoomFullExtent_Click(object sender, EventArgs e)
        {
            _layoutControl.ZoomFitToScreen();
        }

        //Fires the zoom in control on the modeler
        private void _btnZoomIn_Click(object sender, EventArgs e)
        {
            _layoutControl.ZoomIn();
        }

        //Fires the zoom out control on the modeler
        private void _btnZoomOut_Click(object sender, EventArgs e)
        {
            _layoutControl.ZoomOut();
        }

        #endregion
    }
}