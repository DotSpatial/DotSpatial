using System;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using DotSpatial.Controls.Header;
using DotSpatial.Data;
using DotSpatial.Topology;
using Msg = DotSpatial.Controls.MessageStrings;

namespace DotSpatial.Controls
{
    public class DefaultMenuBars
    {
        #region Fields

        private const string FileMenuKey = HeaderControl.ApplicationMenuKey;
        private const string HomeMenuKey = HeaderControl.HomeRootItemKey;

        private SimpleActionItem _ZoomNext;
        private SimpleActionItem _ZoomPrevious;
        private SimpleActionItem _ZoomToLayer;

        private AppManager App { get; set; }

        #endregion

        public DefaultMenuBars(AppManager app)
        {
            if (app == null) throw new ArgumentNullException("app");
            App = app;

            App.MapChanged += (sender, args) => OnAppMapChanged(args);
        }

        public void Initialize(IHeaderControl header)
        {
            if (header == null) throw new ArgumentNullException("header");

            AddItems(header);
            OnAppMapChanged(new MapChangedEventArgs(null, App.Map));
        }

        private void OnAppMapChanged(MapChangedEventArgs args)
        {
            if (args.OldValue != null)
            {
                // Unsubscribe events from old map
                args.OldValue.Layers.LayerSelected -= Layers_LayerSelected;
                if (args.OldValue is Map)
                    ((Map)args.OldValue).ViewExtentsChanged -= MapFrame_ViewExtentsChanged;
            }

            if (args.NewValue != null)
            {
                args.NewValue.Layers.LayerSelected += Layers_LayerSelected;
                if (args.NewValue is Map)
                    ((Map)args.NewValue).ViewExtentsChanged += MapFrame_ViewExtentsChanged;
            }
        }

        private void AddItems(IHeaderControl header)
        {
            // Root items
            header.Add(new RootItem(FileMenuKey, MessageStrings.File) { SortOrder = -20 });
            header.Add(new RootItem(HomeMenuKey, MessageStrings.Home) { SortOrder = -10 });

            // Menu items
            header.Add(new SimpleActionItem(FileMenuKey, Msg.File_New, NewProject_Click) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 5, SmallImage = Images.document_empty_16x16, LargeImage = Images.document_empty_32x32, ToolTipText = Msg.FileNewToolTip });
            header.Add(new SimpleActionItem(FileMenuKey, Msg.File_Open, OpenProject_Click) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 10, SmallImage = Images.folder_16x16, LargeImage = Images.folder_32x32, ToolTipText = Msg.FileOpenToolTip });
            header.Add(new SimpleActionItem(FileMenuKey, Msg.File_Save, SaveProject_Click) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 15, SmallImage = Images.disk_16x16, LargeImage = Images.disk_32x32, });
            header.Add(new SimpleActionItem(FileMenuKey, Msg.File_SaveAs, SaveProjectAs_Click) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 20, SmallImage = Images.save_as_16x16, LargeImage = Images.save_as_32x32, ToolTipText = Msg.FileSaveAsToolTip });

            header.Add(new SimpleActionItem(FileMenuKey, Msg.File_Options, Options_Click) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 25 });
            header.Add(new SimpleActionItem(FileMenuKey, Msg.File_Print, PrintLayout_Click) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 40, SmallImage = Images.printer_16x16, LargeImage = Images.printer_32x32 });

            header.Add(new SimpleActionItem(FileMenuKey, Msg.File_Reset_Layout, ResetLayout_Click) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 200, SmallImage = Images.layout_delete_16x16, LargeImage = Images.layout_delete_32x32 });

            header.Add(new SimpleActionItem(FileMenuKey, Msg.File_Exit, Exit_Click) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 5000, });

            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Add_Layer, AddLayer_Click) { GroupCaption = Msg.Layers_Group, SmallImage = Images.layer_add_16x16, LargeImage = Images.layer_add_32x32 });
            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Remove_Layer, RemoveLayer_Click) { GroupCaption = Msg.Layers_Group, SmallImage = Images.layer_remove_16x16, LargeImage = Images.layer_remove_32x32 });


            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Pan, PanTool_Click) { Key = Msg.Pan, GroupCaption = Msg.View_Group, SmallImage = Images.hand_16x16, LargeImage = Images.hand_32x32, ToggleGroupKey = Msg.Map_Tools_Group });

            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Zoom_In, ZoomIn_Click) { Key = Msg.Zoom_In, GroupCaption = Msg.Zoom_Group, ToolTipText = Msg.Zoom_In_Tooltip, SmallImage = Images.zoom_in_16x16, LargeImage = Images.zoom_in_32x32, ToggleGroupKey = Msg.Map_Tools_Group });
            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Zoom_Out, ZoomOut_Click) { Key = Msg.Zoom_Out, GroupCaption = Msg.Zoom_Group, ToolTipText = Msg.Zoom_Out_Tooltip, SmallImage = Images.zoom_out_16x16, LargeImage = Images.zoom_out_32x32, ToggleGroupKey = Msg.Map_Tools_Group });
            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Zoom_To_Extents, ZoomToMaxExtents_Click) { GroupCaption = Msg.Zoom_Group, ToolTipText = Msg.Zoom_To_Extents_Tooltip, SmallImage = Images.zoom_extend_16x16, LargeImage = Images.zoom_extend_32x32 });
            _ZoomPrevious = new SimpleActionItem(HomeMenuKey, Msg.Zoom_Previous, ZoomPrevious_Click) { GroupCaption = Msg.Zoom_Group, ToolTipText = Msg.Zoom_Previous_Tooltip, SmallImage = Images.zoom_to_previous_16, LargeImage = Images.zoom_to_previous, Enabled = false };
            header.Add(_ZoomPrevious);
            _ZoomNext = new SimpleActionItem(HomeMenuKey, Msg.Zoom_Next, ZoomNext_Click) { GroupCaption = Msg.Zoom_Group, ToolTipText = Msg.Zoom_Next_Tooltip, SmallImage = Images.zoom_to_next_16, LargeImage = Images.zoom_to_next, Enabled = false };
            header.Add(_ZoomNext);
            _ZoomToLayer = new SimpleActionItem(HomeMenuKey, Msg.Zoom_To_Layer, ZoomToLayer_Click) { GroupCaption = Msg.Zoom_Group, SmallImage = Images.zoom_layer_16x16, LargeImage = Images.zoom_layer_32x32 };
            header.Add(_ZoomToLayer);

            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Zoom_To_Coordinates, Coordinates_Click) { GroupCaption = Msg.Zoom_Group, SmallImage = Images.zoom_coordinate_16x16, LargeImage = Images.zoom_coordinate_32x32 });

            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Select, SelectionTool_Click) { Key = Msg.Select, GroupCaption = Msg.Map_Tools_Group, SmallImage = Images.select_16x16, LargeImage = Images.select_32x32, ToggleGroupKey = Msg.Map_Tools_Group });

            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Deselect, DeselectAll_Click) { Key = Msg.Deselect, GroupCaption = Msg.Map_Tools_Group, SmallImage = Images.deselect_16x16, LargeImage = Images.deselect_32x32 });

            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Identify, IdentifierTool_Click) { GroupCaption = Msg.Map_Tools_Group, SmallImage = Images.info_rhombus_16x16, LargeImage = Images.info_rhombus_32x32, ToggleGroupKey = Msg.Map_Tools_Group });

        }

        private static void ShowSaveAsError(string fileName)
        {
            MessageBox.Show(String.Format(Msg.FailedToWriteTheSpecifiedMapFile, fileName), Msg.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Add Data to the Map
        /// </summary>
        private void AddLayer_Click(object sender, EventArgs e)
        {
            App.Map.AddLayers();
        }
        

        void Layers_LayerSelected(object sender, Symbology.LayerSelectedEventArgs e)
        {
            _ZoomToLayer.Enabled = App.Map.Layers.SelectedLayer != null;
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
            var mapFrame = (MapFrame)sender;
            _ZoomNext.Enabled = mapFrame.CanZoomToNext();
            _ZoomPrevious.Enabled = mapFrame.CanZoomToPrevious();
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
                if (MessageBox.Show(Msg.ClearAllDataAndStartANewMap, Msg.DiscardChanges, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    App.SerializationManager.New();
                }
            }
            else
            {
                //the current project is specified - ask the users if they want to save changes to current project
                string saveProjectMessage = String.Format(Msg.SaveChangesToCurrentProject, Path.GetFileName(App.SerializationManager.CurrentProjectFile));
                DialogResult msgBoxResult = MessageBox.Show(saveProjectMessage, Msg.DiscardChanges, MessageBoxButtons.YesNoCancel);

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
                    MessageBox.Show(String.Format(Msg.CouldNotOpenTheSpecifiedMapFile, dlg.FileName), Msg.Error,
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (XmlException)
                {
                    MessageBox.Show(String.Format(Msg.FailedToReadTheSpecifiedMapFile, dlg.FileName), Msg.Error,
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (ArgumentException)
                {
                    MessageBox.Show(String.Format(Msg.FailedToReadAPortionOfTheSpecifiedMapFile, dlg.FileName), Msg.Error,
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Shows the options dialog.
        /// </summary>
        private void Options_Click(object sender, EventArgs e)
        {
            using (OptionsForm form = new OptionsForm(App.Map))
            { form.ShowDialog(); }
        }

        /// <summary>
        /// Set the function mode to pan so user can move it with a mouse. 
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
            // In Mono show the dialog only if printers installed else show error message.
            if (Mono.Mono.IsRunningOnMono())
            {
                if (!new PrinterSettings().IsValid)
                {
                    MessageBox.Show("No printers installed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            using (var layout = new LayoutForm())
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
            try
            {
                IMapGroup selectedLayerParent = (IMapGroup)App.Map.Layers.SelectedLayer.GetParentItem();
                selectedLayerParent.Remove(App.Map.Layers.SelectedLayer);
            }
            catch { }
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
            String fullPath = App.SerializationManager.CurrentProjectFile;
            String file = Path.GetFileNameWithoutExtension(fullPath);

            //Hardcoded names of sample projects into Save button so that if user tries to save over project file they will have to pick a new filename.
            if (String.IsNullOrEmpty(fullPath)
                || file.Contains("SampleProjects")
                || file.ToLower().Contains("elbe")
                || file.Contains("Jacob's Well Spring")
                || file.Contains("World Map")
                || file.Contains("North America Map")
                )
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
        /// Deselect all features in all layers
        /// </summary>
        private void DeselectAll_Click(object sender, EventArgs e)
        {
            IEnvelope env;
            App.Map.MapFrame.ClearSelection(out env);
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
            App.Map.MapFrame.ZoomToNext();
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
            App.Map.MapFrame.ZoomToPrevious();
        }

        /// <summary>
        /// Zoom to the currently selected layer
        /// </summary>
        private void ZoomToLayer_Click(object sender, EventArgs e)
        {
            var layer = App.Map.Layers.SelectedLayer;
            if (layer != null)
            {
                ZoomToLayer(layer);
            }
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

        private void ZoomToLayer(IMapLayer layerToZoom)
        {
            const double eps = 1e-7;
            IEnvelope layerEnvelope = layerToZoom.Extent.ToEnvelope();
            if (layerEnvelope.Width > eps && layerEnvelope.Height > eps)
            {
                layerEnvelope.ExpandBy(layerEnvelope.Width / 10, layerEnvelope.Height / 10); // work item #84
            }
            else
            {
                double zoomInFactor = 0.05; //fixed zoom-in by 10% - 5% on each side
                double newExtentWidth = App.Map.ViewExtents.Width * zoomInFactor;
                double newExtentHeight = App.Map.ViewExtents.Height * zoomInFactor;
                layerEnvelope.ExpandBy(newExtentWidth, newExtentHeight);
            }

            App.Map.ViewExtents = layerEnvelope.ToExtent();
        }

        private void Coordinates_Click(object sender, EventArgs e)
        {
            ZoomToCoordinates();
        }

        private void ZoomToCoordinates()
        {
            using (var CoordinateDialog = new ZoomToCoordinatesDialog(App.Map))
                CoordinateDialog.ShowDialog();
        }

    }
}