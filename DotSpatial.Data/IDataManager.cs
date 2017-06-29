// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/21/2008 9:30:47 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// IDataManager
    /// </summary>
    public interface IDataManager
    {
        #region Methods

        /// <summary>
        /// This can help determine what kind of file format a file is, without actually opening the file.
        /// </summary>
        /// <param name="fileName">The string fileName to test</param>
        /// <returns>A DataFormats enum</returns>
        DataFormat GetFileFormat(string fileName);

        /// <summary>
        /// Instead of opening the specified file, this simply determines the correct
        /// provider, and requests that the provider check the feature type for vector
        /// formats.
        /// </summary>
        /// <param name="fileName">The string fileName to test</param>
        /// <returns>A FeatureTypes enum</returns>
        FeatureType GetFeatureType(string fileName);

        /// <summary>
        /// Checks a dialog filter and returns a list of just the extensions.
        /// </summary>
        /// <param name="dialogFilter">The Dialog Filter to read extensions from</param>
        /// <returns>A list of extensions</returns>
        List<string> GetSupportedExtensions(string dialogFilter);

        #region OpenFile

        /// <summary>
        /// Attempts to call the open fileName method for any IDataProvider plugin
        /// that matches the extension on the string.
        /// </summary>
        /// <param name="fileName">A String fileName to attempt to open.</param>
        IDataSet OpenFile(string fileName);

        /// <summary>
        /// Attempts to call the open fileName method for any IDataProvider plugin
        /// that matches the extension on the string.
        /// </summary>
        /// <param name="fileName">A String fileName to attempt to open.</param>
        /// <param name="inRam">A boolean value that if true will attempt to force a load of the data into memory.  This value overrides the property on this DataManager.</param>
        IDataSet OpenFile(string fileName, bool inRam);

        /// <summary>
        /// Attempts to call the open fileName method for any IDataProvider plugin
        /// that matches the extension on the string.
        /// </summary>
        /// <param name="fileName">A String fileName to attempt to open.</param>
        /// <param name="inRam">A boolean value that if true will attempt to force a load of the data into memory.  This value overrides the property on this DataManager.</param>
        /// <param name="progressHandler">Specifies the progressHandler to receive progress messages.  This value overrides the property on this DataManager.</param>
        IDataSet OpenFile(string fileName, bool inRam, IProgressHandler progressHandler);

        /// <summary>
        /// Opens the file as an Image and returns an IImageData object for interacting with the file.
        /// </summary>
        /// <param name="fileName">The string fileName</param>
        /// <returns>An IImageData object</returns>
        IImageData OpenImage(string fileName);

        /// <summary>
        /// Opens the file as an Image and returns an IImageData object
        /// </summary>
        /// <param name="fileName">The string fileName to open</param>
        /// <param name="progressHandler">The progressHandler to receive progress updates</param>
        /// <returns>An IImageData</returns>
        IImageData OpenImage(string fileName, IProgressHandler progressHandler);

        /// <summary>
        /// Opens the specified fileName, returning an IRaster.  This will return null if a manager
        /// either returns the wrong data format.
        /// </summary>
        /// <param name="fileName">The string fileName to open</param>
        /// <returns>An IRaster loaded from the specified file.</returns>
        IRaster OpenRaster(string fileName);

        /// <summary>
        /// Opens the specified fileName, returning an IRaster.  This will return null if a manager
        /// either returns the wrong data format.
        /// </summary>
        /// <param name="fileName">The string fileName to open</param>
        /// <param name="inRam">boolean, true if this should be loaded into ram</param>
        /// <param name="prog">a progress interface</param>
        /// <returns>An IRaster loaded from the specified file</returns>
        IRaster OpenRaster(string fileName, bool inRam, IProgressHandler prog);

        /// <summary>
        /// Opens a specified file as an IFeatureSet
        /// </summary>
        /// <param name="fileName">The string fileName to open</param>
        /// <param name="inRam">boolean, true if this should be loaded into ram</param>
        /// <param name="prog">a progress interface</param>
        /// <returns>An IFeatureSet loaded from the specified file</returns>
        IFeatureSet OpenVector(string fileName, bool inRam, IProgressHandler prog);

        #endregion

        #region CreateNew

        /// <summary>
        /// Creates a new image using an appropriate data provider
        /// </summary>
        /// <param name="fileName">The string fileName to open an image for</param>
        /// <param name="numRows">The integer number of rows</param>
        /// <param name="numColumns">The integer number of columns</param>
        /// <param name="bandType">The color band type.</param>
        /// <returns>An IImageData interface allowing access to image data</returns>
        IImageData CreateImage(string fileName, int numRows, int numColumns, ImageBandType bandType);

        /// <summary>
        /// Creates a new image using an appropriate data provider
        /// </summary>
        /// <param name="fileName">The string fileName to open an image for</param>
        /// <param name="numRows">The integer number of rows</param>
        /// <param name="numColumns">The integer number of columns</param>
        /// <param name="inRam">Boolean, true if the entire file should be created in memory</param>
        /// <param name="bandType">The color band type.</param>
        /// <returns>An IImageData interface allowing access to image data</returns>
        IImageData CreateImage(string fileName, int numRows, int numColumns, bool inRam, ImageBandType bandType);

        /// <summary>
        /// Creates a new image using an appropriate data provider
        /// </summary>
        /// <param name="fileName">The string fileName to open an image for</param>
        /// <param name="numRows">The integer number of rows</param>
        /// <param name="numColumns">The integer number of columns</param>
        /// <param name="inRam">Boolean, true if the entire file should be created in memory</param>
        /// <param name="progHandler">A Progress handler</param>
        /// <param name="bandType">The color band type.</param>
        /// <returns>An IImageData interface allowing access to image data</returns>
        ///
        IImageData CreateImage(string fileName, int numRows, int numColumns, bool inRam, IProgressHandler progHandler, ImageBandType bandType);

        /// <summary>
        /// Creates a new class of vector that matches the given fileName.
        /// </summary>
        /// <param name="fileName">The string fileName from which to create a vector.</param>
        /// <param name="featureType">Specifies the type of feature for this vector file</param>
        /// <returns>An IFeatureSet that allows working with the dataset.</returns>
        IFeatureSet CreateVector(string fileName, FeatureType featureType);

        /// <summary>
        /// Creates a new class of vector that matches the given fileName.
        /// </summary>
        /// <param name="fileName">The string fileName from which to create a vector.</param>
        /// <param name="featureType">Specifies the type of feature for this vector file</param>
        /// <param name="progHandler">Overrides the default progress handler with the specified progress handler</param>
        /// <returns>An IFeatureSet that allows working with the dataset.</returns>
        IFeatureSet CreateVector(string fileName, FeatureType featureType, IProgressHandler progHandler);

        /// <summary>
        /// Creates a new raster using the specified raster provider and the Data Manager's Progress Handler,
        /// as well as its LoadInRam property.
        /// </summary>
        /// <param name="name">The fileName of the new file to create.</param>
        /// <param name="driverCode">The string code identifying the driver to use to create the raster</param>
        /// <param name="xSize">The number of columns in the raster</param>
        /// <param name="ySize">The number of rows in the raster</param>
        /// <param name="numBands">The number of bands in the raster</param>
        /// <param name="dataType">The data type for the raster</param>
        /// <param name="options">Any additional, driver specific options for creation</param>
        /// <returns>An IRaster representing the created raster.</returns>
        IRaster CreateRaster(string name, string driverCode, int xSize, int ySize, int numBands, Type dataType, string[] options);

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list of IDataProviders that should be used in the project.
        /// </summary>
        IEnumerable<IDataProvider> DataProviders
        {
            get;
        }

        /// <summary>
        /// Gets or sets the dialog read filter to use for opening data files.
        /// </summary>
        string DialogReadFilter
        {
            get;
        }

        /// <summary>
        /// Gets or sets the dialog write filter to use for saving data files.
        /// </summary>
        string DialogWriteFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the default condition for how this data manager should try to load layers.
        /// This will be overridden if the inRam property is specified as a parameter.
        /// </summary>
        bool LoadInRam
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dialog read filter to use for opening data files that are specifically raster formats.
        /// </summary>
        string RasterReadFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dialog write filter to use for saving raster files.
        /// </summary>
        string RasterWriteFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dialog read filter to use for opening vector files.
        /// </summary>
        string VectorReadFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dialog write filter to use for saving vector files.
        /// </summary>
        string VectorWriteFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dialog read filter to use for opening image files.
        /// </summary>
        string ImageReadFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dialog write filter to use for saving image files.
        /// </summary>
        string ImageWriteFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a dictionary of IDataProviders keyed by the extension.  The
        /// standard order is to try to load the data using a PreferredProvider.  If that
        /// fails, then it will check the list of dataProviders, and finally, if that fails,
        /// it will check the plugin Data Providers in directories.
        /// </summary>
        Dictionary<string, IDataProvider> PreferredProviders
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a progress handler for any open operations that are intiated by this
        /// DataManager.
        /// </summary>
        IProgressHandler ProgressHandler
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Occurs after the directory providers have been loaded into the project.
        /// </summary>
        event EventHandler<DataProviderEventArgs> DirectoryProvidersLoaded;
    }
}