// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Modeling.Forms.Elements
{
    /// <summary>
    /// Feature Set element out.
    /// </summary>
    public partial class FeatureSetElementOut : DialogElement
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSetElementOut"/> class.
        /// </summary>
        /// <param name="outputParam">The parameter this element represents.</param>
        /// <param name="dataSets">An array of available data.</param>
        public FeatureSetElementOut(FeatureSetParam outputParam, List<DataSetArray> dataSets)
        {
            // Needed by the designer
            InitializeComponent();

            // We save the parameters passed in
            Param = outputParam;

            // Saves the label
            GroupBox.Text = Param.Name;

            // Sets up the initial status light indicator
            Status = ToolStatus.Empty;
            LightTipText = ModelingMessageStrings.FeaturesetMissing;

            // Populates the dialog with the default parameter value
            if (outputParam.Value != null && outputParam.DefaultSpecified)
            {
                textBox1.Text = outputParam.ModelName;
                Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.FeaturesetValid;
            }
        }

        #endregion

        #region Methods

        private void BtnAddDataClick(object sender, EventArgs e)
        {
            // Replace with something that uses the default data provider
            using (var sfd = new SaveFileDialog { OverwritePrompt = true, Filter = @"Shape Files|*.shp" })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;

                IFeatureSet addedFeatureSet = new FeatureSet();
                addedFeatureSet.Filename = sfd.FileName;

                // If the feature type is good save it
                // This inserts the new featureset into the list
                textBox1.Text = Path.GetFileNameWithoutExtension(addedFeatureSet.Filename);
                Param.Value = addedFeatureSet;
                Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.FeaturesetValid;
            }
        }

        #endregion
    }
}