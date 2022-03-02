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
    [ToolboxItem(false)]
    public partial class ModelerToolStrip : ToolStrip
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelerToolStrip"/> class.
        /// </summary>
        public ModelerToolStrip()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the modeler currently associated with the toolstrip.
        /// </summary>
        public Modeler Modeler { get; set; }

        #endregion

        #region Methods

        // Fires when the user clicks the delete button
        private void BtnDeleteClick(object sender, EventArgs e)
        {
            Modeler.DeleteSelectedElements();
        }

        // Fires when the user clicks the link button
        private void BtnLinkClick(object sender, EventArgs e)
        {
            if (Modeler.EnableLinking)
            {
                _btnLink.Checked = false;
                Modeler.EnableLinking = false;
            }
            else
            {
                _btnLink.Checked = true;
                Modeler.EnableLinking = true;
            }
        }

        private void BtnLoadModelClick(object sender, EventArgs e)
        {
            Modeler.LoadModel();
        }

        private void BtnRunClick(object sender, EventArgs e)
        {
            Parent.Enabled = false;
            string error;
            Modeler.ExecuteModel(out error);
            Parent.Enabled = true;
        }

        private void BtnSaveModelClick(object sender, EventArgs e)
        {
            Modeler.SaveModel();
        }

        // Fires when the user clicks the zoom to full extent button
        private void BtnZoomFullExtentClick(object sender, EventArgs e)
        {
            Modeler.ZoomFullExtent();
        }

        // Fires the zoom in control on the modeler
        private void BtnZoomInClick(object sender, EventArgs e)
        {
            Modeler.ZoomIn();
        }

        // Fires the zoom out control on the modeler
        private void BtnZoomOutClick(object sender, EventArgs e)
        {
            Modeler.ZoomOut();
        }

        #endregion
    }
}