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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Modeling.Forms.Elements
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
                txtDataTable.Text = Path.GetFileNameWithoutExtension(addedTextFile.Filename);
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
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // GroupBox1
            //
            this.GroupBox.Controls.Add(this.btnAddData);
            this.GroupBox.Controls.SetChildIndex(this.btnAddData, 0);
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
            this.Controls.SetChildIndex(this.GroupBox, 0);
            this.Controls.SetChildIndex(this.txtDataTable, 0);
            this.GroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        #endregion
    }
}