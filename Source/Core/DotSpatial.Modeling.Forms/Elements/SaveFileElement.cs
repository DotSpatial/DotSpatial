// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Modeling.Forms.Elements
{
    /// <summary>
    /// Save File Element.
    /// </summary>
    public partial class SaveFileElement : DialogElement
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveFileElement"/> class.
        /// </summary>
        /// <param name="outputParam">The parameter this element represents.</param>
        /// <param name="dataSets">An array of available data.</param>
        public SaveFileElement(FileParam outputParam, List<DataSetArray> dataSets)
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
                txtDataTable.Text = outputParam.ModelName;
                Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.FeaturesetValid;
            }
        }

        #endregion

        #region Methods

        private void BtnAddDataClick(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new())
            {
                sfd.OverwritePrompt = true;

                sfd.Filter = Param is FileParam p ? p.DialogFilter : @"CSV Files|*.csv";

                if (sfd.ShowDialog() != DialogResult.OK) return;

                TextFile addedTextFile = new(sfd.FileName);

                // This inserts the new featureset into the list
                txtDataTable.Text = Path.GetFileNameWithoutExtension(addedTextFile.Filename);
                Param.Value = addedTextFile;
            }

            Status = ToolStatus.Ok;
            LightTipText = ModelingMessageStrings.FeaturesetValid;
        }

        #endregion
    }
}