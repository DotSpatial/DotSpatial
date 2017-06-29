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
    /// Open File Element
    /// </summary>
    public class OpenFileElement : DialogElement
    {
        #region Class Variables

        private readonly List<DataSetArray> _dataSets;
        private DataSetArray _addedTextFile;
        private bool _refreshCombo = true;
        private Button btnAddData;
        private ComboBox comboFile;

        #endregion

        #region Constructor

        /// <summary>
        /// Create an instance of the dialog
        /// </summary>
        /// <param name="param"></param>
        /// <param name="text"></param>
        public OpenFileElement(FileParam param, string text)
        {
            InitializeComponent();
            Param = param;
            //_fileName = text;
            // textBox1.Text = param.Value;
            GroupBox.Text = param.Name;
            DoRefresh();

            //Populates the dialog with the default parameter value
            if (param.Value != null && param.DefaultSpecified)
            {
                //_fileName = param.ModelName;
                base.Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.FeaturesetValid;
            }
        }

        /// <summary>
        /// Creates an instance of the dialog
        /// </summary>
        /// <param name="inputParam">The parameter this element represents</param>
        /// <param name="dataSets">An array of available data</param>
        public OpenFileElement(FileParam inputParam, List<DataSetArray> dataSets)
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
            comboFile.Items.Clear();

            //If the user added a text file
            if (_addedTextFile != null)
            {
                comboFile.Items.Add(_addedTextFile);
                if (Param.Value != null && Param.DefaultSpecified)
                {
                    if (_addedTextFile.DataSet == Param.Value)
                    {
                        comboFile.SelectedItem = _addedTextFile;
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
                    TextFile aTextFile = (dsa.DataSet as TextFile);
                    if (aTextFile != null)
                    {
                        //If the featureset is the correct type and isn't already in the combo box we add it
                        if (comboFile.Items.Contains(dsa) == false)
                        {
                            comboFile.Items.Add(dsa);
                            if (Param.Value != null && Param.DefaultSpecified)
                            {
                                if (dsa.DataSet == Param.Value)
                                {
                                    comboFile.SelectedItem = dsa;
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

        private void btnAddData_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select File Name";
            if (Param == null) Param = new FileParam("open filename");
            FileParam p = Param as FileParam;
            if (p != null) dialog.Filter = p.DialogFilter;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                TextFile tmpTextFile = new TextFile(dialog.FileName);
                _addedTextFile = new DataSetArray(Path.GetFileNameWithoutExtension(dialog.FileName), tmpTextFile);

                Param.ModelName = _addedTextFile.Name;
                Param.Value = _addedTextFile.DataSet;
                Refresh();
                base.Status = ToolStatus.Ok;
            }
            else
            {
                return;
            }
        }

        private void comboFile_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_refreshCombo)
            {
                DataSetArray dsa = comboFile.SelectedItem as DataSetArray;
                if (dsa != null)
                {
                    Param.ModelName = dsa.Name;
                    Param.Value = dsa.DataSet;
                    return;
                }
            }
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAddData = new Button();
            this.comboFile = new ComboBox();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            //
            // GroupBox1
            //
            this.GroupBox1.Controls.Add(this.btnAddData);
            this.GroupBox1.Controls.Add(this.comboFile);
            this.GroupBox1.Controls.SetChildIndex(this.comboFile, 0);
            this.GroupBox1.Controls.SetChildIndex(this.btnAddData, 0);
            //
            // btnAddData
            //
            this.btnAddData.Image = Images.AddLayer;
            this.btnAddData.Location = new Point(450, 10);
            this.btnAddData.Name = "btnAddData";
            this.btnAddData.Size = new Size(26, 26);
            this.btnAddData.TabIndex = 9;
            this.btnAddData.UseVisualStyleBackColor = true;
            this.btnAddData.Click += new EventHandler(this.btnAddData_Click);
            //
            // comboFile
            //
            this.comboFile.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboFile.FormattingEnabled = true;
            this.comboFile.Location = new Point(34, 12);
            this.comboFile.Name = "comboFile";
            this.comboFile.Size = new Size(410, 21);
            this.comboFile.TabIndex = 10;
            this.comboFile.SelectedValueChanged += new EventHandler(this.comboFile_SelectedValueChanged);
            //
            // OpenFileElement
            //
            this.AutoScaleDimensions = new SizeF(6F, 13F);

            this.Name = "OpenFileElement";
            this.GroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}