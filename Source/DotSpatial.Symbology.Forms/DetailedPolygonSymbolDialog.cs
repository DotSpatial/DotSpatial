// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// DetailedPolygonSymbolDialog
    /// </summary>
    public partial class DetailedPolygonSymbolDialog : Form
    {
        private CultureInfo _dlgCultrure;

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
            DPSDialogCulture = new CultureInfo(string.Empty);

            _dialogButtons1.OkClicked += BtnOkClick;
            _dialogButtons1.CancelClicked += BtnCancelClick;
            _dialogButtons1.ApplyClicked += BtnApplyClick;
        }

        private void UpdatePointSymbolResources()
        {
            resources.ApplyResources(_panel1, "_panel1");
            resources.ApplyResources(_dialogButtons1, "_dialogButtons1");
            resources.ApplyResources(_detailedPolygonSymbolControl1, "_detailedPolygonSymbolControl1");
            resources.ApplyResources(this, "$this");

            _detailedPolygonSymbolControl1.DPSControlCulture = _dlgCultrure;
            _dialogButtons1.ButtonsCulture = _dlgCultrure;
        }

        #endregion
    }
}