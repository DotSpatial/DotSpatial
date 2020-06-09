// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;

namespace DotSpatial.Data
{
    /// <summary>
    /// This can be used as a component to work as a DataManager. This also provides the very important DefaultDataManager property,
    /// which is where the developer controls what DataManager should be used for their project.
    /// </summary>
    public class DataManager : IDataManager
    {
        #region Private Variables

        // If this doesn't exist, a new one is created when you "get" this data manager.
        private static IDataManager defaultDataManager;
        private IEnumerable<IDataProvider> _dataProviders;

        private string _dialogReadFilter;
        private string _dialogWriteFilter;
        private string _imageReadFilter;
        private string _imageWriteFilter;

        private string _rasterReadFilter;
        private string _rasterWriteFilter;
        private string _vectorReadFilter;
        private string _vectorWriteFilter;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataManager"/> class.
        /// A data manager is more or less just a list of data providers to use. The very important
        /// DataManager.DefaultDataManager property controls which DataManager will be used
        /// to load data. By default, each DataManager sets itself as the default in its constructor.
        /// </summary>
        public DataManager()
        {
            Configure();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs after the directory providers have been loaded into the project.
        /// </summary>
        public event EventHandler<DataProviderEventArgs> DirectoryProvidersLoaded;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the implementation of IDataManager for the project to use when accessing data.
        /// This is THE place where the DataManager can be replaced by a different data manager.
        /// If you add this data manager to your project, this will automatically set itself as the DefaultDataManager.
        /// However, since each DM will do this, you may have to control this manually
        /// if you add more than one DataManager to the project in order to set the one that will be chosen.
        /// </summary>
        public static IDataManager DefaultDataManager
        {
            get
            {
                return defaultDataManager ?? (defaultDataManager = new DataManager());
            }

            set
            {
                defaultDataManager = value;
            }
        }

        /// <summary>
        /// Gets or sets the list of IDataProviders that should be used in the project.
        /// </summary>
        [Browsable(false)]
        [ImportMany(AllowRecomposition = true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IEnumerable<IDataProvider> DataProviders
        {
            get
            {
                return _dataProviders;
            }

            set
            {
                _dataProviders = value;

                _imageReadFilter = null;
                _imageWriteFilter = null;
                _rasterReadFilter = null;
                _rasterWriteFilter = null;
                _vectorReadFilter = null;
                _vectorWriteFilter = null;

                OnProvidersLoaded(value);
            }
        }

        /// <summary>
        /// Gets or sets the dialog read filter to use for opening data files.
        /// </summary>
        [Category("Filters")]
        [Description("Gets or sets the string that should be used when using this data manager is used to open files.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string DialogReadFilter
        {
            get
            {
                // The developer can bypass the default behavior simply by caching something here.
                if (_dialogReadFilter != null) return _dialogReadFilter;

                var rasterExtensions = new List<string>();
                var vectorExtensions = new List<string>();
                var imageExtensions = new List<string>();
                var extensions = PreferredProviders.Select(item => item.Key).ToList();

                foreach (IDataProvider dp in DataProviders)
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
                                if (dp is IRasterProvider) rasterExtensions.Add(potentialExtension);
                                if (dp is IVectorProvider && potentialExtension != "*.shx"
                                    && potentialExtension != "*.dbf") vectorExtensions.Add(potentialExtension);
                                if (dp is IImageDataProvider) imageExtensions.Add(potentialExtension);
                            }
                        }
                    }
                }

                // We now have a list of all the file extensions supported
                extensions.Remove("*.dbf");
                extensions.Remove("*.shx");
                var result = new StringBuilder("All Supported Formats|" + string.Join(";", extensions.ToArray()));
                if (vectorExtensions.Count > 0) result.Append("|Vectors|" + string.Join(";", vectorExtensions.ToArray()));
                if (rasterExtensions.Count > 0) result.Append("|Rasters|" + string.Join(";", rasterExtensions.ToArray()));
                if (imageExtensions.Count > 0) result.Append("|Images|" + string.Join(";", imageExtensions.ToArray()));

                foreach (KeyValuePair<string, IDataProvider> item in PreferredProviders)
                {
                    // we don't have to check for uniqueness here because it is enforced by the HashTable
                    result.AppendFormat("|{1} - [{0}]| {0}", item.Key, item.Value.Name);
                }

                // Now add each of the individual lines, prepended with the provider name
                foreach (IDataProvider dp in DataProviders)
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
                            if (PreferredProviders.ContainsKey(formats[i]) == false)
                            {
                                string res = formats[i].Replace(";*.shx", string.Empty).Replace("*.shx", string.Empty);
                                res = res.Replace(";*.dbf", string.Empty).Replace("*.dbf", string.Empty);
                                if (formats[i] != "*.shx" && formats[i] != "*.shp")
                                {
                                    result.Append(potentialFormat);
                                    result.Append("|" + res);
                                }
                            }
                        }
                    }
                }

                result.Append("|All Files (*.*) |*.*");
                return result.ToString();
            }

            set
            {
                _dialogReadFilter = value;
            }
        }

        /// <summary>
        /// Gets or sets the dialog write filter to use for saving data files.
        /// </summary>
        [Category("Filters")]
        [Description("Gets or sets the string that should be used when this data manager is used to save files.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string DialogWriteFilter
        {
            get
            {
                // Setting this to something overrides the default
                if (_dialogWriteFilter != null)
                    return _dialogWriteFilter;

                var rasterExtensions = new List<string>();
                var vectorExtensions = new List<string>();
                var imageExtensions = new List<string>();
                var extensions = PreferredProviders.Select(item => item.Key).ToList();

                foreach (IDataProvider dp in DataProviders)
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
                                    rasterExtensions.Add(potentialExtension);
                                if (dp is IVectorProvider)
                                    vectorExtensions.Add(potentialExtension);
                                if (dp is IImageDataProvider)
                                    imageExtensions.Add(potentialExtension);
                            }
                        }
                    }
                }

                // We now have a list of all the file extensions supported
                var result = new StringBuilder("All Supported Formats|" + string.Join(";", extensions.ToArray()));

                if (vectorExtensions.Count > 0)
                    result.Append("|Vectors|" + string.Join(";", vectorExtensions.ToArray()));
                if (rasterExtensions.Count > 0)
                    result.Append("|Rasters|" + string.Join(";", rasterExtensions.ToArray()));
                if (imageExtensions.Count > 0)
                    result.Append("|Images|" + string.Join(";", imageExtensions.ToArray()));

                foreach (KeyValuePair<string, IDataProvider> item in PreferredProviders)
                {
                    // we don't have to check for uniqueness here because it is enforced by the HashTable
                    result.AppendFormat("| [{0}] - {1}| {0}", item.Key, item.Value.Name);
                }

                // Now add each of the individual lines, prepended with the provider name
                foreach (IDataProvider dp in DataProviders)
                {
                    string[] formats = dp.DialogWriteFilter.Split('|');
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
                            if (PreferredProviders.ContainsKey(formats[i]) == false)
                            {
                                result.Append(potentialFormat);
                                result.Append("|" + formats[i]);
                            }
                        }
                    }
                }

                result.Append("|All Files (*.*) |*.*");
                return result.ToString();
            }

            set
            {
                _dialogWriteFilter = value;
            }
        }

        /// <summary>
        /// Gets or sets the dialog read filter to use for opening data files that are specifically raster formats.
        /// </summary>
        [Category("Filters")]
        [Description("Gets or sets the string that should be used when using this data manager is used to open rasters.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string RasterReadFilter
        {
            get
            {
                // The developer can bypass the default behavior simply by caching something here.
                if (_rasterReadFilter != null)
                    return _rasterReadFilter;

                return GetReadFilter<IRasterProvider>("Rasters");
            }

            set
            {
                _rasterReadFilter = value;
            }
        }

        /// <summary>
        /// Gets or sets the dialog write filter to use for saving data files.
        /// </summary>
        [Category("Filters")]
        [Description("Gets or sets the string that should be used when this data manager is used to save rasters.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string RasterWriteFilter
        {
            get
            {
                // Setting this to something overrides the default
                if (_rasterWriteFilter != null)
                    return _rasterWriteFilter;

                return GetWriteFilter<IRasterProvider>("Rasters");
            }

            set
            {
                _rasterWriteFilter = value;
            }
        }

        /// <summary>
        /// Gets or sets the dialog read filter to use for opening data files.
        /// </summary>
        [Category("Filters")]
        [Description("Gets or sets the string that should be used when using this data manager is used to open vectors.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string VectorReadFilter
        {
            get
            {
                // The developer can bypass the default behavior simply by caching something here.
                if (_vectorReadFilter != null)
                    return _vectorReadFilter;

                return GetReadFilter<IVectorProvider>("Vectors");
            }

            set
            {
                _vectorReadFilter = value;
            }
        }

        /// <summary>
        /// Gets or sets the dialog write filter to use for saving data files.
        /// </summary>
        [Category("Filters")]
        [Description("Gets or sets the string that should be used when this data manager is used to save vectors.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string VectorWriteFilter
        {
            get
            {
                // Setting this to something overrides the default
                if (_vectorWriteFilter != null)
                    return _vectorWriteFilter;

                return GetWriteFilter<IVectorProvider>("Vectors");
            }

            set
            {
                _vectorWriteFilter = value;
            }
        }

        /// <summary>
        /// Gets or sets the dialog read filter to use for opening data files.
        /// </summary>
        [Category("Filters")]
        [Description("Gets or sets the string that should be used when using this data manager is used to open images.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string ImageReadFilter
        {
            get
            {
                // The developer can bypass the default behavior simply by caching something here.
                if (_imageReadFilter != null)
                    return _imageReadFilter;

                return GetReadFilter<IImageDataProvider>("Images");
            }

            set
            {
                _imageReadFilter = value;
            }
        }

        /// <summary>
        /// Gets or sets the dialog write filter to use for saving data files.
        /// </summary>
        [Category("Filters")]
        [Description("Gets or sets the string that should be used when this data manager is used to save images.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string ImageWriteFilter
        {
            get
            {
                // Setting this to something overrides the default
                if (_imageWriteFilter != null)
                    return _imageWriteFilter;

                return GetWriteFilter<IImageDataProvider>("Images");
            }

            set
            {
                _imageWriteFilter = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the this data manager should try to load layers by default.
        /// This will be overridden if the inRam property is specified as a parameter.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets the default condition for subsequent load operations which may be overridden by specifying inRam in the parameters.")]
        public bool LoadInRam { get; set; } = true;

        /// <summary>
        /// Gets or sets a dictionary of IDataProviders with corresponding extensions. The standard order is to try to load
        /// the data using a PreferredProvider. If that fails, then it will check the list of dataProviders, and finally,
        /// if that fails, it will check the plugin Data Providers in directories.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Dictionary<string, IDataProvider> PreferredProviders { get; set; }

        /// <summary>
        /// Gets or sets a progress handler for any open operations that are intiated by this
        /// DataManager and don't override this value with an IProgressHandler specified in the parameters.
        /// </summary>
        [Category("Handlers")]
        [Description("Gets or sets the object that implements the IProgressHandler interface for recieving status messages.")]
        public virtual IProgressHandler ProgressHandler { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new class of vector that matches the given fileName.
        /// </summary>
        /// <param name="fileName">The string fileName from which to create a vector.</param>
        /// <param name="featureType">Specifies the type of feature for this vector file.</param>
        /// <returns>An IFeatureSet that allows working with the dataset.</returns>
        public IFeatureSet CreateVector(string fileName, FeatureType featureType)
        {
            return CreateVector(fileName, featureType, ProgressHandler);
        }

        /// <summary>
        /// Creates a new class of vector that matches the given fileName.
        /// </summary>
        /// <param name="fileName">The string fileName from which to create a vector.</param>
        /// <param name="featureType">Specifies the type of feature for this vector file.</param>
        /// <param name="progHandler">Overrides the default progress handler with the specified progress handler.</param>
        /// <returns>An IFeatureSet that allows working with the dataset.</returns>
        /// <exception cref="ArgumentNullException">Raised when fileName is null.</exception>
        /// <exception cref="IOException">Raised when suitable DataProvider not found.</exception>
        public IFeatureSet CreateVector(string fileName, FeatureType featureType, IProgressHandler progHandler)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName), DataStrings.FileNameShouldNotBeNull);

            // To Do: Add Customization that allows users to specify which plugins to use in priority order.

            // First check for the extension in the preferred plugins list
            var ext = Path.GetExtension(fileName);
            IDataProvider pdp;
            if (PreferredProviders.TryGetValue(ext, out pdp))
            {
                var vp = pdp as IVectorProvider;
                var result = vp?.CreateNew(fileName, featureType, true, progHandler);
                if (result != null) return result;

                // if we get here, we found the provider, but it did not succeed in opening the file.
            }

            // Then check the general list of developer specified providers... but not the directory providers
            foreach (var dp in DataProviders.OfType<IVectorProvider>())
            {
                if (GetSupportedExtensions(dp.DialogReadFilter).Contains(ext))
                {
                    // attempt to open with the fileName.
                    var result = dp.CreateNew(fileName, featureType, true, progHandler);
                    if (result != null)
                        return result;
                }
            }

            throw new IOException(DataStrings.FileTypeNotSupported);
        }

        /// <summary>
        /// Checks a dialog filter and returns a list of just the extensions.
        /// </summary>
        /// <param name="dialogFilter">The Dialog Filter to read extensions from.</param>
        /// <returns>A list of extensions.</returns>
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
                    if (extensions.Contains(ext) == false) extensions.Add(ext);
                }
            }

            return extensions;
        }

        /// <summary>
        /// This can help determine what kind of file format a file is, without actually opening the file.
        /// </summary>
        /// <param name="fileName">Path of the file that the file type should be determined for.</param>
        /// <returns>The file format that was detected.</returns>
        public virtual DataFormat GetFileFormat(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            foreach (IDataProvider dp in DataProviders)
            {
                if (GetSupportedExtensions(dp.DialogReadFilter).Contains(ext))
                {
                    IVectorProvider vp = dp as IVectorProvider;
                    if (vp != null)
                        return DataFormat.Vector;
                    IRasterProvider rp = dp as IRasterProvider;
                    if (rp != null)
                        return DataFormat.Raster;
                    IImageDataProvider ip = dp as IImageDataProvider;
                    if (ip != null)
                        return DataFormat.Image;
                    return DataFormat.Custom;
                }
            }

            return DataFormat.Custom;
        }

        /// <summary>
        /// Instead of opening the specified file, this simply determines the correct provider,
        /// and requests that the provider check the feature type for vector formats.
        /// </summary>
        /// <param name="fileName">Path of the file that the feature type should be determined for.</param>
        /// <returns>The feature type that was detected.</returns>
        public virtual FeatureType GetFeatureType(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            if (GetFileFormat(fileName) != DataFormat.Vector)
                return FeatureType.Unspecified;
            foreach (IDataProvider dp in DataProviders)
            {
                if (GetSupportedExtensions(dp.DialogReadFilter).Contains(ext))
                {
                    IVectorProvider vp = dp as IVectorProvider;
                    if (vp == null)
                        continue;
                    return vp.GetFeatureType(fileName);
                }
            }

            return FeatureType.Unspecified;
        }

        /// <summary>
        /// Opens the specified fileName, returning an IRaster. This will return null if a manager
        /// either returns the wrong data format.
        /// </summary>
        /// <param name="fileName">The string fileName to open.</param>
        /// <returns>An IRaster loaded from the specified file.</returns>
        public virtual IRaster OpenRaster(string fileName)
        {
            return OpenFileAsIRaster(fileName, true, ProgressHandler);
        }

        /// <summary>
        /// Opens the specified fileName, returning an IRaster. This will return null if a manager
        /// either returns the wrong data format.
        /// </summary>
        /// <param name="fileName">The string fileName to open.</param>
        /// <param name="inRam">boolean, true if this should be loaded into ram.</param>
        /// <param name="prog">a progress interface.</param>
        /// <returns>An IRaster loaded from the specified file.</returns>
        public virtual IRaster OpenRaster(string fileName, bool inRam, IProgressHandler prog)
        {
            return OpenFileAsIRaster(fileName, inRam, prog);
        }

        /// <summary>
        /// Opens a specified file as an IFeatureSet.
        /// </summary>
        /// <param name="fileName">The string fileName to open.</param>
        /// <param name="inRam">boolean, true if this should be loaded into ram.</param>
        /// <param name="prog">a progress interface.</param>
        /// <returns>An IFeatureSet loaded from the specified file.</returns>
        public virtual IFeatureSet OpenVector(string fileName, bool inRam, IProgressHandler prog)
        {
            return OpenFile(fileName, inRam, prog) as IFeatureSet;
        }

        /// <summary>
        /// Opens the file as an Image and returns an IImageData object for interacting with the file.
        /// </summary>
        /// <param name="fileName">The string fileName.</param>
        /// <returns>An IImageData object.</returns>
        public virtual IImageData OpenImage(string fileName)
        {
            return OpenFile(fileName, LoadInRam, ProgressHandler) as IImageData;
        }

        /// <summary>
        /// Opens the file as an Image and returns an IImageData object.
        /// </summary>
        /// <param name="fileName">The string fileName to open.</param>
        /// <param name="progressHandler">The progressHandler to receive progress updates.</param>
        /// <returns>An IImageData.</returns>
        public virtual IImageData OpenImage(string fileName, IProgressHandler progressHandler)
        {
            return OpenFile(fileName, LoadInRam, progressHandler) as IImageData;
        }

        /// <summary>
        /// Attempts to call the open fileName method for any IDataProvider plugin
        /// that matches the extension on the string.
        /// </summary>
        /// <param name="fileName">A String fileName to attempt to open.</param>
        /// <returns>The opened IDataSet.</returns>
        public virtual IDataSet OpenFile(string fileName)
        {
            return OpenFile(fileName, LoadInRam, ProgressHandler);
        }

        /// <summary>
        /// Attempts to call the open fileName method for any IDataProvider plugin that matches the extension on the string.
        /// </summary>
        /// <param name="fileName">A String fileName to attempt to open.</param>
        /// <param name="inRam">A boolean value that if true will attempt to force a load of the data into memory. This value overrides the property on this DataManager.</param>
        /// <returns>The opened IDataSet.</returns>
        public virtual IDataSet OpenFile(string fileName, bool inRam)
        {
            return OpenFile(fileName, inRam, ProgressHandler);
        }

        /// <summary>
        /// Attempts to call the open fileName method for any IDataProvider plugin that matches the extension on the string.
        /// </summary>
        /// <param name="fileName">A String fileName to attempt to open.</param>
        /// <param name="inRam">A boolean value that if true will attempt to force a load of the data into memory. This value overrides the property on this DataManager.</param>
        /// <param name="progressHandler">Specifies the progressHandler to receive progress messages. This value overrides the property on this DataManager.</param>
        /// <param name="providerName">Name of the provider that should be used for opening. If it is not set or the provider can't open the file, DS takes the first provider that can open the file.</param>
        /// <returns>The opened IDataSet.</returns>
        public virtual IDataSet OpenFile(string fileName, bool inRam, IProgressHandler progressHandler, string providerName = "")
        {
            string ext = Path.GetExtension(fileName)?.ToLower();
            if (ext != null)
            {
                IDataSet result;

                if (providerName != string.Empty)
                {
                    // if a provider name was given we try to find this provider and use it to open the file
                    var provider = PreferredProviders.FirstOrDefault(kvp => kvp.Value.Name == providerName);
                    if (provider.Value != null)
                    {
                        if (GetSupportedExtensions(provider.Value.DialogReadFilter).Contains(ext))
                        {
                            result = provider.Value.Open(fileName);
                            if (result != null)
                                return result;
                        }
                    }

                    var dp = DataProviders.FirstOrDefault(kvp => kvp.Name == providerName);
                    if (dp != null)
                    {
                        if (GetSupportedExtensions(dp.DialogReadFilter).Contains(ext))
                        {
                            result = dp.Open(fileName);
                            if (result != null)
                                return result;
                        }
                    }
                }

                // Check for the extension in the preferred plugins list
                if (PreferredProviders.ContainsKey(ext))
                {
                    result = PreferredProviders[ext].Open(fileName);
                    if (result != null) return result;

                    // if we get here, we found the provider, but it did not succeed in opening the file.
                }

                // Check the general list of developer specified providers... but not the directory providers
                foreach (IDataProvider dp in DataProviders)
                {
                    if (!GetSupportedExtensions(dp.DialogReadFilter).Contains(ext)) continue;

                    // attempt to open with the fileName.
                    dp.ProgressHandler = ProgressHandler;

                    result = dp.Open(fileName);
                    if (result != null) return result;
                }
            }

            throw new ApplicationException(DataStrings.FileTypeNotSupported);
        }

        /// <summary>
        /// Creates a new image using an appropriate data provider.
        /// </summary>
        /// <param name="fileName">The string fileName to open an image for.</param>
        /// <param name="width">The integer width in pixels.</param>
        /// <param name="height">The integer height in pixels.</param>
        /// <param name="bandType">The band color type.</param>
        /// <returns>An IImageData interface allowing access to image data.</returns>
        public virtual IImageData CreateImage(string fileName, int width, int height, ImageBandType bandType)
        {
            return CreateImage(fileName, width, height, LoadInRam, ProgressHandler, bandType);
        }

        /// <summary>
        /// Creates a new image using an appropriate data provider.
        /// </summary>
        /// <param name="fileName">The string fileName to open an image for.</param>
        /// <param name="width">The integer width in pixels.</param>
        /// <param name="height">The integer height in pixels.</param>
        /// <param name="inRam">Boolean, true if the entire file should be created in memory.</param>
        /// <param name="bandType">The band color type.</param>
        /// <returns>An IImageData interface allowing access to image data.</returns>
        public virtual IImageData CreateImage(string fileName, int width, int height, bool inRam, ImageBandType bandType)
        {
            return CreateImage(fileName, width, height, inRam, ProgressHandler, bandType);
        }

        /// <summary>
        /// Creates a new image using an appropriate data provider.
        /// </summary>
        /// <param name="fileName">The string fileName to open an image for.</param>
        /// <param name="width">The integer width in pixels.</param>
        /// <param name="height">The integer height in pixels.</param>
        /// <param name="inRam">Boolean, true if the entire file should be created in memory.</param>
        /// <param name="progHandler">A Progress handler.</param>
        /// <param name="bandType">The band color type.</param>
        /// <returns>An IImageData interface allowing access to image data.</returns>
        public virtual IImageData CreateImage(string fileName, int width, int height, bool inRam, IProgressHandler progHandler, ImageBandType bandType)
        {
            // First check for the extension in the preferred plugins list
            string ext = Path.GetExtension(fileName)?.ToLower();
            if (ext != null)
            {
                IImageData result;
                if (PreferredProviders.ContainsKey(ext))
                {
                    IImageDataProvider rp = PreferredProviders[ext] as IImageDataProvider;
                    if (rp != null)
                    {
                        result = rp.Create(fileName, width, height, inRam, progHandler, bandType);
                        if (result != null)
                            return result;
                    }

                    // if we get here, we found the provider, but it did not succeed in opening the file.
                }

                // Then check the general list of developer specified providers... but not the directory providers
                foreach (IDataProvider dp in DataProviders)
                {
                    if (GetSupportedExtensions(dp.DialogWriteFilter).Contains(ext))
                    {
                        IImageDataProvider rp = dp as IImageDataProvider;
                        if (rp != null)
                        {
                            // attempt to open with the fileName.
                            result = rp.Create(fileName, width, height, inRam, progHandler, bandType);
                            if (result != null)
                                return result;
                        }
                    }
                }
            }

            throw new ApplicationException(DataStrings.FileTypeNotSupported);
        }

        /// <summary>
        /// Creates a new raster using the specified raster provider and the Data Manager's Progress Handler,
        /// as well as its LoadInRam property.
        /// </summary>
        /// <param name="name">The fileName of the new file to create.</param>
        /// <param name="driverCode">The string code identifying the driver to use to create the raster. If no code is specified
        /// the manager will attempt to match the extension with a code specified in the Dialog write filter. </param>
        /// <param name="xSize">The number of columns in the raster.</param>
        /// <param name="ySize">The number of rows in the raster.</param>
        /// <param name="numBands">The number of bands in the raster.</param>
        /// <param name="dataType">The data type for the raster.</param>
        /// <param name="options">Any additional, driver specific options for creation.</param>
        /// <returns>An IRaster representing the created raster.</returns>
        public virtual IRaster CreateRaster(string name, string driverCode, int xSize, int ySize, int numBands, Type dataType, string[] options)
        {
            // First check for the extension in the preferred plugins list
            string ext = Path.GetExtension(name)?.ToLower();
            if (ext != null)
            {
                IRaster result;
                if (PreferredProviders.ContainsKey(ext))
                {
                    IRasterProvider rp = PreferredProviders[ext] as IRasterProvider;
                    if (rp != null)
                    {
                        result = rp.Create(name, driverCode, xSize, ySize, numBands, dataType, options);
                        if (result != null)
                            return result;
                    }

                    // if we get here, we found the provider, but it did not succeed in opening the file.
                }

                // Then check the general list of developer specified providers... but not the directory providers
                foreach (IDataProvider dp in DataProviders)
                {
                    if (GetSupportedExtensions(dp.DialogWriteFilter).Contains(ext))
                    {
                        IRasterProvider rp = dp as IRasterProvider;
                        if (rp != null)
                        {
                            // attempt to open with the fileName.
                            result = rp.Create(name, driverCode, xSize, ySize, numBands, dataType, options);
                            if (result != null)
                                return result;
                        }
                    }
                }
            }

            throw new ApplicationException(DataStrings.FileTypeNotSupported);
        }

        /// <summary>
        /// Opens the file making sure it can be returned as an IRaster.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="inRam">Indicates whether the file should be loaded into ram.</param>
        /// <param name="progressHandler">The progress handler.</param>
        /// <returns>The IRaster that was opened.</returns>
        public virtual IRaster OpenFileAsIRaster(string fileName, bool inRam, IProgressHandler progressHandler)
        {
            // First check for the extension in the preferred plugins list
            string ext = Path.GetExtension(fileName);
            if (ext != null)
            {
                ext = ext.ToLower();
                IRaster result;
                if (PreferredProviders.ContainsKey(ext))
                {
                    result = PreferredProviders[ext].Open(fileName) as IRaster;
                    if (result != null) return result;

                    // if we get here, we found the provider, but it did not succeed in opening the file.
                }

                // Then check the general list of developer specified providers... but not the directory providers
                foreach (IDataProvider dp in DataProviders)
                {
                    if (!GetSupportedExtensions(dp.DialogReadFilter).Contains(ext)) continue;

                    // attempt to open with the fileName.
                    dp.ProgressHandler = ProgressHandler;

                    result = dp.Open(fileName) as IRaster;
                    if (result != null) return result;
                }
            }

            throw new ApplicationException(DataStrings.FileTypeNotSupported);
        }

        /// <summary>
        /// Triggers the DirectoryProvidersLoaded event.
        /// </summary>
        /// <param name="list">List of the providers that were loaded.</param>
        protected virtual void OnProvidersLoaded(IEnumerable<IDataProvider> list)
        {
            DirectoryProvidersLoaded?.Invoke(this, new DataProviderEventArgs(list));
        }

        private void Configure()
        {
            defaultDataManager = this;
            PreferredProviders = new Dictionary<string, IDataProvider>();

            // Provide a number of default providers
            _dataProviders = new List<IDataProvider>
                                 {
                                     new ShapefileDataProvider(),
                                     new BinaryRasterProvider(),
                                     new DotNetImageProvider()
                                 };
        }

        private string GetFilter<T>(string description, Func<IDataProvider, string> dpFilter)
         where T : IDataProvider
        {
            var extensions = new List<string>();

            foreach (KeyValuePair<string, IDataProvider> item in PreferredProviders)
            {
                if (item.Value is T)
                {
                    // we don't have to check for uniqueness here because it is enforced by the HashTable
                    extensions.Add(item.Key);
                }
            }

            foreach (IDataProvider dp in DataProviders)
            {
                string[] formats = dpFilter(dp).Split('|');

                // We don't care about the description strings, just the extensions.
                for (int i = 1; i < formats.Length; i += 2)
                {
                    // Multiple extension types are separated by semicolons
                    string[] potentialExtensions = formats[i].Split(';');
                    foreach (string potentialExtension in potentialExtensions)
                    {
                        if (extensions.Contains(potentialExtension) == false)
                        {
                            if (dp is T) extensions.Add(potentialExtension);
                        }
                    }
                }
            }

            var result = new StringBuilder();

            // We now have a list of all the file extensions supported
            if (extensions.Count > 0) result.Append($"{description}|" + string.Join(";", extensions.ToArray()));

            foreach (KeyValuePair<string, IDataProvider> item in PreferredProviders)
            {
                if (item.Value is T)
                {
                    // we don't have to check for uniqueness here because it is enforced by the HashTable
                    result.AppendFormat("| [{0}] - {1}| {0}", item.Key, item.Value.Name);
                }
            }

            // Now add each of the individual lines, prepended with the provider name
            foreach (IDataProvider dp in DataProviders)
            {
                string[] formats = dpFilter(dp).Split('|');
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
                        if (PreferredProviders.ContainsKey(formats[i]) == false)
                        {
                            if (dp is T)
                            {
                                result.Append(potentialFormat);
                                result.Append("|" + formats[i]);
                            }
                        }
                    }
                }
            }

            result.Append("|All Files (*.*) |*.*");
            return result.ToString();
        }

        private string GetReadFilter<T>(string description)
            where T : IDataProvider
        {
            return GetFilter<T>(description, d => d.DialogReadFilter);
        }

        private string GetWriteFilter<T>(string description)
            where T : IDataProvider
        {
            return GetFilter<T>(description, d => d.DialogWriteFilter);
        }

        #endregion
    }
}