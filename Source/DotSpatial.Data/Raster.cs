// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace DotSpatial.Data
{
    /// <summary>
    /// Handles the native file functions for float grids, without relying on GDAL.
    /// </summary>
    public class Raster : RasterBoundDataSet, IRaster
    {
        #region Private Variables
        private double _maximum;
        private double _mean;
        private double _minimum;
        private int _numColumns; // The count of the columns in the image
        private int _numRows; // The number of rows in an image

        //// todo: determine where _progressHandler should be used
        //// private IProgressHandler _progressHandler;
        private List<IValueRow> _rows;
        private double _stdDev;
        private object _tag;
        private IValueGrid _values;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Raster"/> class with the default values.
        /// </summary>
        public Raster()
        {
            IsInRam = true;
            Bands = new List<IRaster>();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when attempting to copy or save to a fileName that already exists. A developer can tap into this event
        /// in order to display an appropriate message. A cancel property allows the developer (and ultimately the user)
        /// decide if the specified event should ultimately be canceled.
        /// </summary>
        public event EventHandler<MessageCancelEventArgs> FileExists;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the size of each raster element in bytes.
        /// </summary>
        public virtual int ByteSize
        {
            get
            {
                throw new NotImplementedException("This should be implemented at a level where a type parameter can be parsed.");
            }
        }

        /// <summary>
        /// Gets or sets the list of bands, which are in turn rasters. The rasters
        /// contain only one band each, instead of the list of all the bands like the parent raster.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IList<IRaster> Bands { get; set; }

        /// <summary>
        /// Gets or sets the geographic height of a cell the projected units.
        /// Setting this will automatically adjust the affine coefficient to a negative value.
        /// </summary>
        [Category("General")]
        [Description("Gets or sets the geographic height of a cell the projected units. Setting this will automatically adjust the affine coefficient to a negative value.")]
        public virtual double CellHeight
        {
            get
            {
                if (Bounds != null) return Math.Abs(Bounds.AffineCoefficients[5]);
                return 0;
            }

            set
            {
                // For backwards compatibility, people specifying CellHeight can be assumed
                // to want to preserve YllCenter, and not the Affine coefficient.
                Bounds.AffineCoefficients[3] = Bounds.BottomLeft().Y + (value * NumRows);

                // This only allows you to change the magnitude of the cell height, not the direction.
                // For changing direction, control AffineCoefficients[5] directly.
                Bounds.AffineCoefficients[5] = Math.Sign(Bounds.AffineCoefficients[5]) * Math.Abs(value);
            }
        }

        /// <summary>
        /// Gets or sets the geographic width of a cell in the projected units.
        /// </summary>
        [Category("General")]
        [Description("Gets or sets the geographic width of a cell in the projected units.")]
        public virtual double CellWidth
        {
            get
            {
                return Math.Abs(Bounds.AffineCoefficients[1]);
            }

            set
            {
                Bounds.AffineCoefficients[1] = Math.Sign(Bounds.AffineCoefficients[1]) * Math.Abs(value);
            }
        }

        /// <summary>
        /// Gets or sets a zero-based integer band index that specifies which of the internal bands
        /// is currently being used for requests for data.
        /// </summary>
        [Category("Data")]
        [Description("Gets or sets a zero-based integer band index that specifies which of the internal bands is currently being used for requests for data.")]
        public virtual int CurrentBand { get; protected set; }

        /// <summary>
        /// Gets or sets the custom file type. This does nothing unless the FileType property is set to custom.
        /// In such a case, this string allows new file types to be managed.
        /// </summary>
        [Category("Data")]
        [Description("Gets or sets the custom file type. This does nothing unless the FileType property is set to custom. In such a case, this string allows new file types to be managed.")]
        public virtual string CustomFileType { get; protected set; }

        /// <summary>
        /// Gets or sets the RasterDataTypes enumeration clarifying the underlying data type for this raster.
        /// </summary>
        [Category("Data")]
        [Description("Gets or sets the RasterDataTypes enumeration clarifying the underlying data type for this raster.")]
        public Type DataType { get; set; }

        /// <summary>
        /// Gets or sets the driver code for this raster.
        /// </summary>
        public string DriverCode { get; set; }

        /// <summary>
        /// Gets or sets the integer column index for the right column of this raster. Most of the time this will be NumColumns - 1.
        /// However, if this raster is a window taken from a larger raster, then it will be the index of the endColumn from the window.
        /// </summary>
        [Category("Window")]
        [Description("Gets or sets the integer column index for the right column of this raster. Most of the time this will be NumColumns - 1. However, if this raster is a window taken from a larger raster, then it will be the index of the endColumn from the window.")]
        public virtual int EndColumn { get; protected set; }

        /// <summary>
        /// Gets or sets the integer row index for the end row of this raster. Most of the time this will be numRows - 1.
        /// However, if this raster is a window taken from a larger raster, then it will be the index of the endRow from the window.
        /// </summary>
        [Category("Window")]
        [Description("Gets or sets the integer row index for the end row of this raster. Most of the time this will  be numRows - 1. However, if this raster is a window taken from a larger raster, then it will be the index of the endRow from the window.")]
        public virtual int EndRow { get; protected set; }

        /// <summary>
        /// Gets or sets the grid file type. Only Binary or ASCII are supported natively, without GDAL.
        /// </summary>
        [Category("Data")]
        [Description("Gets or sets the grid file type. Only Binary or ASCII are supported natively, without GDAL.")]
        public virtual RasterFileType FileType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the data for this raster is in memory.
        /// </summary>
        [Category("Data")]
        [Description("Gets or sets a value indicating whether the data for this raster is in memory.")]
        public virtual bool IsInRam { get; protected set; }

        /// <summary>
        /// Gets or sets a parameter for accessing some GDAL data. This frequently does nothing and is usually 0.
        /// </summary>
        public int LineSpace { get; set; }

        /// <summary>
        /// Gets or sets the maximum data value, not counting no-data values in the grid.
        /// </summary>
        [Category("Statistics")]
        [Description("Gets or sets the maximum data value, not counting no-data values in the grid.")]
        public virtual double Maximum
        {
            get
            {
                if (Value.Updated) GetStatistics();
                return _maximum;
            }

            protected set
            {
                _maximum = value;
            }
        }

        /// <summary>
        /// Gets or sets the mean of the non-NoData values in this grid. If the data is not InRam, then
        /// the GetStatistics method must be called before these values will be correct.
        /// </summary>
        [Category("Statistics")]
        [Description("Gets or sets the mean of the non-NoData values in this grid. If the data is not InRam, then the GetStatistics method must be called before these values will be correct.")]
        public virtual double Mean
        {
            get
            {
                if (Value.Updated) GetStatistics();
                return _mean;
            }

            protected
                set { _mean = value; }
        }

        /// <summary>
        /// Gets or sets the minimum data value that is not classified as a no data value in this raster.
        /// </summary>
        [Category("Statistics")]
        [Description("Gets or sets the minimum data value that is not classified as a no data value in this raster.")]
        public virtual double Minimum
        {
            get
            {
                if (Value.Updated) GetStatistics();
                return _minimum;
            }

            protected set
            {
                _minimum = value;
            }
        }

        /// <summary>
        /// Gets or sets a double showing the no-data value for this raster.
        /// </summary>
        [Category("Data")]
        [Description("Gets or sets a  double showing the no-data value for this raster.")]
        public virtual double NoDataValue { get; set; }

        /// <summary>
        /// Gets or sets the notes. For binary rasters this will get cut to only 256 characters.
        /// </summary>
        [Category("General")]
        [Description("Gets or sets the notes. For binary rasters this will get cut to only 256 characters.")]
        public virtual string Notes { get; set; }

        /// <summary>
        /// Gets the number of bands. In most traditional grid formats, this is 1. For RGB images, this would be 3. Some formats may have many bands.
        /// </summary>
        [Category("General")]
        [Description("Gets the number of bands. In most traditional grid formats, this is 1. For RGB images, this would be 3. Some formats may have many bands.")]
        public virtual int NumBands => Bands.Count;

        /// <summary>
        /// Gets or sets the horizontal count of the cells in the raster.
        /// </summary>
        [Category("General")]
        [Description("Gets or sets the horizontal count of the cells in the raster.")]
        public virtual int NumColumns
        {
            get
            {
                return _numColumns;
            }

            set
            {
                _numColumns = value;
                if (_numColumns > 0 && _numRows > 0 && Bounds != null)
                {
                    Bounds = new RasterBounds(NumRows, NumColumns, Bounds.AffineCoefficients);
                }
            }
        }

        /// <summary>
        /// Gets or sets the integer count of the number of columns in the source or file that this
        /// raster is a window from. (Usually this will be the same as NumColumns)
        /// </summary>
        [Category("Window")]
        [Description("Gets or sets the integer count of the number of columns in the source or file that this raster is a window from. (Usually this will be the same as NumColumns)")]
        public virtual int NumColumnsInFile { get; protected set; }

        /// <summary>
        /// Gets or sets the vertical count of the cells in the raster.
        /// </summary>
        [Category("General")]
        [Description("Gets or sets the vertical count of the cells in the raster.")]
        public virtual int NumRows
        {
            get
            {
                return _numRows;
            }

            set
            {
                _numRows = value;
                if (_numColumns > 0 && _numRows > 0 && Bounds != null)
                {
                    Bounds = new RasterBounds(value, NumColumns, Bounds.AffineCoefficients);
                }
            }
        }

        /// <summary>
        /// Gets or sets the integer count of the number of rows in the source or file that this
        /// raster is a window from. (Usually this will be the same as NumColumns.)
        /// </summary>
        [Category("Window")]
        [Description("Gets the integer count of the number of rows in the source or file that this raster is a window from. (Usually this will be the same as NumColumns.)")]
        public virtual int NumRowsInFile { get; protected set; }

        /// <summary>
        /// Gets or sets the count of the cells that are not no-data. If the data is not InRam, then
        /// you will have to first call the GetStatistics() method to gain meaningful values.
        /// </summary>
        [Category("Statistics")]
        [Description("Gets or sets the count of the cells that are not no-data. If the data is not InRam, then you will have to first call the GetStatistics() method to gain meaningful values.")]
        public virtual long NumValueCells { get; protected set; }

        /// <summary>
        /// Gets or sets an extra string array of options that exist for support of some types of GDAL supported raster drivers.
        /// </summary>
        public string[] Options { get; set; }

        /// <summary>
        /// Gets or sets a parameter for accessing GDAL. This frequently does nothing and is usually 0.
        /// </summary>
        public int PixelSpace { get; set; }

        /// <summary>
        /// Gets or sets a list of the rows in this raster that can be accessed independantly.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual List<IValueRow> Rows
        {
            get
            {
                return _rows;
            }

            protected
                set { _rows = value; }
        }

        /// <summary>
        /// Gets or sets a statistical sampling. This is designed to cache a small, representative sample of no more than about 10,000 values.
        /// This property does not automatically produce the sample, and so this can be null.
        /// </summary>
        public IEnumerable<double> Sample { get; set; }

        /// <summary>
        /// Gets or sets the integer column index for the left column of this raster. Most of the time this will be 0.
        /// However, if this raster is a window taken from a file, then it will be the row index in the file for the top row of this raster.
        /// </summary>
        [Category("Window")]
        [Description("Gets or sets the integer column index for the left column of this raster. Most of the time this will be 0. However, if this raster is a window taken from a larger raster, then it will be the index of the startRow from the window.")]
        public virtual int StartColumn { get; protected set; }

        /// <summary>
        /// Gets or sets the integer row index for the top row of this raster. Most of the time this will be 0.
        /// However, if this raster is a window taken from a file, then it will be the row index in the file for the left row of this raster.
        /// </summary>
        [Category("Window")]
        [Description("The integer row index for the top row of this raster. Most of the time this will be 0. However, if this raster is a window taken from a larger raster, then it will be the index of the startRow from the window.")]
        public virtual int StartRow { get; protected set; }

        /// <summary>
        /// Gets or sets the standard deviation of all the Non-nodata cells. If the data is not InRam,
        /// then you will have to first call the GetStatistics() method to get meaningful values.
        /// </summary>
        [Category("Statistics")]
        [Description("Gets or sets the standard deviation of all the Non-nodata cells. If the data is not InRam, then you will have to first call the GetStatistics() method to get meaningful values.")]
        public virtual double StdDeviation
        {
            get
            {
                if (Value.Updated) GetStatistics();
                return _stdDev;
            }

            protected set
            {
                _stdDev = value;
            }
        }

        /// <summary>
        /// Gets or sets a tag that is provided for future developers to link this raster to other entities.
        /// It has no function internally, so it can be manipulated safely.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object Tag
        {
            get
            {
                return _tag;
            }

            set
            {
                _tag = value;
            }
        }

        /// <summary>
        /// Gets or sets the value on the CurrentBand given a row and column undex
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IValueGrid Value
        {
            get
            {
                return _values;
            }

            set
            {
                _values = value;
            }
        }

        /// <summary>
        /// Gets or sets the X position of the lower left data cell. Setting this will adjust only the _affine[0] coefficient to ensure
        /// that the lower left corner ends up in the specified location, while keeping all the other affine coefficients the same.
        /// This is like a horizontal Translate that locks into place the center of the lower left corner of the image.
        /// </summary>
        [Category("General")]
        [Description("Gets or sets the X position of the lower left data cell. Setting this will adjust only the _affine[0] coefficient to ensure that the " +
                     "lower left corner ends up in the specified location, while keeping all the other affine coefficients the same. " +
                     "This is like a horizontal Translate that locks into place the center of the lower left corner of the image.")]
        public double Xllcenter
        {
            get
            {
                double[] affine = Bounds.AffineCoefficients;
                return affine[0] + (affine[1] * .5) + (affine[2] * (_numColumns - .5));
            }

            set
            {
                double[] affine = Bounds.AffineCoefficients;
                if (affine == null)
                {
                    affine = new double[6];
                    affine[1] = 1;
                    affine[3] = _numRows;
                    affine[5] = -1;
                }

                if (affine[1] == 0)
                {
                    affine[1] = 1;
                }

                affine[0] = value - (affine[1] * .5) - (affine[2] * (_numColumns - .5));
            }
        }

        /// <summary>
        /// Gets or sets the Y position of the lower left data cell. Setting this will adjust only the _affine[0] coefficient to ensure
        /// that the lower left corner ends up in the specified location, while keeping all the other affine coefficients the same.
        /// This is like a horizontal Translate that locks into place the center of the lower left corner of the image.
        /// </summary>
        [Category("General")]
        [Description("Gets or sets the Y position of the lower left data cell. Setting this will adjust only the _affine[0] coefficient to ensure that the " +
                     "lower left corner ends up in the specified location, while keeping all the other affine coefficients the same. " +
                     "This is like a horizontal Translate that locks into place the center of the lower left corner of the image.")]
        public double Yllcenter
        {
            get
            {
                double[] affine = Bounds.AffineCoefficients;
                return affine[3] + (affine[4] * .5) + (affine[5] * (_numRows - .5));
            }

            set
            {
                double[] affine = Bounds.AffineCoefficients;
                if (affine == null)
                {
                    affine = new double[6];
                    affine[1] = 1;
                    affine[3] = _numRows;
                }

                if (affine[5] == 0)
                {
                    // Cell Height can't be 0
                    affine[5] = -1;
                }

                affine[3] = value - (affine[4] * .5) - (affine[5] * (_numRows - .5));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This create new method implies that this provider has the priority for creating a new file.
        /// An instance of the dataset should be created and then returned. By this time, the fileName
        /// will already be checked to see if it exists, and deleted if the user wants to overwrite it.
        /// </summary>
        /// <param name="name">The string fileName for the new instance.</param>
        /// <param name="driverCode">The string short name of the driver for creating the raster.</param>
        /// <param name="xSize">The number of columns in the raster.</param>
        /// <param name="ySize">The number of rows in the raster.</param>
        /// <param name="numBands">The number of bands to create in the raster.</param>
        /// <param name="dataType">The data type to use for the raster.</param>
        /// <param name="options">The options to be used.</param>
        /// <returns>The raster that was created.</returns>
        public static IRaster Create(string name, string driverCode, int xSize, int ySize, int numBands, Type dataType, string[] options)
        {
            return DataManager.DefaultDataManager.CreateRaster(name, driverCode, xSize, ySize, numBands, dataType, options);
        }

        /// <summary>
        /// This create new method implies that this provider has the priority for creating a new file.
        /// An instance of the dataset should be created and then returned. By this time, the fileName
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
        public static IRaster CreateRaster(string name, string driverCode, int xSize, int ySize, int numBands, Type dataType, string[] options)
        {
            return DataManager.DefaultDataManager.CreateRaster(name, driverCode, xSize, ySize, numBands, dataType, options);
        }

        /// <summary>
        /// For DotSpatial style binary grids, this returns the filetype.
        /// </summary>
        /// <param name="filename">The fileName with extension to test</param>
        /// <returns>A GridFileTypes enumeration listing which file type this is</returns>
        public static RasterFileType GetGridFileType(string filename)
        {
            // Get the extension with period from the fileName
            string extension = Path.GetExtension(filename);

            // Compare the strings, ignoring case
            var eUp = extension?.ToUpper() ?? string.Empty;
            switch (eUp)
            {
                case ".ASC": return RasterFileType.Ascii;
                case ".ARC": return RasterFileType.Ascii;
                case ".BGD": return RasterFileType.Binary;
                case ".FLT": return RasterFileType.Flt;
                case ".ADF": return RasterFileType.Esri;
                case ".ECW": return RasterFileType.Ecw;
                case ".BIL": return RasterFileType.Bil;
                case ".SID": return RasterFileType.MrSid;
                case ".AUX": return RasterFileType.Paux;
                case ".PIX": return RasterFileType.PciDsk;
                case ".DHM": return RasterFileType.Dted;
                case ".DT0": return RasterFileType.Dted;
                case ".DT1": return RasterFileType.Dted;
                case ".TIF": return RasterFileType.GeoTiff;
                case ".IMG": return RasterFileType.Bil;
                case ".DDF": return RasterFileType.Sdts;
            }

            return RasterFileType.Custom;
        }

        /// <summary>
        /// Opens the specified fileName. The DefaultDataManager will determine the best type of raster to handle the specified
        /// file based on the fileName or characteristics of the file.
        /// </summary>
        /// <param name="fileName">The string fileName of the raster to open.</param>
        /// <returns>An IRaster loaded from the specified file.</returns>
        public static IRaster Open(string fileName)
        {
            return DataManager.DefaultDataManager.OpenRaster(fileName, true, DataManager.DefaultDataManager.ProgressHandler);
        }

        /// <summary>
        /// Opens the given file.
        /// </summary>
        /// <param name="fileName">The string full path for the fileName to open</param>
        /// <returns>A Raster object which is actually one of the type specific rasters, like FloatRaster.</returns>
        public static IRaster OpenFile(string fileName)
        {
            // Ensure that the fileName is valid
            if (fileName == null) throw new NullException("fileName");
            if (!File.Exists(fileName)) throw new FileNotFoundException("fileName");

            // default to opening values into ram
            IDataSet dataset = DataManager.DefaultDataManager.OpenFile(fileName);
            return dataset as IRaster;
        }

        /// <summary>
        /// Returns a native raster of the appropriate file type and data type by parsing the fileName.
        /// </summary>
        /// <param name="fileName">The string fileName to attempt to open with a native format</param>
        /// <param name="inRam">The boolean value.</param>
        /// <returns>An IRaster which has been opened to the specified file.</returns>
        public static IRaster OpenFile(string fileName, bool inRam)
        {
            // Ensure that the fileName is valid
            if (fileName == null) throw new NullException("fileName");
            if (File.Exists(fileName) == false) throw new FileNotFoundException("fileName");

            // default to opening values into ram
            IDataSet dataset = DataManager.DefaultDataManager.OpenFile(fileName, inRam);
            return dataset as IRaster;
        }

        /// <summary>
        /// Returns a native raster of the appropriate file type and data type by parsing the fileName.
        /// </summary>
        /// <param name="fileName">The string fileName to attempt to open with a native format</param>
        /// <param name="inRam">The boolean value.</param>
        /// <param name="progressHandler">An overriding progress manager for this process</param>
        /// <returns>An IRaster which has been opened to the specified file.</returns>
        public static IRaster OpenFile(string fileName, bool inRam, IProgressHandler progressHandler)
        {
            if (fileName == null) throw new NullException("fileName");
            if (File.Exists(fileName) == false) throw new FileNotFoundException("fileName");

            // default to opening values into ram
            IDataSet dataset = DataManager.DefaultDataManager.OpenFile(fileName, inRam, progressHandler);
            return dataset as IRaster;
        }

        /// <summary>
        /// By default, Raster does not have any CategoryNames, but this can be overridden.
        /// </summary>
        /// <returns>Null, because rasters don't have category names.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string[] CategoryNames()
        {
            return null;
        }

        /// <summary>
        /// By default, Raster does not have any CategoryColors, but this can be overridden.
        /// </summary>
        /// <returns>Null, because rasters don't have any category colors.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Color[] CategoryColors()
        {
            return null;
        }

        /// <summary>
        /// Creates a memberwise clone of this object. Reference types will be copied, but they will still point to the same original object.
        /// So a clone of this raster is pointing to the same underlying array of values etc.
        /// </summary>
        /// <returns>A Raster clone of this raster.</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// Creates a new IRaster that has the identical characteristics and in-ram data to the original.
        /// </summary>
        /// <returns>A Copy of the original raster.</returns>
        public virtual IRaster Copy()
        {
            throw new NotImplementedException("Copy should be implemented in the internal, or sub classes.");
        }

        /// <summary>
        /// Creates a duplicate version of this file. If copyValues is set to false, then a raster of NoData values is created
        /// that has the same georeferencing information as the source file of this Raster, even if this raster is just a window.
        /// </summary>
        /// <param name="fileName">The string fileName specifying where to create the new file.</param>
        /// <param name="copyValues">If this is false, the same size and georeferencing values are used, but they are all set to NoData.</param>
        public virtual void Copy(string fileName, bool copyValues)
        {
            throw new NotImplementedException("Copy should be implemented in the internal, or sub classes.");
        }

        /// <inheritdoc/>
        public virtual List<double> GetValues(IEnumerable<long> indices)
        {
            // Implemented in subclass
            return null;
        }

        /// <summary>
        /// Even if this is a window, this will cause this raster to show statistics calculated from the entire file.
        /// </summary>
        public virtual void GetStatistics()
        {
            throw new NotImplementedException(DataStrings.NotImplemented);
        }

        /// <summary>
        /// Saves the values to a the same file that was created or loaded.
        /// </summary>
        public virtual void Save()
        {
            throw new NotImplementedException(DataStrings.NotImplemented);
        }

        /// <summary>
        /// Saves the curretn raster to the specified file. The current driver code and options are used.
        /// </summary>
        /// <param name="fileName">The string fileName to save the raster to.</param>
        public virtual void SaveAs(string fileName)
        {
            SaveAs(fileName, DriverCode, Options);
        }

        /// <inheritdoc/>
        public virtual IRaster ReadBlock(int xOff, int yOff, int sizeX, int sizeY)
        {
            // Implemented in Subclass
            return null;
        }

        /// <inheritdoc/>
        public virtual void WriteBlock(IRaster blockValues, int xOff, int yOff, int xSize, int ySize)
        {
            // Implemented in subclass
        }

        /// <summary>
        /// This code is empty, but can be overridden in subtypes. This can be used to set the data of this raster to the data of the original raster.
        /// </summary>
        /// <param name="original">Original raster to get the value from.</param>
        public virtual void SetData(IRaster original)
        {
        }

        /// <summary>
        /// Instructs the file to write header changes only. This is espcially useful for changing the
        /// extents without altering the actual raster values themselves.
        /// </summary>
        public virtual void WriteHeader()
        {
            // Implemented in Sublcasses
        }

        /// <summary>
        /// Gets the no data cell count. Calls GetStatistics() internally.
        /// </summary>
        /// <returns>The number of cells which are equal to NoDataValue.</returns>
        public long GetNoDataCellCount()
        {
            GetStatistics();
            return (NumColumns * NumRows) - NumValueCells;
        }

        /// <summary>
        /// Opens the specified fileName. The DefaultDataManager will determine the best type of raster to handle the specified
        /// file based on the fileName or characteristics of the file.
        /// </summary>
        public virtual void Open()
        {
        }

        /// <summary>
        /// Saves the current raster to the specified file, using the specified driver, but with the
        /// options currently specified in the Options property.
        /// </summary>
        /// <param name="fileName">The string fileName to save this raster as</param>
        /// <param name="driverCode">The string driver code.</param>
        public virtual void SaveAs(string fileName, string driverCode)
        {
            SaveAs(fileName, driverCode, Options);
        }

        /// <summary>
        /// Saves the current raster to the specified file.
        /// </summary>
        /// <param name="fileName">The string fileName to save the current raster to.</param>
        /// <param name="driverCode">The driver code to use.</param>
        /// <param name="options">the string array of options that depend on the format.</param>
        public virtual void SaveAs(string fileName, string driverCode, string[] options)
        {
            // Create a new raster file
            IRaster newRaster = DataManager.DefaultDataManager.CreateRaster(fileName, driverCode, NumColumns, NumRows, NumBands, DataType, options);

            // Copy the file based values
            // newRaster.Copy(Filename, true);
            newRaster.Projection = Projection;

            newRaster.Extent = Extent;

            // Copy the in memory value
            newRaster.SetData(this);

            newRaster.ProgressHandler = ProgressHandler;
            newRaster.NoDataValue = NoDataValue;
            newRaster.GetStatistics();
            newRaster.Bounds = Bounds;

            // Save the in-memory values.
            newRaster.Save();
            newRaster.Close();
        }

        /// <summary>
        /// Gets this raster (or its Internal Raster) as the appropriately typed raster
        /// so that strong typed access methods are available, instead of just the regular methods.
        /// </summary>
        /// <typeparam name="T">The type (int, short, float, etc.)</typeparam>
        /// <returns>The Raster&lt;T&gt; where T are value types like int, short, float"/></returns>
        public Raster<T> ToRaster<T>()
            where T : IEquatable<T>, IComparable<T>
        {
            return this as Raster<T>;
        }

        /// <summary>
        /// Gets this raster (or its Internal Raster) as the appropriately typed raster
        /// so that strong typed access methods are available, instead of just the regular methods.
        /// </summary>
        /// <returns>A Raster&lt;int&gt;</returns>
        public Raster<int> ToIntRaster()
        {
            return ToRaster<int>();
        }

        /// <summary>
        /// Gets this raster (or its Internal Raster) as the appropriately typed raster
        /// so that strong typed access methods are available, instead of just the regular methods.
        /// </summary>
        /// <returns>A Raster&lt;short&gt;</returns>
        public Raster<short> ToShortRaster()
        {
            return ToRaster<short>();
        }

        /// <summary>
        /// Gets this raster (or its Internal Raster) as the appropriately typed raster
        /// so that strong typed access methods are available, instead of just the regular methods.
        /// </summary>
        /// <returns>A Raster&lt;float&gt;</returns>
        public Raster<float> ToFloatRaster()
        {
            return ToRaster<float>();
        }

        /// <summary>
        /// Overrides dispose to correctly handle disposing the objects at the raster level.
        /// </summary>
        /// <param name="disposeManagedResources">Indicatese whether managed resources should be disposed.</param>
        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                if (Bands != null)
                {
                    foreach (var r in Bands)
                    {
                        r.Dispose();
                    }

                    Bands = null;
                }

                Bounds = null;
                CustomFileType = null;
                DriverCode = null;
                Filename = null;
                Notes = null;
                Options = null;
                _rows = null;
                _tag = null;
                _values = null;
            }

            base.Dispose(disposeManagedResources);
        }

        /// <summary>
        /// Fires the FileExists method. If this returns true, then the action should be cancelled.
        /// </summary>
        /// <param name="fileName">The fileName to write to</param>
        /// <returns>Boolean, true if the user doesn't want to overwrite</returns>
        protected bool OnFileExists(string fileName)
        {
            if (FileExists != null)
            {
                MessageCancelEventArgs mc = new MessageCancelEventArgs(DataStrings.File0ExistsOverwrite.Replace("%S", fileName));
                FileExists(this, mc);
                return mc.Cancel;
            }

            return false;
        }

        #endregion
    }
}