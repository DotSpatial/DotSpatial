﻿// Copyright (c) DotSpatial Team. All rights reserved.
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
    public partial class LayoutDocToolStrip : ToolStrip
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutDocToolStrip"/> class.
        /// </summary>
        public LayoutDocToolStrip()
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

        // Fires the new method on the layoutcontrol
        private void BtnNewClick(object sender, EventArgs e)
        {
            LayoutControl.NewLayout(true);
        }

        // Fires the open method on the layoutcontrol
        private void BtnOpenClick(object sender, EventArgs e)
        {
            LayoutControl.LoadLayout(true, true, true);
        }

        // Fires the print method on the layoutcontrol
        private void BtnPrintClick(object sender, EventArgs e)
        {
            LayoutControl.Print();
        }

        // Fires the save method on the layoutcontrol
        private void BtnSaveClick(object sender, EventArgs e)
        {
            LayoutControl.SaveLayout(false);
        }

        // Fires the saveas method on the layoutcontrol
        private void BtnSaveAsClick(object sender, EventArgs e)
        {
            LayoutControl.SaveLayout(true);
        }

        #endregion
    }
}