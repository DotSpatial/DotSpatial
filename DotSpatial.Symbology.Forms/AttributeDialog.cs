// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Projections library.
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
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in Fall 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// Jiri Kadlec (2009-10-30) The attribute Table editor has been moved to a separate user control
// ********************************************************************************************************

using System;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// Atrribute Table editor form
    /// </summary>
    public partial class AttributeDialog : Form
    {
       #region Constructor

        /// <summary>
        /// Creates a new instance of the attribute Table editor form
        /// <param name="featureLayer">The feature layer associated with
        /// this instance and displayed in the editor</param>
        /// </summary>
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

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}