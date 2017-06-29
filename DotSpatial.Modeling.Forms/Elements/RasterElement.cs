// ********************************************************************************************************
// Product Name: DotSpatial.Tools.RasterElement
// Description:  Raster Element for use in the tool dialog
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

namespace DotSpatial.Modeling.Forms
{
    internal class RasterElement : DialogElement
    {
        #region Class Variables

        private DataSetArray _addedRasterSet;
        private List<DataSetArray> _dataSets;
        private bool _refreshCombo = true;
        private Button btnAddData;
        private ComboBox comboRaster;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of the dialog
        /// </summary>
        /// <param name="inputParam">The parameter this element represents</param>
        /// <param name="dataSets">An array of available data</param>
        public RasterElement(RasterParam inputParam, List<DataSetArray> dataSets)
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

        private void DoRefresh()
        {
            //Disable the combo box temporarily
            _refreshCombo = false;

            //We set the combo boxes status to empty to start
            base.Status = ToolStatus.Empty;
            LightTipText = ModelingMessageStrings.FeaturesetMissing;
            comboRaster.Items.Clear();

            //If the user added a raster set
            if (_addedRasterSet != null)
            {
                comboRaster.Items.Add(_addedRasterSet);
                if (Param.Value != null && Param.DefaultSpecified)
                {
                    if (_addedRasterSet.DataSet == Param.Value)
                    {
                        comboRaster.SelectedItem = _addedRasterSet;
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
                    IRaster aRasterSet = (dsa.DataSet as IRaster);
                    if (aRasterSet != null)
                    {
                        //If the featureset is the correct type and isn't already in the combo box we add it
                        if (comboRaster.Items.Contains(dsa) == false)
                        {
                            comboRaster.Items.Add(dsa);
                            if (Param.Value != null && Param.DefaultSpecified)
                            {
                                if (dsa.DataSet == Param.Value)
                                {
                                    comboRaster.SelectedItem = dsa;
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

        #region methods

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
        /// This changes the color of the light and the tooltip of the light based on the status of the text in the box
        /// </summary>
        private void ComboRasterSelectedValueChanged(object sender, EventArgs e)
        {
            if (!_refreshCombo) return;
            DataSetArray dsa = comboRaster.SelectedItem as DataSetArray;
            if (dsa == null) return;
            Param.ModelName = dsa.Name;
            Param.Value = dsa.DataSet;

            return;
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
            IRaster tempRaster = DataManager.DefaultDataManager.OpenRaster();

            //If the feature is null don't do anything the user probably hit cancel on the dialog
            if (tempRaster == null)
                return;

            //If its good add the feature set and save it
            _addedRasterSet = new DataSetArray(Path.GetFileNameWithoutExtension(tempRaster.Filename), tempRaster);
            Param.ModelName = _addedRasterSet.Name;
            Param.Value = _addedRasterSet.DataSet;
        }

        #endregion

        #region Generate by the designer

        private void InitializeComponent()
        {
            btnAddData = new Button();
            comboRaster = new ComboBox();
            GroupBox.SuspendLayout();
            SuspendLayout();
            //
            // groupBox1
            //
            GroupBox.Controls.Add(comboRaster);
            GroupBox.Controls.Add(btnAddData);
            GroupBox.Size = new Size(492, 46);
            GroupBox.Text = "Caption";
            GroupBox.Controls.SetChildIndex(btnAddData, 0);
            GroupBox.Controls.SetChildIndex(comboRaster, 0);
            GroupBox.Controls.SetChildIndex(StatusLabel, 0);
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
            comboRaster.DropDownStyle = ComboBoxStyle.DropDownList;
            comboRaster.FormattingEnabled = true;
            comboRaster.Location = new Point(44, 17);
            comboRaster.Name = "comboFeatures";
            comboRaster.Size = new Size(410, 21);
            comboRaster.TabIndex = 5;
            comboRaster.SelectedValueChanged += ComboRasterSelectedValueChanged;
            comboRaster.Click += DialogElement_Click;
            //
            // RasterElement
            //
            AutoScaleDimensions = new SizeF(6F, 13F);
            Name = "RasterElement";
            Size = new Size(492, 46);
            GroupBox.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
    }
}