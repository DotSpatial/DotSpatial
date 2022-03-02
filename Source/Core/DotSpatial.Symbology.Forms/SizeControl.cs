// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A user control that is specifically designed to control the point sizes.
    /// </summary>
    [DefaultEvent("SelectedSizeChanged")]
    public partial class SizeControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeControl"/> class.
        /// </summary>
        public SizeControl()
        {
            InitializeComponent();
            _editDialog = new Size2DDialog();
            _editDialog.ChangesApplied += EditDialogChangesApplied;
            scSizes.SelectedSizeChanged += ScSizesSelectedSizeChanged;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the size changes on this control, ether through applying changes in the dialog or else
        /// by selecting one of the pre-configured sizes.
        /// </summary>
        public event EventHandler SelectedSizeChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbol to use when drawing the various sizes.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public ISymbol Symbol
        {
            get
            {
                return scSizes.Symbol;
            }

            set
            {
                scSizes.Symbol = value;
                _editDialog.Symbol = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fires the SizeChanged event.
        /// </summary>
        protected virtual void OnSelectedSizeChanged()
        {
            SelectedSizeChanged?.Invoke(this, EventArgs.Empty);
        }

        private void BtnEditClick(object sender, EventArgs e)
        {
            _editDialog.ShowDialog();
        }

        private void EditDialogChangesApplied(object sender, EventArgs e)
        {
            scSizes.Invalidate();
            OnSelectedSizeChanged();
        }

        private void ScSizesSelectedSizeChanged(object sender, EventArgs e)
        {
            // Even though this is a reference type, and the "symbol" property is
            // up to date on the edit dialog, we need to force it to regenerate
            // the actual text values in the text boxes.
            _editDialog.Symbol = scSizes.Symbol;
            OnSelectedSizeChanged();
        }

        #endregion
    }
}