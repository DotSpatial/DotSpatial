// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/21/2009 4:30:25 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// 9/22/09: Chris Wilson--changed order of buttons and removed hotkeys to conform to Windows standards
//          Automatically sets AcceptButton and CancelButton properties of owner form
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// DialogButtons
    /// </summary>
    [DefaultEvent("OkClicked")]
    [ToolboxItem(true)]
    public partial class DialogButtons : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogButtons"/> class.
        /// </summary>
        public DialogButtons()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// The Apply button was clicked
        /// </summary>
        public event EventHandler ApplyClicked;

        /// <summary>
        /// The Cancel button was clicked
        /// </summary>
        public event EventHandler CancelClicked;

        /// <summary>
        /// The OK button was clicked
        /// </summary>
        public event EventHandler OkClicked;

        #endregion

        #region Methods

        /// <summary>
        /// Fires the Apply Clicked event
        /// </summary>
        protected virtual void OnApplyClicked()
        {
            ApplyClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the Cancel Clicked event
        /// </summary>
        protected virtual void OnCancelClicked()
        {
            CancelClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the ok clicked event
        /// </summary>
        protected virtual void OnOkClicked()
        {
            OkClicked?.Invoke(this, EventArgs.Empty);
        }

        private void BtnApplyClick(object sender, EventArgs e)
        {
            OnApplyClicked();
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            OnCancelClicked();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            OnOkClicked();
        }

        #endregion
    }
}