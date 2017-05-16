// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace DotSpatial.Data
{
    /// <summary>
    /// BinaryRasterProvider
    /// </summary>
    public class BinaryRasterProvider : IRasterProvider
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Gets a basic description that will fall next to your plugin in the Add Other Data dialog.
        /// This will only be shown if your plugin does not supply a DialogReadFilter.
        /// </summary>
        public virtual string Description => "This data provider uses a file format, and not the 'other data' methods.";

        /// <summary>
        /// Gets a dialog read filter that lists each of the file type descriptions and file extensions, delimeted
        /// by the | symbol. Each will appear in DotSpatial's open file dialog filter, preceeded by the name provided
        /// on this object.
        /// </summary>
        public virtual string DialogReadFilter => "Binary Files (*.bgd)|*.bgd";

        /// <summary>
        /// Gets a dialog filter that lists each of the file type descriptions and extensions for a Save File Dialog.
        /// Each will appear in DotSpatial's open file dialog filter, preceeded by the name provided on this object.
        /// </summary>
        public virtual string DialogWriteFilter => "Binary Files (*.bgd)|*.bgd";

        /// <summary>
        /// Gets a prefereably short name that identifies this data provider. Example might be GDAL.
        /// This will be prepended to each of the DialogReadFilter members from this plugin.
        /// </summary>
        public virtual string Name => "DotSpatial";

        /// <summary>
        /// Gets or sets the control or method that should report on progress
        /// </summary>
        public IProgressHandler ProgressHandler { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Reads a binary header to determine the appropriate data type.
        /// </summary>
        /// <param name="fileName">File to read the header from.</param>
        /// <returns>The datatype that was found.</returns>
        public static RasterDataType GetDataType(string fileName)
        {
            using (var br = new BinaryReader(new FileStream(fileName, FileMode.Open)))
            {
                br.ReadInt32(); // NumColumns
                br.ReadInt32(); // NumRows
                br.ReadDouble(); // CellWidth
                br.ReadDouble(); // CellHeight
                br.ReadDouble(); // xllcenter
                br.ReadDouble(); // yllcenter
                return (RasterDataType)br.ReadInt32();
            }
        }

        /// <summary>
        /// This create new method implies that this provider has the priority for creating a new file.
        /// An instance of the dataset should be created and then returned. By this time, the fileName
        /// will already be checked to see if it exists, and deleted if the user wants to overwrite it.
        /// </summary>
        /// <param name="name">The string fileName for the new instance</param>
        /// <param name="driverCode">The string short name of the driver for creating the raster</param>
        /// <param name="xSize">The number of columns in the raster</param>
        /// <param name="ySize">The number of rows in the raster</param>
        /// <param name="numBands">The number of bands to create in the raster</param>
        /// <param name="dataType">The data type to use for the raster</param>
        /// <param name="options">The options to be used.</param>
        /// <returns>An IRaster</returns>
        public IRaster Create(string name, string driverCode, int xSize, int ySize, int numBands, Type dataType, string[] options)
        {
            if (dataType == typeof(byte))
                return new BgdRaster<byte>(name, ySize, xSize);

            if (dataType == typeof(short))
                return new BgdRaster<short>(name, ySize, xSize);

            if (dataType == typeof(int))
                return new BgdRaster<int>(name, ySize, xSize);

            if (dataType == typeof(long))
                return new BgdRaster<long>(name, ySize, xSize);

            if (dataType == typeof(float))
                return new BgdRaster<float>(name, ySize, xSize);

            if (dataType == typeof(double))
                return new BgdRaster<double>(name, ySize, xSize);

            if (dataType == typeof(sbyte))
                return new BgdRaster<sbyte>(name, ySize, xSize);

            if (dataType == typeof(ushort))
                return new BgdRaster<ushort>(name, ySize, xSize);

            if (dataType == typeof(uint))
                return new BgdRaster<uint>(name, ySize, xSize);

            if (dataType == typeof(ulong))
                return new BgdRaster<ulong>(name, ySize, xSize);

            if (dataType == typeof(bool))
                return new BgdRaster<bool>(name, ySize, xSize);

            return null;
        }

        /// <summary>
        /// This open method is only called if this plugin has been given priority for one
        /// of the file extensions supported in the DialogReadFilter property supplied by
        /// this control. Failing to provide a DialogReadFilter will result in this plugin
        /// being added to the list of DataProviders being supplied under the Add Other Data
        /// option in the file menu.
        /// </summary>
        /// <param name="fileName">A string specifying the complete path and extension of the file to open.</param>
        /// <returns>An IDataSet to be added to the Map. These can also be groups of datasets.</returns>
        public virtual IRaster Open(string fileName)
        {
            RasterDataType fileDataType = GetDataType(fileName);
            Raster raster = null;
            switch (fileDataType)
            {
                case RasterDataType.BYTE:
                    raster = new BgdRaster<byte>
                             {
                                 ProgressHandler = ProgressHandler,
                                 Filename = fileName
                             };
                    break;
                case RasterDataType.SHORT:
                    raster = new BgdRaster<short>
                             {
                                 ProgressHandler = ProgressHandler,
                                 Filename = fileName
                             };
                    break;
                case RasterDataType.INTEGER:
                    raster = new BgdRaster<int>
                             {
                                 ProgressHandler = ProgressHandler,
                                 Filename = fileName
                             };
                    break;
                case RasterDataType.LONG:
                    raster = new BgdRaster<long>
                             {
                                 ProgressHandler = ProgressHandler,
                                 Filename = fileName
                             };
                    break;
                case RasterDataType.SINGLE:
                    raster = new BgdRaster<float>
                             {
                                 ProgressHandler = ProgressHandler,
                                 Filename = fileName
                             };
                    break;
                case RasterDataType.DOUBLE:
                    raster = new BgdRaster<double>
                             {
                                 ProgressHandler = ProgressHandler,
                                 Filename = fileName
                             };
                    break;
                case RasterDataType.SBYTE:
                    raster = new BgdRaster<sbyte>
                             {
                                 ProgressHandler = ProgressHandler,
                                 Filename = fileName
                             };
                    break;
                case RasterDataType.USHORT:
                    raster = new BgdRaster<ushort>
                             {
                                 ProgressHandler = ProgressHandler,
                                 Filename = fileName
                             };
                    break;
                case RasterDataType.UINTEGER:
                    raster = new BgdRaster<uint>
                             {
                                 ProgressHandler = ProgressHandler,
                                 Filename = fileName
                             };
                    break;
                case RasterDataType.ULONG:
                    raster = new BgdRaster<ulong>
                             {
                                 ProgressHandler = ProgressHandler,
                                 Filename = fileName
                             };
                    break;
                case RasterDataType.BOOL:
                    raster = new BgdRaster<bool>
                             {
                                 ProgressHandler = ProgressHandler,
                                 Filename = fileName
                             };
                    break;
            }

            raster?.Open();
            return raster;
        }

        /// <summary>
        /// A Non-File based open. If no DialogReadFilter is provided, DotSpatial will call
        /// this method when this plugin is selected from the Add Other Data option in the
        /// file menu.
        /// </summary>
        /// <returns>null</returns>
        public virtual List<IDataSet> Open()
        {
            // This data provider uses a file format, and not the 'other data' methods.
            return null;
        }

        /// <summary>
        /// Opens the given file.
        /// </summary>
        /// <param name="fileName">File that should be opened.</param>
        /// <returns>An IDataSet with the files data.</returns>
        IDataSet IDataProvider.Open(string fileName)
        {
            return Open(fileName);
        }

        #endregion
    }
}