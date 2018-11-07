// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// DetailedLineSymbolDialog
    /// </summary>
    public partial class DetailedLineSymbolDialog : Form
    {
        private CultureInfo _lsDialogCultrure;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailedLineSymbolDialog"/> class.
        /// </summary>
        public DetailedLineSymbolDialog()
        {
            InitializeComponent();
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailedLineSymbolDialog"/> class where only
        /// the original is specified and the duplicate is created.
        /// </summary>
        /// <param name="original">The original line symbolizer.</param>
        public DetailedLineSymbolDialog(ILineSymbolizer original)
        {
            InitializeComponent();
            detailedLineSymbolControl.Initialize(original);
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
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ILineSymbolizer Symbolizer
        {
            get
            {
                return detailedLineSymbolControl?.Symbolizer;
            }

            set
            {
                if (detailedLineSymbolControl == null) return;

                detailedLineSymbolControl.Symbolizer = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the culture to use for resources.
        /// </summary>
        public CultureInfo DLSDialogCulture
        {
            get
            {
                return _lsDialogCultrure;
            }

            set
            {
                _lsDialogCultrure = value;

                Thread.CurrentThread.CurrentCulture = _lsDialogCultrure;
                Thread.CurrentThread.CurrentUICulture = _lsDialogCultrure;
                UpdateLineSymbolResources();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fires the ChangesApplied event
        /// </summary>
        protected virtual void OnApplyChanges()
        {
            detailedLineSymbolControl.ApplyChanges();
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

            DLSDialogCulture = new CultureInfo(string.Empty);
        }

        private void UpdateLineSymbolResources()
        {
            resources.ApplyResources(dialogButtons1, "dialogButtons1");
            resources.ApplyResources(detailedLineSymbolControl, "detailedLineSymbolControl");
            resources.ApplyResources(this, "$this");

            dialogButtons1.ButtonsCulture = _lsDialogCultrure;
            detailedLineSymbolControl.DLSDialogCulture = _lsDialogCultrure;
        }

        #endregion
    }
}