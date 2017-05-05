// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The core assembly for the DotSpatial 6.0 distribution.
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/12/2009 12:06:20 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// DetailedPointSymbolDialog
    /// </summary>
    public partial class DetailedPointSymbolDialog : Form
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailedPointSymbolDialog"/> class.
        /// </summary>
        public DetailedPointSymbolDialog()
        {
            InitializeComponent();
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailedPointSymbolDialog"/> class.
        /// </summary>
        /// <param name="original">The original point symbolizer.</param>
        public DetailedPointSymbolDialog(IPointSymbolizer original)
        {
            InitializeComponent();
            detailedPointSymbolControl1.Initialize(original);
            Configure();
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
        /// Gets or sets the symbolizer being used by this control.
        /// </summary>
        public IPointSymbolizer Symbolizer
        {
            get
            {
                return detailedPointSymbolControl1?.Symbolizer;
            }

            set
            {
                if (detailedPointSymbolControl1 == null) return;

                detailedPointSymbolControl1.Symbolizer = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fires the ChangesApplied event
        /// </summary>
        protected virtual void OnApplyChanges()
        {
            detailedPointSymbolControl1.ApplyChanges();
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

        private void BtnOkClick(object sender, EventArgs e)
        {
            OnApplyChanges();
            Close();
        }

        private void Configure()
        {
            dialogButtons1.OkClicked += BtnOkClick;
            dialogButtons1.CancelClicked += BtnCancelClick;
            dialogButtons1.ApplyClicked += BtnApplyClick;
        }

        #endregion
    }
}