// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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
        public void Progress(int percent, string message)
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

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Reset()
        {
            Progress(0, string.Empty);
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