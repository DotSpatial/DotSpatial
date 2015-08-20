using System;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;
using DotSpatial.Symbology;

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
            App.SerializationManager.Deserializing += SerializationManager_Deserializing;
            AttachLayerAddedEvents();
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
        /// Attaches the LayerAdded/LayerRemoved events to the groups and the map.
        /// </summary>
        private void AttachLayerAddedEvents()
        {
            App.Map.Layers.LayerMoved += Layers_LayerMoved;
            App.Map.LayerAdded += Map_LayerAdded;
            App.Map.MapFrame.LayerRemoved += Map_LayerRemoved;
            if (App.Legend != null) App.Legend.OrderChanged += Legend_OrderChanged;

            foreach (ILayer layer in App.Map.Layers)
            {
                AddLayer(layer);
            }
        }

        /// <summary>
        /// Detaches the LayerAdded/LayerRemoved events from the groups.
        /// </summary>
        private void DetachLayerAddedEvents()
        {
            App.Map.Layers.LayerMoved -= Layers_LayerMoved;
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

            var grp = addedLayer as IMapGroup;
            if (grp != null)
            {
                //handle layerAdded event separately for groups because map.layerAdded event doesn't fire for groups.
                grp.LayerAdded += Map_LayerAdded;
                grp.LayerRemoved += Map_LayerRemoved;
                foreach (ILayer layer in grp.Layers)
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
            DetachLayerAddedEvents();
            AttachLayerAddedEvents();
        }

        /// <summary>
        /// Moves the given layer to the new position inside DGV_Selection.
        /// </summary>
        private void Layers_LayerMoved(object sender, LayerMovedEventArgs e)
        {
            _DGV_Selection.MoveLayer(e.Layer, e.NewPosition);
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
