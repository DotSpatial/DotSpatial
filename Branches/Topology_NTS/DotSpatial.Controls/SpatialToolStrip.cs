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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// mwToolBar
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(SpatialToolStrip), "SpatialToolStrip.ico")]
    [Obsolete("Load the DotSpatial.Plugins.MenuBar into a form that implements IHeaderControl instead. See http://tinyurl.com/obsolete1")]
    public class SpatialToolStrip : ToolStrip
    {
        #region Events

        /// <summary>
        /// Occurs when the print button is clicked
        /// </summary>
        public event EventHandler PrintClicked;

        #endregion Events

        #region Private Variables

        private IBasicMap _basicMap;
        private int _currentExtentId;
        private bool _manualExtentsChange;
        private List<Extent> _previousExtents = new List<Extent>();
        private ToolStripButton cmdAddData;
        private ToolStripButton cmdInfo;
        private ToolStripButton cmdLabel;
        private ToolStripButton cmdMaxExtents;
        private ToolStripButton cmdMeasure;
        private ToolStripButton cmdNew;
        private ToolStripButton cmdOpen;
        private ToolStripButton cmdPan;
        private ToolStripButton cmdPrint;
        private ToolStripButton cmdSave;
        private ToolStripButton cmdSelect;
        private ToolStripButton cmdTable;
        private ToolStripButton cmdZoom;
        private ToolStripButton cmdZoomNext;
        private ToolStripButton cmdZoomOut;
        private ToolStripButton cmdZoomPrevious;

        #endregion Private Variables

        #region Constructors

        /// <summary>
        /// Creates a new instance of mwToolBar
        /// </summary>
        public SpatialToolStrip()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructs and initializes this toolbar using the specified IBasicMap
        /// </summary>
        /// <param name="map">The map for the toolbar to interact with</param>
        public SpatialToolStrip(IBasicMap map)
        {
            Init(map);
            InitializeComponent();

            map.MapFrame.ViewExtentsChanged += MapFrame_ViewExtentsChanged;
        }

        private void MapFrame_ViewExtentsChanged(object sender, ExtentArgs e)
        {
            IMapFrame mapFrame = sender as IMapFrame;

            if (mapFrame == null) return;
            if (_previousExtents == null) return;
            if (mapFrame.Layers.Count == 0) return;

            //If m_IsManualExtentsChange = True Then
            //    m_IsManualExtentsChange = False 'reset the flag for the next extents change
            //Else
            //    FlushForwardHistory()
            //    m_Extents.Add(MapMain.Extents)
            //    m_CurrentExtent = m_Extents.Count - 1
            //End If
            if (_manualExtentsChange)
            {
                _manualExtentsChange = false;
            }
            else
            {
                _previousExtents.Add(mapFrame.ViewExtents);
                _currentExtentId = _previousExtents.Count - 1;
            }

            if (_currentExtentId < _previousExtents.Count - 1)
            {
                cmdZoomNext.Enabled = true;
            }

            if ((_previousExtents.Count > 0) && (_currentExtentId > 0))
            {
                cmdZoomPrevious.Enabled = true;
            }
        }

        #endregion Constructors

        #region Windows Form Designer generated code

        /// <summary>
        /// Warning: Using the normal resource manager that is used by default by the program will cause
        /// the entire application to crash in mono.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpatialToolStrip));
            this.cmdNew = new System.Windows.Forms.ToolStripButton();
            this.cmdOpen = new System.Windows.Forms.ToolStripButton();
            this.cmdSave = new System.Windows.Forms.ToolStripButton();
            this.cmdPrint = new System.Windows.Forms.ToolStripButton();
            this.cmdAddData = new System.Windows.Forms.ToolStripButton();
            this.cmdPan = new System.Windows.Forms.ToolStripButton();
            this.cmdSelect = new System.Windows.Forms.ToolStripButton();
            this.cmdZoom = new System.Windows.Forms.ToolStripButton();
            this.cmdZoomOut = new System.Windows.Forms.ToolStripButton();
            this.cmdInfo = new System.Windows.Forms.ToolStripButton();
            this.cmdTable = new System.Windows.Forms.ToolStripButton();
            this.cmdMaxExtents = new System.Windows.Forms.ToolStripButton();
            this.cmdLabel = new System.Windows.Forms.ToolStripButton();
            this.cmdMeasure = new System.Windows.Forms.ToolStripButton();
            this.cmdZoomPrevious = new System.Windows.Forms.ToolStripButton();
            this.cmdZoomNext = new System.Windows.Forms.ToolStripButton();
            this.SuspendLayout();
            //
            // cmdNew
            //
            this.cmdNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdNew.Image = global::DotSpatial.Controls.Images.file_new;
            this.cmdNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdNew.Name = "cmdNew";
            this.cmdNew.Size = new System.Drawing.Size(23, 69);
            this.cmdNew.ToolTipText = "New";
            this.cmdNew.Click += new System.EventHandler(this.cmdNew_Click);
            //
            // cmdOpen
            //
            this.cmdOpen.Image = global::DotSpatial.Controls.Images.FolderOpen;
            this.cmdOpen.Name = "cmdOpen";
            this.cmdOpen.Size = new System.Drawing.Size(23, 69);
            this.cmdOpen.ToolTipText = "Open Project";
            this.cmdOpen.Click += new System.EventHandler(this.cmdOpen_Click);
            //
            // cmdSave
            //
            this.cmdSave.Image = global::DotSpatial.Controls.Images.file_saveas;
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(23, 69);
            this.cmdSave.ToolTipText = "Save Project";
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            //
            // cmdPrint
            //
            this.cmdPrint.Image = global::DotSpatial.Controls.Images.printer;
            this.cmdPrint.Name = "cmdPrint";
            this.cmdPrint.Size = new System.Drawing.Size(23, 20);
            this.cmdPrint.ToolTipText = "Print";
            this.cmdPrint.Click += new System.EventHandler(this.cmdPrint_Click);
            //
            // cmdAddData
            //
            this.cmdAddData.Image = global::DotSpatial.Controls.Images.AddLayer;
            this.cmdAddData.Name = "cmdAddData";
            this.cmdAddData.Size = new System.Drawing.Size(23, 20);
            this.cmdAddData.ToolTipText = "Add Data";
            this.cmdAddData.Click += new System.EventHandler(this.cmdAddData_Click);
            //
            // cmdPan
            //
            this.cmdPan.Image = ((System.Drawing.Image)(resources.GetObject("cmdPan.Image")));
            this.cmdPan.Name = "cmdPan";
            this.cmdPan.Size = new System.Drawing.Size(23, 20);
            this.cmdPan.ToolTipText = "Pan";
            this.cmdPan.CheckedChanged += new System.EventHandler(this.cmdPan_CheckedChanged);
            this.cmdPan.Click += new System.EventHandler(this.cmdPan_Click);
            //
            // cmdSelect
            //
            this.cmdSelect.Image = ((System.Drawing.Image)(resources.GetObject("cmdSelect.Image")));
            this.cmdSelect.Name = "cmdSelect";
            this.cmdSelect.Size = new System.Drawing.Size(23, 20);
            this.cmdSelect.ToolTipText = "Select";
            this.cmdSelect.CheckedChanged += new System.EventHandler(this.cmdSelect_CheckedChanged);
            this.cmdSelect.Click += new System.EventHandler(this.cmdSelect_Click);
            //
            // cmdZoom
            //
            // this.cmdZoom.Image = ((System.Drawing.Image)(resources.GetObject("cmdZoom.Image")));
            this.cmdZoom.Name = "cmdZoom";
            this.cmdZoom.Size = new System.Drawing.Size(23, 20);
            this.cmdZoom.ToolTipText = "Zoom In";
            this.cmdZoom.CheckedChanged += new System.EventHandler(this.cmdZoom_CheckedChanged);
            this.cmdZoom.Click += new System.EventHandler(this.cmdZoom_Click);
            //
            // cmdZoomOut
            //
            //this.cmdZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("cmdZoomOut.Image")));
            this.cmdZoomOut.Name = "cmdZoomOut";
            this.cmdZoomOut.Size = new System.Drawing.Size(23, 20);
            this.cmdZoomOut.ToolTipText = "Zoom Out";
            this.cmdZoomOut.CheckedChanged += new System.EventHandler(this.cmdZoom_CheckedChanged);
            this.cmdZoomOut.Click += new System.EventHandler(this.cmdZoomOut_Click);
            //
            // cmdInfo
            //
            this.cmdInfo.BackColor = System.Drawing.Color.Transparent;
            // this.cmdInfo.Image = global::DotSpatial.Controls.Images.info;
            this.cmdInfo.Name = "cmdInfo";
            this.cmdInfo.Size = new System.Drawing.Size(23, 20);
            this.cmdInfo.ToolTipText = "Identifier";
            this.cmdInfo.CheckedChanged += new System.EventHandler(this.cmdInfo_CheckedChanged);
            this.cmdInfo.Click += new System.EventHandler(this.cmdInfo_Click);
            //
            // cmdTable
            //
            // this.cmdTable.Image = global::DotSpatial.Controls.Images.Table;
            this.cmdTable.Name = "cmdTable";
            this.cmdTable.Size = new System.Drawing.Size(23, 20);
            this.cmdTable.ToolTipText = "Attribute Table";
            this.cmdTable.CheckedChanged += new System.EventHandler(this.cmdTable_CheckedChanged);
            this.cmdTable.Click += new System.EventHandler(this.cmdTable_Click);
            //
            // cmdMaxExtents
            //
            this.cmdMaxExtents.Image = ((System.Drawing.Image)(resources.GetObject("cmdMaxExtents.Image")));
            this.cmdMaxExtents.Name = "cmdMaxExtents";
            this.cmdMaxExtents.Size = new System.Drawing.Size(23, 20);
            this.cmdMaxExtents.ToolTipText = "Zoom to Maximum Extents";
            this.cmdMaxExtents.Click += new System.EventHandler(this.cmdMaxExtents_Click);
            //
            // cmdLabel
            //
            this.cmdLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            //this.cmdLabel.Image = global::DotSpatial.Controls.Images.Label;
            this.cmdLabel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdLabel.Name = "cmdLabel";
            this.cmdLabel.Size = new System.Drawing.Size(23, 23);
            this.cmdLabel.Click += new System.EventHandler(this.cmdLabel_Click);
            //
            // cmdMeasure
            //
            this.cmdMeasure.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            //this.cmdMeasure.Image = global::DotSpatial.Controls.Images.ScaleBar;
            this.cmdMeasure.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdMeasure.Name = "cmdMeasure";
            this.cmdMeasure.Size = new System.Drawing.Size(23, 20);
            this.cmdMeasure.ToolTipText = "Measure Distance";
            this.cmdMeasure.Click += new System.EventHandler(this.cmdMeasure_Click);
            //
            // cmdZoomPrevious
            //
            this.cmdZoomPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdZoomPrevious.Image = ((System.Drawing.Image)(resources.GetObject("cmdZoomPrevious.Image")));
            this.cmdZoomPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdZoomPrevious.Name = "cmdZoomPrevious";
            this.cmdZoomPrevious.Size = new System.Drawing.Size(23, 23);
            this.cmdZoomPrevious.Text = "toolStripButton1";
            this.cmdZoomPrevious.ToolTipText = "Zoom to Previous Extents";
            this.cmdZoomPrevious.Click += new System.EventHandler(this.cmdZoomPrevious_Click);
            //
            // cmdZoomNext
            //
            this.cmdZoomNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdZoomNext.Image = ((System.Drawing.Image)(resources.GetObject("cmdZoomNext.Image")));
            this.cmdZoomNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdZoomNext.Name = "cmdZoomNext";
            this.cmdZoomNext.Size = new System.Drawing.Size(23, 23);
            this.cmdZoomNext.Text = "toolStripButton1";
            this.cmdZoomNext.ToolTipText = "Zoom to Next Extent";
            this.cmdZoomNext.Click += new System.EventHandler(this.cmdZoomNext_Click);
            //
            // SpatialToolStrip
            //
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                                                                             this.cmdNew,
                                                                             this.cmdOpen,
                                                                             this.cmdSave,
                                                                             this.cmdPrint,
                                                                             this.cmdAddData,
                                                                             this.cmdPan,
                                                                             this.cmdSelect,
                                                                             this.cmdZoom,
                                                                             this.cmdZoomOut,
                                                                             this.cmdZoomPrevious,
                                                                             this.cmdZoomNext,
                                                                             this.cmdInfo,
                                                                             this.cmdTable,
                                                                             this.cmdMaxExtents,
                                                                             this.cmdMeasure});
            this.Size = new System.Drawing.Size(100, 72);
            this.ResumeLayout(false);
        }

        /// <summary>
        /// Unchecks all toolstrip buttons except the current button
        /// </summary>
        /// <param name="checkedButton">The toolstrip button which should
        /// stay checked</param>
        private void UncheckOtherButtonsButMe(ToolStripButton checkedButton)
        {
            foreach (ToolStripItem item in this.Items)
            {
                ToolStripButton buttonItem = item as ToolStripButton;
                if (buttonItem != null)
                {
                    if (buttonItem.Name != checkedButton.Name)
                    {
                        buttonItem.Checked = false;
                    }
                }
            }
        }

        private void cmdTable_CheckedChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void cmdInfo_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void cmdZoom_CheckedChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void cmdSelect_CheckedChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void cmdPan_CheckedChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        #endregion Windows Form Designer generated code

        /// <summary>
        /// Allows the editing of labels
        /// </summary>
        private void cmdLabel_Click(object sender, EventArgs e)
        {
            if (_basicMap == null) return;
            _basicMap.FunctionMode = FunctionMode.Label;
        }

        /// <summary>
        /// Zoom to maximum extents
        /// </summary>
        private void cmdMaxExtents_Click(object sender, EventArgs e)
        {
            if (_basicMap == null) return;

            UncheckOtherButtonsButMe(cmdMaxExtents);

            _basicMap.ZoomToMaxExtent();

            _basicMap.FunctionMode = FunctionMode.None;
        }

        /// <summary>
        /// Open attribute table
        /// </summary>
        private void cmdTable_Click(object sender, EventArgs e)
        {
            if (_basicMap == null) return;

            UncheckOtherButtonsButMe(cmdTable);

            Map mainMap = _basicMap as Map;

            if (mainMap != null)
            {
                foreach (ILayer layer in mainMap.GetAllLayers())
                {
                    IFeatureLayer fl = layer as IFeatureLayer;

                    if (fl == null) continue;
                    if (fl.IsSelected == false) continue;
                    fl.ShowAttributes();
                }
            }
            else
            {
                foreach (ILayer layer in _basicMap.GetLayers())
                {
                    IFeatureLayer fl = layer as IFeatureLayer;

                    if (fl == null) continue;
                    if (fl.IsSelected == false) continue;
                    fl.ShowAttributes();
                }
            }
        }

        /// <summary>
        /// Identifier Tool
        /// </summary>
        private void cmdInfo_Click(object sender, EventArgs e)
        {
            if (_basicMap == null) return;

            if (cmdInfo.Checked == false)
            {
                UncheckOtherButtonsButMe(cmdInfo);
                cmdInfo.Checked = true;

                _basicMap.FunctionMode = FunctionMode.Info;
            }
            else
            {
                cmdInfo.Checked = false;
                _basicMap.FunctionMode = FunctionMode.None;
            }
        }

        /// <summary>
        /// Zoom In
        /// </summary>
        private void cmdZoom_Click(object sender, EventArgs e)
        {
            if (_basicMap == null) return;

            if (cmdZoom.Checked == false)
            {
                UncheckOtherButtonsButMe(cmdZoom);
                cmdZoom.Checked = true;
                _basicMap.FunctionMode = FunctionMode.ZoomIn;
            }
        }

        private void cmdZoomOut_Click(object sender, EventArgs e)
        {
            if (_basicMap == null) return;

            if (cmdZoomOut.Checked == false)
            {
                UncheckOtherButtonsButMe(cmdZoomOut);
                cmdZoomOut.Checked = true;
                _basicMap.FunctionMode = FunctionMode.ZoomOut;
            }
        }

        /// <summary>
        /// Select or deselect Features
        /// </summary>
        private void cmdSelect_Click(object sender, EventArgs e)
        {
            if (_basicMap == null) return;

            if (cmdSelect.Checked == false)
            {
                UncheckOtherButtonsButMe(cmdSelect);
                cmdSelect.Checked = true;
                _basicMap.FunctionMode = FunctionMode.Select;
            }
        }

        /// <summary>
        /// Move (Pan) the map
        /// </summary>
        private void cmdPan_Click(object sender, EventArgs e)
        {
            if (_basicMap == null) return;

            if (cmdPan.Checked == false)
            {
                UncheckOtherButtonsButMe(cmdPan);
                cmdPan.Checked = true;

                _basicMap.FunctionMode = FunctionMode.Pan;
            }
        }

        /// <summary>
        /// Measure Distance
        /// </summary>
        private void cmdMeasure_Click(object sender, EventArgs e)
        {
            if (_basicMap == null) return;

            if (cmdMeasure.Checked == false)
            {
                UncheckOtherButtonsButMe(cmdMeasure);
                cmdMeasure.Checked = true;
            }
        }

        /// <summary>
        /// Add Data to the Map
        /// </summary>
        private void cmdAddData_Click(object sender, EventArgs e)
        {
            if (_basicMap == null) return;
            _basicMap.AddLayer();
        }

        /// <summary>
        /// Print Map
        /// </summary>
        private void cmdPrint_Click(object sender, EventArgs e)
        {
            OnPrintClicked();
        }

        /// <summary>
        /// Fires the PrintClicked event
        /// </summary>
        protected virtual void OnPrintClicked()
        {
            if (PrintClicked != null) PrintClicked(this, EventArgs.Empty);
        }

        /// <summary>
        /// Save Map
        /// </summary>
        private void cmdSave_Click(object sender, EventArgs e)
        {
            Contract.Requires(ApplicationManager != null, "ApplicationManager is null.");

            var dlg = new SaveFileDialog();
            dlg.Filter = String.Format("{0} (*.dspx)|*.dspx", "Project File");
            dlg.SupportMultiDottedExtensions = true;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                string fileName = dlg.FileName;
                try
                {
                    ApplicationManager.SerializationManager.SaveProject(fileName);
                }
                catch (XmlException)
                {
                    MessageBox.Show(this, "Failed to write the specified map file " + fileName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (IOException)
                {
                    MessageBox.Show(this, "Could not write to the specified map file " + fileName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Open Map
        /// </summary>
        private void cmdOpen_Click(object sender, EventArgs e)
        {
            Contract.Requires(ApplicationManager != null, "ApplicationManager is null.");

            var dlg = new OpenFileDialog();
            dlg.Filter = "Supported Files |*.map.xml;*.dspx;*.mwprj;*.mwa;*.zip|Project Files (*.dspx)|*.dspx";

            if (dlg.ShowDialog(this) != DialogResult.OK) return;
            string fileName = dlg.FileName;
            try
            {
                ApplicationManager.SerializationManager.OpenProject(fileName);
            }
            catch (IOException exIO)
            {
                MessageBox.Show(this, "Could not open the specified map file " + fileName + Environment.NewLine + Environment.NewLine + exIO.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (XmlException)
            {
                MessageBox.Show(this, "Failed to read the specified map file " + fileName, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException)
            {
                MessageBox.Show(this, "Failed to read a portion of the specified map file " + fileName, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// New Map
        /// </summary>
        private void cmdNew_Click(object sender, EventArgs e)
        {
            Contract.Requires(ApplicationManager != null, "ApplicationManager is null.");

            if (MessageBox.Show(this, "Are you sure you want to clear all data and start a new map?", "Confirm new map",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ApplicationManager.SerializationManager.New();
            }
        }

        /// <summary>
        /// Zoom to previous extent
        /// </summary>
        private void cmdZoomPrevious_Click(object sender, EventArgs e)
        {
            if ((_previousExtents.Count > 0) && (_currentExtentId > 0))
            {
                _manualExtentsChange = true;
                _currentExtentId -= 1;
                this._basicMap.ViewExtents = _previousExtents[_currentExtentId];
            }
            else
            {
                cmdZoomPrevious.Enabled = false;
            }
        }

        /// <summary>
        /// Zoom to previous extent
        /// </summary>
        private void cmdZoomNext_Click(object sender, EventArgs e)
        {
            if (_currentExtentId < _previousExtents.Count - 1)
            {
                _currentExtentId += 1;
                _manualExtentsChange = true;
                _basicMap.ViewExtents = _previousExtents[_currentExtentId];
            }
            else
            {
                cmdZoomNext.Enabled = false;
            }
        }

        #region Methods

        /// <summary>
        /// Initializes the map tool, telling it what map that it will be working with.
        /// </summary>
        /// <param name="map">Any implementation of IBasicMap that the tool can work with</param>
        private void Init(IBasicMap map)
        {
            _basicMap = map;
        }

        #endregion Methods

        #region Properties

        private AppManager _applicationManager;

        /// <summary>
        /// Gets or sets the basic map that this toolbar will interact with by default
        /// </summary>
        public virtual IBasicMap Map
        {
            get { return _basicMap; }
            set
            {
                if (_basicMap != null)
                {
                    _basicMap.MapFrame.ViewExtentsChanged -= MapFrame_ViewExtentsChanged;
                }

                if (ApplicationManager != null && ApplicationManager.Map != null && ApplicationManager.Map != value)
                    throw new ArgumentException("Map cannot be different than the map assigned to the AppManager. Assign this map to the AppManager first.");

                _basicMap = value;
                _basicMap.MapFrame.ViewExtentsChanged += MapFrame_ViewExtentsChanged;
            }
        }

        /// <summary>
        /// Gets or sets the application manager.
        /// </summary>
        /// <value>
        /// The application manager.
        /// </value>
        public virtual AppManager ApplicationManager
        {
            get
            {
                return _applicationManager;
            }
            set
            {
                _applicationManager = value;
            }
        }

        #endregion Properties
    }
}