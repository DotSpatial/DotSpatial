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
    /// Open File Element
    /// </summary>
    public class OpenFileElement : DialogElement
    {
        #region Class Variables

        private readonly List<DataSetArray> _dataSets;
        private DataSetArray _addedTextFile;
        private bool _refreshCombo = true;
        private Button _btnAddData;
        private ComboBox _comboFile;

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
            // _fileName = text;
            // textBox1.Text = param.Value;
            GroupBox.Text = param.Name;
            DoRefresh();

            // Populates the dialog with the default parameter value
            if (param.Value != null && param.DefaultSpecified)
            {
                // _fileName = param.ModelName;
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

        private void DoRefresh()
        {
            // Disable the combo box temporarily
            _refreshCombo = false;

            // We set the combo boxes status to empty to start
            base.Status = ToolStatus.Empty;
            LightTipText = ModelingMessageStrings.FeaturesetMissing;
            _comboFile.Items.Clear();

            // If the user added a text file
            if (_addedTextFile != null)
            {
                _comboFile.Items.Add(_addedTextFile);
                if (Param.Value != null && Param.DefaultSpecified && _addedTextFile.DataSet == Param.Value)
                {
                    _comboFile.SelectedItem = _addedTextFile;
                    base.Status = ToolStatus.Ok;
                    LightTipText = ModelingMessageStrings.FeaturesetValid;
                }
            }

            // Add all the dataSets back to the combo box
            if (_dataSets != null)
            {
                foreach (DataSetArray dsa in _dataSets)
                {
                    TextFile aTextFile = dsa.DataSet as TextFile;
                    if (aTextFile != null && !_comboFile.Items.Contains(dsa))
                    {
                        // If the featureset is the correct type and isn't already in the combo box we add it
                        _comboFile.Items.Add(dsa);
                        if (Param.Value != null && Param.DefaultSpecified && dsa.DataSet == Param.Value)
                        {
                            _comboFile.SelectedItem = dsa;
                            base.Status = ToolStatus.Ok;
                            LightTipText = ModelingMessageStrings.FeaturesetValid;
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
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = ModelingMessageStrings.OpenFileElement_btnAddDataClick_SelectFileName;
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
            }
        }

        private void comboFile_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_refreshCombo)
            {
                DataSetArray dsa = _comboFile.SelectedItem as DataSetArray;
                if (dsa != null)
                {
                    Param.ModelName = dsa.Name;
                    Param.Value = dsa.DataSet;
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
            this._btnAddData = new Button();
            this._comboFile = new ComboBox();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // GroupBox
            //
            this.GroupBox.Controls.Add(this._btnAddData);
            this.GroupBox.Controls.Add(this._comboFile);
            this.GroupBox.Controls.SetChildIndex(this._comboFile, 0);
            this.GroupBox.Controls.SetChildIndex(this._btnAddData, 0);
            //
            // btnAddData
            //
            this._btnAddData.Image = Images.AddLayer;
            this._btnAddData.Location = new Point(450, 10);
            this._btnAddData.Name = "_btnAddData";
            this._btnAddData.Size = new Size(26, 26);
            this._btnAddData.TabIndex = 9;
            this._btnAddData.UseVisualStyleBackColor = true;
            this._btnAddData.Click += new EventHandler(this.btnAddData_Click);
            //
            // comboFile
            //
            this._comboFile.DropDownStyle = ComboBoxStyle.DropDownList;
            this._comboFile.FormattingEnabled = true;
            this._comboFile.Location = new Point(34, 12);
            this._comboFile.Name = "_comboFile";
            this._comboFile.Size = new Size(410, 21);
            this._comboFile.TabIndex = 10;
            this._comboFile.SelectedValueChanged += new EventHandler(this.comboFile_SelectedValueChanged);
            //
            // OpenFileElement
            //
            this.AutoScaleDimensions = new SizeF(6F, 13F);

            this.Name = "OpenFileElement";
            this.GroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}