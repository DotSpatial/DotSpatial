// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuBarPlugin.cs" company="">
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Data;
using DotSpatial.Plugins.MenuBar.Properties;
using Msg = DotSpatial.Plugins.MenuBar.MessageStrings;

namespace DotSpatial.Plugins.MenuBar
{
    public class MenuBarPlugin : Extension
    {
        #region Constants and Fields

        private const string FileMenuKey = HeaderControl.ApplicationMenuKey;
        private const string HomeMenuKey = HeaderControl.HomeRootItemKey;

        private readonly List<Extent> _previousExtents = new List<Extent>();

        private SimpleActionItem _ZoomNext;
        private SimpleActionItem _ZoomPrevious;
        private int _currentExtentId;
        private bool _manualExtentsChange;

        #endregion

        #region Public Methods

        public override void Activate()
        {
            AddHeaderRootItems();
            AddMenuItems();

            App.Map.MapFrame.ViewExtentsChanged += MapFrame_ViewExtentsChanged;
            base.Activate();
        }

        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        #endregion

        #region Methods

        protected void AddHeaderRootItems()
        {
            App.HeaderControl.Add(new RootItem(FileMenuKey, MessageStrings.File) { SortOrder = -20 });
            App.HeaderControl.Add(new RootItem(HomeMenuKey, MessageStrings.Home) { SortOrder = -10 });
        }

        private static void ShowSaveAsError(string fileName)
        {
            MessageBox.Show(String.Format(Resources.FailedToWriteTheSpecifiedMapFile, fileName), Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Add Data to the Map
        /// </summary>
        private void AddLayer_Click(object sender, EventArgs e)
        {
            App.Map.AddLayers();
        }

        private void AddMenuItems()
        {
            IHeaderControl header = App.HeaderControl;
            header.Add(new SimpleActionItem(FileMenuKey, Msg.File_New, NewProject_Click) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 5, SmallImage = Resources.document_empty_16x16, LargeImage = Resources.document_empty_32x32 });
            header.Add(new SimpleActionItem(FileMenuKey, Msg.File_Open, OpenProject_Click) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 10, SmallImage = Resources.folder_16x16, LargeImage = Resources.folder_32x32 });
            header.Add(new SimpleActionItem(FileMenuKey, Msg.File_Save, SaveProject_Click) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 15, SmallImage = Resources.disk_16x16, LargeImage = Resources.disk_32x32, ShowInQuickAccessToolbar = Settings.Default.ShowSaveQuickAccessButton });
            header.Add(new SimpleActionItem(FileMenuKey, Msg.File_SaveAs, SaveProjectAs_Click) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 20, SmallImage = Resources.save_as_16x16, LargeImage = Resources.save_as_32x32 });

            header.Add(new SimpleActionItem(FileMenuKey, Msg.File_Print, PrintLayout_Click) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 40, SmallImage = Resources.printer_16x16, LargeImage = Resources.printer_32x32 });

            header.Add(new SimpleActionItem(FileMenuKey, Msg.File_Reset_Layout, ResetLayout_Click) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 200, SmallImage = Resources.layout_delete_16x16, LargeImage = Resources.layout_delete_32x32 });

            header.Add(new SimpleActionItem(FileMenuKey, Msg.File_Exit, Exit_Click) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 5000, });

            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Add_Layer, AddLayer_Click) { GroupCaption = Msg.Layers_Group, SmallImage = Resources.layer_add_16x16, LargeImage = Resources.layer_add_32x32 });
            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Remove_Layer, RemoveLayer_Click) { GroupCaption = Msg.Layers_Group, SmallImage = Resources.layer_remove_16x16, LargeImage = Resources.layer_remove_32x32 });
            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Save_Layer, null) { GroupCaption = Msg.Layers_Group, SmallImage = Resources.layer_save_16x16, LargeImage = Resources.layer_save_32x32, Enabled = false });

            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Pan, PanTool_Click) { GroupCaption = Msg.View_Group, SmallImage = Resources.hand_16x16, LargeImage = Resources.hand_32x32, ToggleGroupKey = Msg.Map_Tools_Group });

            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Zoom_In, ZoomIn_Click) { GroupCaption = Msg.Zoom_Group, ToolTipText = Msg.Zoom_In_Tooltip, SmallImage = Resources.zoom_in_16x16, LargeImage = Resources.zoom_in_32x32, ToggleGroupKey = Msg.Map_Tools_Group });
            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Zoom_Out, ZoomOut_Click) { GroupCaption = Msg.Zoom_Group, ToolTipText = Msg.Zoom_Out_Tooltip, SmallImage = Resources.zoom_out_16x16, LargeImage = Resources.zoom_out_32x32, ToggleGroupKey = Msg.Map_Tools_Group });
            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Zoom_To_Extents, ZoomToMaxExtents_Click) { GroupCaption = Msg.Zoom_Group, ToolTipText = Msg.Zoom_To_Extents_Tooltip, SmallImage = Resources.zoom_extend_16x16, LargeImage = Resources.zoom_extend_32x32 });
            _ZoomPrevious = new SimpleActionItem(HomeMenuKey, Msg.Zoom_Previous, ZoomPrevious_Click) { GroupCaption = Msg.Zoom_Group, ToolTipText = Msg.Zoom_Previous_Tooltip, SmallImage = Resources.zoom_to_previous_16, LargeImage = Resources.zoom_to_previous, Enabled = false };
            header.Add(_ZoomPrevious);
            _ZoomNext = new SimpleActionItem(HomeMenuKey, Msg.Zoom_Next, ZoomNext_Click) { GroupCaption = Msg.Zoom_Group, ToolTipText = Msg.Zoom_Next_Tooltip, SmallImage = Resources.zoom_to_next_16, LargeImage = Resources.zoom_to_next, Enabled = false };
            header.Add(_ZoomNext);
            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Zoom_To_Layer, ZoomToLayer_Click) { GroupCaption = Msg.Zoom_Group, SmallImage = Resources.zoom_layer_16x16, LargeImage = Resources.zoom_layer_32x32 });

            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Select, SelectionTool_Click) { GroupCaption = Msg.Map_Tools_Group, SmallImage = Resources.select_16x16, LargeImage = Resources.select_32x32, ToggleGroupKey = Msg.Map_Tools_Group });

            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Identify, IdentifierTool_Click) { GroupCaption = Msg.Map_Tools_Group, SmallImage = Resources.info_rhombus_16x16, LargeImage = Resources.info_rhombus_32x32, ToggleGroupKey = Msg.Map_Tools_Group });
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Identifier Tool
        /// </summary>
        private void IdentifierTool_Click(object sender, EventArgs e)
        {
            App.Map.FunctionMode = FunctionMode.Info;
        }

        private void MapFrame_ViewExtentsChanged(object sender, ExtentArgs e)
        {
            IMapFrame mapFrame = sender as IMapFrame;

            if (mapFrame == null)
                return;
            if (_previousExtents == null)
                return;
            if (mapFrame.Layers.Count == 0)
                return;

            if (_manualExtentsChange)
            {
                _manualExtentsChange = false; // reset the flag for the next extents change
            }
            else
            {
                // we're not flusing the forward history.
                _previousExtents.Add(mapFrame.ViewExtents);
                _currentExtentId = _previousExtents.Count - 1;
            }

            _ZoomNext.Enabled = _currentExtentId < (_previousExtents.Count - 1);
            _ZoomPrevious.Enabled = (_previousExtents.Count > 0) && (_currentExtentId > 0);
        }

        private void NewProject_Click(object sender, EventArgs e)
        {
            //if the map is empty or if the current project is already saved, start a new project directly
            if (!App.SerializationManager.IsDirty || App.Map.Layers == null || App.Map.Layers.Count == 0)
            {
                App.SerializationManager.New();
            }
            else if (String.IsNullOrEmpty(App.SerializationManager.CurrentProjectFile))
            {
                //if the current project is not specified - just ask to discard changes
                if (MessageBox.Show(Resources.ClearAllDataAndStartANewMap, Resources.DiscardChanges, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    App.SerializationManager.New();
                }
            }
            else
            {
                //the current project is specified - ask the users if they want to save changes to current project
                string saveProjectMessage = String.Format(Resources.SaveChangesToCurrentProject, Path.GetFileName(App.SerializationManager.CurrentProjectFile));
                DialogResult msgBoxResult = MessageBox.Show(saveProjectMessage, Resources.DiscardChanges, MessageBoxButtons.YesNoCancel);

                if (msgBoxResult == DialogResult.Yes)
                {
                    App.SerializationManager.SaveProject(App.SerializationManager.CurrentProjectFile);
                }
                if (msgBoxResult != DialogResult.Cancel)
                {
                    App.SerializationManager.New();
                }
            }
        }

        private void OpenProject_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = App.SerializationManager.OpenDialogFilterText;
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                try
                {
                    //use the AppManager.SerializationManager to open the project
                    App.SerializationManager.OpenProject(dlg.FileName);
                    App.Map.Invalidate();
                }
                catch (IOException)
                {
                    MessageBox.Show(String.Format(Resources.CouldNotOpenTheSpecifiedMapFile, dlg.FileName), Resources.Error,
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (XmlException)
                {
                    MessageBox.Show(String.Format(Resources.FailedToReadTheSpecifiedMapFile, dlg.FileName), Resources.Error,
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (ArgumentException)
                {
                    MessageBox.Show(String.Format(Resources.FailedToReadAPortionOfTheSpecifiedMapFile, dlg.FileName), Resources.Error,
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Move (Pan) the map
        /// </summary>
        private void PanTool_Click(object sender, EventArgs e)
        {
            App.Map.FunctionMode = FunctionMode.Pan;
        }

        /// <summary>
        /// Handles the Click event of the PrintLayout control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void PrintLayout_Click(object sender, EventArgs e)
        {
            using (LayoutForm layout = new LayoutForm())
            {
                layout.MapControl = App.Map as Map;
                layout.ShowDialog();
            }
        }

        /// <summary>
        /// Remove currently selected layer from the Map
        /// </summary>
        private void RemoveLayer_Click(object sender, EventArgs e)
        {
            App.Map.Layers.Remove(App.Map.Layers.SelectedLayer);
        }

        private void ResetLayout_Click(object sender, EventArgs e)
        {
            App.DockManager.ResetLayout();
        }

        private void SaveProject(string fileName)
        {
            try
            {
                //use the AppManager.SerializationManager to save the project
                App.SerializationManager.SaveProject(fileName);
            }
            catch (XmlException)
            {
                ShowSaveAsError(fileName);
            }
            catch (IOException)
            {
                ShowSaveAsError(fileName);
            }
        }

        private void SaveProjectAs()
        {
            using (var dlg = new SaveFileDialog { Filter = App.SerializationManager.SaveDialogFilterText, SupportMultiDottedExtensions = true })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    SaveProject(dlg.FileName);
                }
            }
        }

        private void SaveProjectAs_Click(object sender, EventArgs e)
        {
            SaveProjectAs();
        }

        private void SaveProject_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(App.SerializationManager.CurrentProjectFile))
            {
                SaveProjectAs();
            }
            else
            {
                SaveProject(App.SerializationManager.CurrentProjectFile);
            }
        }

        /// <summary>
        /// Select or deselect Features
        /// </summary>
        private void SelectionTool_Click(object sender, EventArgs e)
        {
            App.Map.FunctionMode = FunctionMode.Select;
        }

        /// <summary>
        /// Zoom In
        /// </summary>
        private void ZoomIn_Click(object sender, EventArgs e)
        {
            App.Map.FunctionMode = FunctionMode.ZoomIn;
        }

        /// <summary>
        /// Zoom to previous extent
        /// </summary>
        private void ZoomNext_Click(object sender, EventArgs e)
        {
            if (_currentExtentId < _previousExtents.Count - 1)
            {
                _currentExtentId += 1;
                _manualExtentsChange = true;
                App.Map.ViewExtents = _previousExtents[_currentExtentId];
            }
            else
            {
                _ZoomNext.Enabled = false;
            }
        }

        /// <summary>
        /// Zoom Out
        /// </summary>
        private void ZoomOut_Click(object sender, EventArgs e)
        {
            App.Map.FunctionMode = FunctionMode.ZoomOut;
        }

        /// <summary>
        /// Zoom to previous extent
        /// </summary>
        private void ZoomPrevious_Click(object sender, EventArgs e)
        {
            if ((_previousExtents.Count > 0) && (_currentExtentId > 0))
            {
                _manualExtentsChange = true;
                _currentExtentId -= 1;
                App.Map.ViewExtents = _previousExtents[_currentExtentId];
            }
            else
            {
                _ZoomPrevious.Enabled = false;
            }
        }

        /// <summary>
        /// Zoom to the currently selected layer
        /// </summary>
        private void ZoomToLayer_Click(object sender, EventArgs e)
        {
            var layer = App.Map.Layers.SelectedLayer;
            if (layer != null)
                App.Map.ViewExtents = layer.DataSet.Extent;
        }

        /// <summary>
        /// Zoom to maximum extents
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ZoomToMaxExtents_Click(object sender, EventArgs e)
        {
            App.Map.ZoomToMaxExtent();
        }

        #endregion
    }
}