// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/21/2008 9:28:29 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This can be used as a component to work as a LayerManager.  This also provides the
    /// very important DefaultLayerManager property, which is where the developer controls
    /// what LayerManager should be used for their project.
    /// </summary>
    [ToolboxItem(true)]
    public class LayerManager : ILayerManager
    {
        #region Private Variables

        // If this doesn't exist, a new one is created when you "get" this data manager.
        private static ILayerManager _defaultLayerManager;
        private IList<ILayer> _activeProjectLayers;

        private string _dialogReadFilter;

        private string _dialogWriteFilter;
        private string _imageReadFilter;
        private string _imageWriteFilter;

        private List<string> _layerProviderDirectories;
        private List<ILayerProvider> _layerProviders;

        private bool _loadInRam = true;
        private Dictionary<string, ILayerProvider> _preferredProviders;
        private IProgressHandler _progressHandler;
        private string _rasterReadFilter;
        private string _rasterWriteFilter;
        private string _vectorReadFilter;
        private string _vectorWriteFilter;

        /// <summary>
        /// Gets or sets the implemenation of ILayerManager for the project to use when
        /// accessing data.  This is THE place where the LayerManager can be replaced
        /// by a different data manager.  If you add this data manager to your
        /// project, this will automatically set itself as the DefaultLayerManager.
        /// However, since each DM will do this, you may have to control this manually
        /// if you add more than one LayerManager to the project in order to set the
        /// one that will be chosen.
        /// </summary>
        public static ILayerManager DefaultLayerManager
        {
            get
            {
                if (_defaultLayerManager == null)
                {
                    _defaultLayerManager = new LayerManager();
                }
                return _defaultLayerManager;
            }
            set
            {
                _defaultLayerManager = value;
            }
        }

        /// <summary>
        /// Gets or sets a temporary list of active project layers.  This is designed to house
        /// the layers from a map frame when the property grids are shown for layers in that
        /// map frame.  This list on the DefaultLayerManager is what is used to create the
        /// list that populates dropdowns that can take a Layer as a parameter.
        /// </summary>
        public IList<ILayer> ActiveProjectLayers
        {
            get { return _activeProjectLayers; }
            set { _activeProjectLayers = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the LayerManager class.  A data manager is more or less
        /// just a list of data providers to use.  The very important
        /// LayerManager.DefaultLayerManager property controls which LayerManager will be used
        /// to load data.  By default, each LayerManager sets itself as the default in its
        /// constructor.
        /// </summary>
        public LayerManager()
        {
            // InitializeComponent();

            _defaultLayerManager = this;
            _layerProviders = new List<ILayerProvider>();
            // _layerProviders.Add(new ShapefileLayerProvider()); // .shp files
            // _layerProviders.Add(new BinaryLayerProvider()); // .bgd files

            //string path = Application.ExecutablePath;
            _layerProviderDirectories = new List<string>();
            _layerProviderDirectories.Add("\\Plugins");
            _preferredProviders = new Dictionary<string, ILayerProvider>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks a dialog filter and returns a list of just the extensions.
        /// </summary>
        /// <param name="dialogFilter">The Dialog Filter to read extensions from</param>
        /// <returns>A list of extensions</returns>
        public virtual List<string> GetSupportedExtensions(string dialogFilter)
        {
            List<string> extensions = new List<string>();
            string[] formats = dialogFilter.Split('|');
            char[] wild = { '*' };
            // We don't care about the description strings, just the extensions.
            for (int i = 1; i < formats.Length; i += 2)
            {
                // Multiple extension types are separated by semicolons
                string[] potentialExtensions = formats[i].Split(';');
                foreach (string potentialExtension in potentialExtensions)
                {
                    string ext = potentialExtension.TrimStart(wild);
                    if (extensions.Contains(ext) == false)
                    {
                        extensions.Add(ext);
                    }
                }
            }
            return extensions;
        }

        /// <summary>
        /// This opens a file, but populates the dialog filter with only raster formats.
        /// </summary>
        /// <returns>An IRaster with the data from the file specified in an open file dialog</returns>
        public virtual IRasterLayer OpenRasterLayer()
        {
            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Filter = RasterReadFilter;
            //if (ofd.ShowDialog() != DialogResult.OK) return null;
            //return OpenLayer(ofd.FileName, LoadInRam, null, ProgressHandler) as IRasterLayer;
            throw new NotImplementedException();
        }

        /// <summary>
        /// This opens a file, but populates the dialog filter with only raster formats.
        /// </summary>
        /// <returns>An IFeatureSet with the data from the file specified in a dialog</returns>
        public virtual IFeatureLayer OpenVectorLayer()
        {
            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Filter = VectorReadFilter;
            //if (ofd.ShowDialog() != DialogResult.OK) return null;
            //return OpenLayer(ofd.FileName, LoadInRam, null, ProgressHandler) as IFeatureLayer;
            throw new NotImplementedException();
        }

        /// <summary>
        /// This attempts to open the specified raster file and returns an associated layer
        /// </summary>
        /// <param name="fileName">The string fileName to open</param>
        /// <returns>An IRaster with the data from the file specified in an open file dialog</returns>
        public virtual IRasterLayer OpenRasterLayer(string fileName)
        {
            return OpenLayer(fileName, LoadInRam, null, ProgressHandler) as IRasterLayer;
        }

        /// <summary>
        /// This attempts to open the specified vector file and returns an associated layer
        /// </summary>
        /// <param name="fileName">the string fileName to open</param>
        /// <returns>An IFeatureSet with the data from the file specified in a dialog</returns>
        public virtual IFeatureLayer OpenVectorLayer(string fileName)
        {
            return OpenLayer(fileName, LoadInRam, null, ProgressHandler) as IFeatureLayer;
        }

        /// <summary>
        /// Opens a new layer and automatically adds it to the specified container.
        /// </summary>
        /// <param name="container">The container (usually a LayerCollection) to add to</param>
        /// <returns>The layer after it has been created and added to the container</returns>
        public virtual ILayer OpenLayer(ICollection<ILayer> container)
        {
            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Filter = DialogReadFilter;
            //if (ofd.ShowDialog() != DialogResult.OK) return null;
            //return OpenLayer(ofd.FileName, LoadInRam, container, ProgressHandler);
            throw new NotImplementedException();
        }

        /// <summary>
        /// This launches an open file dialog and attempts to load the specified file.
        /// </summary>
        /// <param name="progressHandler">Specifies the progressHandler to receive progress messages.  This value overrides the property on this DataManager.</param>
        /// <returns>A Layer</returns>
        public virtual ILayer OpenLayer(IProgressHandler progressHandler)
        {
            throw new NotImplementedException();
            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Filter = DialogReadFilter;
            //if (ofd.ShowDialog() != DialogResult.OK) return null;
            //return OpenLayer(ofd.FileName, LoadInRam, null, progressHandler);
        }

        /// <summary>
        /// This launches an open file dialog and attempts to load the specified file.
        /// </summary>
        /// <returns>A Layer created from the file</returns>
        public virtual ILayer OpenLayer()
        {
            throw new NotImplementedException();
            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Filter = DialogReadFilter;
            //if (ofd.ShowDialog() != DialogResult.OK) return null;
            //return OpenLayer(ofd.FileName, LoadInRam, null, ProgressHandler);
        }

        /// <summary>
        /// Attempts to call the open fileName method for any ILayerProvider plugin
        /// that matches the extension on the string.
        /// </summary>
        /// <param name="fileName">A String fileName to attempt to open.</param>
        public virtual ILayer OpenLayer(string fileName)
        {
            return OpenLayer(fileName, LoadInRam, null, ProgressHandler);
        }

        /// <summary>
        /// Opens a new layer and automatically adds it to the specified container.
        /// </summary>
        /// <param name="fileName">A String fileName to attempt to open.</param>
        /// <param name="container">The container (usually a LayerCollection) to add to</param>
        /// <returns>The layer after it has been created and added to the container</returns>
        public virtual ILayer OpenLayer(string fileName, ICollection<ILayer> container)
        {
            return OpenLayer(fileName, LoadInRam, container, ProgressHandler);
        }

        /// <summary>
        /// This launches an open file dialog and attempts to load the specified file.
        /// </summary>
        /// <param name="fileName">A String fileName to attempt to open.</param>
        /// <param name="progressHandler">Specifies the progressHandler to receive progress messages.  This value overrides the property on this DataManager.</param>
        /// <returns>A Layer</returns>
        public virtual ILayer OpenLayer(string fileName, IProgressHandler progressHandler)
        {
            return OpenLayer(fileName, LoadInRam, null, progressHandler);
        }

        /// <summary>
        /// Attempts to call the open fileName method for any ILayerProvider plugin
        /// that matches the extension on the string.
        /// </summary>
        /// <param name="fileName">A String fileName to attempt to open.</param>
        /// <param name="inRam">A boolean value that if true will attempt to force a load of the data into memory.  This value overrides the property on this LayerManager.</param>
        /// <param name="container">A container to open this layer in</param>
        /// <param name="progressHandler">Specifies the progressHandler to receive progress messages.  This value overrides the property on this LayerManager.</param>
        /// <returns>An ILayer</returns>
        public virtual ILayer OpenLayer(string fileName, bool inRam, ICollection<ILayer> container, IProgressHandler progressHandler)
        {
            // To Do: Add Customization that allows users to specify which plugins to use in priority order.

            // First check for the extension in the preferred plugins list

            string ext = Path.GetExtension(fileName);
            if (ext != null)
            {
                ILayer result;
                if (_preferredProviders.ContainsKey(ext))
                {
                    result = _preferredProviders[ext].OpenLayer(fileName, inRam, container, progressHandler);
                    if (result != null)
                    {
                        return result;
                    }
                    // if we get here, we found the provider, but it did not succeed in opening the file.
                }

                // Then check the general list of developer specified providers... but not the directory providers

                foreach (ILayerProvider dp in _layerProviders)
                {
                    if (GetSupportedExtensions(dp.DialogReadFilter).Contains(ext))
                    {
                        // attempt to open with the fileName.
                        result = dp.OpenLayer(fileName, inRam, container, progressHandler);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
            }

            throw new ApplicationException(SymbologyMessageStrings.LayerManager_FileTypeNotSupported);
        }

        /// <summary>
        /// This create new method implies that this provider has the priority for creating a new file.
        /// An instance of the dataset should be created and then returned.  By this time, the fileName
        /// will already be checked to see if it exists, and deleted if the user wants to overwrite it.
        /// </summary>
        /// <param name="name">The string fileName for the new instance.</param>
        /// <param name="driverCode">The string short name of the driver for creating the raster.</param>
        /// <param name="xSize">The number of columns in the raster.</param>
        /// <param name="ySize">The number of rows in the raster.</param>
        /// <param name="numBands">The number of bands to create in the raster.</param>
        /// <param name="dataType">The data type to use for the raster.</param>
        /// <param name="options">The options to be used.</param>
        /// <returns>A new IRasterLayer.</returns>
        public virtual IRasterLayer CreateRasterLayer(string name, string driverCode, int xSize, int ySize, int numBands, Type dataType, string[] options)
        {
            // First check for the extension in the preferred plugins list

            string ext = Path.GetExtension(name);
            if (ext != null)
            {
                IRasterLayer result;
                if (_preferredProviders.ContainsKey(ext))
                {
                    IRasterLayerProvider rp = _preferredProviders[ext] as IRasterLayerProvider;
                    if (rp != null)
                    {
                        result = rp.Create(name, driverCode, xSize, ySize, numBands, dataType, options);
                        if (result != null)
                        {
                            return result;
                        }
                    }

                    // if we get here, we found the provider, but it did not succeed in opening the file.
                }

                // Then check the general list of developer specified providers... but not the directory providers

                foreach (ILayerProvider dp in _layerProviders)
                {
                    if (GetSupportedExtensions(dp.DialogReadFilter).Contains(ext))
                    {
                        IRasterLayerProvider rp = dp as IRasterLayerProvider;
                        if (rp != null)
                        {
                            // attempt to open with the fileName.
                            result = rp.Create(name, driverCode, xSize, ySize, numBands, dataType, options);
                            if (result != null)
                            {
                                return result;
                            }
                        }
                    }
                }
            }

            throw new ApplicationException(SymbologyMessageStrings.LayerManager_FileTypeNotSupported);
        }

        /// <summary>
        /// This opens a file, but populates the dialog filter with only raster formats.
        /// </summary>
        /// <returns>for now an ILayerSet</returns>
        public virtual ILayer OpenImageLayer()
        {
            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Filter = ImageReadFilter;
            //if (ofd.ShowDialog() != DialogResult.OK) return null;
            //return OpenLayer(ofd.FileName, LoadInRam, null, ProgressHandler);
            throw new NotImplementedException();
        }

        #endregion

        #region Properties

        // May make this invisible if we can
        /// <summary>
        /// Gets or sets the dialog read filter to use for opening data files.
        /// </summary>
        [Category("Filters"),
         Description("Gets or sets the string that should be used when using this data manager is used to open images.")]
        public virtual string ImageReadFilter
        {
            get
            {
                // The developer can bypass the default behavior simply by caching something here.
                if (_imageReadFilter != null) return _imageReadFilter;
                return GetReadFilter<IImageDataProvider>("Images");
            }
            set { _imageReadFilter = value; }
        }

        /// <summary>
        /// Gets or sets the dialog write filter to use for saving data files.
        /// </summary>
        [Category("Filters"),
         Description("Gets or sets the string that should be used when this data manager is used to save images.")]
        public virtual string ImageWriteFilter
        {
            get
            {
                // Setting this to something overrides the default
                if (_imageWriteFilter != null) return _imageWriteFilter;
                return GetWriteFilter<IImageDataProvider>("Images");
            }
            set { _imageWriteFilter = value; }
        }

        /// <summary>
        /// Gets or sets the list of ILayerProviders that should be used in the project.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual List<ILayerProvider> LayerProviders
        {
            get { return _layerProviders; }
            set
            {
                _layerProviders = value;
            }
        }

        /// <summary>
        /// Gets or sets the path (either as a full path or as a path relative to
        /// the DotSpatial.dll) to search for plugins that implement the ILayerProvider interface.
        /// </summary>
        [Category("Providers"),
         Description("Gets or sets the list of string path names that should be used to search for ILayerProvider interfaces.")]
        public virtual List<string> LayerProviderDirectories
        {
            get { return _layerProviderDirectories; }
            set { _layerProviderDirectories = value; }
        }

        /// <summary>
        /// Gets or sets the dialog read filter to use for opening data files.
        /// </summary>
        [Category("Filters"),
         Description("Gets or sets the string that should be used when using this data manager is used to open files.")]
        public virtual string DialogReadFilter
        {
            get
            {
                // The developer can bypass the default behavior simply by caching something here.
                if (_dialogReadFilter != null) return _dialogReadFilter;

                List<string> rasterExtensions = new List<string>();
                List<string> vectorExtensions = new List<string>();
                List<string> imageExtensions = new List<string>();

                List<string> extensions = _preferredProviders.Select(item => item.Key).ToList();

                foreach (ILayerProvider dp in _layerProviders)
                {
                    string[] formats = dp.DialogReadFilter.Split('|');
                    // We don't care about the description strings, just the extensions.
                    for (int i = 1; i < formats.Length; i += 2)
                    {
                        // Multiple extension types are separated by semicolons
                        string[] potentialExtensions = formats[i].Split(';');
                        foreach (string potentialExtension in potentialExtensions)
                        {
                            if (extensions.Contains(potentialExtension) == false)
                            {
                                extensions.Add(potentialExtension);
                                if (dp is IRasterProvider)
                                {
                                    rasterExtensions.Add(potentialExtension);
                                }
                                if (dp is IVectorProvider)
                                {
                                    vectorExtensions.Add(potentialExtension);
                                }
                                if (dp is IImageDataProvider)
                                {
                                    imageExtensions.Add(potentialExtension);
                                }
                            }
                        }
                    }
                }

                // We now have a list of all the file extensions supported
                string result = "All Supported Formats|" + String.Join(";", extensions.ToArray());
                if (vectorExtensions.Count > 0)
                {
                    result += "|Vectors|" + String.Join(";", vectorExtensions.ToArray());
                }
                if (rasterExtensions.Count > 0)
                {
                    result += "|Rasters|" + String.Join(";", rasterExtensions.ToArray());
                }
                if (imageExtensions.Count > 0)
                {
                    result += "|Images|" + String.Join(";", imageExtensions.ToArray());
                }

                foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
                {
                    // we don't have to check for uniqueness here because it is enforced by the HashTable
                    result += "| [" + item.Key + "] - " + item.Value.Name + "| " + item.Key;
                }
                // Now add each of the individual lines, prepended with the provider name
                foreach (ILayerProvider dp in _layerProviders)
                {
                    string[] formats = dp.DialogReadFilter.Split('|');
                    string potentialFormat = null;
                    for (int i = 0; i < formats.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            // For descriptions, prepend the name:
                            potentialFormat = "|" + dp.Name + " - " + formats[i];
                        }
                        else
                        {
                            // don't add this format if it was already added by a "preferred data provider"
                            if (_preferredProviders.ContainsKey(formats[i]) == false)
                            {
                                result += potentialFormat;
                                result += "|" + formats[i];
                            }
                        }
                    }
                }
                result += "|All Files (*.*) |*.*";
                return result;
            }
            set { _dialogReadFilter = value; }
        }

        /// <summary>
        /// Gets or sets the dialog write filter to use for saving data files.
        /// </summary>
        [Category("Filters"),
         Description("Gets or sets the string that should be used when this data manager is used to save files.")]
        public virtual string DialogWriteFilter
        {
            get
            {
                // Setting this to something overrides the default
                if (_dialogWriteFilter != null) return _dialogWriteFilter;

                List<string> extensions = new List<string>();
                List<string> rasterExtensions = new List<string>();
                List<string> vectorExtensions = new List<string>();
                List<string> imageExtensions = new List<string>();

                foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
                {
                    extensions.Add(item.Key);
                }

                foreach (ILayerProvider dp in _layerProviders)
                {
                    string[] formats = dp.DialogWriteFilter.Split('|');

                    // We don't care about the description strings, just the extensions.
                    for (int i = 1; i < formats.Length; i += 2)
                    {
                        // Multiple extension types are separated by semicolons
                        string[] potentialExtensions = formats[i].Split(';');
                        foreach (string potentialExtension in potentialExtensions)
                        {
                            if (extensions.Contains(potentialExtension) == false)
                            {
                                extensions.Add(potentialExtension);
                                if (dp is IRasterProvider)
                                {
                                    rasterExtensions.Add(potentialExtension);
                                }
                                if (dp is IVectorProvider)
                                {
                                    vectorExtensions.Add(potentialExtension);
                                }
                                if (dp is IImageDataProvider)
                                {
                                    imageExtensions.Add(potentialExtension);
                                }
                            }
                        }
                    }
                }

                // We now have a list of all the file extensions supported
                string result = "All Supported Formats|" + String.Join(";", extensions.ToArray());

                if (vectorExtensions.Count > 0)
                {
                    result += "|Vectors|" + String.Join(";", vectorExtensions.ToArray());
                }
                if (rasterExtensions.Count > 0)
                {
                    result += "|Rasters|" + String.Join(";", rasterExtensions.ToArray());
                }
                if (imageExtensions.Count > 0)
                {
                    result += "|Images|" + String.Join(";", imageExtensions.ToArray());
                }

                foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
                {
                    // we don't have to check for uniqueness here because it is enforced by the HashTable
                    result += "| [" + item.Key + "] - " + item.Value.Name + "| " + item.Key;
                }

                // Now add each of the individual lines, prepended with the provider name
                foreach (ILayerProvider dp in _layerProviders)
                {
                    string[] formats = dp.DialogWriteFilter.Split('|');
                    string potentialFormat = null;
                    for (int i = 0; i < formats.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            // For descriptions, prepend the name:
                            potentialFormat += "|" + dp.Name + " - " + formats[i];
                        }
                        else
                        {
                            if (_preferredProviders.ContainsKey(formats[i]) == false)
                            {
                                result += potentialFormat;
                                result += "|" + formats[i];
                            }
                        }
                    }
                }

                result += "|All Files (*.*) |*.*";
                return result;
            }
            set { _dialogWriteFilter = value; }
        }

        /// <summary>
        /// Gets or sets the dialog read filter to use for opening data files that are specifically raster formats.
        /// </summary>
        [Category("Filters"),
         Description("Gets or sets the string that should be used when using this data manager is used to open rasters.")]
        public virtual string RasterReadFilter
        {
            get
            {
                // The developer can bypass the default behavior simply by caching something here.
                if (_rasterReadFilter != null) return _rasterReadFilter;
                return GetReadFilter<IRasterProvider>("Rasters");
            }
            set { _rasterReadFilter = value; }
        }

        /// <summary>
        /// Gets or sets the dialog write filter to use for saving data files.
        /// </summary>
        [Category("Filters"),
         Description("Gets or sets the string that should be used when this data manager is used to save rasters.")]
        public virtual string RasterWriteFilter
        {
            get
            {
                // Setting this to something overrides the default
                if (_rasterWriteFilter != null) return _rasterWriteFilter;
                return GetWriteFilter<IRasterProvider>("Rasters");
            }
            set { _rasterWriteFilter = value; }
        }

        /// <summary>
        /// Gets or sets the dialog read filter to use for opening data files.
        /// </summary>
        [Category("Filters"),
         Description("Gets or sets the string that should be used when using this data manager is used to open vectors.")]
        public virtual string VectorReadFilter
        {
            get
            {
                // The developer can bypass the default behavior simply by caching something here.
                if (_vectorReadFilter != null) return _vectorReadFilter;
                return GetReadFilter<IVectorProvider>("Vectors");
            }
            set { _vectorReadFilter = value; }
        }

        /// <summary>
        /// Gets or sets the dialog write filter to use for saving data files.
        /// </summary>
        [Category("Filters"),
         Description("Gets or sets the string that should be used when this data manager is used to save vectors.")]
        public virtual string VectorWriteFilter
        {
            get
            {
                // Setting this to something overrides the default
                if (_vectorWriteFilter != null) return _vectorWriteFilter;
                return GetWriteFilter<IVectorProvider>("Vectors");
            }
            set { _vectorWriteFilter = value; }
        }

        /// <summary>
        /// Sets the default condition for how this data manager should try to load layers.
        /// This will be overridden if the inRam property is specified as a parameter.
        /// </summary>
        [Category("Behavior"),
         Description("Gets or sets the default condition for subsequent load operations which may be overridden by specifying inRam in the parameters.")]
        public bool LoadInRam
        {
            get { return _loadInRam; }
            set { _loadInRam = value; }
        }

        /// <summary>
        /// Gets or sets a dictionary of ILayerProviders with corresponding extensions.  The
        /// standard order is to try to load the data using a PreferredProvider.  If that
        /// fails, then it will check the list of dataProviders, and finally, if that fails,
        /// it will check the plugin Layer Providers in directories.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Dictionary<string, ILayerProvider> PreferredProviders
        {
            get { return _preferredProviders; }
            set { _preferredProviders = value; }
        }

        /// <summary>
        /// Gets or sets a progress handler for any open operations that are intiated by this
        /// LayerManager and don't override this value with an IProgressHandler specified in the parameters.
        /// </summary>
        [Category("Handlers")]
        [Description("Gets or sets the object that implements the IProgressHandler interface for recieving status messages.")]
        public virtual IProgressHandler ProgressHandler
        {
            get { return _progressHandler; }
            set { _progressHandler = value; }
        }

        private string GetWriteFilter<T>(string description)
        {
            string result = null;
            List<string> extensions = new List<string>();

            foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
            {
                if (item.Value is T)
                {
                    extensions.Add(item.Key);
                }
            }

            foreach (ILayerProvider dp in _layerProviders)
            {
                string[] formats = dp.DialogWriteFilter.Split('|');

                // We don't care about the description strings, just the extensions.
                for (int i = 1; i < formats.Length; i += 2)
                {
                    // Multiple extension types are separated by semicolons
                    string[] potentialExtensions = formats[i].Split(';');
                    foreach (string potentialExtension in potentialExtensions)
                    {
                        if (extensions.Contains(potentialExtension) == false)
                        {
                            if (dp is T)
                            {
                                extensions.Add(potentialExtension);
                            }
                        }
                    }
                }
            }

            // We now have a list of all the file extensions supported

            if (extensions.Count > 0)
            {
                result += String.Format("{0}|", description) + String.Join(";", extensions.ToArray());
            }

            foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
            {
                if (item.Value is T)
                {
                    // we don't have to check for uniqueness here because it is enforced by the HashTable
                    result += "| [" + item.Key + "] - " + item.Value.Name + "| " + item.Key;
                }
            }

            // Now add each of the individual lines, prepended with the provider name
            foreach (ILayerProvider dp in _layerProviders)
            {
                string[] formats = dp.DialogWriteFilter.Split('|');
                string potentialFormat = null;
                for (int i = 0; i < formats.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        // For descriptions, prepend the name:
                        potentialFormat += "|" + dp.Name + " - " + formats[i];
                    }
                    else
                    {
                        if (_preferredProviders.ContainsKey(formats[i]) == false)
                        {
                            if (dp is T)
                            {
                                result += potentialFormat;
                                result += "|" + formats[i];
                            }
                        }
                    }
                }
            }

            result += "|All Files (*.*) |*.*";
            return result;
        }

        private string GetReadFilter<T>(string description)
        {
            List<string> extensions = new List<string>();

            foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
            {
                if (item.Value is T)
                {
                    // we don't have to check for uniqueness here because it is enforced by the HashTable
                    extensions.Add(item.Key);
                }
            }

            foreach (ILayerProvider dp in _layerProviders)
            {
                string[] formats = dp.DialogReadFilter.Split('|');
                // We don't care about the description strings, just the extensions.
                for (int i = 1; i < formats.Length; i += 2)
                {
                    // Multiple extension types are separated by semicolons
                    string[] potentialExtensions = formats[i].Split(';');
                    foreach (string potentialExtension in potentialExtensions)
                    {
                        if (extensions.Contains(potentialExtension) == false)
                        {
                            if (dp is T)
                            {
                                extensions.Add(potentialExtension);
                            }
                        }
                    }
                }
            }

            // We now have a list of all the file extensions supported
            string result = String.Format("{0}|", description) + String.Join(";", extensions.ToArray());

            foreach (KeyValuePair<string, ILayerProvider> item in _preferredProviders)
            {
                if (item.Value is T)
                {
                    // we don't have to check for uniqueness here because it is enforced by the HashTable
                    result += "| [" + item.Key + "] - " + item.Value.Name + "| " + item.Key;
                }
            }
            // Now add each of the individual lines, prepended with the provider name
            foreach (ILayerProvider dp in _layerProviders)
            {
                string[] formats = dp.DialogReadFilter.Split('|');
                string potentialFormat = null;
                for (int i = 0; i < formats.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        // For descriptions, prepend the name:
                        potentialFormat = "|" + dp.Name + " - " + formats[i];
                    }
                    else
                    {
                        // don't add this format if it was already added by a "preferred data provider"
                        if (_preferredProviders.ContainsKey(formats[i]) == false)
                        {
                            if (dp is T)
                            {
                                result += potentialFormat;
                                result += "|" + formats[i];
                            }
                        }
                    }
                }
            }
            result += "|All Files (*.*) |*.*";
            return result;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs after the directory providers have been loaded into the project.
        /// </summary>
        public event EventHandler<LayerProviders> DirectoryProvidersLoaded;

        /// <summary>
        /// Triggers the DirectoryProvidersLoaded event
        /// </summary>
        protected virtual void OnProvidersLoaded(List<ILayerProvider> list)
        {
            if (DirectoryProvidersLoaded != null)
            {
                DirectoryProvidersLoaded(this, new LayerProviders(list));
            }
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// This should be called once all the permitted directories have been set in the code.
        /// This will not affect the PreferredProviders or the general list of Providers.
        /// These automatically have the lowest priority and will only be used if nothing
        /// else works.  Use the PreferredProviders to force preferential loading of
        /// a plugin LayerProvider.
        /// </summary>
        /// <returns>A list of just the newly added LayerProviders from this method.</returns>
        public virtual List<ILayerProvider> LoadProvidersFromDirectories()
        {
            Assembly asm;
            List<ILayerProvider> result = new List<ILayerProvider>();
            foreach (string directory in _layerProviderDirectories)
            {
                foreach (string file in Directory.GetFiles(directory, "*.dll", SearchOption.AllDirectories))
                {
                    if (file.Contains("Interop")) continue;
                    if (Path.GetFileName(file) == "DotSpatial.dll") continue; // If they forget to turn "copy local" to false.

                    asm = Assembly.LoadFrom(file);
                    try
                    {
                        Type[] coClassList = asm.GetTypes();
                        foreach (Type coClass in coClassList)
                        {
                            Type[] infcList = coClass.GetInterfaces();
                            foreach (Type infc in infcList)
                            {
                                if (infc == typeof(ILayerProvider))
                                {
                                    try
                                    {
                                        object obj = asm.CreateInstance(coClass.FullName);
                                        ILayerProvider dp = obj as ILayerProvider;
                                        if (dp != null)
                                        {
                                            _layerProviders.Add(dp);
                                            result.Add(dp);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine(ex);
                                        // this object didn't work, but keep looking
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        // We will fail frequently.
                    }
                }
            }
            OnProvidersLoaded(result);
            return result;
        }

        /// <summary>
        /// Given a string fileName for the "*.dll" file, this will attempt to load any classes that implement the
        /// ILayerProvder interface.
        /// </summary>
        /// <param name="fileName">The string path of the assembly to load from.</param>
        /// <returns>A list that contains only the providers that were just loaded.  This may be a list of count 0, but shouldn't return null.</returns>
        public virtual List<ILayerProvider> LoadProvidersFromAssembly(string fileName)
        {
            List<ILayerProvider> result = new List<ILayerProvider>();
            if (Path.GetExtension(fileName) != ".dll") return result;
            if (fileName.Contains("Interop")) return result;

            Assembly asm = Assembly.LoadFrom(fileName);
            try
            {
                Type[] coClassList = asm.GetTypes();
                foreach (Type coClass in coClassList)
                {
                    Type[] infcList = coClass.GetInterfaces();

                    foreach (Type infc in infcList)
                    {
                        if (infc != typeof(ILayerProvider)) continue;
                        try
                        {
                            object obj = asm.CreateInstance(coClass.FullName);
                            ILayerProvider dp = obj as ILayerProvider;
                            if (dp != null)
                            {
                                _layerProviders.Add(dp);
                                result.Add(dp);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                            // this object didn't work, but keep looking
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                // We will fail frequently.
            }

            OnProvidersLoaded(result);
            return result;
        }

        #endregion
    }
}