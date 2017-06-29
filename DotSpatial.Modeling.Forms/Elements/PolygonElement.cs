// ********************************************************************************************************
// Product Name: DotSpatial.Tools.PolygonElement
// Description:  Polygon Element for use in the tool dialog
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
using DotSpatial.Topology;
using Point = System.Drawing.Point;

namespace DotSpatial.Modeling.Forms
{
    internal class PolygonElement : DialogElement
    {
        #region Class Variables

        private readonly List<DataSetArray> _dataSets;
        private DataSetArray _addedFeatureSet;
        private Button _btnAddData;
        private ComboBox _comboFeatures;
        private bool _refreshCombo = true;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of the dialog
        /// </summary>
        /// <param name="inputParam">The parameter this element represents</param>
        /// <param name="dataSets">An array of available data</param>
        public PolygonElement(PolygonFeatureSetParam inputParam, List<DataSetArray> dataSets)
        {
            //Needed by the designer
            InitializeComponent();

            //We save the parameters passed in
            Param = inputParam;

            _dataSets = dataSets;

            //Saves the label
            GroupBox.Text = Param.Name;

            DoRefresh();
        }

        #endregion

        #region methods

        private void DoRefresh()
        {
            //Disable the combo box temporarily
            _refreshCombo = false;

            //We set the combo boxes status to empty to start
            base.Status = ToolStatus.Empty;
            LightTipText = ModelingMessageStrings.FeaturesetMissing;
            _comboFeatures.Items.Clear();

            //If the user added a feature set
            if (_addedFeatureSet != null)
            {
                _comboFeatures.Items.Add(_addedFeatureSet);
                if (Param.Value != null && Param.DefaultSpecified)
                {
                    if (_addedFeatureSet.DataSet == Param.Value)
                    {
                        _comboFeatures.SelectedItem = _addedFeatureSet;
                        base.Status = ToolStatus.Ok;
                        LightTipText = ModelingMessageStrings.FeaturesetValid;
                    }
                }
            }

            //Add all the dataSets back to the combo box
            if (_dataSets != null)
            {
                foreach (DataSetArray dsa in _dataSets)
                {
                    IFeatureSet aFeatureSet = (dsa.DataSet as IFeatureSet);
                    if (aFeatureSet != null)
                    {
                        //If the featureset is the correct type and isn't already in the combo box we add it
                        if (aFeatureSet.FeatureType == FeatureType.Polygon && _comboFeatures.Items.Contains(dsa) == false)
                        {
                            _comboFeatures.Items.Add(dsa);
                            if (Param.Value != null && Param.DefaultSpecified)
                            {
                                if (dsa.DataSet == Param.Value)
                                {
                                    _comboFeatures.SelectedItem = dsa;
                                    base.Status = ToolStatus.Ok;
                                    LightTipText = ModelingMessageStrings.FeaturesetValid;
                                }
                            }
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
        private void ComboFeatureSelectedValueChanged(object sender, EventArgs e)
        {
            if (_refreshCombo)
            {
                DataSetArray dsa = _comboFeatures.SelectedItem as DataSetArray;
                if (dsa != null)
                {
                    Param.ModelName = dsa.Name;
                    Param.Value = dsa.DataSet;
                    return;
                }
            }
        }

        /// <summary>
        /// Adds a new entry to the drop down list from data provider
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddDataClick(object sender, EventArgs e)
        {
            /////////////////////////////////
            //Replace with something that uses the default data provider
            IFeatureSet tempFeatureSet = DataManager.DefaultDataManager.OpenVector();

            //If the feature is null don't do anything the user probably hit cancel on the dialog
            if (tempFeatureSet == null)
                return;

            //Else if the wrong feature type is returned don't add it and indicate whats wrong
            if (tempFeatureSet.FeatureType != FeatureType.Polygon)
                MessageBox.Show(ModelingMessageStrings.FeatureTypeException);

                //If its good add the feature set and save it
            else
            {
                _addedFeatureSet = new DataSetArray(Path.GetFileNameWithoutExtension(tempFeatureSet.Filename), tempFeatureSet);
                Param.ModelName = _addedFeatureSet.Name;
                Param.Value = _addedFeatureSet.DataSet;
            }
        }

        #endregion

        #region Generate by the designer

        private void InitializeComponent()
        {
            _btnAddData = new Button();
            _comboFeatures = new ComboBox();
            GroupBox.SuspendLayout();
            SuspendLayout();
            //
            // groupBox1
            //
            GroupBox.Controls.Add(_comboFeatures);
            GroupBox.Controls.Add(_btnAddData);
            GroupBox.Size = new Size(492, 46);
            GroupBox.Text = "Caption";
            GroupBox.Controls.SetChildIndex(_btnAddData, 0);
            GroupBox.Controls.SetChildIndex(_comboFeatures, 0);
            GroupBox.Controls.SetChildIndex(StatusLabel, 0);
            //
            // lblStatus
            //
            StatusLabel.Location = new Point(12, 20);
            //
            // btnAddData
            //
            _btnAddData.Image = Images.AddLayer;
            _btnAddData.Location = new Point(460, 14);
            _btnAddData.Name = "_btnAddData";
            _btnAddData.Size = new Size(26, 26);
            _btnAddData.TabIndex = 4;
            _btnAddData.UseVisualStyleBackColor = true;
            _btnAddData.Click += BtnAddDataClick;
            //
            // comboFeatures
            //
            _comboFeatures.DropDownStyle = ComboBoxStyle.DropDownList;
            _comboFeatures.FormattingEnabled = true;
            _comboFeatures.Location = new Point(44, 17);
            _comboFeatures.Name = "_comboFeatures";
            _comboFeatures.Size = new Size(410, 21);
            _comboFeatures.TabIndex = 5;
            _comboFeatures.SelectedValueChanged += ComboFeatureSelectedValueChanged;
            _comboFeatures.Click += DialogElement_Click;
            //
            // PolygonElement
            //
            AutoScaleDimensions = new SizeF(6F, 13F);
            Name = "PolygonElement";
            Size = new Size(492, 46);
            GroupBox.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
    }
}