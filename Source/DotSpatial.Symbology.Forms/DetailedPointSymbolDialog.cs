// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// DetailedPointSymbolDialog
    /// </summary>
    public partial class DetailedPointSymbolDialog : Form
    {
        private CultureInfo _dlgCultrure;

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

        /// <summary>
        /// Gets or sets a value indicating the culture to use for resources.
        /// </summary>
        public CultureInfo DPSDialogCulture
        {
            get
            {
                return _dlgCultrure;
            }

            set
            {
                if (_dlgCultrure == value) return;
                _dlgCultrure = value;

                if (_dlgCultrure == null) _dlgCultrure = new CultureInfo(string.Empty);

                Thread.CurrentThread.CurrentCulture = _dlgCultrure;
                Thread.CurrentThread.CurrentUICulture = _dlgCultrure;
                Refresh();
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
            DPSDialogCulture = new CultureInfo(string.Empty);

            dialogButtons1.OkClicked += BtnOkClick;
            dialogButtons1.CancelClicked += BtnCancelClick;
            dialogButtons1.ApplyClicked += BtnApplyClick;
        }

        private void UpdatePointSymbolResources()
        {
            resources.ApplyResources(panel1, "panel1");
            resources.ApplyResources(dialogButtons1, "dialogButtons1");
            resources.ApplyResources(detailedPointSymbolControl1, "detailedPointSymbolControl1");
            resources.ApplyResources(this, "$this");

            detailedPointSymbolControl1.DPSControlCulture = _dlgCultrure;
            dialogButtons1.ButtonsCulture = _dlgCultrure;
        }

        #endregion

    }
}