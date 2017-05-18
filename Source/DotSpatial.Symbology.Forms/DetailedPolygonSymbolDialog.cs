// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// DetailedPolygonSymbolDialog
    /// </summary>
    public partial class DetailedPolygonSymbolDialog : Form
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailedPolygonSymbolDialog"/> class.
        /// </summary>
        public DetailedPolygonSymbolDialog()
        {
            InitializeComponent();
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailedPolygonSymbolDialog"/> class.
        /// </summary>
        /// <param name="original">The original polygon symbolizer.</param>
        public DetailedPolygonSymbolDialog(IPolygonSymbolizer original)
        {
            InitializeComponent();
            _detailedPolygonSymbolControl1.Initialize(original);
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
        public IPolygonSymbolizer Symbolizer
        {
            get
            {
                return _detailedPolygonSymbolControl1?.Symbolizer;
            }

            set
            {
                if (_detailedPolygonSymbolControl1 == null) return;

                _detailedPolygonSymbolControl1.Symbolizer = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fires the ChangesApplied event
        /// </summary>
        protected virtual void OnApplyChanges()
        {
            _detailedPolygonSymbolControl1.ApplyChanges();

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
            _dialogButtons1.OkClicked += BtnOkClick;
            _dialogButtons1.CancelClicked += BtnCancelClick;
            _dialogButtons1.ApplyClicked += BtnApplyClick;
        }

        #endregion
    }
}