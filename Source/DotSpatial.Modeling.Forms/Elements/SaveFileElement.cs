// ********************************************************************************************************
// Product Name: DotSpatial.Tools.OpenFileParam
// Description:  Double Parameters returned by an ITool allows the tool to specify a range and default value
//
// ********************************************************************************************************
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Teva Veluppillai. Created in Feb, 2010.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Modeling.Forms.Elements
{
    /// <summary>
    /// Save File Element
    /// </summary>
    public partial class SaveFileElement : DialogElement
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveFileElement"/> class.
        /// </summary>
        /// <param name="outputParam">The parameter this element represents</param>
        /// <param name="dataSets">An array of available data</param>
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
            FileParam p = Param as FileParam;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.OverwritePrompt = true;

                sfd.Filter = p != null ? p.DialogFilter : @"CSV Files|*.csv";

                if (sfd.ShowDialog() != DialogResult.OK) return;

                TextFile addedTextFile = new TextFile(sfd.FileName);

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