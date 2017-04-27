// ********************************************************************************************************
// Product Name: DotSpatial.Tools.PolygonElement
// Description:  Polygon Element for use in the tool dialog
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
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Modeling.Forms.Elements
{
    /// <summary>
    /// Feature Set element out
    /// </summary>
    public class FeatureSetElementOut : DialogElement
    {
        #region Class Variables

        private Button btnAddData;
        private TextBox textBox1;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of the dialog
        /// </summary>
        /// <param name="outputParam">The parameter this element represents</param>
        /// <param name="dataSets">An array of available data</param>
        public FeatureSetElementOut(FeatureSetParam outputParam, List<DataSetArray> dataSets)
        {
            // Needed by the designer
            InitializeComponent();

            // We save the parameters passed in
            Param = outputParam;

            // Saves the label
            GroupBox.Text = Param.Name;

            // Sets up the initial status light indicator
            base.Status = ToolStatus.Empty;
            LightTipText = ModelingMessageStrings.FeaturesetMissing;

            // Populates the dialog with the default parameter value
            if (outputParam.Value != null && outputParam.DefaultSpecified)
            {
                textBox1.Text = outputParam.ModelName;
                base.Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.FeaturesetValid;
            }
        }

        #endregion

        #region Events

        private void btnAddData_Click(object sender, EventArgs e)
        {
            /////////////////////////////////
            // Replace with something that uses the default data provider
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.OverwritePrompt = true;
            sfd.Filter = "Shape Files|*.shp";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                IFeatureSet _addedFeatureSet = new FeatureSet();
                _addedFeatureSet.Filename = sfd.FileName;

                // If the features set is null do nothing the user probably hit cancel
                if (_addedFeatureSet == null)
                    return;

                // If the feature type is good save it
                // This inserts the new featureset into the list
                textBox1.Text = Path.GetFileNameWithoutExtension(_addedFeatureSet.Filename);
                Param.Value = _addedFeatureSet;
                base.Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.FeaturesetValid;
            }
        }

        #endregion

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.btnAddData = new Button();
            this.textBox1 = new TextBox();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // groupBox1
            //
            this.GroupBox.Controls.Add(this.textBox1);
            this.GroupBox.Controls.Add(this.btnAddData);
            this.GroupBox.Controls.SetChildIndex(this.StatusLabel, 0);
            this.GroupBox.Controls.SetChildIndex(this.btnAddData, 0);
            this.GroupBox.Controls.SetChildIndex(this.textBox1, 0);
            //
            // lblStatus
            //
            this.StatusLabel.Location = new Point(12, 20);
            //
            // btnAddData
            //
            this.btnAddData.Image = Images.AddLayer;
            this.btnAddData.Location = new Point(460, 14);
            this.btnAddData.Name = "btnAddData";
            this.btnAddData.Size = new Size(26, 26);
            this.btnAddData.TabIndex = 5;
            this.btnAddData.UseVisualStyleBackColor = true;
            this.btnAddData.Click += new EventHandler(this.btnAddData_Click);
            //
            // textBox1
            //
            this.textBox1.Location = new Point(44, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new Size(410, 20);
            this.textBox1.TabIndex = 6;
            //
            // PolygonElementOut
            //
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.Name = "LineElementOut";
            this.GroupBox.ResumeLayout(false);
            this.GroupBox.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion
    }
}