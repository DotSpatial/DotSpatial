// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;
using DotSpatial.Symbology;

namespace DotSpatial.Plugins.SetSelectable
{
    /// <summary>
    /// A plugin that adds a panel to the dock manager that enables the user to manage the layers selections.
    /// </summary>
    public class SetSelectablePlugin : Extension
    {
        #region Fields

        private DgvSelect _dgvSelection;
        private DockablePanel _selectPanel;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the priority. The high Priority allows legend to be loaded first.
        /// </summary>
        public override int Priority => 100;

        #endregion

        #region Methods

        /// <inheritdoc />
        public override void Activate()
        {
            _dgvSelection = new DgvSelect();

            _selectPanel = new DockablePanel("kSetSelectable", LocalizationStrings.PanelHeader, _dgvSelection, DockStyle.Left);
            App.DockManager.Add(_selectPanel);
            App.SerializationManager.Deserializing += SerializationManagerDeserializing;
            AttachLayerAddedEvents();
            App.Legend.UseLegendForSelection = false;
            App.AppCultureChanged += OnAppCultureChanged;
            base.Activate();
        }

        /// <inheritdoc />
        public override void Deactivate()
        {
            App.AppCultureChanged -= OnAppCultureChanged;

            // detach events
            if (App.Legend != null)
            {
                App.Legend.OrderChanged -= LegendOrderChanged;
                App.Legend.UseLegendForSelection = true;
            }

            App.SerializationManager.Deserializing -= SerializationManagerDeserializing;
            DetachLayerAddedEvents();
            base.Deactivate();
        }

        /// <summary>
        /// Adds the layer to DgvSelection if its not a MapGroup. Otherwise the EventHandlers get attached and the groups children get added to DgvSelection.
        /// </summary>
        /// <param name="addedLayer">Layer, that should be added to DGV_Selection.</param>
        private void AddLayer(ILayer addedLayer)
        {
            if (addedLayer == null) return;

            var grp = addedLayer as IMapGroup;
            if (grp != null)
            {
                // handle layerAdded event separately for groups because map.layerAdded event doesn't fire for groups.
                grp.LayerAdded += MapLayerAdded;
                grp.LayerRemoved += MapLayerRemoved;
                foreach (IMapLayer layer in grp.Layers)
                {
                    AddLayer(layer);
                }
            }
            else
            {
                _dgvSelection.AddLayer(addedLayer);
            }
        }

        /// <summary>
        /// Attaches the LayerAdded/LayerRemoved events to the groups and the map.
        /// </summary>
        private void AttachLayerAddedEvents()
        {
            App.Map.Layers.LayerMoved += LayersLayerMoved;
            App.Map.LayerAdded += MapLayerAdded;
            App.Map.MapFrame.LayerRemoved += MapLayerRemoved;
            if (App.Legend != null) App.Legend.OrderChanged += LegendOrderChanged;

            foreach (IMapLayer layer in App.Map.Layers)
            {
                AddLayer(layer);
            }
        }

        /// <summary>
        /// Detaches the LayerAdded/LayerRemoved events from the groups.
        /// </summary>
        private void DetachLayerAddedEvents()
        {
            App.Map.Layers.LayerMoved -= LayersLayerMoved;
            App.Map.LayerAdded -= MapLayerAdded;
            App.Map.MapFrame.LayerRemoved -= MapLayerRemoved;
            foreach (var grp in App.Map.MapFrame.GetAllGroups())
            {
                grp.LayerAdded -= MapLayerAdded;
                grp.LayerRemoved -= MapLayerRemoved;
            }
        }

        /// <summary>
        /// Moves the given layer to the new position inside DGV_Selection.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void LayersLayerMoved(object sender, LayerMovedEventArgs e)
        {
            _dgvSelection.MoveLayer(e.Layer, e.NewPosition);
        }

        /// <summary>
        /// Sorts the selectable layers according to the changed legend-order.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void LegendOrderChanged(object sender, EventArgs e)
        {
            _dgvSelection.MoveLayers(App.Map.Layers);
        }

        /// <summary>
        /// Adds the layer that was added to the map to the DGV_Selection.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void MapLayerAdded(object sender, LayerEventArgs e)
        {
            _dgvSelection.AddLayer(e.Layer);
        }

        /// <summary>
        /// Removes the layer, that was removed from map from the DGV_Selection.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void MapLayerRemoved(object sender, LayerEventArgs e)
        {
            _dgvSelection.RemoveLayer(e.Layer);
        }

        /// <summary>
        /// Adds the layers to DGV_Selection after a project gets loaded.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void SerializationManagerDeserializing(object sender, SerializingEventArgs e)
        {
            DetachLayerAddedEvents();
            AttachLayerAddedEvents();
        }

        private void OnAppCultureChanged(object sender, CultureInfo appCulture)
        {
            ExtensionCulture = appCulture;
            UpdatePlugInItems();
        }

        private void UpdatePlugInItems()
        {
            _selectPanel.Caption = LocalizationStrings.PanelHeader;
            if (_dgvSelection != null) _dgvSelection.UpdatesSelectResources();
        }
        #endregion
    }
}