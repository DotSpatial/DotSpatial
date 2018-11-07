// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This dialog is used for de-/activing snapping.
    /// </summary>
    public partial class LegendSettingsDialog : Form
    {
        #region Fields

        private List<ILayer> _layerslist;
        private IMap _map;
        private CultureInfo _settingsCulture;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="LegendSettingsDialog"/> class.
        /// </summary>
        /// <param name="map">The map that contains the layers.</param>
        public LegendSettingsDialog(IMap map)
        {
            if (map == null || map.Legend == null) return;

            _map = map;
            InitializeComponent();

            chkEditLegendBoxes.Checked = _map.Legend.EditLegendItemBoxes;
            chkShowLegendMenus.Checked = _map.Legend.ShowContextMenu;

            _layerslist = new List<ILayer>();

            foreach (var layer in map.GetLayers())
            {
                _layerslist.Add(layer);
                dgvLayer.Rows.Add(layer.LegendItemVisible, layer.LegendText);
            }

            dgvLayer.AutoGenerateColumns = false;
            SettingsCulture = map.Legend.LegendCulture;
        }

        #region Porperties

        /// <summary>
        /// sets a value indicating the culture to use for resources.
        /// </summary>
        public CultureInfo SettingsCulture
        {
            set
            {
                if (_settingsCulture == value) return;

                _settingsCulture = value;

                if (_settingsCulture == null) _settingsCulture = new CultureInfo(string.Empty);

                Thread.CurrentThread.CurrentCulture = _settingsCulture;
                Thread.CurrentThread.CurrentUICulture = _settingsCulture;
                UpdateRessources();
                Refresh();
            }
        }

        #endregion

        /// <summary>
        /// Sets the check mark of the selected cell.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void DgvLayerCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvLayer.Columns[e.ColumnIndex].Name == dgvcShow.Name)
            {
                DataGridViewRow row = dgvLayer.Rows[e.RowIndex];
                row.Cells[e.ColumnIndex].Value = !(bool)row.Cells[e.ColumnIndex].Value;
                _layerslist[e.RowIndex].LegendItemVisible = (bool)row.Cells[e.ColumnIndex].Value;
                _map.Legend.RefreshNodes();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            _map.Legend.EditLegendItemBoxes = chkEditLegendBoxes.Checked;
            _map.Legend.ShowContextMenu = chkShowLegendMenus.Checked;
        }
    }
}
