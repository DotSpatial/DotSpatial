// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/8/2008 2:13:10 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------
// Jiri Kadlec        |  2/18/2010         |  Added zoom out button and custom mouse cursors
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Preconfigured tool strip menu.
    /// </summary>
    //This control will no longer be visible
    [ToolboxItem(false)]
   // [ToolboxBitmap(typeof(SpatialToolStrip), "SpatialToolStrip.ico")]
    public partial class SpatialToolStrip : ToolStrip
    {
        #region Private Variables

        private IMap _basicMap;
        private AppManager _applicationManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of mwToolBar
        /// </summary>
        public SpatialToolStrip()
            : this(null)
        {
        }

        /// <summary>
        /// Constructs and initializes this toolbar using the specified IBasicMap
        /// </summary>
        /// <param name="map">The map for the toolbar to interact with</param>
        public SpatialToolStrip(IMap map)
        {
            InitializeComponent();
            
            Map = map;
            EnableControlsToMap();
            EnableControlsToAppManager();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the basic map that this toolbar will interact with by default
        /// </summary>
        [Description(" Gets or sets the basic map that this toolbar will interact with by default")]
        public IMap Map
        {
            get
            {
                return _basicMap;
            }
            set
            {
                if (_basicMap != null)
                {
                    _basicMap.MapFrame.ViewExtentsChanged -= MapFrame_ViewExtentsChanged;
                }

                if (ApplicationManager != null && ApplicationManager.Map != null && ApplicationManager.Map != value)
                    throw new ArgumentException("Map cannot be different than the map assigned to the AppManager. Assign this map to the AppManager first.");

                _basicMap = value;
                if (_basicMap != null)
                {
                    _basicMap.MapFrame.ViewExtentsChanged += MapFrame_ViewExtentsChanged;
                }

                EnableControlsToMap();
            }
        }

        /// <summary>
        /// Gets or sets the application manager.
        /// </summary>
        /// <value>
        /// The application manager.
        /// </value>
        [Description("Gets or sets the application manager.")]
        public AppManager ApplicationManager
        {
            get { return _applicationManager; }
            set
            {
                if (_applicationManager == value) return;
                _applicationManager = value;
                EnableControlsToAppManager();
            }
        }
       
        #endregion

        private void EnableControlsToMap()
        {
            // Enable buttons which depends from Map
            cmdZoomPrevious.Enabled =
                cmdZoomNext.Enabled =
                    cmdAddData.Enabled =
                        cmdPan.Enabled =
                            cmdSelect.Enabled =
                                cmdZoom.Enabled =
                                    cmdZoomOut.Enabled =
                                        cmdInfo.Enabled =
                                            cmdTable.Enabled =
                                                cmdMaxExtents.Enabled =
                                                    cmdLabel.Enabled =
                                                        cmdZoomToCoordinates.Enabled =
                                                            Map != null;
        }

        private void EnableControlsToAppManager()
        {
            // Enable buttons which depends from ApplicationManager
            cmdNew.Enabled =
                cmdOpen.Enabled =
                    cmdSave.Enabled = ApplicationManager != null;
        }

        private void MapFrame_ViewExtentsChanged(object sender, ExtentArgs e)
        {
            var mapFrame = sender as MapFrame;
            if (mapFrame == null) return;

            cmdZoomNext.Enabled = mapFrame.CanZoomToNext();
            cmdZoomPrevious.Enabled = mapFrame.CanZoomToPrevious();
        }
        
        private void cmdLabel_Click(object sender, EventArgs e)
        {
            Map.FunctionMode = FunctionMode.Label;
        }
        
        private void cmdMaxExtents_Click(object sender, EventArgs e)
        {
            Map.ZoomToMaxExtent();
        }
        
        private void cmdTable_Click(object sender, EventArgs e)
        {
            foreach (var fl in Map.MapFrame.GetAllLayers()
                .Where(l => l.IsSelected)
                .OfType<IFeatureLayer>())
            {
                fl.ShowAttributes();
            }
        }
       
        private void cmdInfo_Click(object sender, EventArgs e)
        {
            Map.FunctionMode = FunctionMode.Info;
        }
       
        private void cmdZoom_Click(object sender, EventArgs e)
        {
            Map.FunctionMode = FunctionMode.ZoomIn;
        }

        private void cmdZoomOut_Click(object sender, EventArgs e)
        {
            Map.FunctionMode = FunctionMode.ZoomOut;
        }
     
        private void cmdSelect_Click(object sender, EventArgs e)
        {
            Map.FunctionMode = FunctionMode.Select;
        }
        
        private void cmdPan_Click(object sender, EventArgs e)
        {
            Map.FunctionMode = FunctionMode.Pan;
        }
        
        private void cmdAddData_Click(object sender, EventArgs e)
        {
            Map.AddLayer();
        }
       
        private void cmdPrint_Click(object sender, EventArgs e)
        {
            using (var layout = new LayoutForm())
            {
                layout.MapControl = Map as Map;
                layout.ShowDialog(this);
            }
        }
        
        private void cmdSave_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog
            {
                Filter = ApplicationManager.SerializationManager.SaveDialogFilterText,
                SupportMultiDottedExtensions = true
            })
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                string fileName = dlg.FileName;
                try
                {
                    ApplicationManager.SerializationManager.SaveProject(fileName);
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
        }

        private static void ShowSaveAsError(string fileName)
        {
            MessageBox.Show(String.Format(MessageStrings.FailedToWriteTheSpecifiedMapFile, fileName), MessageStrings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        
        private void cmdOpen_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = ApplicationManager.SerializationManager.OpenDialogFilterText;
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                try
                {
                    //use the AppManager.SerializationManager to open the project
                    ApplicationManager.SerializationManager.OpenProject(dlg.FileName);
                    ApplicationManager.Map.Invalidate();
                }
                catch (IOException)
                {
                    MessageBox.Show(String.Format(MessageStrings.CouldNotOpenTheSpecifiedMapFile, dlg.FileName),
                        MessageStrings.Error,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (XmlException)
                {
                    MessageBox.Show(String.Format(MessageStrings.FailedToReadTheSpecifiedMapFile, dlg.FileName),
                        MessageStrings.Error,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (ArgumentException)
                {
                    MessageBox.Show(String.Format(MessageStrings.FailedToReadAPortionOfTheSpecifiedMapFile, dlg.FileName),
                        MessageStrings.Error,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
     
        private void cmdNew_Click(object sender, EventArgs e)
        {
            var app = ApplicationManager; // to avoid long names

            // if the map is empty or if the current project is already saved, start a new project directly
            if (!app.SerializationManager.IsDirty || app.Map.Layers == null || app.Map.Layers.Count == 0)
            {
                app.SerializationManager.New();
            }
            else if (String.IsNullOrEmpty(app.SerializationManager.CurrentProjectFile))
            {
                //if the current project is not specified - just ask to discard changes
                if (MessageBox.Show(MessageStrings.ClearAllDataAndStartANewMap, MessageStrings.DiscardChanges, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    app.SerializationManager.New();
                }
            }
            else
            {
                //the current project is specified - ask the users if they want to save changes to current project
                var saveProjectMessage = String.Format(MessageStrings.SaveChangesToCurrentProject, Path.GetFileName(app.SerializationManager.CurrentProjectFile));
                var msgBoxResult = MessageBox.Show(saveProjectMessage, MessageStrings.DiscardChanges, MessageBoxButtons.YesNoCancel);

                if (msgBoxResult == DialogResult.Yes)
                {
                    app.SerializationManager.SaveProject(ApplicationManager.SerializationManager.CurrentProjectFile);
                }
                if (msgBoxResult != DialogResult.Cancel)
                {
                    app.SerializationManager.New();
                }
            }
        }
      
        private void cmdZoomPrevious_Click(object sender, EventArgs e)
        {
            Map.MapFrame.ZoomToPrevious();
        }

        private void cmdZoomNext_Click(object sender, EventArgs e)
        {
            Map.MapFrame.ZoomToNext();
        }

        private void cmdZoomToCoordinates_Click(object sender, EventArgs e)
        {
            using (var dialog = new ZoomToCoordinatesDialog(Map))
                dialog.ShowDialog();
        }
    }
}