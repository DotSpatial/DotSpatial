// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The core assembly for the DotSpatial 6.0 distribution.
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/16/2009 1:18:11 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// ProgressCancelDialog
    /// </summary>
    public partial class ProgressCancelDialog : Form, ICancelProgressHandler
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressCancelDialog"/> class.
        /// </summary>
        public ProgressCancelDialog()
        {
            InitializeComponent();
        }

        #endregion

        private delegate void SetMess(string message);

        #region Events

        /// <summary>
        /// Fires the canceled event.
        /// </summary>
        public event EventHandler Cancelled;

        #endregion

        #region Properties

        /// <inheritdoc />
        public bool Cancel { get; private set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Progress(string key, int percent, string message)
        {
            mwProgressBar1.Value = percent;
            SetMess ms = SetMessage;
            if (InvokeRequired)
            {
                Invoke(ms, message);
            }
            else
            {
                SetMessage(message);
            }
        }

        private void Button1Click(object sender, EventArgs e)
        {
            Cancel = true;
            Cancelled?.Invoke(this, EventArgs.Empty);
            Hide();
        }

        private void SetMessage(string text)
        {
            lblProgressText.Text = text;
        }

        #endregion
    }
}