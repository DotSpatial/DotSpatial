// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Windows.Forms;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// Size2DDialog.
    /// </summary>
    public partial class Size2DDialog : Form
    {
        #region Fields

        private Size2D _editValue;
        private ISymbol _original;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Size2DDialog"/> class.
        /// </summary>
        public Size2DDialog()
        {
            InitializeComponent();
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Size2DDialog"/> class.
        /// </summary>
        /// <param name="symbol">Symbol to edit when "Apply Changes" is clicked.</param>
        public Size2DDialog(ISymbol symbol)
            : this()
        {
            _original = symbol;
            _editValue = _original.Size.Copy();
            dbxHeight.Value = _editValue.Height;
            dbxWidth.Value = _editValue.Width;
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
        /// Gets or sets the symbol that should be edited whenever apply changes is clicked.
        /// </summary>
        public ISymbol Symbol
        {
            get
            {
                return _original;
            }

            set
            {
                _original = value;
                _editValue = _original.Size.Copy();
                dbxHeight.Value = _editValue.Height;
                dbxWidth.Value = _editValue.Width;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fires the ChangesApplied event.
        /// </summary>
        protected virtual void OnApplyChanges()
        {
            ChangesApplied?.Invoke(this, EventArgs.Empty);
        }

        private void BtnApplyClick(object sender, EventArgs e)
        {
            _original.Size = _editValue.Copy();
            OnApplyChanges();
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void CmdOkClick(object sender, EventArgs e)
        {
            _original.Size = _editValue.Copy();
            OnApplyChanges();
            Close();
        }

        private void Configure()
        {
            dialogButtons1.OkClicked += CmdOkClick;
            dialogButtons1.CancelClicked += BtnCancelClick;
            dialogButtons1.ApplyClicked += BtnApplyClick;
        }

        private void DbxHeightTextChanged(object sender, EventArgs e)
        {
            _editValue.Height = dbxHeight.Value;
        }

        private void DbxWidthTextChanged(object sender, EventArgs e)
        {
            _editValue.Width = dbxWidth.Value;
        }

        #endregion
    }
}