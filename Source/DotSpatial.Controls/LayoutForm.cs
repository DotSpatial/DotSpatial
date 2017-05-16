﻿// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This is the primary form where the print layout content is organized before printing.
    /// </summary>
    public partial class LayoutForm : Form
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutForm"/> class.
        /// </summary>
        public LayoutForm()
        {
            InitializeComponent();

            if (Mono.IsRunningOnMono())
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

        #endregion

        #region Properties

        /// <summary>
        /// Gets the layout control.
        /// </summary>
        public LayoutControl LayoutControl => _layoutControl1;

        /// <summary>
        /// Gets or sets the map that will be used in the layout
        /// </summary>
        public Map MapControl
        {
            get
            {
                return _layoutControl1.MapControl;
            }

            set
            {
                _layoutControl1.MapControl = value;
            }
        }

        #endregion

        #region Methods

        private void LayoutControl1FilenameChanged(object sender, EventArgs e)
        {
            Text = !string.IsNullOrEmpty(_layoutControl1.Filename) ? "DotSpatial Print Layout - " + Path.GetFileName(_layoutControl1.Filename) : "DotSpatial Print Layout";
        }

        private void LayoutFormLoad(object sender, EventArgs e)
        {
            if (MapControl != null)
            {
                var mapElement = _layoutControl1.CreateMapElement();
                mapElement.Size = _layoutControl1.Size;
                _layoutControl1.AddToLayout(mapElement);
            }
        }

        private void LayoutMenuStrip1CloseClicked(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}