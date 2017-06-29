// ********************************************************************************************************
// Product Name: DotSpatial.Plugins.ShapeEditor.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/10/2009 5:10:05 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Topology;

namespace DotSpatial.Plugins.ShapeEditor
{
    /// <summary>
    /// A Dialog that displays options for feature type when creating a new feature.
    /// </summary>
    public class FeatureTypeDialog : Form
    {
        private Button _btnCancel;
        private Button _btnOk;
        private CheckBox _chkM;
        private CheckBox _chkZ;
        private ComboBox _cmbFeatureType;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeatureTypeDialog));
            this._btnOk = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this._cmbFeatureType = new System.Windows.Forms.ComboBox();
            this._chkM = new System.Windows.Forms.CheckBox();
            this._chkZ = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            //
            // _btnOk
            //
            resources.ApplyResources(this._btnOk, "_btnOk");
            this._btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnOk.Name = "_btnOk";
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new System.EventHandler(this.OkButton_Click);
            //
            // _btnCancel
            //
            resources.ApplyResources(this._btnCancel, "_btnCancel");
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this.CancelButton_Click);
            //
            // _cmbFeatureType
            //
            resources.ApplyResources(this._cmbFeatureType, "_cmbFeatureType");
            this._cmbFeatureType.FormattingEnabled = true;
            this._cmbFeatureType.Items.AddRange(new object[] {
                                                                 resources.GetString("_cmbFeatureType.Items"),
                                                                 resources.GetString("_cmbFeatureType.Items1"),
                                                                 resources.GetString("_cmbFeatureType.Items2"),
                                                                 resources.GetString("_cmbFeatureType.Items3")});
            this._cmbFeatureType.Name = "_cmbFeatureType";
            //
            // _chkM
            //
            resources.ApplyResources(this._chkM, "_chkM");
            this._chkM.Name = "_chkM";
            this._chkM.UseVisualStyleBackColor = true;
            //
            // _chkZ
            //
            resources.ApplyResources(this._chkZ, "_chkZ");
            this._chkZ.Name = "_chkZ";
            this._chkZ.UseVisualStyleBackColor = true;
            //
            // FeatureTypeDialog
            //
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._chkZ);
            this.Controls.Add(this._chkM);
            this.Controls.Add(this._cmbFeatureType);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FeatureTypeDialog";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the FeatureTypeDialog class.
        /// </summary>
        public FeatureTypeDialog()
        {
            InitializeComponent();
            _cmbFeatureType.SelectedIndex = 0;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets the feature type chosen by this dialog.
        /// </summary>
        public FeatureType FeatureType
        {
            get
            {
                switch (_cmbFeatureType.SelectedIndex)
                {
                    case 0: return FeatureType.Point;
                    case 1: return FeatureType.Line;
                    case 2: return FeatureType.Polygon;
                    case 3:
                        return FeatureType.MultiPoint;
                }
                return FeatureType.Unspecified;
            }
        }

        /// <summary>
        /// Gets the Coordinate type for this dialog.
        /// </summary>
        public CoordinateType CoordinateType
        {
            get
            {
                if (_chkZ.Checked) { return CoordinateType.Z; }
                if (_chkM.Checked) { return CoordinateType.M; }
                return CoordinateType.Regular;
            }
        }

        #endregion

        #region Events

        #endregion

        #region Event Handlers

        #endregion

        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}