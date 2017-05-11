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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/1/2009 11:42:01 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |     Name          |    Date     |              Comments
// |-------------------|-------------|-------------------------------------------------------------------
// |Ben tidyup Tombs   |18/11/2010   | Modified to add GDAL Helper class GdalHelper.Configure use for initialization of the GDAL Environment
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using DotSpatial.Projections;
using OSGeo.GDAL;

namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    /// A GDAL raster.
    /// </summary>
    /// <typeparam name="T">Type of the contained items.</typeparam>
    internal class GdalRaster<T> : Raster<T>
        where T : IEquatable<T>, IComparable<T>
    {
        #region Fields

        private readonly Band _band;
        private readonly Dataset _dataset;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GdalRaster{T}"/> class.
        /// This can be a raster with multiple bands.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="fromDataset">The dataset.</param>
        public GdalRaster(string fileName, Dataset fromDataset)
            : base(fromDataset.RasterYSize, fromDataset.RasterXSize)
        {
            _dataset = fromDataset;
            Filename = fileName;
            Name = Path.GetFileNameWithoutExtension(fileName);
            ReadHeader();
            int numBands = _dataset.RasterCount;
            if (numBands == 1)
            {
                _band = _dataset.GetRasterBand(1);
            }
            else
            {
                for (int i = 1; i <= numBands; i++)
                {
                    Bands.Add(new GdalRaster<T>(fileName, fromDataset, _dataset.GetRasterBand(i)));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GdalRaster{T}"/> class.
        /// Creates a new raster from the specified band.
        /// </summary>
        /// <param name="fileName">The string path of the file if any.</param>
        /// <param name="fromDataset">The dataset.</param>
        /// <param name="fromBand">The band.</param>
        public GdalRaster(string fileName, Dataset fromDataset, Band fromBand)
            : base(fromDataset.RasterYSize, fromDataset.RasterXSize)
        {
            _dataset = fromDataset;
            _band = fromBand;
            Filename = fileName;
            Name = Path.GetFileNameWithoutExtension(fileName);
            ReadHeader();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the GDAL data type.
        /// </summary>
        public DataType GdalDataType => _band.DataType;

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        public override double Maximum
        {
            get
            {
                return base.Maximum;
            }

            protected set
            {
                base.Maximum = value;
                if (_band != null)
                {
                    _band.SetStatistics(Minimum, value, Mean, StdDeviation);
                    _band.SetMetadataItem("STATISTICS_MAXIMUM", Maximum.ToString(), string.Empty);
                }
                else
                {
                    foreach (GdalRaster<T> raster in Bands)
                    {
                        raster.Maximum = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the mean.
        /// </summary>
        public override double Mean
        {
            get
            {
                return base.Mean;
            }

            protected set
            {
                base.Mean = value;
                if (_band != null)
                {
                    _band.SetStatistics(Minimum, Maximum, value, StdDeviation);
                    _band.SetMetadataItem("STATISTICS_MEAN", Mean.ToString(), string.Empty);
                }
                else
                {
                    foreach (GdalRaster<T> raster in Bands)
                    {
                        raster.Mean = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        public override double Minimum
        {
            get
            {
                return base.Minimum;
            }

            protected set
            {
                base.Minimum = value;
                if (_band != null)
                {
                    _band.SetStatistics(value, Maximum, Mean, StdDeviation);
                    _band.SetMetadataItem("STATISTICS_MINIMUM", Minimum.ToString(), string.Empty);
                }
                else
                {
                    foreach (GdalRaster<T> raster in Bands)
                    {
                        raster.Minimum = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the NoDataValue.
        /// </summary>
        public override double NoDataValue
        {
            get
            {
                return base.NoDataValue;
            }

            set
            {
                base.NoDataValue = value;
                if (_band != null)
                {
                    _band.SetNoDataValue(value);
                }
                else
                {
                    foreach (var raster in Bands)
                    {
                        raster.NoDataValue = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the standard deviation.
        /// </summary>
        public override double StdDeviation
        {
            get
            {
                return base.StdDeviation;
            }

            protected set
            {
                base.StdDeviation = value;
                if (_band != null)
                {
                    _band.SetStatistics(Minimum, Maximum, Mean, value);
                    _band.SetMetadataItem("STATISTICS_STDDEV", StdDeviation.ToString(), string.Empty);
                }
                else
                {
                    foreach (GdalRaster<T> raster in Bands)
                    {
                        raster.StdDeviation = value;
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the category colors.
        /// </summary>
        /// <returns>The category colors.</returns>
        public override Color[] CategoryColors()
        {
            Color[] colors = null;
            ColorTable table = GetColorTable();
            if (table != null)
            {
                int colorCount = table.GetCount();
                if (colorCount > 0)
                {
                    colors = new Color[colorCount];
                    for (int colorIndex = 0; colorIndex < colorCount; colorIndex += 1)
                    {
                        colors[colorIndex] = Color.DimGray;
                        ColorEntry entry = table.GetColorEntry(colorIndex);
                        switch (table.GetPaletteInterpretation())
                        {
                            case PaletteInterp.GPI_RGB:
                                colors[colorIndex] = Color.FromArgb(entry.c4, entry.c1, entry.c2, entry.c3);
                                break;
                            case PaletteInterp.GPI_Gray:
                                colors[colorIndex] = Color.FromArgb(255, entry.c1, entry.c1, entry.c1);
                                break;

                                // TODO: do any files use these types?
                                // case PaletteInterp.GPI_HLS
                                // case PaletteInterp.GPI_CMYK
                        }
                    }
                }
            }

            return colors;
        }

        /// <summary>
        /// Gets the category names.
        /// </summary>
        /// <returns>The category names.</returns>
        public override string[] CategoryNames()
        {
            if (_band != null)
            {
                return _band.GetCategoryNames();
            }

            foreach (GdalRaster<T> raster in Bands)
            {
                return raster._band.GetCategoryNames();
            }

            return null;
        }

        /// <summary>
        /// Closes the raster.
        /// </summary>
        public override void Close()
        {
            base.Close();
            if (_band != null)
            {
                _band.Dispose();
            }
            else
            {
                foreach (IRaster raster in Bands)
                {
                    raster.Close();
                    raster.Dispose();
                }
            }

            if (_dataset != null)
            {
                _dataset.FlushCache();
                _dataset.Dispose();
            }
        }

        /// <summary>
        /// Copies the fileName.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="copyValues">Indicates whether the values should be copied.</param>
        public override void Copy(string fileName, bool copyValues)
        {
            using (Driver d = _dataset.GetDriver())
            {
                DataType myType = OSGeo.GDAL.DataType.GDT_Int32;
                if (_band != null)
                {
                    myType = _band.DataType;
                }
                else
                {
                    GdalRaster<T> r = Bands[0] as GdalRaster<T>;
                    if (r != null)
                    {
                        myType = r.GdalDataType;
                    }
                }

                if (copyValues)
                {
                    d.CreateCopy(fileName, _dataset, 1, Options, GdalProgressFunc, "Copy Progress");
                }
                else
                {
                    d.Create(fileName, NumColumnsInFile, NumRowsInFile, NumBands, myType, Options);
                }
            }
        }

        /// <summary>
        /// Gets the mean, standard deviation, minimum and maximum
        /// </summary>
        public override void GetStatistics()
        {
            if (IsInRam && this.IsFullyWindowed())
            {
                base.GetStatistics();
                return;
            }

            if (_band != null)
            {
                double min, max, mean, std;
                CPLErr err;
                try
                {
                    if (Value.Updated) err = _band.ComputeStatistics(false, out min, out max, out mean, out std, null, null);
                    else err = _band.GetStatistics(0, 1, out min, out max, out mean, out std);

                    Value.Updated = false;
                    Minimum = min;
                    Maximum = max;
                    Mean = mean;
                    StdDeviation = std;
                }
                catch (Exception ex)
                {
                    err = CPLErr.CE_Failure;
                    max = min = std = mean = 0;
                    Trace.WriteLine(ex);
                }

                Value.Updated = false;

                // http://dotspatial.codeplex.com/workitem/22221
                // GetStatistics didn't return anything, so try use the raster default method.
                if (err != CPLErr.CE_None || (max == 0 && min == 0 && std == 0 && mean == 0)) base.GetStatistics();
            }
            else
            {
                // ?? doesn't this mean the stats get overwritten several times.
                foreach (IRaster raster in Bands)
                {
                    raster.GetStatistics();
                }
            }
        }

        /// <summary>
        /// Most reading is optimized to read in a block at a time and process it. This method is designed
        /// for seeking through the file.  It should work faster than the buffered methods in cases where
        /// an unusually arranged collection of values are required.  Sorting the list before calling
        /// this should significantly improve performance.
        /// </summary>
        /// <param name="indices">A list or array of long values that are (Row * NumRowsInFile + Column)</param>
        /// <returns>The values.</returns>
        public override List<T> GetValuesT(IEnumerable<long> indices)
        {
            if (IsInRam) return base.GetValuesT(indices);

            if (_band == null)
            {
                Raster<T> ri = Bands[CurrentBand] as Raster<T>;

                return ri?.GetValuesT(indices);
            }
#if DEBUG
            var sw = new Stopwatch();
            sw.Start();
#endif
            List<T> result = new List<T>();
            foreach (long index in indices)
            {
                int row = (int)(index / NumColumnsInFile);
                int col = (int)(index % NumColumnsInFile);

                T[] data = new T[1];

                // http://trac.osgeo.org/gdal/wiki/GdalOgrCsharpRaster
                GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                try
                {
                    IntPtr ptr = handle.AddrOfPinnedObject();
                    _band.ReadRaster(col, row, 1, 1, ptr, 1, 1, GdalDataType, PixelSpace, LineSpace);
                }
                finally
                {
                    if (handle.IsAllocated)
                    {
                        handle.Free();
                    }
                }

                result.Add(data[0]);
            }

#if DEBUG
            sw.Stop();
            Debug.WriteLine("Time to read values from file:" + sw.ElapsedMilliseconds);
#endif
            return result;
        }

        /// <summary>
        /// Reads values from the raster to the jagged array of values
        /// </summary>
        /// <param name="xOff">The horizontal offset from the left to start reading from</param>
        /// <param name="yOff">The vertical offset from the top to start reading from</param>
        /// <param name="sizeX">The number of cells to read horizontally</param>
        /// <param name="sizeY">The number of cells ot read vertically</param>
        /// <returns>A jagged array of values from the raster</returns>
        public override T[][] ReadRaster(int xOff, int yOff, int sizeX, int sizeY)
        {
            T[][] result = new T[sizeY][];
            T[] rawData = new T[sizeY * sizeX];

            if (_band == null)
            {
                Raster<T> ri = Bands[CurrentBand] as Raster<T>;
                if (ri != null)
                {
                    return ri.ReadRaster(xOff, yOff, sizeX, sizeY);
                }
            }
            else
            {
                GCHandle handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
                try
                {
                    IntPtr ptr = handle.AddrOfPinnedObject();
                    _band.ReadRaster(xOff, yOff, sizeX, sizeY, ptr, sizeX, sizeY, GdalDataType, PixelSpace, LineSpace);
                }
                finally
                {
                    if (handle.IsAllocated)
                    {
                        handle.Free();
                    }
                }

                for (int row = 0; row < sizeY; row++)
                {
                    result[row] = new T[sizeX];
                    Array.Copy(rawData, row * sizeX, result[row], 0, sizeX);
                }

                return result;
            }

            return null;
        }

        /// <summary>
        /// Writes values from the jagged array to the raster at the specified location
        /// </summary>
        /// <param name="buffer">A jagged array of values to write to the raster</param>
        /// <param name="xOff">The horizontal offset from the left to start reading from</param>
        /// <param name="yOff">The vertical offset from the top to start reading from</param>
        /// <param name="xSize">The number of cells to write horizontally</param>
        /// <param name="ySize">The number of cells ot write vertically</param>
        public override void WriteRaster(T[][] buffer, int xOff, int yOff, int xSize, int ySize)
        {
            if (_band == null)
            {
                Raster<T> ri = Bands[CurrentBand] as Raster<T>;
                if (ri != null)
                {
                    ri.NoDataValue = NoDataValue;

                    ri.WriteRaster(buffer, xOff, yOff, xSize, ySize);
                }
            }
            else
            {
                T[] rawValues = new T[xSize * ySize];
                for (int row = 0; row < ySize; row++)
                {
                    Array.Copy(buffer[row], 0, rawValues, row * xSize, xSize);
                }

                GCHandle handle = GCHandle.Alloc(rawValues, GCHandleType.Pinned);
                try
                {
                    IntPtr ptr = handle.AddrOfPinnedObject();

                    // int stride = ((xSize * sizeof(T) + 7) / 8);
                    _band.WriteRaster(xOff, yOff, xSize, ySize, ptr, xSize, ySize, GdalDataType, PixelSpace, 0);
                    _band.FlushCache();
                    _dataset.FlushCache();
                }
                finally
                {
                    if (handle.IsAllocated)
                    {
                        handle.Free();
                    }
                }
            }
        }

        /// <summary>
        /// Updates the header information about the projection and the affine coefficients
        /// </summary>
        protected override void UpdateHeader()
        {
            _dataset.SetGeoTransform(Bounds.AffineCoefficients);

            if (Projection != null)
            {
                _dataset.SetProjection(Projection.ToEsriString());
            }
        }

        /// <summary>
        /// Handles the callback progress content.
        /// </summary>
        /// <param name="complete">Percent of completeness.</param>
        /// <param name="message">Message is not used.</param>
        /// <param name="data">Data is not used.</param>
        /// <returns>0</returns>
        private int GdalProgressFunc(double complete, IntPtr message, IntPtr data)
        {
            ProgressHandler.Progress("Copy Progress", Convert.ToInt32(complete), "Copy Progress");
            return 0;
        }

        private ColorTable GetColorTable()
        {
            if (_band != null)
            {
                return _band.GetColorTable();
            }

            foreach (GdalRaster<T> raster in Bands)
            {
                return raster._band.GetColorTable();
            }

            return null;
        }

        private void ReadHeader()
        {
            DataType = typeof(T);
            NumColumnsInFile = _dataset.RasterXSize;
            NumColumns = NumColumnsInFile;
            NumRowsInFile = _dataset.RasterYSize;
            NumRows = NumRowsInFile;

            // Todo: look for prj file if GetProjection returns null.
            // Do we need to read this as an Esri string if we don't get a proj4 string?
            string projString = _dataset.GetProjection();
            Projection = ProjectionInfo.FromProj4String(projString);
            if (_band != null)
            {
                double val;
                int hasInterval;
                _band.GetNoDataValue(out val, out hasInterval);
                base.NoDataValue = val;
            }

            double[] affine = new double[6];
            _dataset.GetGeoTransform(affine);

            // in gdal (row,col) coordinates are defined relative to the top-left corner of the top-left cell
            // shift them by half a cell to give coordinates relative to the center of the top-left cell
            affine = new AffineTransform(affine).TransfromToCorner(0.5, 0.5);
            ProjectionString = projString;
            Bounds = new RasterBounds(NumRows, NumColumns, affine);
            PixelSpace = Marshal.SizeOf(typeof(T));
        }

        #endregion
    }
}