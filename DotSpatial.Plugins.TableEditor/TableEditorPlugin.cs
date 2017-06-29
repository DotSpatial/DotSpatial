using System;
using System.Collections.Generic;
using System.Linq;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Plugins.TableEditor.Properties;
using DotSpatial.Symbology;

namespace DotSpatial.Plugins.TableEditor
{
    public class TableEditorPlugin : Extension
    {
        //context menu item name
        //TODO: make this localizable
        const string contextMenuItemName = "Attribute Table Editor";

        public override void Activate()
        {
            App.HeaderControl.Add(new SimpleActionItem(HeaderControl.HomeRootItemKey, "View Attribute Table", AttributeTable_Click) { GroupCaption = "Map Tool", SmallImage = Resources.table_16x16, LargeImage = Resources.table_32x32 });
            App.Map.LayerAdded += Map_LayerAdded;
            App.SerializationManager.Deserializing += SerializationManager_Deserializing;
            // TODO: if layers were loaded before this plugin, do something about adding them to the context menu.
            base.Activate();
        }

        private void SerializationManager_Deserializing(object sender, SerializingEventArgs e)
        {
            // context menu items are added to layers when opening a project
            // this call is necessary because the LayerAdded event doesn't fire when a project is opened.
            foreach (ILayer layer in App.Map.MapFrame.GetAllLayers())
            {
                IFeatureLayer fl = layer as IFeatureLayer;
                if (fl != null)
                {
                    if (!fl.ContextMenuItems.Exists(item => item.Name == contextMenuItemName))
                    {
                        // add context menu item.
                        var menuItem = new SymbologyMenuItem(contextMenuItemName, delegate { ShowAttributes(fl); });
                        menuItem.Image = Resources.table_16x16;
                        fl.ContextMenuItems.Insert(2, menuItem);
                    }
                }
            }
            //attach layer added events to existing groups
            foreach (IGroup grp in App.Map.MapFrame.GetAllGroups())
            {
                grp.LayerAdded += Map_LayerAdded;
            }
        }

        private void Map_LayerAdded(object sender, LayerEventArgs e)
        {
            if (e.Layer == null)
                return;

            AddContextMenuItems(e.Layer);
        }

        private void AddContextMenuItems(ILayer addedLayer)
        {
            IMapGroup grp = addedLayer as IMapGroup;
            if (grp != null)
            {
                // map.layerAdded event doesn't fire for groups. Therefore, it's necessary
                // to handle this event separately for groups.
                grp.LayerAdded += Map_LayerAdded;
            }

            if (addedLayer.ContextMenuItems.Exists(item => item.Name == contextMenuItemName))
            {
                // assume menu item already exists. Do nothing.
                return;
            }

            // add context menu item.
            var menuItem = new SymbologyMenuItem(contextMenuItemName, delegate { ShowAttributes(addedLayer as IFeatureLayer); });
            menuItem.Image = Resources.table_16x16;
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

        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();

            // detach events
            DetachLayerAddedEvents();
            App.SerializationManager.Deserializing -= SerializationManager_Deserializing;
            // remove context menu items.
            RemoveContextMenuItems();
            base.Deactivate();
        }

        private void DetachLayerAddedEvents()
        {
            App.Map.LayerAdded -= Map_LayerAdded;
            foreach (IGroup grp in App.Map.MapFrame.GetAllGroups())
            {
                grp.LayerAdded -= Map_LayerAdded;
            }
        }

        private void RemoveContextMenuItems()
        {
            foreach (ILayer lay in App.Map.MapFrame.GetAllLayers())
            {
                if (lay.ContextMenuItems.Exists(item => item.Name == contextMenuItemName))
                {
                    lay.ContextMenuItems.Remove(lay.ContextMenuItems.First(item => item.Name == contextMenuItemName));
                    return;
                }
            }
        }

        private static void ShowAttributes(IFeatureLayer layer)
        {
            if (layer != null)
                layer.ShowAttributes();
        }

        /// <summary>
        /// Open attribute table
        /// </summary>
        private void AttributeTable_Click(object sender, EventArgs e)
        {
            IMapFrame mainMapFrame = App.Map.MapFrame;
            List<ILayer> layers = mainMapFrame.GetAllLayers();

            foreach (ILayer layer in layers.Where(l => l.IsSelected))
            {
                IFeatureLayer fl = layer as IFeatureLayer;

                if (fl == null)
                    continue;
                ShowAttributes(fl);
            }
        }
    }
}