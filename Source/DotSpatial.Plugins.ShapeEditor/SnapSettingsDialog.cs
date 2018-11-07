// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using DotSpatial.Controls;

namespace DotSpatial.Plugins.ShapeEditor
{
    /// <summary>
    /// This dialog is used for de-/activing snapping.
    /// </summary>
    public partial class SnapSettingsDialog : Form
    {
        #region Fields

        private CultureInfo _snappCulture;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapSettingsDialog"/> class.
        /// </summary>
        /// <param name="map">The map that contains the layers.</param>
        public SnapSettingsDialog(IMap map)
        {
            InitializeComponent();
            List<SnapLayer> snaplist = new List<SnapLayer>();

            foreach (var layer in map.GetFeatureLayers())
            {
                snaplist.Add(new SnapLayer(layer));
            }

            dgvLayer.AutoGenerateColumns = false;
            dgvLayer.DataSource = snaplist;

            SnappCulture = new CultureInfo(string.Empty);
        }

        #region Porperties

        /// <summary>
        /// Gets or sets a value indicating whether snapping is active.
        /// </summary>
        public bool DoSnapping
        {
            get { return cbPerformSnap.Checked; }
            set { cbPerformSnap.Checked = value; }
        }

        /// <summary>
        /// sets a value indicating the culture to use for resources.
        /// </summary>
        public CultureInfo SnappCulture
        {
            set
            {
                if (_snappCulture == value) return;

                _snappCulture = value;

                if (_snappCulture == null) _snappCulture = new CultureInfo(string.Empty);

                Thread.CurrentThread.CurrentCulture = _snappCulture;
                Thread.CurrentThread.CurrentUICulture = _snappCulture;
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

            if (dgvLayer.Columns[e.ColumnIndex].Name == dgvcSnappable.Name
                || dgvLayer.Columns[e.ColumnIndex].Name == dgvcSnapVertices.Name
                || dgvLayer.Columns[e.ColumnIndex].Name == dgvcSnapStartPoint.Name
                || dgvLayer.Columns[e.ColumnIndex].Name == dgvcSnapEndPoint.Name
                || dgvLayer.Columns[e.ColumnIndex].Name == dgvcSnapEdges.Name)
            {
                DataGridViewRow row = dgvLayer.Rows[e.RowIndex];
                row.Cells[e.ColumnIndex].Value = !(bool)row.Cells[e.ColumnIndex].Value;
            }
        }
    }
}
