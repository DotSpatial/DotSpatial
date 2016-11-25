// ********************************************************************************************************
// Product Name: DotSpatial.Forms.LayoutForm
// Description:  A form that shows the mapwindow layout
// ********************************************************************************************************
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
using System.Drawing;
using System.IO;
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
                _toolStripContainer1.TopToolStripPanel.Size = new Size(_toolStripContainer1.TopToolStripPanel.Size.Width, 1);
                _toolStripContainer1.BottomToolStripPanel.Size = new Size(_toolStripContainer1.BottomToolStripPanel.Size.Width, 1);
                _toolStripContainer1.LeftToolStripPanel.Size = new Size(1, _toolStripContainer1.LeftToolStripPanel.Size.Height);
                _toolStripContainer1.RightToolStripPanel.Size = new Size(1, _toolStripContainer1.RightToolStripPanel.Size.Height);
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
                ? "DotSpatial Print Layout - " + Path.GetFileName(this._layoutControl1.Filename)
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