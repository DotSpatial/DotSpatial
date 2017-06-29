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
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in January 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// A general
    /// </summary>
    public interface IRaster : IRasterBoundDataSet, ICloneable
    {
        #region Methods

        /// <summary>
        /// This only works for in-ram rasters.  This basically makes a new raster that has all the same
        /// in memory values
        /// </summary>
        /// <returns>An IRaster that is a duplicate of this class</returns>
        IRaster Copy();

        /// <summary>
        /// Creates a duplicate version of this file.  If copyValues is set to false, then a raster of NoData values is created
        /// that has the same georeferencing information as the source file of this Raster, even if this raster is just a window.
        /// </summary>
        /// <param name="fileName">The string fileName specifying where to create the new file.</param>
        /// <param name="copyValues">If this is false, the same size and georeferencing values are used, but they are all set to NoData.</param>
        void Copy(string fileName, bool copyValues);

        /// <summary>
        /// Even if this is a window, this will cause this raster to show statistics calculated from the entire file.
        /// </summary>
        void GetStatistics();

        /// <summary>
        /// Most raster methods are optimized for reading in lines or blocks at a time.  This one is designed
        /// to be used for scattered points.
        /// </summary>
        /// <param name="indices">The zero based integer index that is Row * NumColumnsInFile + Column</param>
        /// <returns>The list of double values</returns>
        List<double> GetValues(IEnumerable<long> indices);

        /// <summary>
        /// Returns a subset from the file that includes only the specified offsets.  The result is a raster,
        /// and the extents are calculated, but the row and column values are in terms of the window,
        /// not the original raster.  The band can be controlled by setting the "Current Band" first.
        /// </summary>
        /// <param name="xOff">X axis or horizontal offset (0 based from left)</param>
        /// <param name="yOff">Y axis or vertical offset (0 based from top)</param>
        /// <param name="sizeX">Number of columns</param>
        /// <param name="sizeY">Number of rows</param>
        /// <returns>An IRaster created from the appropriate type;.</returns>
        IRaster ReadBlock(int xOff, int yOff, int sizeX, int sizeY);

        /// <summary>
        /// This writes the values to the file, even if the IRaster has more values than the xSize or ySize
        /// stipulate, and even if the source raster has values of a different type.
        /// </summary>
        /// <param name="blockValues">The IRaster that contains data values to write to the file</param>
        /// <param name="xOff">The 0 based integer horizontal offset from the left</param>
        /// <param name="yOff">The 0 based integer vertical offset from the top</param>
        /// <param name="xSize">The integer number of columns to write</param>
        /// <param name="ySize">The integer number of rows to write</param>
        void WriteBlock(IRaster blockValues, int xOff, int yOff, int xSize, int ySize);

        /// <summary>
        /// Saves changes from any values that are in memory to the file.  This will preserve the existing
        /// structure and attempt to only write values to the parts of the file that match the loaded window.
        /// </summary>
        void Save();

        /// <summary>
        /// This will save whatever is specified in the startRow, endRow, startColumn, endColumn bounds
        /// to a new file with the specified fileName.
        /// </summary>
        /// <param name="fileName">The string fileName to save the current raster to.</param>
        void SaveAs(string fileName);

        /// <summary>
        /// This code is required to allow a new format to save data from an old format quickly.
        /// It essentially quickly transfers the underlying data values to the new raster.
        /// </summary>
        /// <param name="original"></param>
        void SetData(IRaster original);

        /// <summary>
        /// Gets this raster (or its Internal Raster) as the appropriately typed raster
        /// so that strong typed access methods are available, instead of just the
        /// regular methods, or null if the type is incorrect.  (Check datatype property).
        /// </summary>
        /// <typeparam name="T">The type (int, short, float, etc.)</typeparam>
        /// <returns>The Raster&lt;T&gt; where T are value types like int, short and float.  Returns null if the type is wrong.</returns>
        Raster<T> ToRaster<T>() where T : IEquatable<T>, IComparable<T>;

        /// <summary>
        /// Instructs the raster to write only the header content.  This is especially useful if you just want to update
        /// the extents or the projection of the raster, without changing the data values.
        /// </summary>
        void WriteHeader();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the integer size of each data member of the raster in bytes.
        /// </summary>
        int ByteSize
        {
            get;
        }

        /// <summary>
        /// Gets or sets the list of bands, which are in turn rasters.  The rasters
        /// contain only one band each, instead of the list of all the bands like the
        /// parent raster.
        /// </summary>
        IList<IRaster> Bands
        {
            get;
            set;
        }

        /// <summary>
        /// The geographic height of a cell the projected units.  Setting this will
        /// automatically adjust the affine coefficient to a negative value.
        /// </summary>
        double CellHeight
        {
            get;
            set;
        }

        /// <summary>
        /// The geographic width of a cell in the projected units
        /// </summary>
        double CellWidth
        {
            get;
            set;
        }

        /// <summary>
        /// This provides a zero-based integer band index that specifies which of the internal bands
        /// is currently being used for requests for data.
        /// </summary>
        int CurrentBand
        {
            get;
        }

        /// <summary>
        /// This does nothing unless the FileType property is set to custom.
        /// In such a case, this string allows new file types to be managed.
        /// </summary>
        string CustomFileType
        {
            get;
        }

        /// <summary>
        /// This reveals the underlying data type
        /// </summary>
        Type DataType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a short string to identify which driver to use.  This is primarilly used by GDAL rasters.
        /// </summary>
        string DriverCode
        {
            get;
            set;
        }

        /// <summary>
        /// The integer column index for the right column of this raster.  Most of the time this will
        /// be NumColumns - 1.  However, if this raster is a window taken from a larger raster, then
        /// it will be the index of the endColumn from the window.
        /// </summary>
        int EndColumn
        {
            get;
        }

        /// <summary>
        /// The integer row index for the end row of this raster.  Most of the time this will
        /// be numRows - 1.  However, if this raster is a window taken from a larger raster, then
        /// it will be the index of the endRow from the window.
        /// </summary>
        int EndRow
        {
            get;
        }

        /// <summary>
        /// Gets the complete path and fileName of the current file
        /// </summary>
        string Filename
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the file type of this grid.
        /// </summary>
        RasterFileType FileType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a boolean that is true if the data for this raster is in memory.
        /// </summary>
        bool IsInRam
        {
            get;
        }

        /// <summary>
        /// Gets the maximum data value, not counting no-data values in the grid.
        /// </summary>
        double Maximum
        {
            get;
        }

        /// <summary>
        /// Gets the mean of the non-NoData values in this grid.  If the data is not InRam, then
        /// the GetStatistics method must be called before these values will be correct.
        /// </summary>
        double Mean
        {
            get;
        }

        /// <summary>
        /// Gets the minimum data value that is not classified as a no data value in this raster.
        /// </summary>
        double Minimum
        {
            get;
        }

        /// <summary>
        /// A float showing the no-data values
        /// </summary>
        double NoDataValue
        {
            get;
            set;
        }

        /// <summary>
        /// For binary rasters this will get cut to only 256 characters.
        /// </summary>
        string Notes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the number of bands.  In most traditional grid formats, this is 1.  For RGB images,
        /// this would be 3.  Some formats may have many bands.
        /// </summary>
        int NumBands
        {
            get;
        }

        /// <summary>
        /// Gets the horizontal count of the cells in the raster.
        /// </summary>
        int NumColumns
        {
            get;
        }

        /// <summary>
        /// Gets the integer count of columns in the file, as opposed to the
        /// number represented by this raster, which might just represent a window.
        /// </summary>
        int NumColumnsInFile
        {
            get;
        }

        /// <summary>
        /// Gets the vertical count of the cells in the raster.
        /// </summary>
        int NumRows
        {
            get;
        }

        /// <summary>
        /// Gets the number of rows in the source file, as opposed to the number
        /// represented by this raster, which might just represent part of a file.
        /// </summary>
        int NumRowsInFile
        {
            get;
        }

        /// <summary>
        /// Gets the count of the cells that are not no-data.  If the data is not InRam, then
        /// you will have to first call the GetStatistics() method to gain meaningul values.
        /// </summary>
        long NumValueCells
        {
            get;
        }

        /// <summary>
        /// Gets or sets the string array of options to use when saving this raster.
        /// </summary>
        string[] Options
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a list of the rows in this raster that can be accessed independantly.
        /// </summary>
        List<IValueRow> Rows
        {
            get;
        }

        /// <summary>
        /// The integer column index for the left column of this raster.  Most of the time this will
        /// be 0.  However, if this raster is a window taken from a file, then
        /// it will be the row index in the file for the top row of this raster.
        /// </summary>
        int StartColumn
        {
            get;
        }

        /// <summary>
        /// The integer row index for the top row of this raster.  Most of the time this will
        /// be 0.  However, if this raster is a window taken from a file, then
        /// it will be the row index in the file for the left row of this raster.
        /// </summary>
        int StartRow
        {
            get;
        }

        /// <summary>
        /// Gets the standard deviation of all the Non-nodata cells.  If the data is not InRam,
        /// then you will have to first call the GetStatistics() method to get meaningful values.
        /// </summary>
        double StdDeviation
        {
            get;
        }

        /// <summary>
        /// This is provided for future developers to link this raster to other entities.
        /// It has no function internally, so it can be manipulated safely.
        /// </summary>
        object Tag
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value on the CurrentBand given a row and column undex
        /// </summary>
        IValueGrid Value
        {
            get;
            set;
        }

        /// <summary>
        /// The horizontal or longitude coordinate for the lower left cell in the grid
        /// </summary>
        double Xllcenter
        {
            get;
            set;
        }

        /// <summary>
        /// The vertical or latitude coordinate for the lower left cell in the grid
        /// </summary>
        double Yllcenter
        {
            get;
            set;
        }

        /// <summary>
        /// This doesn't perform calculations, but is simply a place holder where sample
        /// values can be stored and retrieved as a cache.
        /// </summary>
        IEnumerable<double> Sample { get; set; }

        /// <summary>
        /// A raster can contain predefined names for its categories
        /// </summary>
        /// <returns>null if raster has no category names</returns>
        String[] CategoryNames();

        /// <summary>
        /// A raster can contain predefined colors for its categories, for example NLCD GeoTIFF has a palette
        /// </summary>
        /// <returns>null if raster has no category colors</returns>
        System.Drawing.Color[] CategoryColors();

        #endregion

        #region Events

        /// <summary>
        /// Occurs when attempting to copy or save to a fileName that already exists.  A developer can tap into this event
        /// in order to display an appropriate message.  A cancel property allows the developer (and ultimately the user)
        /// decide if the specified event should ultimately be cancelled.
        /// </summary>
        event EventHandler<MessageCancelEventArgs> FileExists;

        #endregion
    }
}