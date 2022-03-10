// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Symbology;

namespace DotSpatial.Plugins.TableEditor
{
    /// <summary>
    /// Adds the attribute table editor.
    /// </summary>
    public class TableEditorPlugin : Extension
    {
        #region Methods

        /// <inheritdoc />
        public override void Activate()
        {
            App.HeaderControl.Add(
                new SimpleActionItem(HeaderControl.HomeRootItemKey, Resources.ViewAttributeTable, AttributeTableClick)
                {
                    GroupCaption = "Map Tool",
                    SmallImage = Resources.table_16x16,
                    LargeImage = Resources.table_32x32
                });
            App.Map.LayerAdded += MapLayerAdded;
            App.SerializationManager.Deserializing += SerializationManagerDeserializing;

            base.Activate();
            foreach (IMapLayer l in App.Map.Layers)
            {
                AddContextMenuItems(l);
            }
        }

        /// <inheritdoc />
        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();

            // detach events
            DetachLayerAddedEvents();
            App.SerializationManager.Deserializing -= SerializationManagerDeserializing;

            // remove context menu items.
            RemoveContextMenuItems();
            base.Deactivate();
        }

        private static void ShowAttributes(IFeatureLayer layer)
        {
            layer?.ShowAttributes();
        }

        private void AddContextMenuItems(ILayer addedLayer)
        {
            if (addedLayer is IMapGroup grp)
            {
                // map.layerAdded event doesn't fire for groups. Therefore, it's necessary
                // to handle this event separately for groups.
                grp.LayerAdded += MapLayerAdded;
            }

            if (addedLayer.ContextMenuItems.Exists(item => item.Name == Resources.AttributeTableEditor))
            {
                // assume menu item already exists. Do nothing.
                return;
            }

            // add context menu item.
            var menuItem = new SymbologyMenuItem(Resources.AttributeTableEditor, (sender, args) => ShowAttributes(addedLayer as IFeatureLayer))
            {
                Image = Resources.table_16x16
            };
            var cmi = addedLayer.ContextMenuItems;
            if (cmi.Count > 2)
            {
                addedLayer.ContextMenuItems.Insert(2, menuItem);
            }
            else
            {
                addedLayer.ContextMenuItems.Add(menuItem);
            }
        }

        /// <summary>
        /// Open attribute table.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void AttributeTableClick(object sender, EventArgs e)
        {
            IMapFrame mainMapFrame = App.Map.MapFrame;
            List<ILayer> layers = mainMapFrame.GetAllLayers();

            foreach (ILayer layer in layers.Where(l => l.IsSelected))
            {
                if (layer is not IFeatureLayer fl)
                {
                    continue;
                }

                ShowAttributes(fl);
            }
        }

        private void DetachLayerAddedEvents()
        {
            App.Map.LayerAdded -= MapLayerAdded;
            foreach (IMapGroup grp in App.Map.MapFrame.GetAllGroups())
            {
                grp.LayerAdded -= MapLayerAdded;
            }
        }

        private void MapLayerAdded(object sender, LayerEventArgs e)
        {
            if (e.Layer == null) return;

            AddContextMenuItems(e.Layer);
        }

        private void RemoveContextMenuItems()
        {
            foreach (ILayer lay in App.Map.MapFrame.GetAllLayers())
            {
                if (lay.ContextMenuItems.Exists(item => item.Name == Resources.AttributeTableEditor))
                {
                    lay.ContextMenuItems.Remove(lay.ContextMenuItems.First(item => item.Name == Resources.AttributeTableEditor));
                    return;
                }
            }
        }

        private void SerializationManagerDeserializing(object sender, SerializingEventArgs e)
        {
            // context menu items are added to layers when opening a project
            // this call is necessary because the LayerAdded event doesn't fire when a project is opened.
            foreach (ILayer layer in App.Map.MapFrame.GetAllLayers())
            {
                if (layer is IFeatureLayer fl)
                {
                    if (!fl.ContextMenuItems.Exists(item => item.Name == Resources.AttributeTableEditor))
                    {
                        // add context menu item.
                        var menuItem = new SymbologyMenuItem(Resources.AttributeTableEditor, (o, args) => ShowAttributes(fl))
                        {
                            Image = Resources.table_16x16
                        };
                        fl.ContextMenuItems.Insert(2, menuItem);
                    }
                }
            }

            // attach layer added events to existing groups
            foreach (IMapGroup grp in App.Map.MapFrame.GetAllGroups())
            {
                grp.LayerAdded += MapLayerAdded;
            }
        }

        #endregion
    }
}