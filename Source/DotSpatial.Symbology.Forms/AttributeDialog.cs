// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Projections library.
//
// ********************************************************************************************************
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