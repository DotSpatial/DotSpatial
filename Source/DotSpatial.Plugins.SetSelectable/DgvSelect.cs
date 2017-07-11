// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Symbology;

namespace DotSpatial.Plugins.SetSelectable
{
    /// <summary>
    /// The datagrid view used for managing the layers selection.
    /// </summary>
    public partial class DgvSelect : UserControl
    {
        #region Fields

        private readonly List<LayerSelection> _layers;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DgvSelect"/> class.
        /// </summary>
        public DgvSelect()
        {
            InitializeComponent();
            _layers = new List<LayerSelection>();
            DgvLayer.AutoGenerateColumns = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the given layer to the list.
        /// </summary>
        /// <param name="layer">Layer, that should be added.</param>
        public void AddLayer(ILayer layer)
        {
            IFeatureLayer mLayer = layer as IFeatureLayer;
            if (mLayer != null)
            {
                mLayer.SelectionChanged += SelectionChanged;
                LayerSelection layerSelection = _layers.FirstOrDefault(f => ReferenceEquals(f.Layer, mLayer));
                if (layerSelection != null) _layers.Remove(layerSelection);
                else layerSelection = new LayerSelection(mLayer);
                _layers.Insert(0, layerSelection);

                ChangeDataSource();
            }
        }

        /// <summary>
        /// Updates the datagridviews datasource and corrects the column order.
        /// </summary>
        public void ChangeDataSource()
        {
            if (DgvLayer.DataSource != null) DgvLayer.DataSource = null;
            DgvLayer.DataSource = _layers;
        }

        /// <summary>
        /// Moves the given layer to the new position.
        /// </summary>
        /// <param name="layer">Layer that gets moved.</param>
        /// <param name="newPosition">Position  the layer gets moved to.</param>
        public void MoveLayer(ILayer layer, int newPosition)
        {
            IFeatureLayer mLayer = layer as IFeatureLayer;
            if (mLayer == null) return;

            int index = _layers.FindIndex(f => ReferenceEquals(f.Layer, mLayer));
            if (index < 0 || index == newPosition) return;

            LayerSelection old = _layers[index];
            _layers.RemoveAt(index);

            if (_layers.Count <= newPosition) // after the list
                _layers.Add(old);
            else if (newPosition < 0) // before the list
                _layers.Insert(0, old);
            else _layers.Insert(newPosition, old); // somewhere inside
            ChangeDataSource();
        }

        /// <summary>
        /// Sorts the FeatureLayers according to the order of the given collection.
        /// </summary>
        /// <param name="collection">Collection to sort by.</param>
        public void MoveLayers(IMapLayerCollection collection)
        {
            int reverseI = 0; // position in layers is reverse to position in collection
            bool moved = false;
            for (int i = collection.Count - 1; i >= 0; i--)
            {
                IFeatureLayer mLayer = collection[i] as IFeatureLayer;
                if (mLayer != null)
                {
                    int index = _layers.FindIndex(f => ReferenceEquals(f.Layer, mLayer));
                    if (index != reverseI)
                    {
                        var layer = _layers[index];
                        _layers.Remove(layer);
                        _layers.Insert(reverseI, layer);
                        moved = true;
                    }

                    reverseI += 1; // non-FeatureLayers get ignored
                }
            }

            if (moved) ChangeDataSource();
        }

        /// <summary>
        /// Deletes the given layer from the list.
        /// </summary>
        /// <param name="layer">Layer, that should be removed.</param>
        public void RemoveLayer(ILayer layer)
        {
            IFeatureLayer mLayer = layer as IFeatureLayer;
            if (mLayer != null)
            {
                int index = _layers.FindIndex(f => ReferenceEquals(f.Layer, mLayer));
                if (index > -1)
                {
                    mLayer.SelectionChanged -= SelectionChanged;
                    _layers.RemoveAt(index);
                }

                ChangeDataSource();
            }
        }

        /// <summary>
        /// Corrects the text in the datagridview, when the selection of a layer changes.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        public void SelectionChanged(object sender, EventArgs e)
        {
            DgvLayer.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            DgvLayer.Refresh();
        }

        /// <summary>
        /// If Selectable-Column is clicked the checkmark whether the corresponding layer allows selection gets set. If the Unselect-Button is clicked the corresponding features get unselected.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void DgvLayerCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (DgvLayer.Columns[e.ColumnIndex].Name == DgvcSelectable.Name)
            {
                DataGridViewRow row = DgvLayer.Rows[e.RowIndex];
                row.Cells[e.ColumnIndex].Value = !(bool)row.Cells[e.ColumnIndex].Value;
            }
            else if (DgvLayer.Columns[e.ColumnIndex].Name == DgvcUnselect.Name)
            {
                var layer = ((LayerSelection)DgvLayer.Rows[e.RowIndex].DataBoundItem).Layer;
                var enabled = layer.SelectionEnabled;
                layer.SelectionEnabled = true;
                layer.UnSelectAll();
                layer.SelectionEnabled = enabled;
            }
        }

        /// <summary>
        /// Shows the tooltip for the cell the mouse is hovering over.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void DgvLayerCellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.ColumnIndex < 0) return;

            if (DgvLayer.Columns[e.ColumnIndex].Name == DgvcSelectable.Name)
            {
                e.ToolTipText = LocalizationStrings.DgvSelectableTooltip;
            }
            else if (DgvLayer.Columns[e.ColumnIndex].Name == DgvcUnselect.Name)
            {
                e.ToolTipText = LocalizationStrings.DgvcUnselectTooltip;
            }
            else if (DgvLayer.Columns[e.ColumnIndex].Name == DgvcCount.Name)
            {
                e.ToolTipText = LocalizationStrings.DgvcCountTooltip;
            }
        }

        /// <summary>
        /// Initializes the datagridview.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void DgvSelectLoad(object sender, EventArgs e)
        {
            ChangeDataSource();
        }

        /// <summary>
        /// Sets the checkmark for all layers.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void TsbCheckAllClick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in DgvLayer.Rows)
            {
                row.Cells[DgvcSelectable.Name].Value = true;
            }
        }

        /// <summary>
        /// Removes the checkmark for all layers.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void TsbCheckNoneClick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in DgvLayer.Rows)
            {
                row.Cells[DgvcSelectable.Name].Value = false;
            }
        }

        /// <summary>
        /// Selects all features of all selectable layers.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void TsbSelectAllClick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in DgvLayer.Rows)
            {
                if ((bool)row.Cells[DgvcSelectable.Name].Value)
                {
                    ((LayerSelection)row.DataBoundItem).Layer.DataSet.UpdateExtent();
                    ((LayerSelection)row.DataBoundItem).Layer.SelectAll();
                }
            }
        }

        /// <summary>
        /// Unselects all features of all selectable layers.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void TsbSelectNoneClick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in DgvLayer.Rows)
            {
                if ((bool)row.Cells[DgvcSelectable.Name].Value)
                {
                    ((LayerSelection)row.DataBoundItem).Layer.UnSelectAll();
                }
            }
        }

        #endregion
    }
}