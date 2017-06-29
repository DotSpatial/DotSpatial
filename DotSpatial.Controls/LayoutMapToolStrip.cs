// ********************************************************************************************************
// Product Name: DotSpatial.Layout.LayoutMapToolStrip
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
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A Brian Marchioni original toolstrip... preloaded with content.
    /// </summary>
    [ToolboxItem(true)]
    public class LayoutMapToolStrip : ToolStrip
    {
        #region "Private Variables"

        private ToolStripButton _btnPan;
        private ToolStripButton _btnZoomFullExtent;
        private ToolStripButton _btnZoomIn;
        private ToolStripButton _btnZoomOut;
        private ToolStripButton _btnZoomViewExtent;
        private LayoutControl _layoutControl;

        #endregion

        #region "Constructor"

        /// <summary>
        /// Creates an instance of the toolstrip
        /// </summary>
        public LayoutMapToolStrip()
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
            }
        }

        #endregion

        private void InitializeComponent()
        {
            this._btnZoomIn = new ToolStripButton();
            this._btnZoomOut = new ToolStripButton();
            this._btnZoomFullExtent = new ToolStripButton();
            this._btnZoomViewExtent = new ToolStripButton();
            this._btnPan = new ToolStripButton();
            this.SuspendLayout();
            //
            // _btnZoomIn
            //
            this._btnZoomIn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnZoomIn.Image = Images.ZoomInMap;
            this._btnZoomIn.ImageTransparentColor = Color.Magenta;
            this._btnZoomIn.Name = "_btnZoomIn";
            this._btnZoomIn.Size = new Size(23, 22);
            this._btnZoomIn.Text = MessageStrings.LayoutMapToolStripZoomIn;
            this._btnZoomIn.Click += this._btnZoomIn_Click;
            //
            // _btnZoomOut
            //
            this._btnZoomOut.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnZoomOut.Image = Images.ZoomOutMap;
            this._btnZoomOut.ImageTransparentColor = Color.Magenta;
            this._btnZoomOut.Name = "_btnZoomOut";
            this._btnZoomOut.Size = new Size(23, 22);
            this._btnZoomOut.Text = MessageStrings.LayoutMapToolStripZoomOut;
            this._btnZoomOut.Click += this._btnZoomOut_Click;
            //
            // _btnZoomFullExtent
            //
            this._btnZoomFullExtent.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnZoomFullExtent.Image = Images.ZoomFullMap;
            this._btnZoomFullExtent.ImageTransparentColor = Color.Magenta;
            this._btnZoomFullExtent.Name = "_btnZoomFullExtent";
            this._btnZoomFullExtent.Size = new Size(23, 22);
            this._btnZoomFullExtent.Text = MessageStrings.LayoutMapToolStripMaxExtent;
            this._btnZoomFullExtent.Click += this._btnZoomFullExtent_Click;
            //
            // _btnZoomFullExtent
            //
            this._btnZoomViewExtent.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnZoomViewExtent.Image = Images.ZoomFullView;
            this._btnZoomViewExtent.ImageTransparentColor = Color.Magenta;
            this._btnZoomViewExtent.Name = "_btnZoomViewExtent";
            this._btnZoomViewExtent.Size = new Size(23, 22);
            this._btnZoomViewExtent.Text = MessageStrings.LayoutMapToolStripViewExtent;
            this._btnZoomViewExtent.Click += _btnZoomViewExtent_Click;
            //
            // _btnPan
            //
            this._btnPan.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnPan.Image = Images.PanMap;
            this._btnPan.CheckOnClick = true;
            this._btnPan.Checked = false;
            this._btnPan.ImageTransparentColor = Color.Magenta;
            this._btnPan.Name = "_btnZoomFullExtent";
            this._btnPan.Size = new Size(23, 22);
            this._btnPan.Text = MessageStrings.LayoutMapToolStripPan;
            this._btnPan.Click += _btnPan_Click;
            //
            // LayoutToolStrip
            //
            this.Items.AddRange(new ToolStripItem[] {
            this._btnZoomIn,
            this._btnZoomOut,
            this._btnZoomFullExtent,
            this._btnZoomViewExtent,
            this._btnPan});
            this.EnabledChanged += LayoutMapToolStrip_EnabledChanged;
            this.Enabled = false;
            this.ResumeLayout(false);
        }

        #region "Envent Handlers"

        //Fires when the user clicks the zoom to full extent button
        private void _btnZoomFullExtent_Click(object sender, EventArgs e)
        {
            LayoutControl.ZoomFullExtentMap(_layoutControl.SelectedLayoutElements[0] as LayoutMap);
        }

        //Fires the zoom in control on the modeler
        private void _btnZoomIn_Click(object sender, EventArgs e)
        {
            LayoutControl.ZoomInMap(_layoutControl.SelectedLayoutElements[0] as LayoutMap);
        }

        //Fires the zoom out control on the modeler
        private void _btnZoomOut_Click(object sender, EventArgs e)
        {
            LayoutControl.ZoomOutMap(_layoutControl.SelectedLayoutElements[0] as LayoutMap);
        }

        //Fires when the user clicks the pan button
        private void _btnPan_Click(object sender, EventArgs e)
        {
            if (_btnPan.Checked)
                _layoutControl.MapPanMode = true;
            else
                _layoutControl.MapPanMode = false;
        }

        //If the toolbar is disabled we disable the pan checked button state
        private void LayoutMapToolStrip_EnabledChanged(object sender, EventArgs e)
        {
            _btnPan.Checked = false;
        }

        //Zoom the map to the extent of the layout
        private void _btnZoomViewExtent_Click(object sender, EventArgs e)
        {
            LayoutControl.ZoomFullViewExtentMap(_layoutControl.SelectedLayoutElements[0] as LayoutMap);
        }

        #endregion
    }
}