// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using DotSpatial.Controls.Header;
using DotSpatial.Data;
using DotSpatial.Symbology;
using GeoAPI.Geometries;
using Msg = DotSpatial.Controls.MessageStrings;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This contains menu bars and tools that are always added to the header control.
    /// </summary>
    public class DefaultMenuBars
    {
        #region Fields

        private const string FileMenuKey = HeaderControl.ApplicationMenuKey;
        private const string HomeMenuKey = HeaderControl.HomeRootItemKey;

        private RootItem _fileMenuKey;
        private RootItem _homeMenuKey;
        private SimpleActionItem _file_New;
        private SimpleActionItem _file_Open;
        private SimpleActionItem _file_Save;
        private SimpleActionItem _file_SaveAs;
        private SimpleActionItem _file_Options;
        private SimpleActionItem _file_Print;
        private SimpleActionItem _file_Reset_Layout;
        private SimpleActionItem _file_Exit;

        private SimpleActionItem _add_Layer;
        private SimpleActionItem _remove_Layer;
        private SimpleActionItem _legend_Settings;

        private SimpleActionItem _pan;
        private SimpleActionItem _zoom_In;
        private SimpleActionItem _zoom_Out;
        private SimpleActionItem _zoom_To_Extents;

        private SimpleActionItem _zoomNext;
        private SimpleActionItem _zoomPrevious;
        private SimpleActionItem _zoomToLayer;
        private SimpleActionItem _zoom_To_Coordinates;

        private SimpleActionItem _select;
        private SimpleActionItem _deselect;
        private SimpleActionItem _identify;

        private HeaderControl _header;
        private CultureInfo _defaultMenuBarCulture;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMenuBars"/> class with the given AppManager.
        /// </summary>
        /// <param name="app">AppManager that contains the Map the tools should work on.</param>
        public DefaultMenuBars(AppManager app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            App = app;

            App.MapChanged += (sender, args) => OnAppMapChanged(args);
            App.AppCultureChanged += OnAppCultureChanged;
        }

        #endregion

        #region Properties

        /// <summary>
        /// sets a value indicating the culture to use for resources.
        /// </summary>
        public CultureInfo DefaultMenuBarCulture
        {
            set
            {
                if (_defaultMenuBarCulture == value) return;

                _defaultMenuBarCulture = value;

                if (_defaultMenuBarCulture == null) _defaultMenuBarCulture = new CultureInfo(string.Empty);

                Thread.CurrentThread.CurrentCulture = _defaultMenuBarCulture;
                Thread.CurrentThread.CurrentUICulture = _defaultMenuBarCulture;

                UpdateDefaultMenuItems();
            }
        }

        private AppManager App { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the default tools to the given header.
        /// </summary>
        /// <param name="header">IHeaderControl the default tools should be added to.</param>
        public void Initialize(IHeaderControl header)
        {
            if (header == null) throw new ArgumentNullException(nameof(header));

            _header = header as HeaderControl;
            AddItems(header);
            OnAppMapChanged(new MapChangedEventArgs(null, App.Map));

            DefaultMenuBarCulture = new CultureInfo(string.Empty);
        }

        private static void ShowSaveAsError(string fileName)
        {
            MessageBox.Show(string.Format(Msg.FailedToWriteTheSpecifiedMapFile, fileName), Msg.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void AddItems(IHeaderControl header)
        {
            // Root items
            _fileMenuKey = new RootItem(FileMenuKey, MessageStrings.File)
            {
                SortOrder = -20
            };
            header.Add(_fileMenuKey);

            _homeMenuKey = new RootItem(HomeMenuKey, MessageStrings.Home)
            {
                SortOrder = -10
            };
            header.Add(_homeMenuKey);

            // Menu items
            _file_New = new SimpleActionItem(FileMenuKey, Msg.File_New, NewProjectClick)
            {
                GroupCaption = HeaderControl.ApplicationMenuKey,
                SortOrder = 5,
                SmallImage = Images.document_empty_16x16,
                LargeImage = Images.document_empty_32x32,
                ToolTipText = Msg.FileNewToolTip
            };
            header.Add(_file_New);

            _file_Open = new SimpleActionItem(FileMenuKey, Msg.File_Open, OpenProjectClick)
            {
                GroupCaption = HeaderControl.ApplicationMenuKey,
                SortOrder = 10,
                SmallImage = Images.folder_16x16,
                LargeImage = Images.folder_32x32,
                ToolTipText = Msg.FileOpenToolTip
            };
            header.Add(_file_Open);

            _file_Save = new SimpleActionItem(FileMenuKey, Msg.File_Save, SaveProjectClick)
            {
                GroupCaption = HeaderControl.ApplicationMenuKey,
                SortOrder = 15,
                SmallImage = Images.disk_16x16,
                LargeImage = Images.disk_32x32,
            };
            header.Add(_file_Save);

            _file_SaveAs = new SimpleActionItem(FileMenuKey, Msg.File_SaveAs, SaveProjectAsClick)
            {
                GroupCaption = HeaderControl.ApplicationMenuKey,
                SortOrder = 20,
                SmallImage = Images.save_as_16x16,
                LargeImage = Images.save_as_32x32,
                ToolTipText = Msg.FileSaveAsToolTip
            };
            header.Add(_file_SaveAs);

            _file_Options = new SimpleActionItem(FileMenuKey, Msg.File_Options, OptionsClick)
            {
                GroupCaption = HeaderControl.ApplicationMenuKey,
                SortOrder = 25
            };
            header.Add(_file_Options);

            _file_Print = new SimpleActionItem(FileMenuKey, Msg.File_Print, PrintLayoutClick)
            {
                GroupCaption = HeaderControl.ApplicationMenuKey,
                SortOrder = 40,
                SmallImage = Images.printer_16x16,
                LargeImage = Images.printer_32x32
            };
            header.Add(_file_Print);

            _file_Reset_Layout = new SimpleActionItem(FileMenuKey, Msg.File_Reset_Layout, ResetLayoutClick)
            {
                GroupCaption = HeaderControl.ApplicationMenuKey,
                SortOrder = 200,
                SmallImage = Images.layout_delete_16x16,
                LargeImage = Images.layout_delete_32x32
            };
            header.Add(_file_Reset_Layout);

            _file_Exit = new SimpleActionItem(FileMenuKey, Msg.File_Exit, ExitClick)
            {
                GroupCaption = HeaderControl.ApplicationMenuKey,
                SortOrder = 5000,
            };
            header.Add(_file_Exit);

            _add_Layer = new SimpleActionItem(HomeMenuKey, Msg.Add_Layer, AddLayerClick)
            {
                GroupCaption = Msg.Layers_Group,
                SmallImage = Images.layer_add_16x16,
                LargeImage = Images.layer_add_32x32
            };
            header.Add(_add_Layer);

            _remove_Layer = new SimpleActionItem(HomeMenuKey, Msg.Remove_Layer, RemoveLayerClick)
            {
                GroupCaption = Msg.Layers_Group,
                SmallImage = Images.layer_remove_16x16,
                LargeImage = Images.layer_remove_32x32
            };
            header.Add(_remove_Layer);

            _legend_Settings = new SimpleActionItem(HomeMenuKey, Msg.Legend_Settings, LegendSettingsClick)
            {
                GroupCaption = Msg.Layers_Group,
                SmallImage = Images.Legend,
                LargeImage = Images.Legend
            };
            header.Add(_legend_Settings);

            _pan = new SimpleActionItem(HomeMenuKey, Msg.Pan, PanToolClick)
            {
                Key = Msg.Pan,
                GroupCaption = Msg.View_Group,
                SmallImage = Images.hand_16x16,
                LargeImage = Images.hand_32x32,
                ToggleGroupKey = Msg.Map_Tools_Group
            };
            header.Add(_pan);

            _zoom_In = new SimpleActionItem(HomeMenuKey, Msg.Zoom_In, ZoomInClick)
            {
                Key = Msg.Zoom_In,
                GroupCaption = Msg.Zoom_Group,
                ToolTipText = Msg.Zoom_In_Tooltip,
                SmallImage = Images.zoom_in_16x16,
                LargeImage = Images.zoom_in_32x32,
                ToggleGroupKey = Msg.Map_Tools_Group
            };
            header.Add(_zoom_In);

            _zoom_Out = new SimpleActionItem(HomeMenuKey, Msg.Zoom_Out, ZoomOutClick)
            {
                Key = Msg.Zoom_Out,
                GroupCaption = Msg.Zoom_Group,
                ToolTipText = Msg.Zoom_Out_Tooltip,
                SmallImage = Images.zoom_out_16x16,
                LargeImage = Images.zoom_out_32x32,
                ToggleGroupKey = Msg.Map_Tools_Group
            };
            header.Add(_zoom_Out);

            _zoom_To_Extents = new SimpleActionItem(HomeMenuKey, Msg.Zoom_To_Extents, ZoomToMaxExtentsClick)
            {
                GroupCaption = Msg.Zoom_Group,
                ToolTipText = Msg.Zoom_To_Extents_Tooltip,
                SmallImage = Images.zoom_extend_16x16,
                LargeImage = Images.zoom_extend_32x32
            };
            header.Add(_zoom_To_Extents);

            _zoomPrevious = new SimpleActionItem(HomeMenuKey, Msg.Zoom_Previous, ZoomPreviousClick)
            {
                GroupCaption = Msg.Zoom_Group,
                ToolTipText = Msg.Zoom_Previous_Tooltip,
                SmallImage = Images.zoom_to_previous_16,
                LargeImage = Images.zoom_to_previous,
                Enabled = false
            };
            header.Add(_zoomPrevious);
            _zoomNext = new SimpleActionItem(HomeMenuKey, Msg.Zoom_Next, ZoomNextClick)
            {
                GroupCaption = Msg.Zoom_Group,
                ToolTipText = Msg.Zoom_Next_Tooltip,
                SmallImage = Images.zoom_to_next_16,
                LargeImage = Images.zoom_to_next,
                Enabled = false
            };
            header.Add(_zoomNext);
            _zoomToLayer = new SimpleActionItem(HomeMenuKey, Msg.Zoom_To_Layer, ZoomToLayerClick)
            {
                GroupCaption = Msg.Zoom_Group,
                SmallImage = Images.zoom_layer_16x16,
                LargeImage = Images.zoom_layer_32x32
            };
            header.Add(_zoomToLayer);

            _zoom_To_Coordinates = new SimpleActionItem(HomeMenuKey, Msg.Zoom_To_Coordinates, CoordinatesClick)
            {
                GroupCaption = Msg.Zoom_Group,
                SmallImage = Images.zoom_coordinate_16x16,
                LargeImage = Images.zoom_coordinate_32x32
            };
            header.Add(_zoom_To_Coordinates);

            _select = new SimpleActionItem(HomeMenuKey, Msg.Select, SelectionToolClick)
            {
                Key = Msg.Select,
                GroupCaption = Msg.Map_Tools_Group,
                SmallImage = Images.select_16x16,
                LargeImage = Images.select_32x32,
                ToggleGroupKey = Msg.Map_Tools_Group
            };
            header.Add(_select);

            _deselect = new SimpleActionItem(HomeMenuKey, Msg.Deselect, DeselectAllClick)
            {
                Key = Msg.Deselect,
                GroupCaption = Msg.Map_Tools_Group,
                SmallImage = Images.deselect_16x16,
                LargeImage = Images.deselect_32x32
            };
            header.Add(_deselect);

            _identify = new SimpleActionItem(HomeMenuKey, Msg.Identify, IdentifierToolClick)
            {
                GroupCaption = Msg.Map_Tools_Group,
                SmallImage = Images.info_rhombus_16x16,
                LargeImage = Images.info_rhombus_32x32,
                ToggleGroupKey = Msg.Map_Tools_Group
            };
            header.Add(_identify);
        }

        /// <summary>
        /// Add Data to the Map
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void AddLayerClick(object sender, EventArgs e)
        {
            App.Map.AddLayers();
        }

        private void CoordinatesClick(object sender, EventArgs e)
        {
            ZoomToCoordinates();
        }

        /// <summary>
        /// Deselect all features in all layers
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void DeselectAllClick(object sender, EventArgs e)
        {
            App.Map.MapFrame.ClearSelection();
        }

        private void ExitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Identifier Tool
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void IdentifierToolClick(object sender, EventArgs e)
        {
            App.Map.FunctionMode = FunctionMode.Info;
        }

        /// <summary>
        /// De-/activates the zoom to layer button depending on a layer being selected.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void LayersLayerSelected(object sender, LayerSelectedEventArgs e)
        {
            _zoomToLayer.Enabled = App.Map.Layers.SelectedLayer != null;
        }

        private void MapFrameViewExtentsChanged(object sender, ExtentArgs e)
        {
            var mapFrame = (MapFrame)sender;
            _zoomNext.Enabled = mapFrame.CanZoomToNext();
            _zoomPrevious.Enabled = mapFrame.CanZoomToPrevious();
        }

        private void NewProjectClick(object sender, EventArgs e)
        {
            // if the map is empty or if the current project is already saved, start a new project directly
            if (!App.SerializationManager.IsDirty || App.Map.Layers == null || App.Map.Layers.Count == 0)
            {
                App.SerializationManager.New();
            }
            else if (string.IsNullOrEmpty(App.SerializationManager.CurrentProjectFile))
            {
                // if the current project is not specified - just ask to discard changes
                if (MessageBox.Show(Msg.ClearAllDataAndStartANewMap, Msg.DiscardChanges, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    App.SerializationManager.New();
                }
            }
            else
            {
                // the current project is specified - ask the users if they want to save changes to current project
                string saveProjectMessage = string.Format(Msg.SaveChangesToCurrentProject, Path.GetFileName(App.SerializationManager.CurrentProjectFile));
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

        private void OnAppMapChanged(MapChangedEventArgs args)
        {
            if (args.OldValue != null)
            {
                // Unsubscribe events from old map
                args.OldValue.Layers.LayerSelected -= LayersLayerSelected;
                var map = args.OldValue as Map;
                if (map != null)
                    map.ViewExtentsChanged -= MapFrameViewExtentsChanged;
            }

            if (args.NewValue != null)
            {
                args.NewValue.Layers.LayerSelected += LayersLayerSelected;
                var map = args.NewValue as Map;
                if (map != null)
                    map.ViewExtentsChanged += MapFrameViewExtentsChanged;
            }
        }

        private void OpenProjectClick(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = App.SerializationManager.OpenDialogFilterText;
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                try
                {
                    // use the AppManager.SerializationManager to open the project
                    App.SerializationManager.OpenProject(dlg.FileName);
                    App.Map.Invalidate();
                }
                catch (IOException)
                {
                    MessageBox.Show(string.Format(Msg.CouldNotOpenTheSpecifiedMapFile, dlg.FileName), Msg.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (XmlException)
                {
                    MessageBox.Show(string.Format(Msg.FailedToReadTheSpecifiedMapFile, dlg.FileName), Msg.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (ArgumentException)
                {
                    MessageBox.Show(string.Format(Msg.FailedToReadAPortionOfTheSpecifiedMapFile, dlg.FileName), Msg.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Shows the options dialog.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void OptionsClick(object sender, EventArgs e)
        {
            using (var form = new OptionsForm(App))
            {
                form.ShowDialog();
            }
        }

        /// <summary>
        /// Set the function mode to pan so user can move it with a mouse.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void PanToolClick(object sender, EventArgs e)
        {
            App.Map.FunctionMode = FunctionMode.Pan;
        }

        /// <summary>
        /// Handles the Click event of the PrintLayout control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void PrintLayoutClick(object sender, EventArgs e)
        {
            // In Mono show the dialog only if printers installed else show error message.
            if (Mono.IsRunningOnMono())
            {
                if (!new PrinterSettings().IsValid)
                {
                    MessageBox.Show(Msg.NoPrintersInstalled, Msg.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void RemoveLayerClick(object sender, EventArgs e)
        {
            try
            {
                IMapGroup selectedLayerParent = (IMapGroup)App.Map.Layers.SelectedLayer.GetParentItem();
                selectedLayerParent.Remove(App.Map.Layers.SelectedLayer);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Settings currently used Legend control from the Map
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void LegendSettingsClick(object sender, EventArgs e)
        {
            var legendSettings = new LegendSettingsDialog(App.Map);
            legendSettings.ShowDialog();
        }

        private void ResetLayoutClick(object sender, EventArgs e)
        {
            App.DockManager.ResetLayout();
        }

        private void SaveProject(string fileName)
        {
            try
            {
                // use the AppManager.SerializationManager to save the project
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

        private void SaveProjectClick(object sender, EventArgs e)
        {
            string fullPath = App.SerializationManager.CurrentProjectFile;
            string file = Path.GetFileNameWithoutExtension(fullPath);

            // Hardcoded names of sample projects into Save button so that if user tries to save over project file they will have to pick a new filename.
            if (string.IsNullOrEmpty(fullPath) || string.IsNullOrEmpty(file) || file.Contains("SampleProjects") || file.ToLower().Contains("elbe") || file.Contains("Jacob's Well Spring") || file.Contains("World Map") || file.Contains("North America Map"))
            {
                SaveProjectAs();
            }
            else
            {
                SaveProject(App.SerializationManager.CurrentProjectFile);
            }
        }

        private void SaveProjectAs()
        {
            using (var dlg = new SaveFileDialog
            {
                Filter = App.SerializationManager.SaveDialogFilterText,
                SupportMultiDottedExtensions = true
            })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    SaveProject(dlg.FileName);
                }
            }
        }

        private void SaveProjectAsClick(object sender, EventArgs e)
        {
            SaveProjectAs();
        }

        /// <summary>
        /// Select or deselect Features
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void SelectionToolClick(object sender, EventArgs e)
        {
            App.Map.FunctionMode = FunctionMode.Select;
        }

        /// <summary>
        /// Zoom In
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void ZoomInClick(object sender, EventArgs e)
        {
            App.Map.FunctionMode = FunctionMode.ZoomIn;
        }

        /// <summary>
        /// Zoom to previous extent
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void ZoomNextClick(object sender, EventArgs e)
        {
            App.Map.MapFrame.ZoomToNext();
        }

        /// <summary>
        /// Zoom Out
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void ZoomOutClick(object sender, EventArgs e)
        {
            App.Map.FunctionMode = FunctionMode.ZoomOut;
        }

        /// <summary>
        /// Zoom to previous extent
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void ZoomPreviousClick(object sender, EventArgs e)
        {
            App.Map.MapFrame.ZoomToPrevious();
        }

        private void ZoomToCoordinates()
        {
            using (var coordinateDialog = new ZoomToCoordinatesDialog(App.Map))
                coordinateDialog.ShowDialog();
        }

        private void ZoomToLayer(IMapLayer layerToZoom)
        {
            const double Eps = 1e-7;
            Envelope layerEnvelope = layerToZoom.Extent.ToEnvelope();

            if (App.Map.MapFrame.ExtendBuffer)
            {
                layerEnvelope.ExpandBy(layerEnvelope.Width, layerEnvelope.Height);
            }

            if (layerEnvelope.Width > Eps && layerEnvelope.Height > Eps)
            {
                layerEnvelope.ExpandBy(layerEnvelope.Width / 10, layerEnvelope.Height / 10); // work item #84
            }
            else
            {
                double zoomInFactor = 0.05; // fixed zoom-in by 10% - 5% on each side
                double newExtentWidth = App.Map.ViewExtents.Width * zoomInFactor;
                double newExtentHeight = App.Map.ViewExtents.Height * zoomInFactor;
                layerEnvelope.ExpandBy(newExtentWidth, newExtentHeight);
            }

            App.Map.ViewExtents = layerEnvelope.ToExtent();
        }

        /// <summary>
        /// Zoom to the currently selected layer
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void ZoomToLayerClick(object sender, EventArgs e)
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
        private void ZoomToMaxExtentsClick(object sender, EventArgs e)
        {
            App.Map.ZoomToMaxExtent();
        }

        private void OnAppCultureChanged(object sender, CultureInfo appCulture)
        {
            DefaultMenuBarCulture = appCulture;
        }

        private void UpdateDefaultMenuItems()
        {
            _fileMenuKey.Caption = MessageStrings.File;
            _homeMenuKey.Caption = MessageStrings.Home;

            _file_New.Caption = MessageStrings.File_New;
            _file_Open.Caption = MessageStrings.File_Open;
            _file_Save.Caption = MessageStrings.File_Save;
            _file_SaveAs.Caption = MessageStrings.File_SaveAs;
            _file_Options.Caption = MessageStrings.File_Options;
            _file_Print.Caption = MessageStrings.File_Print;
            _file_Reset_Layout.Caption = MessageStrings.File_Reset_Layout;
            _file_Exit.Caption = MessageStrings.File_Exit;

            _add_Layer.Caption = MessageStrings.Add_Layer;
            _remove_Layer.Caption = MessageStrings.Remove_Layer;

            _pan.Caption = MessageStrings.Pan;
            _zoom_In.Caption = MessageStrings.Zoom_In;
            _zoom_Out.Caption = MessageStrings.Zoom_Out;
            _zoom_To_Extents.Caption = MessageStrings.Zoom_To_Extents;

            _zoomNext.Caption = MessageStrings.Zoom_Next;
            _zoomPrevious.Caption = MessageStrings.Zoom_Previous;
            /*_zoomToLayer.Caption = MessageStrings.la;*/
            _zoom_To_Coordinates.Caption = MessageStrings.Zoom_To_Coordinates;

            _select.Caption = MessageStrings.Select;
            _deselect.Caption = MessageStrings.Deselect;
            _identify.Caption = MessageStrings.Identify;

            // TooltipText
            _add_Layer.ToolTipText = MessageStrings.Add_Layer;
            _remove_Layer.ToolTipText = MessageStrings.Remove_Layer;

            _pan.ToolTipText = MessageStrings.Pan;
            _zoom_In.ToolTipText = MessageStrings.Zoom_In_Tooltip;
            _zoom_Out.ToolTipText = MessageStrings.Zoom_Out_Tooltip;
            _zoom_To_Extents.ToolTipText = MessageStrings.Zoom_To_Extents_Tooltip;

            _zoomNext.ToolTipText = MessageStrings.Zoom_Next;
            _zoomPrevious.ToolTipText = MessageStrings.Zoom_Previous;
            /*_zoomToLayer.Caption = MessageStrings.la;*/
            _zoom_To_Coordinates.ToolTipText = MessageStrings.Zoom_To_Coordinates;

            _select.ToolTipText = MessageStrings.Select;
            _deselect.ToolTipText = MessageStrings.Deselect;
            _identify.ToolTipText = MessageStrings.Identify;
        }

        #endregion
    }
}