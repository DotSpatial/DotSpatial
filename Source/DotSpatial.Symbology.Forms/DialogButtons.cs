// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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