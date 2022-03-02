// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// Atrribute Table editor form.
    /// </summary>
    public partial class AttributeDialog : Form
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeDialog"/> class.
        /// </summary>
        /// <param name="featureLayer">The feature layer associated with this instance and displayed in the editor.</param>
        public AttributeDialog(IFeatureLayer featureLayer)
        {
            InitializeComponent();
            if (featureLayer != null)
            {
                tableEditorControl1.FeatureLayer = featureLayer;
            }
        }

        #endregion

        #region Event Handlers

        private void BtnCloseClick1(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}