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

namespace DotSpatial.Modeling.Forms
{
    internal class PolygonElementOut : DialogElement
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
        public PolygonElementOut(PolygonFeatureSetParam outputParam, List<DataSetArray> dataSets)
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
                textBox1.Text = outputParam.ModelName;
                base.Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.FeaturesetValid;
            }
        }

        #endregion

        #region Events

        private void BtnAddDataClick(object sender, EventArgs e)
        {
            /////////////////////////////////
            //Replace with something that uses the default data provider
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.OverwritePrompt = true;
            sfd.Filter = "Shape Files|*.shp";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                IFeatureSet addedFeatureSet = new PolygonShapefile();
                addedFeatureSet.Filename = sfd.FileName;

                //This inserts the new featureset into the list
                textBox1.Text = Path.GetFileNameWithoutExtension(addedFeatureSet.Filename);
                Param.Value = addedFeatureSet;
                base.Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.FeaturesetValid;
            }
        }

        #endregion

        private void InitializeComponent()
        {
            btnAddData = new Button();
            textBox1 = new TextBox();
            GroupBox.SuspendLayout();
            SuspendLayout();
            //
            // groupBox1
            //
            GroupBox.Controls.Add(textBox1);
            GroupBox.Controls.Add(btnAddData);
            GroupBox.Controls.SetChildIndex(StatusLabel, 0);
            GroupBox.Controls.SetChildIndex(btnAddData, 0);
            GroupBox.Controls.SetChildIndex(textBox1, 0);
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
            btnAddData.TabIndex = 5;
            btnAddData.UseVisualStyleBackColor = true;
            btnAddData.Click += BtnAddDataClick;
            //
            // textBox1
            //
            textBox1.Location = new Point(44, 17);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(410, 20);
            textBox1.TabIndex = 6;
            //
            // PolygonElementOut
            //
            AutoScaleDimensions = new SizeF(6F, 13F);
            Name = "PolygonElementOut";
            GroupBox.ResumeLayout(false);
            GroupBox.PerformLayout();
            ResumeLayout(false);
        }
    }
}