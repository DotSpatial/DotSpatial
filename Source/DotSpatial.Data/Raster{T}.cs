// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NetTopologySuite.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// A raster of the given type.
    /// </summary>
    /// <typeparam name="T">Type of the raster.</typeparam>
    public class Raster<T> : Raster
        where T : IEquatable<T>, IComparable<T>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Raster{T}"/> class.
        /// </summary>
        public Raster()
        {
            DataType = typeof(T);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Raster{T}"/> class.
        /// </summary>
        /// <param name="numRows">The number of rows in the raster.</param>
        /// <param name="numColumns">The number of columns in the raster.</param>
        /// <param name="valueGrid">The default ValueGrid only supports standard numeric types, but if a different kind of value grid is needed, this allows it.</param>
        public Raster(int numRows, int numColumns, ValueGrid<T> valueGrid)
        {
            NumRows = numRows;
            NumColumns = numColumns;
            IsInRam = true;
            DataType = typeof(T);
            if (numRows * numColumns > 64000000)
            {
                NumRowsInFile = numRows;
                NumColumnsInFile = numColumns;
                IsInRam = false;
                Bounds = new RasterBounds(numRows, numColumns, new[] { 0.5, 1.0, 0.0, numRows - .5, 0.0, -1.0 });
                NoDataValue = 0; // sets the no-data value to the minimum value for the specified type.
                ValuesT = valueGrid;
                Value = ValuesT;
                return;
            }

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Raster{T}"/> class.
        /// </summary>
        /// <param name="numRows">The number of rows in the raster.</param>
        /// <param name="numColumns">The number of columns in the raster.</param>
        public Raster(int numRows, int numColumns)
        {
            NumRows = numRows;
            NumColumns = numColumns;
            DataType = typeof(T);
            if (numRows * numColumns > 64000000)
            {
                IsInRam = false;
                NumRowsInFile = numRows;
                NumColumnsInFile = numColumns;
                IsInRam = false;
                Bounds = new RasterBounds(numRows, numColumns, new[] { 0.5, 1.0, 0.0, numRows - .5, 0.0, -1.0 });
                NoDataValue = 0; // sets the no-data value to the minimum value for the specified type.
                ValuesT = new ValueGrid<T>(this);
                Value = ValuesT;
                return;
            }

            IsInRam = true;
            Initialize();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the size of each raster element in bytes.
        /// </summary>
        /// <remarks>
        /// This only works for a few numeric types, and will return 0 if it is not identifiable as one
        /// of these basic types: byte, short, int, long, float, double, decimal, sbyte, ushort, uint, ulong, bool.
        /// </remarks>
        public override int ByteSize => GetByteSize(default(T));

        /// <summary>
        /// Gets or sets the actual data values, stored as a jagged array of values of type T.
        /// </summary>
        public T[][] Data { get; set; }

        /// <summary>
        /// Gets or sets the values. This is the same as the "Value" member except that it is type specific.
        /// This also supports the "ToDouble" method.
        /// </summary>
        protected ValueGrid<T> ValuesT { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a deep copy of this raster object so that the data values can be manipulated without
        /// interfering with the original raster.
        /// </summary>
        /// <returns>A deep copy of this raster object.</returns>
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
            foreach (IRaster band in Bands)
            {
                copy.Bands.Add(band != this ? band.Copy() : copy);
            }

            return copy;
        }

        /// <summary>
        /// This creates a completely new raster from the windowed domain on the original raster. This new raster
        /// will not have a source file, and values like NumRowsInFile will correspond to the in memory version.
        /// All the values will be copied to the new source file. InRam must be true at this level.
        /// </summary>
        /// <param name="fileName">Name of the file whose data gets copied.</param>
        /// <param name="startRow">The 0 based integer index of the top row to copy from this raster. If this raster is itself a window, 0 represents the startRow from the file.</param>
        /// <param name="endRow">The integer index of the bottom row to copy from this raster. The largest allowed value is NumRows - 1.</param>
        /// <param name="startColumn">The 0 based integer index of the leftmost column to copy from this raster. If this raster is a window, 0 represents the startColumn from the file.</param>
        /// <param name="endColumn">The 0 based integer index of the rightmost column to copy from this raster. The largest allowed value is NumColumns - 1.</param>
        /// <param name="copyValues">If this is true, the values are saved to the file. If this is false and the data can be loaded into Ram, no file handling is done. Otherwise, a file of NoData values is created.</param>
        /// <param name="inRam">Boolean. If this is true and the window is small enough, a copy of the values will be loaded into memory.</param>
        /// <returns>An implementation of IRaster.</returns>
        public IRaster CopyWindow(string fileName, int startRow, int endRow, int startColumn, int endColumn, bool copyValues, bool inRam)
        {
            if (inRam == false || (endColumn - startColumn + 1) * (endRow - startRow + 1) > 64000000) throw new ArgumentException(DataStrings.RasterRequiresCast);
            if (IsInRam == false) throw new ArgumentException(DataStrings.RasterRequiresCast);

            int numCols = endColumn - startColumn + 1;
            int numRows = endRow - startRow + 1;

            var result = new Raster<T>(numRows, numCols)
            {
                Projection = Projection,
                Bounds =
                {
                    // The affine coefficients defining the world file are the same except that they are translated over. Only the position of the
                    // upper left corner changes. Everything else is the same as the previous raster.
                    AffineCoefficients = new AffineTransform(Bounds.AffineCoefficients).TransfromToCorner(startColumn, startRow)
                }
            };

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
        /// Gets the statistics of all the values. If the entire content is not currently in-ram,
        /// ReadRow will be used to read individual lines and perform the calculations.
        /// </summary>
        public override void GetStatistics()
        {
            ProgressMeter pm = new ProgressMeter(ProgressHandler, DataStrings.CalculatingStatistics, NumRows);

            T min = Global.MaximumValue<T>();
            T max = Global.MinimumValue<T>();

            double total = 0;
            double sqrTotal = 0;
            int count = 0;

            if (!IsInRam || !this.IsFullyWindowed())
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

            Value.Updated = false;
            Minimum = Global.ToDouble(min);
            Maximum = Global.ToDouble(max);
            Mean = total / count;
            NumValueCells = count;
            StdDeviation = (float)Math.Sqrt((sqrTotal / NumValueCells) - (total / NumValueCells * (total / NumValueCells)));

            pm.Reset();
        }

        /// <inheritdoc/>
        public override List<double> GetValues(IEnumerable<long> indices)
        {
            // There is no need to override this any further, but GetValuesT should be implemented more intelligently in subclasses.
            List<T> vals = GetValuesT(indices);
            return vals.Select(Global.ToDouble).ToList();
        }

        /// <summary>
        /// Most reading is optimized to read in a block at a time and process it. This method is designed for seeking through the file.
        /// It should work faster than the buffered methods in cases where an unusually arranged collection of values are required.
        /// Sorting the list before calling this should significantly improve performance.
        /// </summary>
        /// <param name="indices">A list or array of long values that are (Row * NumRowsInFile + Column).</param>
        /// <returns>List of the gotten values.</returns>
        public virtual List<T> GetValuesT(IEnumerable<long> indices)
        {
            if (IsInRam)
            {
                List<T> values = (from index in indices let row = (int)Math.Floor(index / (double)NumColumnsInFile) let col = (int)(index % NumColumnsInFile) select Data[row][col]).ToList();
                return values;
            }

            // This code should never be called because it should get replaced
            // by file access code that is optimized, but if not, then this will be slow.
            return (from index in indices let row = (int)Math.Floor(index / (double)NumColumnsInFile) let col = (int)(index % NumColumnsInFile) select (T)Convert.ChangeType(Value[row, col], typeof(T))).ToList();
        }

        /// <summary>
        /// This creates an IN MEMORY ONLY window from the in-memory window of this raster. If, however, the requested range
        /// is outside of what is contained in the in-memory portions of this raster, an appropriate cast
        /// is required to ensure that you have the correct File handling, like a BinaryRaster etc.
        /// </summary>
        /// <param name="startRow">The 0 based integer index of the top row to get from this raster. If this raster is itself a window, 0 represents the startRow from the file.</param>
        /// <param name="endRow">The integer index of the bottom row to get from this raster. The largest allowed value is NumRows - 1.</param>
        /// <param name="startColumn">The 0 based integer index of the leftmost column to get from this raster. If this raster is a window, 0 represents the startColumn from the file.</param>
        /// <param name="endColumn">The 0 based integer index of the rightmost column to get from this raster. The largest allowed value is NumColumns - 1.</param>
        /// <param name="inRam">Boolean. If this is true and the window is small enough, a copy of the values will be loaded into memory.</param>
        /// <returns>An implementation of IRaster.</returns>
        public IRaster GetWindow(int startRow, int endRow, int startColumn, int endColumn, bool inRam)
        {
            if (IsInRam == false) throw new ArgumentException(DataStrings.RasterRequiresCast);
            if (startRow < StartRow || endRow > EndRow || StartColumn < startColumn || EndColumn > endColumn)
            {
                // the requested extents are outside of the extents that have been windowed into ram. File Handling is required.
                throw new ArgumentException(DataStrings.RasterRequiresCast);
            }

            int numCols = endColumn - startColumn + 1;
            int numRows = endRow - startRow + 1;
            Raster<T> result = new Raster<T>(numRows, numCols)
            {
                Filename = Filename,
                Projection = Projection,
                DataType = typeof(int),
                NumRows = numRows,
                NumColumns = numCols,
                NumRowsInFile = NumRowsInFile,
                NumColumnsInFile = NumColumnsInFile,
                NoDataValue = NoDataValue,
                StartColumn = startColumn,
                StartRow = startRow,
                EndColumn = endColumn,
                EndRow = EndRow,
                FileType = FileType,
                Bounds =
                                   {
                                       AffineCoefficients = new AffineTransform(Bounds.AffineCoefficients).TransfromToCorner(startColumn, startRow)
                                   }
            };

            // Reposition the new "raster" so that it matches the specified window, not the whole raster

            // Now we can copy any values currently in memory.
            ProgressMeter pm = new ProgressMeter(ProgressHandler, DataStrings.CopyingValues, endRow)
            {
                StartValue = startRow
            };

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
        /// This only works if the window is also InRam.
        /// </summary>
        public void GetWindowStatistics()
        {
            if (IsInRam == false) throw new ArgumentException(DataStrings.RasterRequiresCast);

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
            StdDeviation = (float)Math.Sqrt((sqrTotal / NumValueCells) - (total / NumValueCells * (total / NumValueCells)));
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

        /// <inheritdoc/>
        public override IRaster ReadBlock(int xOff, int yOff, int sizeX, int sizeY)
        {
            Raster<T> result = new Raster<T>(sizeY, sizeX)
            {
                Data = ReadRaster(xOff, yOff, sizeX, sizeY)
            };
            Coordinate topLeft = Bounds.CellCenterToProj(yOff, xOff);
            double[] aff = new double[6];
            Array.Copy(Bounds.AffineCoefficients, 0, aff, 0, 6);
            aff[0] = topLeft.X;
            aff[3] = topLeft.Y;
            result.Bounds = new RasterBounds(sizeY, sizeX, aff);
            result.NoDataValue = NoDataValue;
            result.Projection = Projection;
            result.IsInRam = true;
            return result;
        }

        /// <summary>
        /// This method reads the values from the entire band into an array and returns the array as a single array.
        /// This assumes 0 offsets, the size of the entire image, and 0 for the pixel or line space.
        /// </summary>
        /// <returns>An array of values of type T, in row major order.</returns>
        public T[][] ReadRaster()
        {
            return ReadRaster(0, 0, NumColumns, NumRows);
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
        /// Reads a specific row.
        /// </summary>
        /// <param name="row">The vertical offset of the window to read values from.</param>
        /// <returns>The array of raster values of type T belonging to the row.</returns>
        public T[] ReadRow(int row)
        {
            return ReadRaster(0, row, NumColumns, 1)[0];
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
        /// Used especially by the "save as" situation, this simply creates a new reference pointer for the actual data values.
        /// </summary>
        /// <param name="original">The original the data is gotten from.</param>
        public override void SetData(IRaster original)
        {
            Raster<T> temp = original as Raster<T>;
            if (temp == null) return;

            Data = temp.Data;
            Value = temp.Value;
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

        /// <summary>
        /// Reads a specific.
        /// </summary>
        /// <param name="buffer">The one dimensional array of values containing all the data for this particular content.</param>
        /// <param name="row">The integer row to write to the raster.</param>
        public void WriteRow(T[] buffer, int row)
        {
            T[][] bufferJagged = new T[1][];
            bufferJagged[0] = buffer;
            WriteRaster(bufferJagged, 0, row, NumColumns, 1);
        }

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
        /// Calls the basic setup for the raster.
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

        /// <summary>
        /// During a save opperation, this instructs the program to perform any writing that is not handled by
        /// the write raster content.
        /// </summary>
        protected virtual void UpdateHeader()
        {
            throw new NotImplementedException("This should be overridden by classes that specify a file format.");
        }

        private static int GetByteSize(object value)
        {
            if (value is byte) return 1;
            if (value is short) return 2;
            if (value is int) return 4;
            if (value is long) return 8;
            if (value is float) return 4;
            if (value is double) return 8;

            if (value is sbyte) return 1;
            if (value is ushort) return 2;
            if (value is uint) return 4;
            if (value is ulong) return 8;

            if (value is bool) return 1;

            return 0;
        }

        #endregion
    }
}