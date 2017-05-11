// ********************************************************************************************************
// Product Name: DotSpatial.Gdal
// Description:  This is a data extension for the System.Spatial framework.
// ********************************************************************************************************
// The contents of this file are subject to the Gnu Lesser General Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from a plugin for MapWindow version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/1/2009 11:39:44 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |     Name          |    Date     |              Comments
// |-------------------|-------------|-------------------------------------------------------------------
// |Ben tidyup Tombs   |18/11/2010   | Modified to add GDAL Helper class use for initialization of the GDAL Environment
// ********************************************************************************************************

using System;
using System.Diagnostics;
using System.IO;
using OSGeo.GDAL;

namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    /// GdalRasterProvider
    /// </summary>
    public class GdalRasterProvider : IRasterProvider
    {
        #region Constructors

        static GdalRasterProvider()
        {
            GdalConfiguration.ConfigureGdal();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GdalRasterProvider"/> class.
        /// </summary>
        public GdalRasterProvider()
        {
            // Add ourself in for these extensions, unless another provider is registered for them.
            string[] extensions = { ".tif", ".tiff", ".adf" };
            foreach (string extension in extensions)
            {
                if (!DataManager.DefaultDataManager.PreferredProviders.ContainsKey(extension))
                {
                    DataManager.DefaultDataManager.PreferredProviders.Add(extension, this);
                }
            }
        }

        #endregion

        #region Properties

        // This function checks if a GeoTiff dataset should be interpreted as a one-band raster
        // or as an image. Returns true if the dataset is a valid one-band raster.

        /// <summary>
        /// Gets the description of the raster.
        /// </summary>
        public string Description => "GDAL Integer Raster";

        /// <summary>
        /// Gets the dialog filter to use when opening a file.
        /// </summary>
        public string DialogReadFilter => "GDAL Rasters|*.asc;*.adf;*.bil;*.gen;*.thf;*.blx;*.xlb;*.bt;*.dt0;*.dt1;*.dt2;*.tif;*.dem;*.ter;*.mem;*.img;*.nc";

        /// <summary>
        /// Gets the dialog filter to use when saving to a file.
        /// </summary>
        public string DialogWriteFilter => "AAIGrid|*.asc;*.adf|DTED|*.dt0;*.dt1;*.dt2|GTiff|*.tif;*.tiff|TERRAGEN|*.ter|GenBin|*.bil|netCDF|*.nc|Imagine|*.img|GFF|*.gff|Terragen|*.ter";

        /// <summary>
        /// Gets the name of the provider.
        /// </summary>
        public string Name => "GDAL Raster Provider";

        /// <summary>
        /// Gets or sets the progress handler that gets updated with progress information.
        /// </summary>
        public IProgressHandler ProgressHandler { get; set; }

        #endregion

        #region Methods

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
        /// <returns>An IRaster</returns>
        public IRaster Create(string name, string driverCode, int xSize, int ySize, int numBands, Type dataType, string[] options)
        {
            if (File.Exists(name)) File.Delete(name);
            if (string.IsNullOrEmpty(driverCode))
            {
                driverCode = GetDriverCode(Path.GetExtension(name));
            }

            Driver d = Gdal.GetDriverByName(driverCode);
            if (d == null)
            {
                // We didn't find a matching driver.
                return null;
            }

            // Assign the Gdal dataType.
            DataType dt;
            if (dataType == typeof(int)) dt = DataType.GDT_Int32;
            else if (dataType == typeof(short)) dt = DataType.GDT_Int16;
            else if (dataType == typeof(uint)) dt = DataType.GDT_UInt32;
            else if (dataType == typeof(ushort)) dt = DataType.GDT_UInt16;
            else if (dataType == typeof(double)) dt = DataType.GDT_Float64;
            else if (dataType == typeof(float)) dt = DataType.GDT_Float32;
            else if (dataType == typeof(byte)) dt = DataType.GDT_Byte;
            else dt = DataType.GDT_Unknown;

            // Width, Height, and bands must "be positive"
            if (numBands == 0) numBands = 1;

            Dataset dataset = d.Create(name, xSize, ySize, numBands, dt, options);

            return WrapDataSetInRaster(name, dataType, dataset);
        }

        /// <summary>
        /// Opens the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file that gets opened.</param>
        /// <returns>The file as IRaster.</returns>
        public IRaster Open(string fileName)
        {
            IRaster result = null;
            var dataset = Helpers.Open(fileName);
            if (dataset != null)
            {
                // Single band rasters are easy, just return the band as the raster.
                // TODO: make a more complicated raster structure with individual bands.
                result = GetBand(fileName, dataset, dataset.GetRasterBand(1));

                // If we opened the dataset but did not find a raster to return, close the dataset
                if (result == null)
                {
                    dataset.Dispose();
                }
            }

            return result;
        }

        /// <summary>
        /// Opens the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file that gets opened.</param>
        /// <returns>The file as IDataSet.</returns>
        IDataSet IDataProvider.Open(string fileName)
        {
            return Open(fileName);
        }

        private static IRaster GetBand(string fileName, Dataset dataset, Band band)
        {
            Raster result = null;

            switch (band.DataType)
            {
                case DataType.GDT_Byte:
                    result = new GdalRaster<byte>(fileName, dataset, band);
                    break;
                case DataType.GDT_CFloat32:
                case DataType.GDT_CFloat64:
                case DataType.GDT_CInt16:
                case DataType.GDT_CInt32: break;
                case DataType.GDT_Float32:
                    result = new GdalRaster<float>(fileName, dataset, band);
                    break;
                case DataType.GDT_Float64:
                    result = new GdalRaster<double>(fileName, dataset, band);
                    break;
                case DataType.GDT_Int16:
                    result = new GdalRaster<short>(fileName, dataset, band);
                    break;
                case DataType.GDT_UInt16:
                case DataType.GDT_Int32:
                    result = new GdalRaster<int>(fileName, dataset, band);
                    break;
                case DataType.GDT_TypeCount: break;

                case DataType.GDT_UInt32:
                    result = new GdalRaster<long>(fileName, dataset, band);
                    break;
                case DataType.GDT_Unknown: break;
                default: break;
            }

            result?.Open();

            return result;
        }

        private static string GetDriverCode(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension))
            {
                return null;
            }

            switch (fileExtension.ToLower())
            {
                case ".asc": return "AAIGrid";

                case ".adf": return "AAIGrid";

                case ".tiff":
                case ".tif": return "GTiff";

                case ".img": return "HFA";

                case ".gff": return "GFF";

                case ".dt0":
                case ".dt1":
                case ".dt2": return "DTED";

                case ".ter": return "Terragen";

                case ".nc": return "netCDF";

                default: return null;
            }
        }

        private static IRaster WrapDataSetInRaster(string name, Type dataType, Dataset dataset)
        {
            // todo: what about UInt32?
            if (dataType == typeof(int) || dataType == typeof(ushort))
            {
                return new GdalRaster<int>(name, dataset);
            }

            if (dataType == typeof(short))
            {
                return new GdalRaster<short>(name, dataset);
            }

            if (dataType == typeof(float))
            {
                return new GdalRaster<float>(name, dataset);
            }

            if (dataType == typeof(double))
            {
                return new GdalRaster<double>(name, dataset);
            }

            if (dataType == typeof(byte))
            {
                return new GdalRaster<byte>(name, dataset);
            }

            // It was an unsupported type.
            if (dataset != null)
            {
                dataset.Dispose();
                if (File.Exists(name)) File.Delete(name);
            }

            return null;
        }

        #endregion
    }
}