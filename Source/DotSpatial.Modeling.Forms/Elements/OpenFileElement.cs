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
    /// Open File Element
    /// </summary>
    public partial class OpenFileElement : DialogElement
    {
        #region Fields

        private readonly List<DataSetArray> _dataSets;
        private DataSetArray _addedTextFile;
        private bool _refreshCombo = true;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenFileElement"/> class.
        /// </summary>
        /// <param name="param">The parameter this element represents</param>
        /// <param name="text">Not used.</param>
        public OpenFileElement(FileParam param, string text)
        {
            InitializeComponent();
            Param = param;

            GroupBox.Text = param.Name;
            DoRefresh();

            // Populates the dialog with the default parameter value
            if (param.Value != null && param.DefaultSpecified)
            {
                Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.FeaturesetValid;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenFileElement"/> class.
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

        #region Methods

        /// <summary>
        /// updates the param if something's been changed
        /// </summary>
        public override void Refresh()
        {
            DoRefresh();
        }

        private void BtnAddDataClick(object sender, EventArgs e)
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
                    Status = ToolStatus.Ok;
                }
            }
        }

        private void ComboFileSelectedValueChanged(object sender, EventArgs e)
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

        private void DoRefresh()
        {
            // Disable the combo box temporarily
            _refreshCombo = false;

            // We set the combo boxes status to empty to start
            Status = ToolStatus.Empty;
            LightTipText = ModelingMessageStrings.FeaturesetMissing;
            _comboFile.Items.Clear();

            // If the user added a text file
            if (_addedTextFile != null)
            {
                _comboFile.Items.Add(_addedTextFile);
                if (Param.Value != null && Param.DefaultSpecified && _addedTextFile.DataSet == Param.Value)
                {
                    _comboFile.SelectedItem = _addedTextFile;
                    Status = ToolStatus.Ok;
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
                            Status = ToolStatus.Ok;
                            LightTipText = ModelingMessageStrings.FeaturesetValid;
                        }
                    }
                }
            }

            _refreshCombo = true;
        }

        #endregion
    }
}