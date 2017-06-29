// ********************************************************************************************************
// Product Name: DotSpatial.Forms.LayoutForm
// Description:  A form that shows the mapwindow layout
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
// The Initial Developer of this Original Code is by Brian Marchionni Aug 2009
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// ------------------|------------|---------------------------------------------------------------
// Ted Dunsford      | 8/28/2009  | Cleaned up some code formatting using resharper
// ********************************************************************************************************

using System;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This is the primary form where the print layout content is organized before printing
    /// </summary>
    public partial class LayoutForm : Form
    {
        /// <summary>
        /// Default constructor for creating a new instance of hte Layout form
        /// </summary>
        public LayoutForm()
        {
            InitializeComponent();

            if (Mono.Mono.IsRunningOnMono())
            {
                // On Mac and possibly other Mono platforms, GdipCreateLineBrushFromRect
                // in gdiplus native lib returns InvalidParameter in Mono file LinearGradientBrush.cs
                // if a StripPanel's Width or Height is 0, so force them to non-0.
                _toolStripContainer1.TopToolStripPanel.Size = new System.Drawing.Size(_toolStripContainer1.TopToolStripPanel.Size.Width, 1);
                _toolStripContainer1.BottomToolStripPanel.Size = new System.Drawing.Size(_toolStripContainer1.BottomToolStripPanel.Size.Width, 1);
                _toolStripContainer1.LeftToolStripPanel.Size = new System.Drawing.Size(1, _toolStripContainer1.LeftToolStripPanel.Size.Height);
                _toolStripContainer1.RightToolStripPanel.Size = new System.Drawing.Size(1, _toolStripContainer1.RightToolStripPanel.Size.Height);
            }
        }

        /// <summary>
        /// Gets or sets the map that will be used in the layout
        /// </summary>
        public Map MapControl
        {
            get { return _layoutControl1.MapControl; }
            set { _layoutControl1.MapControl = value; }
        }

        /// <summary>
        /// Gets layout control.
        /// </summary>
        public LayoutControl LayoutControl
        {
            get { return _layoutControl1; }
        }

        private void layoutMenuStrip1_CloseClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void layoutControl1_FilenameChanged(object sender, EventArgs e)
        {
            Text = !string.IsNullOrEmpty(_layoutControl1.Filename)
                ? "DotSpatial Print Layout - " + System.IO.Path.GetFileName(this._layoutControl1.Filename)
                : "DotSpatial Print Layout";
        }

        private void LayoutForm_Load(object sender, EventArgs e)
        {
            if (MapControl != null)
            {
                var mapElement = _layoutControl1.CreateMapElement();
                mapElement.Size = _layoutControl1.Size;
                _layoutControl1.AddToLayout(mapElement);
            }
        }
    }
}