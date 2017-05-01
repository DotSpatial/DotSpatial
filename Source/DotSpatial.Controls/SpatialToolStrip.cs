// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
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
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(SpatialToolStrip), "Resources.SpatialToolStrip.ico")]
    public partial class SpatialToolStrip : ToolStrip
    {
        #region Fields

        private AppManager _applicationManager;

        private IMap _basicMap;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpatialToolStrip"/> class.
        /// </summary>
        public SpatialToolStrip()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpatialToolStrip"/> class using the specified IMap.
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
        /// Gets or sets the application manager.
        /// </summary>
        /// <value>
        /// The application manager.
        /// </value>
        [Description("Gets or sets the application manager.")]
        public AppManager ApplicationManager
        {
            get
            {
                return _applicationManager;
            }

            set
            {
                if (_applicationManager == value) return;
                _applicationManager = value;
                EnableControlsToAppManager();
            }
        }

        /// <summary>
        /// Gets or sets the basic map that this toolbar will interact with by default
        /// </summary>
        [Description("Gets or sets the basic map that this toolbar will interact with by default")]
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
                    _basicMap.MapFrame.ViewExtentsChanged -= MapFrameViewExtentsChanged;
                }

                if (ApplicationManager?.Map != null && ApplicationManager.Map != value)
                    throw new ArgumentException("Map cannot be different than the map assigned to the AppManager. Assign this map to the AppManager first.");

                _basicMap = value;
                if (_basicMap != null)
                {
                    _basicMap.MapFrame.ViewExtentsChanged += MapFrameViewExtentsChanged;
                }

                EnableControlsToMap();
            }
        }

        #endregion

        #region Methods

        private static void ShowSaveAsError(string fileName)
        {
            MessageBox.Show(string.Format(MessageStrings.FailedToWriteTheSpecifiedMapFile, fileName), MessageStrings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void CmdAddDataClick(object sender, EventArgs e)
        {
            Map.AddLayer();
        }

        private void CmdInfoClick(object sender, EventArgs e)
        {
            Map.FunctionMode = FunctionMode.Info;
        }

        private void CmdLabelClick(object sender, EventArgs e)
        {
            Map.FunctionMode = FunctionMode.Label;
        }

        private void CmdMaxExtentsClick(object sender, EventArgs e)
        {
            Map.ZoomToMaxExtent();
        }

        private void CmdNewClick(object sender, EventArgs e)
        {
            var app = ApplicationManager; // to avoid long names

            // if the map is empty or if the current project is already saved, start a new project directly
            if (!app.SerializationManager.IsDirty || app.Map.Layers == null || app.Map.Layers.Count == 0)
            {
                app.SerializationManager.New();
            }
            else if (string.IsNullOrEmpty(app.SerializationManager.CurrentProjectFile))
            {
                // if the current project is not specified - just ask to discard changes
                if (MessageBox.Show(MessageStrings.ClearAllDataAndStartANewMap, MessageStrings.DiscardChanges, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    app.SerializationManager.New();
                }
            }
            else
            {
                // the current project is specified - ask the users if they want to save changes to current project
                var saveProjectMessage = string.Format(MessageStrings.SaveChangesToCurrentProject, Path.GetFileName(app.SerializationManager.CurrentProjectFile));
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

        private void CmdOpenClick(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = ApplicationManager.SerializationManager.OpenDialogFilterText;
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                try
                {
                    // use the AppManager.SerializationManager to open the project
                    ApplicationManager.SerializationManager.OpenProject(dlg.FileName);
                    ApplicationManager.Map.Invalidate();
                }
                catch (IOException)
                {
                    MessageBox.Show(string.Format(MessageStrings.CouldNotOpenTheSpecifiedMapFile, dlg.FileName), MessageStrings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (XmlException)
                {
                    MessageBox.Show(string.Format(MessageStrings.FailedToReadTheSpecifiedMapFile, dlg.FileName), MessageStrings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (ArgumentException)
                {
                    MessageBox.Show(string.Format(MessageStrings.FailedToReadAPortionOfTheSpecifiedMapFile, dlg.FileName), MessageStrings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CmdPanClick(object sender, EventArgs e)
        {
            Map.FunctionMode = FunctionMode.Pan;
        }

        private void CmdPrintClick(object sender, EventArgs e)
        {
            using (var layout = new LayoutForm())
            {
                layout.MapControl = Map as Map;
                layout.ShowDialog(this);
            }
        }

        private void CmdSaveClick(object sender, EventArgs e)
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

        private void CmdSelectClick(object sender, EventArgs e)
        {
            Map.FunctionMode = FunctionMode.Select;
        }

        private void CmdTableClick(object sender, EventArgs e)
        {
            foreach (var fl in Map.MapFrame.GetAllLayers().Where(l => l.IsSelected).OfType<IFeatureLayer>())
            {
                fl.ShowAttributes();
            }
        }

        private void CmdZoomClick(object sender, EventArgs e)
        {
            Map.FunctionMode = FunctionMode.ZoomIn;
        }

        private void CmdZoomNextClick(object sender, EventArgs e)
        {
            Map.MapFrame.ZoomToNext();
        }

        private void CmdZoomOutClick(object sender, EventArgs e)
        {
            Map.FunctionMode = FunctionMode.ZoomOut;
        }

        private void CmdZoomPreviousClick(object sender, EventArgs e)
        {
            Map.MapFrame.ZoomToPrevious();
        }

        private void CmdZoomToCoordinatesClick(object sender, EventArgs e)
        {
            using (var dialog = new ZoomToCoordinatesDialog(Map))
                dialog.ShowDialog();
        }

        private void EnableControlsToAppManager()
        {
            // Enable buttons which depends from ApplicationManager
            cmdNew.Enabled = cmdOpen.Enabled = cmdSave.Enabled = ApplicationManager != null;
        }

        private void EnableControlsToMap()
        {
            // Enable buttons which depends from Map
            cmdZoomPrevious.Enabled = cmdZoomNext.Enabled = cmdAddData.Enabled = cmdPan.Enabled = cmdSelect.Enabled = cmdZoom.Enabled = cmdZoomOut.Enabled = cmdInfo.Enabled = cmdTable.Enabled = cmdMaxExtents.Enabled = cmdLabel.Enabled = cmdZoomToCoordinates.Enabled = Map != null;
        }

        private void MapFrameViewExtentsChanged(object sender, ExtentArgs e)
        {
            var mapFrame = sender as MapFrame;
            if (mapFrame == null) return;

            cmdZoomNext.Enabled = mapFrame.CanZoomToNext();
            cmdZoomPrevious.Enabled = mapFrame.CanZoomToPrevious();
        }

        #endregion
    }
}