// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The core assembly for the DotSpatial 6.0 distribution.
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/7/2010 9:23:01 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// DynamicVisibilityModeDialog
    /// </summary>
    public partial class DynamicVisibilityModeDialog : Form
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicVisibilityModeDialog"/> class.
        /// </summary>
        public DynamicVisibilityModeDialog()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the dynamic visibility mode for this dialog.
        /// This stores the result from this dialog.
        /// </summary>
        public DynamicVisibilityMode DynamicVisibilityMode { get; set; }

        #endregion

        #region Methods

        private void BtnAlwaysClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void BtnZoomedInClick(object sender, EventArgs e)
        {
            DynamicVisibilityMode = DynamicVisibilityMode.ZoomedIn;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnZoomedOutClick(object sender, EventArgs e)
        {
            DynamicVisibilityMode = DynamicVisibilityMode.ZoomedOut;
            DialogResult = DialogResult.OK;
            Close();
        }

        #endregion
    }
}