using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Data;
using System.Diagnostics;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;
using DotSpatial.Symbology;
using System.Windows.Forms;

namespace DotSpatial.Plugins.SetSelectable
{
    public class SetSelectablePlugin : Extension
    {
        #region variables and properties

        DGV_Select _DGV_Selection;

        /// <summary>
        /// High Priority allows legend to be loaded first.
        /// </summary>
        public override int Priority
        {
            get { return 100; }
        }
        #endregion

        public override void Activate()
        {
            _DGV_Selection = new DGV_Select();
            App.DockManager.Add(new DockablePanel("kSetSelectable", LocalizationStrings.PanelHeader, _DGV_Selection, DockStyle.Left));

            App.Map.LayerAdded += Map_LayerAdded;
            App.Map.MapFrame.LayerRemoved += Map_LayerRemoved;
            App.SerializationManager.Deserializing += SerializationManager_Deserializing;
            if (App.Legend != null) App.Legend.OrderChanged += Legend_OrderChanged;

            foreach (Layer layer in App.Map.Layers)
            {
                AddLayer(layer);
            }
            base.Activate();
        }

        public override void Deactivate()
        {
            // detach events
            if (App.Legend != null) App.Legend.OrderChanged -= Legend_OrderChanged;
            App.SerializationManager.Deserializing -= SerializationManager_Deserializing;
            DetachLayerAddedEvents();
            base.Deactivate();
        }

        /// <summary>
        /// Detaches the LayerAdded/LayerRemoved events from the groups.
        /// </summary>
        private void DetachLayerAddedEvents()
        {
            App.Map.LayerAdded -= Map_LayerAdded;
            App.Map.MapFrame.LayerRemoved -= Map_LayerRemoved;
            foreach (IGroup grp in App.Map.MapFrame.GetAllGroups())
            {
                grp.LayerAdded -= Map_LayerAdded;
                grp.LayerRemoved -= Map_LayerRemoved;
            }
        }

        /// <summary>
        /// Adds the layer to DGV_Selection if its not a MapGroup. Else the EventHandlers get attached and the Groups children get added to DGV_Selection.
        /// </summary>
        /// <param name="addedLayer">Layer, that should be added to DGV_Selection.</param>
        private void AddLayer(ILayer addedLayer)
        {
            if (addedLayer == null) return;
            if (addedLayer is IMapGroup)
            {
                IMapGroup grp = (IMapGroup)addedLayer;
                //handle layerAdded event separately for groups because map.layerAdded event doesn't fire for groups.
                grp.LayerAdded += Map_LayerAdded;
                grp.LayerRemoved += Map_LayerRemoved;
                foreach (Layer layer in grp.Layers)
                {
                    AddLayer(layer);
                }
            }
            else
            {
                _DGV_Selection.AddLayer(addedLayer);
            }
        }

        /// <summary>
        /// Sorts the selectable layers according to the changed legend-order.
        /// </summary>
        private void Legend_OrderChanged(object sender, EventArgs e)
        {
            _DGV_Selection.MoveLayers(App.Map.Layers);
        }

        /// <summary>
        /// Adds the layers to DGV_Selection after a project gets loaded.
        /// </summary>
        private void SerializationManager_Deserializing(object sender, SerializingEventArgs e)
        {
            //This call is necessary because the LayerAdded event doesn't fire when a project is opened.
            foreach (Layer layer in App.Map.Layers)
            {
                AddLayer(layer);
            }
        }

        /// <summary>
        /// Adds the layer that was added to the map to the DGV_Selection.
        /// </summary>
        private void Map_LayerAdded(object sender, LayerEventArgs e)
        {
            _DGV_Selection.AddLayer(e.Layer);
        }

        /// <summary>
        /// Removes the layer, that was removed from map from the DGV_Selection.
        /// </summary>
        private void Map_LayerRemoved(object sender, LayerEventArgs e)
        {
            _DGV_Selection.RemoveLayer(e.Layer);
        }
    }
}
