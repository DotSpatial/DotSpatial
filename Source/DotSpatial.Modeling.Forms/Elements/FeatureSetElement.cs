// ********************************************************************************************************
// Product Name: DotSpatial.Tools.FeaturesetElement
// Description:  Featureset Element for use in the tool dialog
//
// ********************************************************************************************************
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Nov, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Data.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Modeling.Forms.Elements
{
    /// <summary>
    /// Feature Set element
    /// </summary>
    public class FeatureSetElement : DialogElement
    {
        #region Class Variables

        private DataSetArray _addedFeatureSet;
        private readonly List<DataSetArray> _dataSets;
        private bool _refreshCombo = true;
        private Button _btnAddData;
        private ComboBox _comboFeatures;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of the dialog
        /// </summary>
        /// <param name="inputParam">The parameter this element represents</param>
        /// <param name="dataSets">An array of available data</param>
        public FeatureSetElement(FeatureSetParam inputParam, List<DataSetArray> dataSets)
        {
            // Needed by the designer
            InitializeComponent();

            // We save the parameters passed in
            Param = inputParam;

            _dataSets = dataSets;

            // Saves the label
            GroupBox.Text = Param.Name;

            DoRefresh();
        }

        #endregion

        #region methods

        private void DoRefresh()
        {
            // Disable the combo box temporarily
            _refreshCombo = false;

            // We set the combo boxes status to empty to start
            base.Status = ToolStatus.Empty;
            LightTipText = ModelingMessageStrings.FeaturesetMissing;
            _comboFeatures.Items.Clear();

            // If the user added a feature set
            if (_addedFeatureSet != null)
            {
                _comboFeatures.Items.Add(_addedFeatureSet);
                if (Param != null && Param.Value != null && Param.DefaultSpecified && _addedFeatureSet.DataSet == Param.Value)
                {
                    _comboFeatures.SelectedItem = _addedFeatureSet;
                    base.Status = ToolStatus.Ok;
                    LightTipText = ModelingMessageStrings.FeaturesetValid;
                }
            }

            // Add all the dataSets back to the combo box
            if (_dataSets != null)
            {
                foreach (DataSetArray dsa in _dataSets)
                {
                    IFeatureSet aFeatureSet = dsa.DataSet as IFeatureSet;
                    if (aFeatureSet != null && !_comboFeatures.Items.Contains(dsa))
                    {
                        // If the featureset is the correct type and isn't already in the combo box we add it
                        _comboFeatures.Items.Add(dsa);
                        if (Param != null && Param.Value != null && Param.DefaultSpecified && dsa.DataSet == Param.Value)
                        {
                            _comboFeatures.SelectedItem = dsa;
                            base.Status = ToolStatus.Ok;
                            LightTipText = ModelingMessageStrings.FeaturesetValid;
                        }
                    }
                }
            }

            _refreshCombo = true;
        }

        /// <summary>
        /// updates the param if something's been changed
        /// </summary>
        public override void Refresh()
        {
            DoRefresh();
        }

        #endregion

        #region Events

        /// <summary>
        /// This fires when the selected value in the combo box is changed
        /// </summary>
        private void comboFeature_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_refreshCombo)
            {
                DataSetArray dsa = _comboFeatures.SelectedItem as DataSetArray;
                if (dsa != null)
                {
                    Param.ModelName = dsa.Name;
                    Param.Value = dsa.DataSet;
                }
            }
        }

        /// <summary>
        /// Adds a new entry to the drop down list from data provider
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddData_Click(object sender, EventArgs e)
        {
            // Replace with something that uses the default data provider
            IFeatureSet tempFeatureSet = DataManager.DefaultDataManager.OpenVector();

            // If the feature is null don't do anything the user probably hit cancel on the dialog
            if (tempFeatureSet == null)
                return;

            // If its good add the feature set and save it
            _addedFeatureSet = new DataSetArray(Path.GetFileNameWithoutExtension(tempFeatureSet.Filename), tempFeatureSet);
            Param.ModelName = _addedFeatureSet.Name;
            Param.Value = _addedFeatureSet.DataSet;
        }

        #endregion

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this._btnAddData = new Button();
            this._comboFeatures = new ComboBox();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // groupBox1
            //
            this.GroupBox.Controls.Add(this._comboFeatures);
            this.GroupBox.Controls.Add(this._btnAddData);
            this.GroupBox.Size = new Size(492, 46);
            this.GroupBox.Text = "Caption";
            this.GroupBox.Controls.SetChildIndex(this._btnAddData, 0);
            this.GroupBox.Controls.SetChildIndex(this._comboFeatures, 0);
            this.GroupBox.Controls.SetChildIndex(this.StatusLabel, 0);
            //
            // btnAddData
            //
            this._btnAddData.Image = Images.AddLayer;
            this._btnAddData.Location = new Point(460, 14);
            this._btnAddData.Name = "_btnAddData";
            this._btnAddData.Size = new Size(26, 26);
            this._btnAddData.TabIndex = 4;
            this._btnAddData.UseVisualStyleBackColor = true;
            this._btnAddData.Click += new EventHandler(this.btnAddData_Click);
            //
            // comboFeatures
            //
            this._comboFeatures.DropDownStyle = ComboBoxStyle.DropDownList;
            this._comboFeatures.FormattingEnabled = true;
            this._comboFeatures.Location = new Point(44, 17);
            this._comboFeatures.Name = "_comboFeatures";
            this._comboFeatures.Size = new Size(410, 21);
            this._comboFeatures.TabIndex = 5;
            this._comboFeatures.SelectedValueChanged += new EventHandler(this.comboFeature_SelectedValueChanged);
            this._comboFeatures.Click += new EventHandler(base.DialogElement_Click);
            //
            // FeatureSetElement
            //
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.Name = "FeatureSetElement";
            this.Size = new Size(492, 46);
            this.GroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}