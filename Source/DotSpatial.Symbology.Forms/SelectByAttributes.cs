// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// SelectByAttributes.
    /// </summary>
    public partial class SelectByAttributes : Form
    {
        #region Fields

        private IFeatureLayer _activeLayer;
        private IFeatureLayer[] _layersToSelect;
        private IFrame _mapFrame;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectByAttributes"/> class.
        /// </summary>
        public SelectByAttributes()
        {
            InitializeComponent();
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectByAttributes"/> class.
        /// </summary>
        /// <param name="mapFrame">The MapFrame containing the layers.</param>
        public SelectByAttributes(IFrame mapFrame)
        {
            _mapFrame = mapFrame;

            InitializeComponent();
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectByAttributes"/> class.
        /// </summary>
        /// <param name="layersToSelect">Layers to select.</param>
        public SelectByAttributes(params IFeatureLayer[] layersToSelect)
        {
            if (layersToSelect == null) throw new ArgumentNullException(nameof(layersToSelect));

            _layersToSelect = layersToSelect;

            InitializeComponent();
            Configure();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the map frame to use for this control.
        /// </summary>
        public IFrame MapFrame
        {
            get
            {
                return _mapFrame;
            }

            set
            {
                _layersToSelect = null;
                _mapFrame = value;
                Configure();
            }
        }

        #endregion

        #region Methods

        private void ApplyFilter()
        {
            string filter = sqlQueryControl1.ExpressionText;
            if (_activeLayer != null)
            {
                try
                {
                    _activeLayer.SelectByAttribute(filter, GetSelectMode());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    MessageBox.Show(SymbologyFormsMessageStrings.SelectByAttributes_ErrorWhileAttemptingToApplyExpression);
                }
            }
        }

        private void BtnApplyClick(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void BtnCloseClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            ApplyFilter();
            Close();
        }

        private void CmbLayersSelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = cmbLayers.SelectedValue as DataRowView;
            if (drv != null)
            {
                _activeLayer = drv.Row["Value"] as IFeatureLayer;
            }
            else
            {
                _activeLayer = cmbLayers.SelectedValue as IFeatureLayer;
            }

            if (_activeLayer == null) return;
            if (!_activeLayer.DataSet.AttributesPopulated && _activeLayer.DataSet.NumRows() < 50000)
            {
                _activeLayer.DataSet.FillAttributes();
            }

            if (_activeLayer.EditMode || _activeLayer.DataSet.AttributesPopulated)
            {
                sqlQueryControl1.Table = _activeLayer.DataSet.DataTable;
            }
            else
            {
                sqlQueryControl1.AttributeSource = _activeLayer.DataSet;
            }
        }

        private void Configure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(IFeatureLayer));

            IEnumerable<ILayer> layersSource;
            if (_layersToSelect != null)
            {
                layersSource = _layersToSelect;
            }
            else if (_mapFrame != null)
            {
                layersSource = _mapFrame;
            }
            else
            {
                layersSource = Enumerable.Empty<ILayer>();
            }

            foreach (var layer in layersSource.OfType<IFeatureLayer>())
            {
                DataRow dr = dt.NewRow();
                dr["Name"] = layer.LegendText;
                dr["Value"] = layer;
                dt.Rows.Add(dr);
            }

            cmbLayers.DataSource = dt;
            cmbLayers.DisplayMember = "Name";
            cmbLayers.ValueMember = "Value";

            cmbMethod.SelectedIndex = 0;

            if (cmbLayers.Items.Count > 0)
            {
                cmbLayers.SelectedIndex = 0;
            }
        }

        private ModifySelectionMode GetSelectMode()
        {
            switch (cmbMethod.SelectedIndex)
            {
                case 0:
                    return ModifySelectionMode.Replace;
                case 1:
                    return ModifySelectionMode.Append;
                case 2:
                    return ModifySelectionMode.Subtract;
                case 3:
                    return ModifySelectionMode.SelectFrom;
            }

            return ModifySelectionMode.Replace;
        }

        #endregion
    }
}