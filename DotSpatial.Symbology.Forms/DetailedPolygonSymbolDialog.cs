// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The core assembly for the DotSpatial 6.0 distribution.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/12/2009 11:29:25 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// DetailedPolygonSymbolDialog
    /// </summary>
    public class DetailedPolygonSymbolDialog : Form
    {
        #region Events

        /// <summary>
        /// Occurs whenever the apply changes button is clicked, or else when the ok button is clicked.
        /// </summary>
        public event EventHandler ChangesApplied;

        #endregion

        private DetailedPolygonSymbolControl _detailedPolygonSymbolControl1;
        private DialogButtons _dialogButtons1;
        private Panel _panel1;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DetailedPolygonSymbolDialog));
            this._panel1 = new System.Windows.Forms.Panel();
            this._dialogButtons1 = new DotSpatial.Symbology.Forms.DialogButtons();
            this._detailedPolygonSymbolControl1 = new DotSpatial.Symbology.Forms.DetailedPolygonSymbolControl();
            this._panel1.SuspendLayout();
            this.SuspendLayout();
            //
            // _panel1
            //
            this._panel1.Controls.Add(this._dialogButtons1);
            resources.ApplyResources(this._panel1, "_panel1");
            this._panel1.Name = "_panel1";
            //
            // _dialogButtons1
            //
            resources.ApplyResources(this._dialogButtons1, "_dialogButtons1");
            this._dialogButtons1.Name = "_dialogButtons1";
            //
            // _detailedPolygonSymbolControl1
            //
            resources.ApplyResources(this._detailedPolygonSymbolControl1, "_detailedPolygonSymbolControl1");
            this._detailedPolygonSymbolControl1.Name = "_detailedPolygonSymbolControl1";
            //
            // DetailedPolygonSymbolDialog
            //
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this._detailedPolygonSymbolControl1);
            this.Controls.Add(this._panel1);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DetailedPolygonSymbolDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this._panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of CollectionPropertyGrid
        /// </summary>
        public DetailedPolygonSymbolDialog()
        {
            InitializeComponent();
            Configure();
        }

        /// <summary>
        /// Creates a new DetailedPolygonSymbolControl using the specified
        /// </summary>
        /// <param name="original"></param>
        public DetailedPolygonSymbolDialog(IPolygonSymbolizer original)
        {
            InitializeComponent();
            _detailedPolygonSymbolControl1.Initialize(original);
            Configure();
        }

        private void Configure()
        {
            _dialogButtons1.OkClicked += btnOk_Click;
            _dialogButtons1.CancelClicked += btnCancel_Click;
            _dialogButtons1.ApplyClicked += btnApply_Click;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbolizer being used by this control.
        /// </summary>
        public IPolygonSymbolizer Symbolizer
        {
            get
            {
                if (_detailedPolygonSymbolControl1 == null) return null;
                return _detailedPolygonSymbolControl1.Symbolizer;
            }
            set
            {
                if (_detailedPolygonSymbolControl1 == null) return;
                _detailedPolygonSymbolControl1.Symbolizer = value;
            }
        }

        #endregion

        #region Events

        #endregion

        #region Event Handlers

        private void btnApply_Click(object sender, EventArgs e)
        {
            OnApplyChanges();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            OnApplyChanges();
            Close();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the ChangesApplied event
        /// </summary>
        protected virtual void OnApplyChanges()
        {
            _detailedPolygonSymbolControl1.ApplyChanges();

            if (ChangesApplied != null) ChangesApplied(this, EventArgs.Empty);
        }

        #endregion
    }
}