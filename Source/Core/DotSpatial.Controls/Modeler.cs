// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using Timer = System.Windows.Forms.Timer;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A modeler form which allows users to create models with visual representations of tools.
    /// </summary>
    // This control will no longer be visible
    [ToolboxItem(false)]
    public partial class Modeler : UserControl
    {
        #region Fields

        // These lists contain all the selected and unselected elements in the model
        private readonly Color _arrowColor = Color.Black;
        private readonly List<ModelElement> _modelElements = new(); // All the model elements, selected or otherwise
        private readonly List<ModelElement> _modelElementsSelected = new(); // All the selected model elements
        private readonly Timer _scrollTimerHorizontal;
        private readonly Timer _scrollTimerVertical;
        private Bitmap _backBuffer;

        private bool _enableLinking;
        private int _horLastValue;
        private bool _isInitialized;
        private ArrowElement _linkArrow;

        // Used for loading and saving models
        private string _modelFilename;
        private bool _mouseDownOnElement;
        private bool _mouseMoved;

        // Used for mouse movements
        private bool _mouseMoveDone = true;
        private Point _mouseOldPoint;
        private bool _newModel;
        private bool _scrollLock;

        // Used for drawing the selection box when a user left clicks and drags
        private Rectangle _selectBox;
        private bool _selectBoxDraw;
        private Point _selectBoxPt;

        /// <summary>
        /// Decides if the the DotSpatial watermark is present in the lowerleft corner.
        /// </summary>
        private bool _showWaterMark = true;

        // These variables define how the tool elements of the model are drawn
        private Color _toolColor = Color.Khaki;
        private Font _toolFont = SystemFonts.DialogFont;
        private ModelShape _toolShape = ModelShape.Rectangle;
        private int _toolToExeCount;

        // These variables define how the data elements of the model are drawn
        private int _verLastValue;
        private Point _virtualOffset; // The location of the model origin in the form
        private float _virtualZoom; // The zoom factor of the model

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Modeler"/> class.
        /// </summary>
        public Modeler()
        {
            InitializeComponent();

            // Sets up the scroll bars
            _verScroll.Minimum = 0;
            _verScroll.Maximum = Height - 16;
            _verScroll.Value = Height / 2;
            _verLastValue = _verScroll.Value;
            _verScroll.LargeChange = 50;
            _verScroll.SmallChange = 10;
            _verScroll.ValueChanged += VerScrollValueChanged;
            _horScroll.Minimum = 0;
            _horScroll.Maximum = Width - 16;
            _horScroll.Value = Width / 2;
            _horLastValue = _horScroll.Value;
            _horScroll.LargeChange = 50;
            _horScroll.SmallChange = 10;
            _horScroll.ValueChanged += HorScrollValueChanged;
            _scrollTimerHorizontal = new Timer
            {
                Interval = 100
            };
            _scrollTimerHorizontal.Tick += ScrollTimerHorizontalTick;
            _scrollTimerVertical = new Timer
            {
                Interval = 100
            };
            _scrollTimerVertical.Tick += ScrollTimerVerticalTick;
            _scrollLock = false;

            // Sets up the zoom and offset
            _virtualZoom = 1F;
            _virtualOffset = new Point(0, 0);

            // Sets the default project extension
            DefaultFileExtension = "mwm";

            // Default number of simulatenous tool execution threads
            MaxExecutionThreads = 2;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the model fileName has changed
        /// </summary>
        public event EventHandler ModelFilenameChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the color that data should have in the modeler.
        /// </summary>
        [Category("Model Appearance")]
        [Description("Gets or sets the color that data should have in the modeler")]
        public Color DataColor { get; set; } = Color.LightGreen;

        /// <summary>
        /// Gets or sets the font that will be used to label the data in the modeler.
        /// </summary>
        [Category("Model Appearance")]
        [Description("Gets or sets the font that will be used to label the data in the modeler")]
        public Font DataFont { get; set; } = SystemFonts.DialogFont;

        /// <summary>
        /// Gets or sets the shape that data should be represented with in the modeler.
        /// </summary>
        [Category("Model Appearance")]
        [Description("Gets or sets the shape that data should be represented with in the modeler")]
        public ModelShape DataShape { get; set; } = ModelShape.Ellipse;

        /// <summary>
        /// Gets or sets the extension used by default for opening, saving and creating new models. ex "mwm".
        /// </summary>
        public string DefaultFileExtension { get; set; }

        /// <summary>
        /// Gets or Sets the drawing quality for the toolbox.
        /// </summary>
        [Category("Model Appearance")]
        [Description("Gets or Sets the drawing quality for the toolbox")]
        public SmoothingMode DrawingQuality { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether linking by mouse is on.
        /// </summary>
        public bool EnableLinking
        {
            get
            {
                return _enableLinking;
            }

            set
            {
                _enableLinking = value;
                if (_enableLinking)
                {
                    Cursor = Cursors.Cross;
                    ClearSelectedElements();
                }
                else
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the model drawing needs to be initialized.
        /// </summary>
        [Category("Model Appearance")]
        [Description("Gets or Sets if the model drawing needs to be initialized (This must always be true at design time)")]
        public bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }

            set
            {
                _isInitialized = value;
                if (_isInitialized == false)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of processes to execute at the same time.
        /// </summary>
        public int MaxExecutionThreads { get; set; }

        /// <summary>
        /// Gets or sets the fileName of the current model.
        /// </summary>
        public string ModelFilename
        {
            get
            {
                return _modelFilename;
            }

            set
            {
                _modelFilename = value;
                OnModelFilenameChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the DotSpatial watermark in the lower right hand corner can be seen. Defautl true.
        /// </summary>
        [Category("Model Appearance")]
        [Description("Turns the DotSpatial watermark in the lower right hand corner on or off")]
        public bool ShowWaterMark
        {
            get
            {
                return _showWaterMark;
            }

            set
            {
                _showWaterMark = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the color that tools should have in the modeler.
        /// </summary>
        [Category("Model Appearance")]
        [Description("Gets or sets the color that tools should be represented with in the modeler")]
        public Color ToolColor
        {
            get
            {
                return _toolColor;
            }

            set
            {
                foreach (var modelEl in _modelElements.OfType<ToolElement>())
                {
                    modelEl.Color = value;
                }

                _toolColor = value;
                IsInitialized = false;
            }
        }

        /// <summary>
        /// Gets or sets the font that will be used to label the tools in the modeler.
        /// </summary>
        [Category("Model Appearance")]
        [Description("Gets or sets the font that will be used to label the tools in the modeler")]
        public Font ToolFont
        {
            get
            {
                return _toolFont;
            }

            set
            {
                foreach (var modelEl in _modelElements.OfType<ToolElement>())
                {
                    modelEl.Font = value;
                }

                _toolFont = value;
                IsInitialized = false;
            }
        }

        /// <summary>
        /// Gets or sets the toolManager used to create instances of all the tools.
        /// </summary>
        [Category("Model Appearance")]
        [Description("Sets the toolManager used to create instances of all the tools.")]
        public ToolManager ToolManager { get; set; }

        /// <summary>
        /// Gets or sets the shape that tools should be represented with in the modeler.
        /// </summary>
        [Category("Model Appearance")]
        [Description("Gets or sets the shape that tools should be represented with in the modeler")]
        public ModelShape ToolShape
        {
            get
            {
                return _toolShape;
            }

            set
            {
#if DEBUG
                var sw = new Stopwatch();
                sw.Start();
#endif
                foreach (var modelEl in _modelElements.OfType<ToolElement>())
                {
                    modelEl.Shape = value;
                }

#if DEBUG
                sw.Stop();
                Debug.WriteLine("my sw: " + sw.ElapsedMilliseconds);
#endif

                _toolShape = value;
                IsInitialized = false;
            }
        }

        /// <summary>
        /// Gets or sets the working path for the model.
        /// </summary>
        public string WorkingPath { get; set; }

        /// <summary>
        /// Gets or sets the zoom factor of the map values below 0 zoom out, values above 1 zoom in. 1 = no zoom.
        /// </summary>
        public float ZoomFactor
        {
            get
            {
                return _virtualZoom;
            }

            set
            {
                _virtualZoom = value < 0.0005F ? 0.0005F : value;
                Invalidate();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Centers the map on a given point.
        /// </summary>
        /// <param name="centerPoint">A Point in the modelers virtual coordinant system.</param>
        public void CenterModelerOnPoint(Point centerPoint)
        {
            Point virtBr = PixelToVirtual(Width, Height);
            Point virtTl = PixelToVirtual(0, 0);
            Point oldVirtCenterPoint = new(virtTl.X + ((virtBr.X - virtTl.X) / 2), virtTl.Y + ((virtBr.Y - virtTl.Y) / 2));

            _virtualOffset = new Point(_virtualOffset.X + Convert.ToInt32((oldVirtCenterPoint.X - centerPoint.X) * _virtualZoom), _virtualOffset.Y + Convert.ToInt32((oldVirtCenterPoint.Y - centerPoint.Y) * _virtualZoom));

            virtBr = PixelToVirtual(Width, Height);
            virtTl = PixelToVirtual(0, 0);

            // Point newVirtCenterPoint = new Point(virtTl.X + ((virtBr.X - virtTl.X) / 2), virtTl.Y + ((virtBr.Y - virtTl.Y) / 2));

            // Debuging code
            //            Debug.WriteLine("New Virtyual Center: X=" + newVirtCenterPoint.X + "Y=" + newVirtCenterPoint.Y);
            IsInitialized = false;
        }

        /// <summary>
        /// Creates a new model.
        /// </summary>
        /// <param name="promptSave">if true prompts user to save current model if it is not saved.</param>
        public void CreateNewModel(bool promptSave)
        {
            string fileName = Path.Combine(Path.GetTempPath(), "New_Model") + "." + DefaultFileExtension;
            int i = 1;
            while (File.Exists(fileName))
            {
                fileName = Path.Combine(Path.GetTempPath(), "New_Model") + "_" + i + "." + DefaultFileExtension;
                i++;
            }

            ModelFilename = fileName;
            _newModel = true;

            // Resets all the model properties
            _modelElements.Clear();
            _modelElementsSelected.Clear();
            Invalidate();
            _enableLinking = false;
        }

        /// <summary>
        /// Deletes all of the selected elements if it is allowed.
        /// </summary>
        public void DeleteSelectedElements()
        {
            if (_modelElementsSelected.Count <= 0)
                return;

            // Thic creates a list of all the model elements that will need to be deleted due to their associations to data and tools
            bool promptDelParents = false;
            List<ModelElement> tempSelection = new();
            foreach (ModelElement selectedElement in _modelElementsSelected)
            {
                tempSelection.Add(selectedElement);
                if (selectedElement is DataElement)
                {
                    foreach (ModelElement parentEle in selectedElement.GetParents())
                    {
                        if (!_modelElementsSelected.Contains(parentEle))
                        {
                            tempSelection.Add(parentEle);
                            promptDelParents = true;
                        }
                    }
                }
                else if (selectedElement is ToolElement)
                {
                    foreach (ModelElement childEle in selectedElement.GetChildren())
                    {
                        if (!_modelElementsSelected.Contains(childEle))
                        {
                            tempSelection.Add(childEle);
                            promptDelParents = true;
                        }
                    }
                }
                else
                {
                    if (selectedElement is ArrowElement ae)
                    {
                        if (ae.StartElement is ToolElement)
                        {
                            if (!_modelElementsSelected.Contains(ae.StartElement) || !_modelElementsSelected.Contains(ae.StopElement))
                            {
                                promptDelParents = true;
                                tempSelection.Add(ae.StartElement);
                                tempSelection.Add(ae.StopElement);
                            }
                        }
                    }
                }
            }

            // We prompt the user to see if they want to delete the parents and children, if they say no we return
            if (promptDelParents)
            {
                if (MessageBox.Show(MessageStrings.ModelerConfirmDeleteSource, MessageStrings.ModelerConfirmDelete, MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

            // Then we delete all the elements
            ClearSelectedElements();

            foreach (ModelElement selectedElement in tempSelection)
            {
                if (selectedElement is ArrowElement arrow && arrow.StopElement is ToolElement toolEle)
                {
                    foreach (Parameter par in toolEle.Tool.InputParameters)
                    {
                        if (arrow.StartElement is DataElement el)
                        {
                            if (par == el.Parameter)
                            {
                                par.DefaultSpecified = false;
                            }
                        }
                    }
                }

                DeleteElement(selectedElement);
            }

            // Loop through all remaining tool elements to make sure they are not trying to reference something thats been deleted
            var modelData = _modelElements.OfType<DataElement>().ToList();

            foreach (ToolElement selectedElement in _modelElements.OfType<ToolElement>())
            {
                foreach (Parameter par in selectedElement.Tool.InputParameters)
                {
                    if (par.ParamVisible == ShowParamInModel.Always || par.ParamVisible == ShowParamInModel.Yes)
                    {
                        Parameter parameter = par;
                        par.DefaultSpecified = modelData.Any(o => o.Parameter == parameter);
                    }
                }

                selectedElement.UpdateStatus();
            }

            // Finaly we clean up any arrows that are hanging around
            foreach (var selectedElement in _modelElements.OfType<ArrowElement>())
            {
                if (!_modelElements.Contains(selectedElement.StartElement) || !_modelElements.Contains(selectedElement.StopElement))
                    DeleteElement(selectedElement);
            }

            Debug.WriteLine("Model elements remaining: " + _modelElements.Count);
        }

        /// <summary>
        /// Executes a model after verifying that it is ready.
        /// </summary>
        /// <param name="error">A string parameter which will contains a error string if one is generated.</param>
        /// <returns>Returns true if it executed successfully.</returns>
        public bool ExecuteModel(out string error)
        {
            // Make sure all the tools are ready for execution
            foreach (ToolElement te in _modelElements.OfType<ToolElement>())
            {
                if (te.ToolStatus == ToolStatus.Error || te.ToolStatus == ToolStatus.Empty)
                {
                    error = te.Name + " is not ready for execution";
                    return false;
                }
            }

            // The number of tools to execute
            _toolToExeCount = _modelElements.OfType<ToolElement>().Count();

            // First we create the progress form
            ToolProgress progForm = new(_toolToExeCount);

            // We create a background worker thread to execute the tool
            BackgroundWorker bw = new();
            bw.DoWork += BwToolThreader;

            object[] threadParamerter = new object[1];
            threadParamerter[0] = progForm;

            // Show the progress dialog and kick off the Async thread
            bw.RunWorkerAsync(threadParamerter);
            progForm.ShowDialog(this);

            error = string.Empty;

            // Make sure all the tools are ready for execution
            foreach (ToolElement te in _modelElements.OfType<ToolElement>())
                te.ExecutionStatus = ToolExecuteStatus.NotRun;

            return true;
        }

        /// <summary>
        /// Creates a default output locations for tools.
        /// </summary>
        /// <param name="par">Parameter used for generating.</param>
        public void GenerateDefaultOutput(Parameter par)
        {
            IFeatureSet addedFeatureSet;
            switch (par.ParamType)
            {
                case "DotSpatial FeatureSet Param":
                    addedFeatureSet = new FeatureSet
                    {
                        Filename = Path.GetTempPath() + Path.DirectorySeparatorChar + par.ModelName + ".shp"
                    };
                    par.Value = addedFeatureSet;
                    break;
                case "DotSpatial LineFeatureSet Param":
                    addedFeatureSet = new LineShapefile
                    {
                        Filename = Path.GetTempPath() + Path.DirectorySeparatorChar + par.ModelName + ".shp"
                    };
                    par.Value = addedFeatureSet;
                    break;
                case "DotSpatial PointFeatureSet Param":
                    addedFeatureSet = new PointShapefile
                    {
                        Filename = Path.GetTempPath() + Path.DirectorySeparatorChar + par.ModelName + ".shp"
                    };
                    par.Value = addedFeatureSet;
                    break;
                case "DotSpatial PolygonFeatureSet Param":
                    addedFeatureSet = new PolygonShapefile
                    {
                        Filename = Path.GetTempPath() + Path.DirectorySeparatorChar + par.ModelName + ".shp"
                    };
                    par.Value = addedFeatureSet;
                    break;
                case "DotSpatial Raster Param":
                    break;
                default:
                    par.GenerateDefaultOutput(Path.GetTempPath());
                    break;
            }
        }

        /// <summary>
        /// Prompts the user to load a model and asks them if they want to save the current model first.
        /// </summary>
        public void LoadModel()
        {
            // Prompt the user to save
            DialogResult dr = MessageBox.Show(this, MessageStrings.ModelSaveCurrentModel.Replace("%S", Path.GetFileNameWithoutExtension(ModelFilename)), MessageStrings.ModelerSaveDialogTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
            if (dr == DialogResult.Cancel)
                return;
            if (dr == DialogResult.Yes)
                SaveModel();

            // Prompts user to pick file
            using OpenFileDialog ofd = new()
            {
                Filter = $@"Model *.{DefaultFileExtension}|*.{DefaultFileExtension}",
                DefaultExt = DefaultFileExtension,
                AddExtension = true,
                CheckPathExists = true
            };
            if (ofd.ShowDialog(this) == DialogResult.Cancel)
                return;

            LoadModel(ofd.FileName);
        }

        /// <summary>
        /// Loads a model from file closing the existing model without saving it.
        /// </summary>
        /// <param name="fileName">Filename of the file whose model should be loaded.</param>
        public void LoadModel(string fileName)
        {
            CreateNewModel(false);

            // Open the file and make sure its a DotSpatialModelFile
            XmlDocument modelXmlDoc = new();
            modelXmlDoc.Load(fileName);
            XmlElement root = modelXmlDoc.DocumentElement;
            if (root == null) return;
            if (root.Name == "DotSpatialModelFile")
            {
                // Find all the tools and add them
                XmlNode toolXml = root.FirstChild;
                while (toolXml != null && toolXml.Name == "Tool")
                {
                    if (toolXml.Attributes != null)
                    {
                        string toolUniqueName = toolXml.Attributes["ToolUniqueName"].Value;
                        string toolModelName = toolXml.Attributes["ModelName"].Value;
                        Point toolLocation = new(Convert.ToInt32(toolXml.Attributes["X"].Value), Convert.ToInt32(toolXml.Attributes["Y"].Value));
                        Version toolVersion = new(toolXml.Attributes["Version"].Value);

                        if (ToolManager.CanCreateTool(toolUniqueName))
                        {
                            ITool newTool = ToolManager.GetTool(toolUniqueName);
                            if (Version.Parse(newTool.Version) < toolVersion)
                            {
                                MessageBox.Show(MessageStrings.Modeler_ToolVersionMismatch);
                            }
                            else
                            {
                                // This code creates the tool and restores its outputs locations and names
                                ToolElement te = AddTool(newTool, toolModelName, toolLocation);
                                XmlNode outputDataXml = toolXml.FirstChild;
                                while (outputDataXml != null && outputDataXml.Name == "OutputData")
                                {
                                    if (outputDataXml.Attributes != null)
                                    {
                                        string dataParameterName = outputDataXml.Attributes["ParameterName"].Value;
                                        string dataModelName = outputDataXml.Attributes["ModelName"].Value;
                                        Point dataLocation = new(Convert.ToInt32(outputDataXml.Attributes["X"].Value), Convert.ToInt32(outputDataXml.Attributes["Y"].Value));
                                        foreach (DataElement de in te.GetChildren().OfType<DataElement>())
                                        {
                                            if (de.Parameter.Name == dataParameterName)
                                            {
                                                de.Name = dataModelName;
                                                de.Location = dataLocation;
                                                break;
                                            }
                                        }
                                    }

                                    outputDataXml = outputDataXml.NextSibling;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show(MessageStrings.Modeler_ToolCouldNotBeCreated);
                        }
                    }

                    toolXml = toolXml.NextSibling;
                }

                // Next we re-join all the input parameters
                toolXml = root.FirstChild;
                while (toolXml != null && toolXml.Name == "Tool")
                {
                    if (toolXml.Attributes != null)
                    {
                        string toolModelName = toolXml.Attributes["ModelName"].Value;
                        XmlNode inputDataXml = toolXml.LastChild;
                        while (inputDataXml != null && inputDataXml.Name == "InputData")
                        {
                            if (inputDataXml.Attributes != null)
                            {
                                string dataModelName = inputDataXml.Attributes["ModelName"].Value;
                                if (_modelElements.Find(o => o.Name == dataModelName) is DataElement de && _modelElements.Find(o => o.Name == toolModelName) is ToolElement te)
                                {
                                    foreach (Parameter parameter in te.Tool.InputParameters)
                                    {
                                        if (!parameter.DefaultSpecified && parameter.ParamType == de.Parameter.ParamType)
                                        {
                                            AddArrow(de, te);
                                            parameter.Value = de.Parameter.Value;
                                            parameter.ModelName = de.Parameter.ModelName;
                                            te.UpdateStatus();
                                            break;
                                        }
                                    }
                                }
                            }

                            inputDataXml = inputDataXml.PreviousSibling;
                        }
                    }

                    toolXml = toolXml.NextSibling;
                }

                // This updates all the arrows so that they properly link to their output ends (since they might have moved since being created)
                foreach (ArrowElement ae in _modelElements.OfType<ArrowElement>())
                    ae.UpdateDimensions();

                // Zooms to the full extent of the model
                ZoomFullExtent();
            }
            else
            {
                MessageBox.Show("The file selected DotSpatial Model File");
            }
        }

        /// <summary>
        /// Saves the current project, prompting the user to save as if the project is new.
        /// </summary>
        public void SaveModel()
        {
            if (_newModel)
                SaveModel(true, true);
            else
                SaveModel(false, false);
        }

        /// <summary>
        /// Saves the current project to disk.
        /// </summary>
        /// <param name="promptOverwrite">True to prompt for overwrite if file exists.</param>
        /// <param name="promptSaveAs">True show save as dialog.</param>
        public void SaveModel(bool promptOverwrite, bool promptSaveAs)
        {
            string fileName = ModelFilename;
            if (promptSaveAs)
            {
                using var sfd = new SaveFileDialog
                {
                    OverwritePrompt = promptOverwrite,
                    Filter = $@"Model *.{DefaultFileExtension}|*.{DefaultFileExtension}",
                    DefaultExt = DefaultFileExtension,
                    AddExtension = true,
                    CheckPathExists = true,
                    FileName = ModelFilename
                };
                if (sfd.ShowDialog(this) == DialogResult.Cancel)
                    return;
                fileName = sfd.FileName;
            }

            if (SaveModel(fileName))
                ModelFilename = fileName;
        }

        /// <summary>
        /// Saves a copy of the model to the selected file without renaming the current project.
        /// </summary>
        /// <param name="fileName">File to save to.</param>
        /// <returns>True.</returns>
        public bool SaveModel(string fileName)
        {
            // Creates the model xml document
            XmlDocument modelXmlDoc = new();
            XmlElement root = modelXmlDoc.CreateElement("DotSpatialModelFile");
            root.SetAttribute("Version", "1.0");
            modelXmlDoc.AppendChild(root);

            // Saves the Tools and their output configuration to the model
            foreach (var te in _modelElements.OfType<ToolElement>())
            {
                XmlElement tool = modelXmlDoc.CreateElement("Tool");
                tool.SetAttribute("ToolUniqueName", te.Tool.AssemblyQualifiedName);
                tool.SetAttribute("ModelName", te.Name);
                tool.SetAttribute("Version", te.Tool.Version);
                tool.SetAttribute("X", te.Location.X.ToString());
                tool.SetAttribute("Y", te.Location.Y.ToString());

                foreach (DataElement de in te.GetChildren().OfType<DataElement>())
                {
                    XmlElement outputData = modelXmlDoc.CreateElement("OutputData");
                    outputData.SetAttribute("ParameterName", de.Parameter.Name);
                    outputData.SetAttribute("ModelName", de.Name);
                    outputData.SetAttribute("X", de.Location.X.ToString());
                    outputData.SetAttribute("Y", de.Location.Y.ToString());
                    tool.AppendChild(outputData);
                }

                foreach (DataElement de in te.GetParents().OfType<DataElement>())
                {
                    XmlElement inputData = modelXmlDoc.CreateElement("InputData");
                    inputData.SetAttribute("ParameterName", de.Parameter.Name);
                    inputData.SetAttribute("ModelName", de.Name);
                    inputData.SetAttribute("X", de.Location.X.ToString());
                    inputData.SetAttribute("Y", de.Location.Y.ToString());
                    tool.AppendChild(inputData);
                }

                foreach (Parameter inputParam in te.Tool.InputParameters)
                {
                    if (inputParam.ParamVisible == ShowParamInModel.No)
                    {
                        XmlElement inputData = modelXmlDoc.CreateElement("InputData");
                        inputData.SetAttribute("ParameterName", inputParam.Name);
                    }
                }

                root.AppendChild(tool);
            }

            modelXmlDoc.Save(fileName);
            return true;
        }

        /// <summary>
        /// Zooms to the extent of all elements in the model and centers the modeler on them.
        /// </summary>
        public void ZoomFullExtent()
        {
            // If there are no elements bring the modeler back to the origin with zoom = 1
            if (_modelElements.Count < 1)
            {
                _virtualOffset = new Point(0, 0);
                _virtualZoom = 1F;
                IsInitialized = false;
                return;
            }

            // The top left and right virtual points of the model
            Point virtTl = new(_modelElements[0].Location.X, _modelElements[0].Location.Y);
            Point virtBr = new(_modelElements[0].Location.X + _modelElements[0].Width, _modelElements[0].Location.Y + _modelElements[0].Height);

            // Calculates the full virtual extent
            for (int i = 1; i < _modelElements.Count; i++)
            {
                if (virtTl.X > _modelElements[i].Location.X)
                    virtTl.X = _modelElements[i].Location.X;
                if (virtTl.Y > _modelElements[i].Location.Y)
                    virtTl.Y = _modelElements[i].Location.Y;
                if (virtBr.X < (_modelElements[i].Location.X + _modelElements[i].Width))
                    virtBr.X = _modelElements[i].Location.X + _modelElements[i].Width;
                if (virtBr.Y < (_modelElements[i].Location.Y + _modelElements[i].Height))
                    virtBr.Y = _modelElements[i].Location.Y + _modelElements[i].Height;
            }

            // Calculates the new zoom level
            double virtWidth = Math.Abs(virtTl.X - virtBr.X);
            double virtHeight = Math.Abs(virtTl.Y - virtBr.Y);
            ZoomFactor = ((Width - 100) / virtWidth) < ((Height - 100) / virtHeight) ? Convert.ToSingle((Width - 100) / virtWidth) : Convert.ToSingle((Height - 100) / virtHeight);

            // Centers the modeler on the model elements
            Point centerPoint = new(virtTl.X + ((virtBr.X - virtTl.X) / 2), virtTl.Y + ((virtBr.Y - virtTl.Y) / 2));
            CenterModelerOnPoint(centerPoint);

            IsInitialized = false;
        }

        /// <summary>
        /// Zooms the model in 20%.
        /// </summary>
        public void ZoomIn()
        {
            Point virtBr = PixelToVirtual(Width, Height);
            Point virtTl = PixelToVirtual(0, 0);
            Point oldVirtCenterPoint = new(virtTl.X + ((virtBr.X - virtTl.X) / 2), virtTl.Y + ((virtBr.Y - virtTl.Y) / 2));

            // Debuging code
            Debug.WriteLine("Window size: X=" + Width + "Y=" + Height);
            Debug.WriteLine("Old Virtyual Center: X=" + oldVirtCenterPoint.X + "Y=" + oldVirtCenterPoint.Y);

            ZoomFactor += .2F * ZoomFactor;

            CenterModelerOnPoint(oldVirtCenterPoint);

            IsInitialized = false;
        }

        /// <summary>
        /// Zooms the model out 20%.
        /// </summary>
        public void ZoomOut()
        {
            Point virtBr = PixelToVirtual(Width, Height);
            Point virtTl = PixelToVirtual(0, 0);
            Point oldVirtCenterPoint = new(virtTl.X + ((virtBr.X - virtTl.X) / 2), virtTl.Y + ((virtBr.Y - virtTl.Y) / 2));

            // Debuging code
            Debug.WriteLine("Window size: X=" + Width + "Y=" + Height);
            Debug.WriteLine("Old Virtyual Center: X=" + oldVirtCenterPoint.X + "Y=" + oldVirtCenterPoint.Y);

            ZoomFactor -= .23F * ZoomFactor;

            CenterModelerOnPoint(oldVirtCenterPoint);

            IsInitialized = false;
        }

        /// <summary>
        /// When the user double clicks on the model this event fires.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);


            // If the user left clicked on a selected element we do nothing
            if (e is not MouseEventArgs mouseE) return;
            Point virPt = PixelToVirtual(mouseE.X, mouseE.Y);
            foreach (ModelElement me in _modelElementsSelected)
            {
                // Point pt = new Point(virPt.X - me.Location.X, virPt.Y - me.Location.Y);
                if (me.PointInElement(virPt))
                {
                    // We create a new instance if its a toolelement
                    if (me is ToolElement meAsTool)
                    {
                        // All of the parameters are copied from the original one
                        ITool newToolCopy = ToolManager.GetTool(meAsTool.Tool.AssemblyQualifiedName);
                        for (int i = 0; i < meAsTool.Tool.InputParameters.Length; i++)
                            if (newToolCopy.InputParameters[i] != null) newToolCopy.InputParameters[i] = meAsTool.Tool.InputParameters[i].Copy();
                        for (int i = 0; i < meAsTool.Tool.OutputParameters.Length; i++)
                            if (newToolCopy.OutputParameters[i] != null) newToolCopy.OutputParameters[i] = meAsTool.Tool.OutputParameters[i].Copy();

                        // This code ensures that no children are passed into the tool
                        List<ModelElement> tempList = new();
                        foreach (ModelElement mEle in _modelElements)
                        {
                            if (mEle.IsDownstreamOf(meAsTool)) continue;
                            tempList.Add(mEle);
                        }

                        ToolElement tempTool = new(newToolCopy, tempList);

                        // If the user hits ok we dispose of the saved copy if they don't we restore the old values
                        if (tempTool.DoubleClick())
                        {
                            for (int i = 0; i < meAsTool.Tool.InputParameters.Length; i++)
                                if (meAsTool.Tool.InputParameters[i] != null) meAsTool.Tool.InputParameters[i] = tempTool.Tool.InputParameters[i];
                            for (int i = 0; i < meAsTool.Tool.OutputParameters.Length; i++)
                            {
                                if (tempTool.Tool.OutputParameters[i] != null)
                                {
                                    if (tempTool.Tool.OutputParameters[i].DefaultSpecified)
                                    {
                                        meAsTool.Tool.OutputParameters[i].ModelName = tempTool.Tool.OutputParameters[i].ModelName;
                                        meAsTool.Tool.OutputParameters[i].Value = tempTool.Tool.OutputParameters[i].Value;
                                    }
                                }
                            }

                            // First we clear all the arrows linking to the current tool (We use a temporary array since we will be removing items from the list)
                            ModelElement[] tempArray = new ModelElement[_modelElements.Count];
                            _modelElements.CopyTo(tempArray);
                            foreach (ModelElement mele in tempArray)
                            {
                                if (mele is ArrowElement ae)
                                {
                                    if (ae.StopElement == meAsTool)
                                        DeleteElement(ae);
                                }
                            }

                            // We go through each parameters of the tool to the model if they haven't been added already
                            int j = 0;
                            foreach (Parameter par in meAsTool.Tool.InputParameters)
                            {
                                if (par == null) continue;

                                // If its not supposed to be visible we ignore it.
                                if (par.ParamVisible == ShowParamInModel.No)
                                    continue;

                                // If no default has been specified continue
                                if (par.DefaultSpecified == false)
                                    continue;

                                // If the parameter has not been assigned a model name we add the data to the model
                                bool addParam = true;
                                foreach (ModelElement modElem in _modelElements)
                                {
                                    if (modElem is not DataElement dataElem) continue;
                                    if (dataElem.Parameter.Value == par.Value)
                                    {
                                        addParam = false;
                                        break;
                                    }
                                }

                                if (addParam)
                                {
                                    Point newLocation = new(meAsTool.Location.X - Convert.ToInt32(meAsTool.Width * 1.1), meAsTool.Location.Y + Convert.ToInt32(j * meAsTool.Height * 1.1));
                                    AddArrow(AddData(par, newLocation), meAsTool);
                                    j++;
                                }
                                else
                                {
                                    // If the parameter has already been added we make sure that we have linked to it properly
                                    tempArray = new ModelElement[_modelElements.Count];
                                    _modelElements.CopyTo(tempArray);
                                    foreach (ModelElement mele in tempArray)
                                    {
                                        if (mele is DataElement de)
                                        {
                                            if (par.ModelName == de.Parameter.ModelName)
                                            {
                                                AddArrow(mele, meAsTool);
                                            }
                                        }
                                    }
                                }
                            }

                            // This updates the status light
                            meAsTool.UpdateStatus();
                        }
                    }
                    else
                    {
                        me.DoubleClick();
                    }

                    Invalidate();
                    return;
                }
            }
        }

        /// <summary>
        /// Adds a tool to the modeler.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);
            if (!e.Data.GetDataPresent(DataFormats.StringFormat)) return;
            if (e.Data.GetData(DataFormats.StringFormat) is not string fromDrag) return;
            if (fromDrag.StartsWith("ITool: "))
            {
                fromDrag = fromDrag.Replace("ITool: ", string.Empty);
                if (ToolManager.CanCreateTool(fromDrag))
                {
                    ITool tool = ToolManager.GetTool(fromDrag);
                    AddTool(tool, PixelToVirtual(PointToClient(MousePosition)));
                }
            }
        }

        /// <summary>
        /// Allows a drag into the modeler.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                if (e.Data.GetData(DataFormats.StringFormat) is string fromDrag)
                {
                    if (fromDrag.StartsWith("ITool: "))
                    {
                        fromDrag = fromDrag.Replace("ITool: ", string.Empty);
                        if (ToolManager.CanCreateTool(fromDrag))
                        {
                            e.Effect = DragDropEffects.Copy;
                            return;
                        }
                    }
                }
            }

            e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// On Key Up.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            Debug.WriteLine("Key Pressed: " + e.KeyCode);
            base.OnKeyUp(e);
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    DeleteSelectedElements();
                    break;
            }
        }

        /// <summary>
        /// Fires when the Filename of the project is changed.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnModelFilenameChanged(EventArgs e)
        {
            ModelFilenameChanged?.Invoke(this, e);
        }

        /// <summary>
        /// When the users clicks the mouse this event fires.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _mouseDownOnElement = false;
            _mouseMoved = false;
            Point virPt = PixelToVirtual(e.Location);
            _mouseOldPoint = MousePosition;

            // Deals with left clicks when the model is in link mode
            if (e.Button == MouseButtons.Left && _enableLinking)
            {
                // figure out if the user click on a element
                foreach (ModelElement me in _modelElements)
                {
                    if (me.PointInElement(virPt))
                    {
                        if (me is DataElement de)
                        {
                            BlankElement linkDestination = new(_modelElements);
                            AddElement(linkDestination, virPt);
                            _linkArrow = AddArrow(de, linkDestination);
                            AddSelectedElement(linkDestination);
                            return;
                        }
                    }
                }

                _linkArrow = null;
                return;
            }

            // Deals with left clicks when the model is not in link mode
            if (e.Button == MouseButtons.Left)
            {
                // If the user left clicked on a selected element we do nothing
                foreach (ModelElement me in _modelElementsSelected)
                {
                    // Point pt = new Point(virPt.X - me.Location.X, virPt.Y - me.Location.Y);
                    if (me.PointInElement(virPt))
                    {
                        if (ModifierKeys == Keys.Control)
                        {
                            RemoveSelectedElement(me);
                            _mouseDownOnElement = false;
                        }

                        _mouseDownOnElement = true;
                        return;
                    }
                }

                // If the user left clicked on a unselected element we clear the selected list and highlight the new element
                foreach (ModelElement me in _modelElements)
                {
                    if (_modelElementsSelected.Contains(me))
                        continue;

                    // Point pt = new Point(virPt.X - me.Location.X, virPt.Y - me.Location.Y);
                    if (me.PointInElement(virPt))
                    {
                        if (ModifierKeys != Keys.Control)
                        {
                            ClearSelectedElements();
                        }

                        _mouseDownOnElement = true;
                        AddSelectedElement(me);
                        break;
                    }
                }

                // If the mouse is clicked on a white area we draw a box
                if (_mouseDownOnElement == false)
                {
                    ClearSelectedElements();
                    _selectBoxDraw = true;
                    _selectBoxPt = e.Location;
                    _selectBox = new Rectangle(virPt.X, virPt.Y, 0, 0);
                }
            }
        }

        /// <summary>
        /// Occurs when the mouse leaves.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            Debug.WriteLine("mouse left");
        }

        /// <summary>
        /// When the mouse is moved this event fires.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_mouseMoveDone == false)
                return;
            _mouseMoveDone = false;

            // Makes sure the mouse actually moved before we do anything
            if ((MousePosition.X != _mouseOldPoint.X) || (MousePosition.Y != _mouseOldPoint.Y))
            {
                _mouseMoved = true;
                Point virtualOld = PixelToVirtual(_mouseOldPoint);
                Point virtualNew = PixelToVirtual(MousePosition);
                Point virtualDelta = new(virtualNew.X - virtualOld.X, virtualNew.Y - virtualOld.Y);
                int inflateFactor = Convert.ToInt32(5 * _virtualZoom);
                if (inflateFactor < 5) inflateFactor = 5;

                // If were in link mode draw a update link arrow
                if (_linkArrow != null)
                {
                    // This code invalidates the elements OLD location
                    Point pt1 = VirtualToPixel(_linkArrow.Location.X + _linkArrow.StartPoint.X, _linkArrow.Location.Y + _linkArrow.StartPoint.Y);
                    Point pt2 = VirtualToPixel(_linkArrow.Location.X + _linkArrow.StopPoint.X, _linkArrow.Location.Y + _linkArrow.StopPoint.Y);
                    Rectangle invalid = new(Math.Max(0, Math.Min(Math.Min(pt1.X, pt2.X) - 5, Width)), Math.Max(0, Math.Min(Math.Min(pt1.Y, pt2.Y) - 5, Height)), Math.Max(pt1.X, pt2.X) + 10, Math.Max(pt1.Y, pt2.Y) + 10);
                    invalid.Inflate(inflateFactor, inflateFactor);
                    Invalidate(invalid);

                    // Sets the new dimentions
                    _linkArrow.StopElement.Location = PixelToVirtual(e.Location);
                    _linkArrow.UpdateDimensions();

                    // This code invalidates the elements NEW location
                    pt1 = VirtualToPixel(_linkArrow.Location.X + _linkArrow.StartPoint.X, _linkArrow.Location.Y + _linkArrow.StartPoint.Y);
                    pt2 = VirtualToPixel(_linkArrow.Location.X + _linkArrow.StopPoint.X, _linkArrow.Location.Y + _linkArrow.StopPoint.Y);
                    invalid = new Rectangle(Math.Max(0, Math.Min(Math.Min(pt1.X, pt2.X) - 5, Width)), Math.Max(0, Math.Min(Math.Min(pt1.Y, pt2.Y) - 5, Height)), Math.Max(pt1.X, pt2.X) + 10, Math.Max(pt1.Y, pt2.Y) + 10);
                    invalid.Inflate(inflateFactor, inflateFactor);
                    Invalidate(invalid);

                    Application.DoEvents();
                    _mouseOldPoint = MousePosition;
                }

                // If the mouse was clicked and held on a item this code fires
                if (_mouseDownOnElement)
                {
                    if ((Math.Abs(virtualDelta.X) >= 1) || (Math.Abs(virtualDelta.Y) >= 1))
                    {
                        // This moves elements that are not arrows
                        foreach (ModelElement me in _modelElementsSelected)
                        {
                            if ((me as ArrowElement) == null)
                            {
                                // This code invalidates the elements OLD location
                                Point pt1 = VirtualToPixel(me.Location);
                                Point pt2 = VirtualToPixel(me.Location.X + me.Width, me.Location.Y + me.Height);
                                Rectangle invalid = new(Math.Max(0, Math.Min(Math.Min(pt1.X, pt2.X) - 5, Width)), Math.Max(0, Math.Min(Math.Min(pt1.Y, pt2.Y) - 5, Height)), Math.Max(pt1.X, pt2.X) + 10, Math.Max(pt1.Y, pt2.Y) + 10);
                                invalid.Inflate(inflateFactor, inflateFactor);
                                Invalidate(invalid);

                                // Updates the elements location
                                me.Location = new Point(me.Location.X + virtualDelta.X, me.Location.Y + virtualDelta.Y);

                                // This code invalidates the elements NEW location
                                pt1 = VirtualToPixel(me.Location);
                                pt2 = VirtualToPixel(me.Location.X + me.Width, me.Location.Y + me.Height);
                                invalid = new Rectangle(Math.Max(0, Math.Min(Math.Min(pt1.X, pt2.X) - 5, Width)), Math.Max(0, Math.Min(Math.Min(pt1.Y, pt2.Y) - 5, Height)), Math.Max(pt1.X, pt2.X) + 10, Math.Max(pt1.Y, pt2.Y) + 10);
                                invalid.Inflate(inflateFactor, inflateFactor);
                                Invalidate(invalid);
                            }
                        }

                        // This moves all the arrow elements second so they don't drag
                        foreach (ModelElement me in _modelElementsSelected)
                        {
                            if ((me as ArrowElement) != null)
                            {
                                ArrowElement ar = me as ArrowElement;

                                // This code invalidates the elements OLD location
                                Point pt1 = VirtualToPixel(ar.Location.X + ar.StartPoint.X, ar.Location.Y + ar.StartPoint.Y);
                                Point pt2 = VirtualToPixel(ar.Location.X + ar.StopPoint.X, ar.Location.Y + ar.StopPoint.Y);

                                // Updates the elements location
                                ar.UpdateDimensions();

                                Rectangle invalid = new(Math.Max(0, Math.Min(Math.Min(pt1.X, pt2.X) - 5, Width)), Math.Max(0, Math.Min(Math.Min(pt1.Y, pt2.Y) - 5, Height)), Math.Max(pt1.X, pt2.X) + 10, Math.Max(pt1.Y, pt2.Y) + 10);
                                invalid.Inflate(inflateFactor, inflateFactor);
                                Invalidate(invalid);

                                // This code invalidates the elements NEW location
                                pt1 = VirtualToPixel(ar.Location.X + ar.StartPoint.X, ar.Location.Y + ar.StartPoint.Y);
                                pt2 = VirtualToPixel(ar.Location.X + ar.StopPoint.X, ar.Location.Y + ar.StopPoint.Y);
                                invalid = new Rectangle(Math.Max(0, Math.Min(Math.Min(pt1.X, pt2.X) - 5, Width)), Math.Max(0, Math.Min(Math.Min(pt1.Y, pt2.Y) - 5, Height)), Math.Max(pt1.X, pt2.X) + 10, Math.Max(pt1.Y, pt2.Y) + 10);
                                invalid.Inflate(inflateFactor, inflateFactor);
                                Invalidate(invalid);
                            }
                        }

                        _mouseOldPoint = MousePosition;
                    }

                    Application.DoEvents();
                }

                // If the mouse was clicked outside of an element
                if (_selectBoxDraw)
                {
                    Invalidate(_selectBox);
                    _selectBox.Width = Math.Abs(_selectBoxPt.X - e.X);
                    _selectBox.Height = Math.Abs(_selectBoxPt.Y - e.Y);
                    _selectBox.X = Math.Min(_selectBoxPt.X, e.X);
                    _selectBox.Y = Math.Min(_selectBoxPt.Y, e.Y);
                    Invalidate(_selectBox);
                }
            }

            _mouseMoveDone = true;
        }

        /// <summary>
        /// When the user mouses up after a single click this event fires.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Point virPt = PixelToVirtual(e.Location);

            // If we are in link mode we run this
            if (_linkArrow != null)
            {
                ClearSelectedElements();
                ModelElement[] tempElements = new ModelElement[_modelElements.Count];
                _modelElements.CopyTo(tempElements);
                foreach (ModelElement me in tempElements)
                {
                    if (me.PointInElement(virPt) && me != _linkArrow.StartElement)
                    {
                        if (me is ToolElement te)
                        {
                            // If the user let go over a tool we try to link to to, assuming it doesn't create a loop
                            if (_linkArrow.StartElement != null)
                            {
                                if (_linkArrow.StartElement.IsDownstreamOf(te))
                                {
                                    // If the tool is the data sets parent
                                    MessageBox.Show(MessageStrings.linkErrorCircle, MessageStrings.linkError, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;
                                }
                            }

                            bool showError = true;
                            foreach (Parameter t in te.Tool.InputParameters)
                            {
                                if (_linkArrow.StartElement is DataElement linkStart)
                                {
                                    if (t.DefaultSpecified == false && t.ParamType == linkStart.Parameter.ParamType)
                                    {
                                        AddArrow(linkStart, te);
                                        t.Value = linkStart.Parameter.Value;
                                        t.ModelName = linkStart.Parameter.ModelName;
                                        showError = false;
                                        te.UpdateStatus();
                                        break;
                                    }
                                }
                            }

                            if (showError) MessageBox.Show(MessageStrings.linkNoFreeInput, MessageStrings.linkError, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }

                        MessageBox.Show(MessageStrings.linkErrorToData, MessageStrings.linkError, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }
                }

                DeleteElement(_linkArrow.StopElement);
                DeleteElement(_linkArrow);
                _linkArrow = null;
                IsInitialized = false;
                Invalidate();
                Application.DoEvents();
                return;
            }

            // If we detect the user clicked on a element and didn't move the mouse we select that element and clear the others
            if (_mouseMoved == false && _mouseDownOnElement)
            {
                if (ModifierKeys != Keys.Control)
                {
                    ClearSelectedElements();
                    foreach (ModelElement me in _modelElements)
                    {
                        if (_modelElementsSelected.Contains(me))
                            continue;
                        if (me.PointInElement(virPt))
                        {
                            if (ModifierKeys == Keys.Control)
                            {
                                RemoveSelectedElement(me);
                                _mouseDownOnElement = false;
                            }

                            AddSelectedElement(me);
                            _mouseDownOnElement = false;
                            return;
                        }
                    }
                }
            }

            // When the user lets go of the select box we find out which elements it selected
            if (_selectBoxDraw && _mouseMoved)
            {
                ClearSelectedElements();
                List<ModelElement> elementBox = new();
                foreach (ModelElement me in _modelElements)
                {
                    if (_modelElementsSelected.Contains(me))
                        continue;
                    if (me.ElementInRectangle(PixelRectToVirtualRect(_selectBox)))
                    {
                        elementBox.Add(me);
                    }
                }

                for (int i = elementBox.Count - 1; i >= 0; i--)
                {
                    AddSelectedElement(elementBox[i]);
                }
            }

            // After a mouse up we reset the mouse variables
            _selectBoxDraw = false;
            Invalidate(_selectBox);
            _mouseDownOnElement = false;
            _mouseMoved = false;
        }

        /// <summary>
        /// When the element is called on to be painted this method is called.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap tempBuffer = new(Width, Height);
            Graphics tempGraph = Graphics.FromImage(tempBuffer);
            tempGraph.SmoothingMode = DrawingQuality;
            Rectangle inflatedInvalidationRect = Rectangle.Inflate(e.ClipRectangle, 5, 5);

            if (IsInitialized)
            {
                tempGraph.DrawImage(_backBuffer, inflatedInvalidationRect, inflatedInvalidationRect, GraphicsUnit.Pixel);
            }
            else
            {
                UpdateBackBuffer();
                tempGraph.DrawImage(_backBuffer, new Point(0, 0));
            }

            // Draws selected shapes last
            if (_modelElementsSelected.Count > 0)
            {
                for (int i = _modelElementsSelected.Count - 1; i >= 0; i--)
                {
                    ModelElement me = _modelElementsSelected[i];
                    Matrix m = new();
                    Point translator = VirtualToPixel(me.Location.X, me.Location.Y);
                    m.Translate(translator.X, translator.Y);
                    m.Scale(_virtualZoom, _virtualZoom);
                    tempGraph.Transform = m;
                    me.Paint(tempGraph);
                    tempGraph.Transform = new Matrix();
                }
            }

            // If the users is dragging a select box we draw it here
            if (_selectBoxDraw)
            {
                SolidBrush highlightBrush = new(Color.FromArgb(30, SystemColors.Highlight));
                tempGraph.FillRectangle(highlightBrush, Rectangle.Inflate(_selectBox, -1, -1));
                Rectangle outlineRect = new(_selectBox.X, _selectBox.Y, _selectBox.Width - 1, _selectBox.Height - 1);
                tempGraph.DrawRectangle(SystemPens.Highlight, outlineRect);

                // garbage collection
                highlightBrush.Dispose();
            }

            // Draws the temporary bitmap to the screen
            e.Graphics.SmoothingMode = DrawingQuality;
            e.Graphics.DrawImage(tempBuffer, inflatedInvalidationRect, inflatedInvalidationRect, GraphicsUnit.Pixel);

            // Garbage collection
            tempBuffer.Dispose();
            tempGraph.Dispose();
        }

        /// <summary>
        /// When the form draws the background do nothing.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        /// <summary>
        /// Redraws the background when the form is resized.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnResize(EventArgs e)
        {
            IsInitialized = false;
            base.OnResize(e);
        }

        /// <summary>
        /// Adds an arrow element given a source and destination element.
        /// </summary>
        /// <param name="sourceElement">The source element.</param>
        /// <param name="destElement">The destination element.</param>
        /// <returns>The arrow element that was added.</returns>
        private ArrowElement AddArrow(ModelElement sourceElement, ModelElement destElement)
        {
            ArrowElement ae = new(sourceElement, destElement, _modelElements)
            {
                Color = _arrowColor,
                Name = sourceElement.Name + "_" + destElement.Name
            };
            AddElement(ae, ae.Location);
            return ae;
        }

        /// <summary>
        /// Adds a data element to the modeler based on a parameter descrition.
        /// </summary>
        /// <param name="par">The parameter to add to the modeler.</param>
        /// <param name="location">A point representing the virtual location of the element.</param>
        /// <returns>The data element, that was added to the modeler.</returns>
        private DataElement AddData(Parameter par, Point location)
        {
            return AddData(par, location, par.Name);
        }

        /// <summary>
        /// Adds a data element to the modeler.
        /// </summary>
        /// <param name="par">The data set to add to the modeler.</param>
        /// <param name="location">A point representing the virtual location of the data element.</param>
        /// <param name="name">The name to give the element.</param>
        /// <returns>The data element, that was added to the modeler.</returns>
        private DataElement AddData(Parameter par, Point location, string name)
        {
            DataElement de = new(par, _modelElements)
            {
                Color = DataColor,
                Font = DataFont,
                Shape = DataShape,
                Name = name
            };
            de.Name = string.IsNullOrEmpty(par.ModelName) ? par.Name : par.ModelName;
            AddElement(de, location);
            par.ModelName = de.Name;
            return de;
        }

        /// <summary>
        /// Adds a element to the model.
        /// </summary>
        /// <param name="element">The new model Element to add to the model form.</param>
        /// <param name="location">A point representing the virtual location of the element.</param>
        private void AddElement(ModelElement element, Point location)
        {
            List<string> elementNames = new();
            foreach (ModelElement mEle in _modelElements)
                elementNames.Add(mEle.Name);

            string tempName = element.Name;
            int i = 1;
            while (elementNames.Contains(tempName))
            {
                tempName = element.Name + "_" + i;
                i++;
            }

            element.Name = tempName;
            _modelElements.Add(element);
            element.Location = location;
            IsInitialized = false;
        }

        /// <summary>
        /// Adds an element to the _modelElementsSelected Array and highlights it.
        /// </summary>
        /// <param name="element">The element to add to the selected elements.</param>
        private void AddSelectedElement(ModelElement element)
        {
            _modelElementsSelected.Insert(0, element);
            element.Highlighted(true);

            // We check if the element has any arrows connected to it and if it does we select them too
            foreach (ArrowElement ae in _modelElements.OfType<ArrowElement>())
            {
                if (ae.StartElement == element || ae.StopElement == element)
                {
                    _modelElementsSelected.Add(ae);
                    ae.Highlighted(true);
                }
            }

            IsInitialized = false;
        }

        /// <summary>
        /// Adds a tool to the Modeler.
        /// </summary>
        /// <param name="tool">the tool to add to the modeler.</param>
        /// <param name="location">A point representing the virtual location of the tool.</param>
        /// <returns>The tool element that was added.</returns>
        private ToolElement AddTool(ITool tool, Point location)
        {
            return AddTool(tool, tool.Name, location);
        }

        /// <summary>
        /// Adds a tool to the Modeler.
        /// </summary>
        /// <param name="tool">the tool to add to the modeler.</param>
        /// <param name="modelName">The name to give the element.</param>
        /// <param name="location">A point representing the virtual location of the tool.</param>
        /// <returns>The tool element that was added.</returns>
        private ToolElement AddTool(ITool tool, string modelName, Point location)
        {
            if (tool == null)
                return null;

            ToolElement te = new(tool, _modelElements)
            {
                Font = _toolFont,
                Color = _toolColor,
                Shape = _toolShape,
                Name = modelName
            };
            AddElement(te, location);
            int j = 0;
            foreach (Parameter par in te.Tool.OutputParameters)
            {
                Point newLocation = new(te.Location.X + Convert.ToInt32(te.Width * 1.1), te.Location.Y + Convert.ToInt32(j * te.Height * 1.1));
                DataElement de = AddData(par, newLocation);
                par.GenerateDefaultOutput(Path.GetDirectoryName(ModelFilename));
                AddArrow(te, de);
                j++;
            }

            return te;
        }

        private void BwToolThreader(object sender, DoWorkEventArgs e)
        {
            if (e.Argument is not object[] threadParamerter) return;
            ToolProgress progForm = threadParamerter[0] as ToolProgress;

            // Loops through all the tools and executes them
            int i = 0;
            while (i < _toolToExeCount)
            {
                foreach (ToolElement te in _modelElements.OfType<ToolElement>())
                {
                    if (te.ExecutionStatus != ToolExecuteStatus.NotRun)
                        continue;

                    bool ready = true;
                    foreach (ModelElement me in te.GetParents())
                    {
                        List<ModelElement> parentElements = me.GetParents();
                        if (parentElements.Count > 0 && ((ToolElement)parentElements[0]).ExecutionStatus != ToolExecuteStatus.Done)
                        {
                            ready = false;
                            break;
                        }
                    }

                    if (ready)
                    {
                        if (progForm != null)
                        {
                            progForm.Progress(0, "==================");
                            progForm.Progress(0, "Executing Tool: " + te.Name);
                            progForm.Progress(0, "==================");
                            te.Tool.Execute(progForm);
                            progForm.Progress(100, "==================");
                            progForm.Progress(100, "Done Executing Tool: " + te.Name);
                            progForm.Progress(100, "==================");
                        }

                        te.ExecutionStatus = ToolExecuteStatus.Done;
                        i++;
                        break;
                    }
                }
            }

            progForm?.ExecutionComplete();
        }

        /// <summary>
        /// Clears all the elements in the _selectedElements list, removes their highlights and puts them back in the _modelElements list.
        /// </summary>
        private void ClearSelectedElements()
        {
            foreach (ModelElement me in _modelElementsSelected)
            {
                me.Highlighted(false);
            }

            _modelElementsSelected.Clear();
            IsInitialized = false;
        }

        /// <summary>
        /// Removes a specific element from the model clearing any links it has to other elements.
        /// </summary>
        /// <param name="element">The element to remove.</param>
        private void DeleteElement(ModelElement element)
        {
            if (element is ToolElement te)
            {
                if (_modelElements.Contains(te))
                {
                    _modelElements.Remove(te);
                }

                return;
            }

            if (element is DataElement de)
            {
                if (_modelElements.Contains(de))
                {
                    _modelElements.Remove(de);
                }

                return;
            }

            if (element is ArrowElement ae)
            {
                if (_modelElements.Contains(ae))
                {
                    _modelElements.Remove(ae);
                }

                return;
            }

            if (element is BlankElement be)
            {
                if (_modelElements.Contains(be))
                {
                    _modelElements.Remove(be);
                }
            }
        }

        private void HorScrollValueChanged(object sender, EventArgs e)
        {
            _virtualOffset.X -= _horScroll.Value - _horLastValue;
            _horLastValue = _horScroll.Value;
            if ((_horScroll.Value <= _horScroll.Minimum) && (_horScroll.Minimum - 50 > (int.MinValue + 50)) && !_scrollLock)
            {
                _scrollLock = true;
                _scrollTimerHorizontal.Start();
                _horScroll.Minimum -= 50;
            }

            if ((_horScroll.Value + _horScroll.LargeChange >= _horScroll.Maximum) && (_horScroll.Maximum + 50 < (int.MaxValue - 50)) && !_scrollLock)
            {
                _scrollLock = true;
                _scrollTimerHorizontal.Start();
                _horScroll.Maximum += 50;
            }

            IsInitialized = false;
            Invalidate();
        }

        /// <summary>
        /// Returns a rectangle in virtual coordinantes based on a pixel rectangle.
        /// </summary>
        /// <param name="rect">Pixel rectangle.</param>
        /// <returns>Rectangle in virtual coordinates.</returns>
        private Rectangle PixelRectToVirtualRect(Rectangle rect)
        {
            Point tl = PixelToVirtual(rect.Location);
            Point br = PixelToVirtual(rect.Location.X + rect.Width, rect.Location.Y + rect.Height);
            return new Rectangle(tl.X, tl.Y, br.X - tl.X, br.Y - tl.Y);
        }

        /// <summary>
        /// Translates between pixel location and virtual location.
        /// </summary>
        /// <param name="pt">A point in pixel coordinantes. 0, 0 is the top left of the modeler.</param>
        /// <returns>A point in the virtual model coordinantes.</returns>
        private Point PixelToVirtual(Point pt)
        {
            return PixelToVirtual(pt.X, pt.Y);
        }

        /// <summary>
        /// Translates between pixel location and virtual location.
        /// </summary>
        /// <param name="x">the X location in pixel coordinantes.</param>
        /// <param name="y">the Y location in pixel coordinantes.</param>
        /// <returns>A point in the virtual model coordinantes.</returns>
        private Point PixelToVirtual(int x, int y)
        {
            int ptX = Convert.ToInt32((x / _virtualZoom) - (_virtualOffset.X / _virtualZoom));
            int ptY = Convert.ToInt32((y / _virtualZoom) - (_virtualOffset.Y / _virtualZoom));
            return new Point(ptX, ptY);
        }

        /// <summary>
        /// Removes the specified element from the _selectedElements list.
        /// </summary>
        /// <param name="element">Element that gets removed.</param>
        private void RemoveSelectedElement(ModelElement element)
        {
            if (_modelElementsSelected.Contains(element))
            {
                _modelElementsSelected.Remove(element);
                element.Highlighted(false);
                IsInitialized = false;
            }
        }

        private void ScrollTimerHorizontalTick(object sender, EventArgs e)
        {
            _scrollLock = false;
            _horScroll.Minimum -= 50;
            _horScroll.Maximum += 50;
            _scrollTimerHorizontal.Stop();
        }

        private void ScrollTimerVerticalTick(object sender, EventArgs e)
        {
            _scrollLock = false;
            _verScroll.Minimum -= 50;
            _verScroll.Maximum += 50;
            _scrollTimerVertical.Stop();
        }

        /// <summary>
        /// Paints the elements to the backbuffer.
        /// </summary>
        private void UpdateBackBuffer()
        {
            _backBuffer = new Bitmap(Width, Height);
            Graphics graph = Graphics.FromImage(_backBuffer);
            graph.SmoothingMode = DrawingQuality;
            graph.FillRectangle(Brushes.White, 0, 0, _backBuffer.Width, _backBuffer.Height);

            // Check if there are any model elements to draw
            foreach (ModelElement me in _modelElements)
            {
                if (!_modelElementsSelected.Contains(me))
                {
                    Matrix m = new();
                    Point translator = VirtualToPixel(new Point(me.Location.X, me.Location.Y));
                    m.Translate(translator.X, translator.Y);
                    m.Scale(_virtualZoom, _virtualZoom);
                    graph.Transform = m;
                    me.Paint(graph);
                    graph.Transform = new Matrix();
                }
            }

            // Updates is initialized
            IsInitialized = true;
        }

        private void VerScrollValueChanged(object sender, EventArgs e)
        {
            _virtualOffset.Y -= _verScroll.Value - _verLastValue;
            _verLastValue = _verScroll.Value;
            if ((_verScroll.Value <= _verScroll.Minimum) && (_verScroll.Minimum - 50 > (int.MinValue + 50)) && _scrollLock == false)
            {
                _scrollLock = true;
                _scrollTimerVertical.Start();
                _verScroll.Minimum -= 50;
            }

            if ((_verScroll.Value + _verScroll.LargeChange >= _verScroll.Maximum) && (_verScroll.Maximum + 50 < (int.MaxValue - 50)) && _scrollLock == false)
            {
                _scrollLock = true;
                _scrollTimerVertical.Start();
                _verScroll.Maximum += 50;
            }

            IsInitialized = false;
            Invalidate();
        }

        /// <summary>
        /// Translates between virtual model coords and pixels.
        /// </summary>
        /// <param name="pt">A point in the virtual model coordinantes.</param>
        /// <returns>A point in pixel coordinantes. 0, 0 is the top left of the modeler.</returns>
        private Point VirtualToPixel(Point pt)
        {
            return VirtualToPixel(pt.X, pt.Y);
        }

        /// <summary>
        /// Translates between virtual model coords and pixels.
        /// </summary>
        /// <param name="x">the X location in virtual model coordinantes.</param>
        /// <param name="y">the Y location in virtual model coordinantes.</param>
        /// <returns>A point in pixel coordinantes. 0, 0 is the top left of the modeler.</returns>
        private Point VirtualToPixel(int x, int y)
        {
            int ptX = Convert.ToInt32((x * _virtualZoom) + _virtualOffset.X);
            int ptY = Convert.ToInt32((y * _virtualZoom) + _virtualOffset.Y);
            return new Point(ptX, ptY);
        }

        #endregion
    }
}