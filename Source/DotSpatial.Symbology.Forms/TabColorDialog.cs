// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The core assembly for the DotSpatial 6.0 distribution.
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/12/2009 12:02:12 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// TabColorDialog
    /// </summary>
    public partial class TabColorDialog : Form
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TabColorDialog"/> class.
        /// </summary>
        public TabColorDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs whenever the apply changes button is clicked, or else when the ok button is clicked.
        /// </summary>
        public event EventHandler ChangesApplied;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the end color for this control.
        /// </summary>
        public Color EndColor
        {
            get
            {
                return tabColorControl1.EndColor;
            }

            set
            {
                tabColorControl1.EndColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the start color for this control
        /// </summary>
        public Color StartColor
        {
            get
            {
                return tabColorControl1.StartColor;
            }

            set
            {
                tabColorControl1.StartColor = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fires the ChangesApplied event
        /// </summary>
        protected virtual void OnApplyChanges()
        {
            ChangesApplied?.Invoke(this, EventArgs.Empty);
        }

        private void BtnApplyClick(object sender, EventArgs e)
        {
            OnApplyChanges();
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void CmdOkClick(object sender, EventArgs e)
        {
            OnApplyChanges();
            Close();
        }

        #endregion
    }
}