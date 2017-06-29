// ********************************************************************************************************
// Product Name: DotSpatial.Tools.LineElement
// Description:  Line Element for use in the tool dialog
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
    internal class LineElement : DialogElement
    {
        #region Class Variables

        private DataSetArray _addedFeatureSet;
        private List<DataSetArray> _dataSets;
        private bool _refreshCombo = true;
        private Button btnAddData;
        private ComboBox comboFeatures;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of the dialog
        /// </summary>
        /// <param name="inputParam">The parameter this element represents</param>
        /// <param name="dataSets">An array of available data</param>
        public LineElement(LineFeatureSetParam inputParam, List<DataSetArray> dataSets)
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
            comboFeatures.Items.Clear();

            //If the user added a feature set
            if (_addedFeatureSet != null)
            {
                comboFeatures.Items.Add(_addedFeatureSet);
                if (Param.Value != null && Param.DefaultSpecified)
                {
                    if (_addedFeatureSet.DataSet == Param.Value)
                    {
                        comboFeatures.SelectedItem = _addedFeatureSet;
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
                        if (aFeatureSet.FeatureType == FeatureType.Line && comboFeatures.Items.Contains(dsa) == false)
                        {
                            comboFeatures.Items.Add(dsa);
                            if (Param.Value != null && Param.DefaultSpecified)
                            {
                                if (dsa.DataSet == Param.Value)
                                {
                                    comboFeatures.SelectedItem = dsa;
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
                DataSetArray dsa = comboFeatures.SelectedItem as DataSetArray;
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
            if (tempFeatureSet.FeatureType != FeatureType.Line)
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
            btnAddData = new Button();
            comboFeatures = new ComboBox();
            GroupBox.SuspendLayout();
            SuspendLayout();
            //
            // groupBox1
            //
            GroupBox.Controls.Add(comboFeatures);
            GroupBox.Controls.Add(btnAddData);
            GroupBox.Size = new Size(492, 46);
            GroupBox.Text = "Caption";
            GroupBox.Controls.SetChildIndex(btnAddData, 0);
            GroupBox.Controls.SetChildIndex(comboFeatures, 0);
            GroupBox.Controls.SetChildIndex(StatusLabel, 0);
            //
            // lblStatus
            //
            StatusLabel.Location = new Point(12, 20);
            //
            // btnAddData
            //
            btnAddData.Image = Images.AddLayer;
            btnAddData.Location = new Point(460, 14);
            btnAddData.Name = "btnAddData";
            btnAddData.Size = new Size(26, 26);
            btnAddData.TabIndex = 4;
            btnAddData.UseVisualStyleBackColor = true;
            btnAddData.Click += BtnAddDataClick;
            //
            // comboFeatures
            //
            comboFeatures.DropDownStyle = ComboBoxStyle.DropDownList;
            comboFeatures.FormattingEnabled = true;
            comboFeatures.Location = new Point(44, 17);
            comboFeatures.Name = "comboFeatures";
            comboFeatures.Size = new Size(410, 21);
            comboFeatures.TabIndex = 5;
            comboFeatures.SelectedValueChanged += ComboFeatureSelectedValueChanged;
            comboFeatures.Click += DialogElement_Click;
            //
            // LineElement
            //
            AutoScaleDimensions = new SizeF(6F, 13F);
            Name = "LineElement";
            Size = new Size(492, 46);
            GroupBox.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
    }
}