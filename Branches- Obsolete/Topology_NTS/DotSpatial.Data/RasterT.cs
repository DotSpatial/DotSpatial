// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/1/2009 4:01:55 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// Raster
    /// </summary>
    public class Raster<T> : Raster where T : IEquatable<T>, IComparable<T>
    {
        #region Private Variables

        /// <summary>
        /// The actual data values, stored as a jagged array of values of type T
        /// </summary>
        public T[][] Data;

        /// <summary>
        /// This is the same as the "Value" member except that it is type specific.
        /// This also supports the "ToDouble" method.
        /// </summary>
        protected ValueGrid<T> ValuesT { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Raster
        /// </summary>
        public Raster()
        {
            DataType = typeof(T);
        }

        /// <summary>
        /// Creates an raster of data type T
        /// </summary>
        /// <param name="numRows">The number of rows in the raster</param>
        /// <param name="numColumns">The number of columns in the raster</param>
        /// <param name="valueGrid">The default ValueGrid only supports standard numeric types, but if a different kind of value grid is needed, this allows it.</param>
        public Raster(int numRows, int numColumns, ValueGrid<T> valueGrid)
        {
            base.NumRows = numRows;
            base.NumColumns = numColumns;
            base.IsInRam = true;
            DataType = typeof(T);
            if (numRows * numColumns > 64000000)
            {
                base.NumRowsInFile = numRows;
                base.NumColumnsInFile = numColumns;
                base.IsInRam = false;
                base.Bounds = new RasterBounds(numRows, numColumns, new[] { 0.5, 1.0, 0.0, numRows - .5, 0.0, -1.0 });
                base.NoDataValue = 0; // sets the no-data value to the minimum value for the specified type.
                ValuesT = valueGrid;
                base.Value = ValuesT;
                return;
            }

            Initialize();
        }

        /// <summary>
        /// Creates a raster of data type T.
        /// </summary>
        /// <param name="numRows">The number of rows in the raster</param>
        /// <param name="numColumns">The number of columns in the raster</param>
        public Raster(int numRows, int numColumns)
        {
            base.NumRows = numRows;
            base.NumColumns = numColumns;
            DataType = typeof(T);
            if (numRows * numColumns > 64000000)
            {
                base.IsInRam = false;
                base.NumRowsInFile = numRows;
                base.NumColumnsInFile = numColumns;
                base.IsInRam = false;
                base.Bounds = new RasterBounds(numRows, numColumns, new[] { 0.5, 1.0, 0.0, numRows - .5, 0.0, -1.0 });
                base.NoDataValue = 0; // sets the no-data value to the minimum value for the specified type.
                ValuesT = new ValueGrid<T>(this);
                base.Value = ValuesT;
                return;
            }
            base.IsInRam = true;

            Initialize();
        }

        /// <summary>
        /// Used especially by the "save as" situation, this simply creates a new reference pointer for the actual data values.
        /// </summary>
        /// <param name="original"></param>
        public override void SetData(IRaster original)
        {
            Raster<T> temp = original as Raster<T>;
            if (temp == null) return;
            Data = temp.Data;
            Value = temp.Value;
        }

        /// <summary>
        /// Calls the basic setup for the raster
        /// </summary>
        protected void Initialize()
        {
            StartRow = 0;
            EndRow = NumRows - 1;
            StartColumn = 0;
            EndColumn = NumColumns - 1;
            NumColumnsInFile = NumColumns;
            NumRowsInFile = NumRows;

            // Just set the cell size to one

            NumValueCells = 0;
            if (IsInRam)
            {
                Bounds = new RasterBounds(NumRows, NumColumns, new[] { 0.5, 1.0, 0.0, NumRows - .5, 0.0, -1.0 });
                Data = new T[NumRows][];
                for (int row = 0; row < NumRows; row++)
                {
                    Data[row] = new T[NumColumns];
                }
            }
            Value = new ValueGrid<T>(this);
            NoDataValue = Global.ToDouble(Global.MinimumValue<T>());
            DataType = typeof(T);
        }

        #endregion

        #region Methods

        /// <inheritdoc/>
        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                Data = null;
                ValuesT = null;
            }
            base.Dispose(disposeManagedResources);
        }

        /// <summary>
        /// Creates a deep copy of this raster object so that the data values can be manipulated without
        /// interfering with the original raster.
        /// </summary>
        /// <returns></returns>
        public new Raster<T> Copy()
        {
            Raster<T> copy = MemberwiseClone() as Raster<T>;
            if (copy == null) return null;
            copy.Bounds = Bounds.Copy();
            copy.Data = new T[NumRows][];
            for (int row = 0; row < NumRows; row++)
            {
                copy.Data[row] = new T[NumColumns];
                for (int col = 0; col < NumColumns; col++)
                {
                    copy.Data[row][col] = Data[row][col];
                }
            }
            copy.Value = new ValueGrid<T>(copy);
            copy.Bands = new List<IRaster>();
            foreach (IRaster band in base.Bands)
            {
                if (band != this)
                    copy.Bands.Add(band.Copy());
                else
                    copy.Bands.Add(copy);
            }
            return copy;
        }

        // ------------------------------------------FROM AND TO IN RAM ONLY -----------------
        /// <summary>
        /// This creates a completely new raster from the windowed domain on the original raster.  This new raster
        /// will not have a source file, and values like NumRowsInFile will correspond to the in memory version.
        /// All the values will be copied to the new source file.  InRam must be true at this level.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="startRow">The 0 based integer index of the top row to copy from this raster.  If this raster is itself a window, 0 represents the startRow from the file.</param>
        /// <param name="endRow">The integer index of the bottom row to copy from this raster.  The largest allowed value is NumRows - 1.</param>
        /// <param name="startColumn">The 0 based integer index of the leftmost column to copy from this raster.  If this raster is a window, 0 represents the startColumn from the file.</param>
        /// <param name="endColumn">The 0 based integer index of the rightmost column to copy from this raster.  The largest allowed value is NumColumns - 1</param>
        /// <param name="copyValues">If this is true, the values are saved to the file.  If this is false and the data can be loaded into Ram, no file handling is done.  Otherwise, a file of NoData values is created.</param>
        /// <param name="inRam">Boolean.  If this is true and the window is small enough, a copy of the values will be loaded into memory.</param>
        /// <returns>An implementation of IRaster</returns>
        public IRaster CopyWindow(string fileName, int startRow, int endRow, int startColumn, int endColumn,
                                  bool copyValues, bool inRam)
        {
            if (inRam == false || (endColumn - startColumn + 1) * (endRow - startRow + 1) > 64000000)
                throw new ArgumentException(DataStrings.RasterRequiresCast);
            if (IsInRam == false)
                throw new ArgumentException(DataStrings.RasterRequiresCast);
            int numCols = endColumn - startColumn + 1;
            int numRows = endRow - startRow + 1;

            var result = new Raster<T>(numRows, numCols);

            result.Projection = Projection;

            // The affine coefficients defining the world file are the same except that they are translated over.  Only the position of the
            // upper left corner changes.  Everything else is the same as the previous raster.
            result.Bounds.AffineCoefficients = new AffineTransform(Bounds.AffineCoefficients).TransfromToCorner(startColumn, startRow);

            ProgressMeter pm = new ProgressMeter(ProgressHandler, DataStrings.CopyingValues, numRows);
            // copy values directly using both data structures
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    result.Data[row][col] = Data[startRow + row][startColumn + col];
                }
                pm.CurrentValue = row;
            }
            pm.Reset();
            result.Value = new ValueGrid<T>(result);
            return result;
        }

        /// <summary>
        /// Gets the statistics all the values.  If the entire content is not currently in-ram,
        /// ReadRow will be used to read individual lines and performing the calculations.
        /// </summary>
        public override void GetStatistics()
        {
            ProgressMeter pm = new ProgressMeter(ProgressHandler, DataStrings.CalculatingStatistics, NumRows);

            T min = Global.MaximumValue<T>();
            T max = Global.MinimumValue<T>();

            double total = 0;
            double sqrTotal = 0;
            int count = 0;

            if (IsInRam == false || this.IsFullyWindowed() == false)
            {
                for (int row = 0; row < NumRowsInFile; row++)
                {
                    T[] values = ReadRow(row);
                    for (int col = 0; col < NumColumnsInFile; col++)
                    {
                        T val = values[col];
                        double dblVal = Global.ToDouble(val);
                        if (dblVal == NoDataValue) continue;
                        if (val.CompareTo(max) > 0) max = val;
                        if (val.CompareTo(min) < 0) min = val;
                        total += dblVal;
                        sqrTotal += dblVal * dblVal;
                        count++;
                    }
                    pm.CurrentValue = row;
                }
            }
            else
            {
                for (int row = 0; row < NumRows; row++)
                {
                    for (int col = 0; col < NumColumns; col++)
                    {
                        T val = Data[row][col];
                        double dblVal = Global.ToDouble(val);
                        if (dblVal == NoDataValue)
                        {
                            continue;
                        }
                        if (val.CompareTo(max) > 0) max = val;
                        if (val.CompareTo(min) < 0) min = val;
                        total += dblVal;
                        sqrTotal += dblVal * dblVal;
                        count++;
                    }
                    pm.CurrentValue = row;
                }
            }

            double test = Global.ToDouble(min);

            Value.Updated = false;
            Minimum = test;
            Maximum = Global.ToDouble(max);
            Mean = total / count;
            NumValueCells = count;
            StdDeviation = (float)Math.Sqrt((sqrTotal / NumValueCells) - (total / NumValueCells) * (total / NumValueCells));

            pm.Reset();
        }

        // ----------------------------------- FROM AND TO IN RAM ONLY ---------------------------------
        /// <summary>
        /// This creates an IN MEMORY ONLY window from the in-memory window of this raster.  If, however, the requested range
        /// is outside of what is contained in the in-memory portions of this raster, an appropriate cast
        /// is required to ensure that you have the correct File handling, like a BinaryRaster etc.
        /// </summary>
        /// <param name="startRow">The 0 based integer index of the top row to get from this raster.  If this raster is itself a window, 0 represents the startRow from the file.</param>
        /// <param name="endRow">The integer index of the bottom row to get from this raster.  The largest allowed value is NumRows - 1.</param>
        /// <param name="startColumn">The 0 based integer index of the leftmost column to get from this raster.  If this raster is a window, 0 represents the startColumn from the file.</param>
        /// <param name="endColumn">The 0 based integer index of the rightmost column to get from this raster.  The largest allowed value is NumColumns - 1</param>
        /// <param name="inRam">Boolean.  If this is true and the window is small enough, a copy of the values will be loaded into memory.</param>
        /// <returns>An implementation of IRaster</returns>
        public IRaster GetWindow(int startRow, int endRow, int startColumn, int endColumn, bool inRam)
        {
            if (IsInRam == false)
                throw new ArgumentException(DataStrings.RasterRequiresCast);
            if (startRow < StartRow || endRow > EndRow || StartColumn < startColumn || EndColumn > endColumn)
            {
                // the requested extents are outside of the extents that have been windowed into ram.  File Handling is required.
                throw new ArgumentException(DataStrings.RasterRequiresCast);
            }

            int numCols = endColumn - startColumn + 1;
            int numRows = endRow - startRow + 1;
            Raster<T> result = new Raster<T>(numRows, numCols);
            result.Filename = Filename;
            result.Projection = Projection;
            result.DataType = typeof(int);
            result.NumRows = numRows;
            result.NumColumns = numCols;
            result.NumRowsInFile = NumRowsInFile;
            result.NumColumnsInFile = NumColumnsInFile;
            result.NoDataValue = NoDataValue;
            result.StartColumn = startColumn;
            result.StartRow = startRow;
            result.EndColumn = endColumn;
            result.EndRow = EndRow;
            result.FileType = FileType;

            // Reposition the new "raster" so that it matches the specified window, not the whole raster
            result.Bounds.AffineCoefficients = new AffineTransform(Bounds.AffineCoefficients).TransfromToCorner(startColumn, startRow);
            // Now we can copy any values currently in memory.

            ProgressMeter pm = new ProgressMeter(ProgressHandler, DataStrings.CopyingValues, endRow);
            pm.StartValue = startRow;
            // copy values directly using both data structures
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    result.Data[row][col] = Data[startRow + row][startColumn + col];
                }
                pm.CurrentValue = row;
            }
            pm.Reset();

            result.Value = new ValueGrid<T>(result);
            return result;
        }

        /// <summary>
        /// Obtains only the statistics for the small window specified by startRow, endRow etc.
        /// this only works if the window is also InRam.
        /// </summary>
        public void GetWindowStatistics()
        {
            if (IsInRam == false)
                throw new ArgumentException(DataStrings.RasterRequiresCast);

            ProgressMeter pm = new ProgressMeter(ProgressHandler, "Calculating Statistics.", NumRows);

            double total = 0;
            double sqrTotal = 0;
            int count = 0;
            T min = Global.MaximumValue<T>();
            T max = Global.MinimumValue<T>();

            for (int row = 0; row < NumRows; row++)
            {
                for (int col = 0; col < NumColumns; col++)
                {
                    T val = Data[row][col];
                    double dblVal = Global.ToDouble(val);
                    if (dblVal != NoDataValue)
                    {
                        if (val.CompareTo(max) > 0) max = val;
                        if (val.CompareTo(min) < 0) min = val;

                        total += dblVal;
                        sqrTotal += dblVal * dblVal;
                        count++;
                    }
                }
                pm.CurrentValue = row;
            }

            Value.Updated = false;
            Minimum = Global.ToDouble(min);
            Maximum = Global.ToDouble(max);
            NumValueCells = count;
            StdDeviation = (float)Math.Sqrt((sqrTotal / NumValueCells) - (total / NumValueCells) * (total / NumValueCells));
        }

        /// <summary>
        /// Prevent the base raster "factory" style open function from working in subclasses.
        /// </summary>
        public override void Open()
        {
            if (IsInRam)
            {
                Data = ReadRaster();
                GetStatistics();
            }
        }

        /// <summary>
        /// This saves content from memory stored in the Data field to the file using whatever
        /// file format the file already exists as.
        /// </summary>
        public override void Save()
        {
            UpdateHeader();
            WriteRaster(Data);
        }

        /// <summary>
        /// Reads a specific
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public T[] ReadRow(int row)
        {
            return ReadRaster(0, row, NumColumns, 1)[0];
        }

        /// <summary>
        /// This method reads the values from the entire band into an array and returns the array as a single array.
        /// This assumes 0 offsets, the size of the entire image, and 0 for the pixel or line space.
        /// </summary>
        /// <returns>An array of values of type T, in row major order</returns>
        public T[][] ReadRaster()
        {
            return ReadRaster(0, 0, NumColumns, NumRows);
        }

        /// <summary>
        /// Most reading is optimized to read in a block at a time and process it.  This method is designed
        /// for seeking through the file.  It should work faster than the buffered methods in cases where
        /// an unusually arranged collection of values are required.  Sorting the list before calling
        /// this should significantly improve performance.
        /// </summary>
        /// <param name="indices">A list or array of long values that are (Row * NumRowsInFile + Column)</param>
        public virtual List<T> GetValuesT(IEnumerable<long> indices)
        {
            if (IsInRam)
            {
                List<T> values = (from index in indices
                                  let row = (int)Math.Floor(index / (double)NumColumnsInFile)
                                  let col = (int)index % NumColumnsInFile
                                  select Data[row][col]).ToList();
                return values;
            }
            // This code should never be called because it should get replaced
            // by file access code that is optimized, but if not, then this will be slow.
            return (from index in indices
                    let row = (int)Math.Floor(index / (double)NumColumnsInFile)
                    let col = (int)index % NumColumnsInFile
                    select (T)Convert.ChangeType(Value[row, col], typeof(T))).ToList();
        }

        // There is no need to override this any further, but GetValuesT should be implemented more intelligently in subclasses.
        /// <inheritdoc/>
        public override List<double> GetValues(IEnumerable<long> indices)
        {
            List<T> vals = GetValuesT(indices);
            return vals.Select(Global.ToDouble).ToList();
        }

        /// <inheritdoc/>
        public override IRaster ReadBlock(int xOff, int yOff, int sizeX, int sizeY)
        {
            Raster<T> result = new Raster<T>(sizeY, sizeX);
            result.Data = ReadRaster(xOff, yOff, sizeX, sizeY);
            Coordinate topLeft = Bounds.CellCenter_ToProj(yOff, xOff);
            double[] aff = new double[6];
            Array.Copy(Bounds.AffineCoefficients, 0, aff, 0, 6);
            aff[0] = topLeft.X;
            aff[3] = topLeft.Y;
            result.Bounds = new RasterBounds(sizeX, sizeY, aff);
            result.NoDataValue = NoDataValue;
            result.Projection = Projection;
            result.IsInRam = true;
            return result;
        }

        /// <summary>
        /// This Method should be overrridden by classes, and provides the primary ability.
        /// </summary>
        /// <param name="xOff">The horizontal offset of the area to read values from.</param>
        /// <param name="yOff">The vertical offset of the window to read values from.</param>
        /// <param name="sizeX">The number of values to read into the buffer.</param>
        /// <param name="sizeY">The vertical size of the window to read into the buffer.</param>
        /// <returns>The jagged array of raster values of type T.</returns>
        public virtual T[][] ReadRaster(int xOff, int yOff, int sizeX, int sizeY)
        {
            throw new NotImplementedException("This should be overridden by classes that specify a file format.");
        }

        /// <summary>
        /// Reads a specific
        /// </summary>
        /// <param name="buffer">The one dimensional array of values containing all the data for this particular content.</param>
        /// <param name="row">The integer row to write to the raster</param>
        public void WriteRow(T[] buffer, int row)
        {
            T[][] bufferJagged = new T[1][];
            bufferJagged[0] = buffer;
            WriteRaster(bufferJagged, 0, row, NumColumns, 1);
        }

        /// <summary>
        /// This method reads the values from the entire band into an array and returns the array as a single array.
        /// This assumes 0 offsets, the size of the entire image, and 0 for the pixel or line space.
        /// </summary>
        /// <param name="buffer">The one dimensional array of values containing all the data for this particular content.</param>
        public void WriteRaster(T[][] buffer)
        {
            WriteRaster(buffer, 0, 0, NumColumns, NumRows);
        }

        /// <summary>
        /// This method reads the values from the entire band into an array and returns the array as a single array.
        /// This specifies a window where the xSize and ySize specified and 0 is used for the pixel and line space.
        /// </summary>
        /// <param name="buffer">The one dimensional array of values containing all the data for this particular content.</param>
        /// <param name="xOff">The horizontal offset of the area to read values from.</param>
        /// <param name="yOff">The vertical offset of the window to read values from.</param>
        /// <param name="xSize">The number of values to read into the buffer.</param>
        /// <param name="ySize">The vertical size of the window to read into the buffer.</param>
        public virtual void WriteRaster(T[][] buffer, int xOff, int yOff, int xSize, int ySize)
        {
            throw new NotImplementedException("This should be overridden by classes that specify a file format.");
        }

        /// <inheritdoc/>
        public override void WriteBlock(IRaster blockValues, int xOff, int yOff, int xSize, int ySize)
        {
            Raster<T> source = blockValues as Raster<T>;
            if (source != null)
            {
                if (ySize == source.NumColumns && xSize == source.NumColumns)
                {
                    WriteRaster(source.Data, xOff, yOff, xSize, ySize);
                }
                else
                {
                    T[][] values = new T[ySize][];
                    // data is of the same type so just set the values.
                    for (int row = 0; row < ySize; row++)
                    {
                        if (xSize == NumColumns)
                        {
                            values[row] = source.Data[row];
                        }
                        else
                        {
                            values[row] = new T[xSize];
                            Array.Copy(source.Data[row], 0, values[row], 0, xSize);
                        }
                    }
                    WriteRaster(values, xOff, yOff, xSize, ySize);
                }
            }
            else
            {
                T[][] values = new T[ySize][];
                // data is of the same type so just set the values.
                for (int row = 0; row < ySize; row++)
                {
                    for (int col = 0; col < xSize; col++)
                    {
                        values[row][col] = (T)Convert.ChangeType(blockValues.Value[row, col], typeof(T));
                    }
                }
                WriteRaster(values, xOff, yOff, xSize, ySize);
            }
        }

        /// <summary>
        /// During a save opperation, this instructs the program to perform any writing that is not handled by
        /// the write raster content.
        /// </summary>
        protected virtual void UpdateHeader()
        {
            throw new NotImplementedException("This should be overridden by classes that specify a file format.");
        }

        #endregion

        #region Properties

        /// <summary>
        /// This only works for a few numeric types, and will return 0 if it is not identifiable as one
        /// of these basic types: byte, short, int, long, float, double, decimal, UInt16, UInt32, UInt64,
        /// </summary>
        public override int ByteSize
        {
            get
            {
                return GetByteSize(default(T));
            }
        }

        private static int GetByteSize(object value)
        {
            if (value is byte) return 1;
            if (value is short) return 2;
            if (value is int) return 4;
            if (value is long) return 8;
            if (value is float) return 4;
            if (value is double) return 8;
            if (value is decimal) return 16;
            if (value is UInt16) return 2;
            if (value is UInt32) return 4;
            if (value is UInt64) return 8;

            return 0;
        }

        #endregion
    }
}