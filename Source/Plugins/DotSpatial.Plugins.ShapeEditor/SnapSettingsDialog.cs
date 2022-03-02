// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.Windows.Forms;
using DotSpatial.Controls;

namespace DotSpatial.Plugins.ShapeEditor
{
    /// <summary>
    /// This dialog is used for de-/activing snapping.
    /// </summary>
    public partial class SnapSettingsDialog : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SnapSettingsDialog"/> class.
        /// </summary>
        /// <param name="map">The map that contains the layers.</param>
        public SnapSettingsDialog(IMap map)
        {
            InitializeComponent();
            List<SnapLayer> snaplist = new();

            foreach (var layer in map.GetFeatureLayers())
            {
                snaplist.Add(new SnapLayer(layer));
            }

            dgvLayer.AutoGenerateColumns = false;
            dgvLayer.DataSource = snaplist;
        }

        /// <summary>
        /// Gets or sets a value indicating whether snapping is active.
        /// </summary>
        public bool DoSnapping
        {
            get { return cbPerformSnap.Checked; }
            set { cbPerformSnap.Checked = value; }
        }

        /// <summary>
        /// Sets the check mark of the selected cell.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void DgvLayerCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvLayer.Columns[e.ColumnIndex].Name == dgvcSnappable.Name)
            {
                DataGridViewRow row = dgvLayer.Rows[e.RowIndex];
                row.Cells[e.ColumnIndex].Value = !(bool)row.Cells[e.ColumnIndex].Value;
            }
        }
    }
}
