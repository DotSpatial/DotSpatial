// ********************************************************************************************************
// Product Name: DotSpatial.Tools.OpenFileParam
// Description:  Double Parameters returned by an ITool allows the tool to specify a range and default value
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
// The Initial Developer of this Original Code is Teva Veluppillai. Created in Feb, 2010.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// Save File Element
    /// </summary>
    public class SaveFileElement : DialogElement
    {
        #region Constructor

        /// <summary>
        /// Creates an instance of the dialog
        /// </summary>
        /// <param name="outputParam">The parameter this element represents</param>
        /// <param name="dataSets">An array of available data</param>
        public SaveFileElement(FileParam outputParam, List<DataSetArray> dataSets)
        {
            //Needed by the designer
            InitializeComponent();

            //We save the parameters passed in
            Param = outputParam;

            //Saves the label
            GroupBox.Text = Param.Name;

            //Sets up the initial status light indicator
            base.Status = ToolStatus.Empty;
            LightTipText = ModelingMessageStrings.FeaturesetMissing;

            //Populates the dialog with the default parameter value
            if (outputParam.Value != null && outputParam.DefaultSpecified)
            {
                txtDataTable.Text = outputParam.ModelName;
                base.Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.FeaturesetValid;
            }
        }

        #endregion

        #region Events

        private Button btnAddData;
        private TextBox txtDataTable;

        private void btnAddData_Click(object sender, EventArgs e)
        {
            FileParam p = Param as FileParam;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.OverwritePrompt = true;

                if (p != null)
                {
                    sfd.Filter = p.DialogFilter;
                }
                else
                {
                    sfd.Filter = "CSV Files|*.csv";
                }

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;
                TextFile addedTextFile = new TextFile(sfd.FileName);
                //This inserts the new featureset into the list
                txtDataTable.Text = Path.GetFileNameWithoutExtension(addedTextFile.FileName);
                Param.Value = addedTextFile;
            }
            base.Status = ToolStatus.Ok;
            LightTipText = ModelingMessageStrings.FeaturesetValid;
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtDataTable = new TextBox();
            this.btnAddData = new Button();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            //
            // GroupBox1
            //
            this.GroupBox1.Controls.Add(this.btnAddData);
            this.GroupBox1.Controls.SetChildIndex(this.btnAddData, 0);
            //
            // txtDataTable
            //
            this.txtDataTable.Location = new Point(45, 13);
            this.txtDataTable.Name = "txtDataTable";
            this.txtDataTable.Size = new Size(356, 20);
            this.txtDataTable.TabIndex = 9;
            //
            // btnAddData
            //
            this.btnAddData.Image = Images.AddLayer;
            this.btnAddData.Location = new Point(432, 10);
            this.btnAddData.Name = "btnAddData";
            this.btnAddData.Size = new Size(26, 26);
            this.btnAddData.TabIndex = 8;
            this.btnAddData.UseVisualStyleBackColor = true;
            this.btnAddData.Click += new EventHandler(this.btnAddData_Click);
            //
            // SaveFileElement
            //
            this.AutoScaleDimensions = new SizeF(6F, 13F);

            this.Controls.Add(this.txtDataTable);
            this.Name = "SaveFileElement";
            this.Controls.SetChildIndex(this.GroupBox1, 0);
            this.Controls.SetChildIndex(this.txtDataTable, 0);
            this.GroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        #endregion
    }
}