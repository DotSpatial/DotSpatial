// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using DotSpatial.Data;
using DotSpatial.Data.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Modeling.Forms.Elements
{
    /// <summary>
    /// Raster Element for use in the tool dialog
    /// </summary>
    internal partial class RasterElement : DialogElement
    {
        #region Fields
        private readonly List<DataSetArray> _dataSets;
        private DataSetArray _addedRasterSet;
        private bool _refreshCombo = true;
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterElement"/> class.
        /// </summary>
        /// <param name="inputParam">The parameter this element represents</param>
        /// <param name="dataSets">An array of available data</param>
        public RasterElement(RasterParam inputParam, List<DataSetArray> dataSets)
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

        #region Methods

        /// <summary>
        /// updates the param if something's been changed
        /// </summary>
        public override void Refresh()
        {
            DoRefresh();
        }

        /// <summary>
        /// Adds a new entry to the drop down list from data provider
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void BtnAddDataClick(object sender, EventArgs e)
        {
            // Replace with something that uses the default data provider
            IRaster tempRaster = DataManager.DefaultDataManager.OpenRaster();

            // If the feature is null don't do anything the user probably hit cancel on the dialog
            if (tempRaster == null)
                return;

            // If its good add the feature set and save it
            _addedRasterSet = new DataSetArray(Path.GetFileNameWithoutExtension(tempRaster.Filename), tempRaster);
            Param.ModelName = _addedRasterSet.Name;
            Param.Value = _addedRasterSet.DataSet;
        }

        /// <summary>
        /// This changes the color of the light and the tooltip of the light based on the status of the text in the box
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void ComboRasterSelectedValueChanged(object sender, EventArgs e)
        {
            if (!_refreshCombo) return;
            DataSetArray dsa = comboRaster.SelectedItem as DataSetArray;
            if (dsa == null) return;
            Param.ModelName = dsa.Name;
            Param.Value = dsa.DataSet;
        }

        private void DoRefresh()
        {
            // Disable the combo box temporarily
            _refreshCombo = false;

            // We set the combo boxes status to empty to start
            Status = ToolStatus.Empty;
            LightTipText = ModelingMessageStrings.FeaturesetMissing;
            comboRaster.Items.Clear();

            // If the user added a raster set
            if (_addedRasterSet != null)
            {
                comboRaster.Items.Add(_addedRasterSet);
                if (Param.Value != null && Param.DefaultSpecified)
                {
                    if (_addedRasterSet.DataSet == Param.Value)
                    {
                        comboRaster.SelectedItem = _addedRasterSet;
                        Status = ToolStatus.Ok;
                        LightTipText = ModelingMessageStrings.FeaturesetValid;
                    }
                }
            }

            // Add all the dataSets back to the combo box
            if (_dataSets != null)
            {
                foreach (DataSetArray dsa in _dataSets)
                {
                    IRaster aRasterSet = dsa.DataSet as IRaster;
                    if (aRasterSet != null)
                    {
                        // If the featureset is the correct type and isn't already in the combo box we add it
                        if (comboRaster.Items.Contains(dsa) == false)
                        {
                            comboRaster.Items.Add(dsa);
                            if (Param.Value != null && Param.DefaultSpecified)
                            {
                                if (dsa.DataSet == Param.Value)
                                {
                                    comboRaster.SelectedItem = dsa;
                                    Status = ToolStatus.Ok;
                                    LightTipText = ModelingMessageStrings.FeaturesetValid;
                                }
                            }
                        }
                    }
                }
            }

            _refreshCombo = true;
        }

        #endregion
    }
}