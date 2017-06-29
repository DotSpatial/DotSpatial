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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using DotSpatial.Projections;
using MiscUtil;

namespace DotSpatial.Data
{
    /// <summary>
    /// This basically demonstrates how you would combine a type with a calculator in order to
    /// construct a Binary Raster for the Integer type.  It is effectively the same as
    /// constructing a new BinaryRaster and specifying the parameter types int and IntCalculator.
    /// </summary>
    internal class BinaryRaster<T> : Raster<T> where T : struct, IEquatable<T>, IComparable<T>
    {
        public static readonly T MaxValue = ReadStaticField("MaxValue");
        public static readonly T MinValue = ReadStaticField("MinValue");

        #region Constructors

        /// <summary>
        /// Creates a completely empty raster that can be custom configured
        /// </summary>
        public BinaryRaster()
        {
            // This is basically an empty place holder until someone calls an Open or something.
        }

        /// <summary>
        /// Creates a new BinaryRaster with the specified rows and columns.
        /// If if the raster is less than 64 Million cells, it will be created only in memory,
        /// and a Save method should be called when ready to save it to a file.  Otherwise, it creates a blank file with
        /// NoData values...which start out as 0.
        /// </summary>
        /// <param name="fileName">The fileName to write to</param>
        /// <param name="numRows">Integer number of rows</param>
        /// <param name="numColumns">Integer number of columns</param>
        public BinaryRaster(string fileName, int numRows, int numColumns) :
            this(fileName, numRows, numColumns, true)
        {
            // this just forces the inRam to default to true.
        }

        /// <summary>
        /// Creates a new BinaryRaster with the specified rows and columns.
        /// If inRam is true and the raster is less than 64 Million cells, it will be created only in memory,
        /// and a Save method should be called when ready to save it to a file.  Otherwise, it creates a blank file with
        /// NoData values.
        /// </summary>
        /// <param name="fileName">The fileName to write to</param>
        /// <param name="numRows">Integer number of rows</param>
        /// <param name="numColumns">Integer number of columns</param>
        /// <param name="inRam">If this is true and the raster is small enough, it will load this into memory and not save anything to the file.</param>
        public BinaryRaster(string fileName, int numRows, int numColumns, bool inRam)
        {
            if (File.Exists(fileName))
                base.Filename = fileName;
            if (inRam && numColumns * numRows < 64000000)
                base.IsInRam = true;
            else
                base.IsInRam = false;
            base.NumRows = numRows;
            base.NumColumns = numColumns;
            Initialize();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a duplicate version of this file.  If copyValues is set to false, then a raster of NoData values is created
        /// that has the same georeferencing information as the source file of this Raster, even if this raster is just a window.
        /// If the specified fileName exists, rather than throwing an exception or taking an "overwrite" parameter, this
        /// will throw the FileExists event, and cancel the copy if the cancel argument is set to true.
        /// </summary>
        /// <param name="fileName">The string fileName specifying where to create the new file.</param>
        /// <param name="copyValues">If this is false, the same size and georeferencing values are used, but they are all set to NoData.</param>
        public override void Copy(string fileName, bool copyValues)
        {
            if (fileName == Filename)
                throw new ArgumentException(DataStrings.CannotCopyToSelf_S.Replace("%S", fileName));
            if (File.Exists(fileName))
            {
                if (OnFileExists(fileName))
                    return; // The copy event was cancelled
                // The copy event was not cancelled, so overwrite the file
                File.Delete(fileName);
            }
            if (copyValues)
            {
                // this should be faster than copying values in code
                File.Copy(Filename, fileName);
            }
            else
            {
                // since at this point, there is no file, a blank file will be created with empty values.
                Write(fileName);
            }
        }

        /// <summary>
        /// This creates a completely new raster from the windowed domain on the original raster.  This new raster
        /// will have a separate source file, and values like NumRowsInFile will correspond to the newly created file.
        /// All the values will be copied to the new source file.  If inRam = true and the new raster is small enough,
        /// the raster values will be loaded into memory.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="startRow">The 0 based integer index of the top row to copy from this raster.  If this raster is itself a window, 0 represents the startRow from the file.</param>
        /// <param name="endRow">The integer index of the bottom row to copy from this raster.  The largest allowed value is NumRows - 1.</param>
        /// <param name="startColumn">The 0 based integer index of the leftmost column to copy from this raster.  If this raster is a window, 0 represents the startColumn from the file.</param>
        /// <param name="endColumn">The 0 based integer index of the rightmost column to copy from this raster.  The largest allowed value is NumColumns - 1</param>
        /// <param name="copyValues">If this is true, the values are saved to the file.  If this is false and the data can be loaded into Ram, no file handling is done.  Otherwise, a file of NoData values is created.</param>
        /// <param name="inRam">Boolean.  If this is true and the window is small enough, a copy of the values will be loaded into memory.</param>
        /// <returns>An implementation of IRaster</returns>
        public new IRaster CopyWindow(string fileName, int startRow, int endRow, int startColumn, int endColumn,
                                      bool copyValues, bool inRam)
        {
            int numCols = endColumn - startColumn + 1;
            int numRows = endRow - startRow + 1;

            var result = new BinaryRaster<T>(fileName, numCols, numRows, inRam) {Projection = Projection};

            // The affine coefficients defining the world file are the same except that they are translated over.  Only the position of the
            // upper left corner changes.  Everything else is the same as the previous raster.
            var ac = new AffineTransform(Bounds.AffineCoefficients).TransfromToCorner(startColumn, startRow);
            result.Bounds = new RasterBounds(result.NumRows, result.NumColumns, ac);
            
            if (IsInRam)
            {
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

                if (result.IsInRam == false)
                {
                    // Force the result raster to write itself to a file and then purge its memory.
                    result.Write(fileName);
                    result.Data = null;
                }
            }
            else
            {
                if (result.IsInRam)
                {
                    // the source is not in memory, so we just read the values from the file as if opening it directly from the file.
                    result.OpenWindow(Filename, startRow, endRow, startColumn, endColumn, true);
                }
                else
                {
                    // Both sources are file based so we basically copy rows of bytes from one to the other.
                    FileStream source = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                    result.WriteHeader(fileName);
                    FileStream dest = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None);
                    source.Seek(HeaderSize, SeekOrigin.Begin);
                    BinaryReader bReader = new BinaryReader(source);
                    BinaryWriter bWriter = new BinaryWriter(dest);
                    ProgressMeter pm = new ProgressMeter(ProgressHandler, DataStrings.CopyingValues, numRows);
                    // copy values directly using both data structures
                    source.Seek(NumColumnsInFile * startRow * ByteSize, SeekOrigin.Current);
                    for (int row = 0; row < numRows; row++)
                    {
                        source.Seek(numCols * ByteSize, SeekOrigin.Current);
                        byte[] rowData = bReader.ReadBytes(ByteSize * numCols);
                        bWriter.Write(rowData);
                        source.Seek(NumColumnsInFile - endColumn + 1, SeekOrigin.Current);
                        bWriter.Flush();
                        pm.CurrentValue = row;
                    }
                    pm.Reset();
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the statistics for the entire file, not just the window portion specified for this raster.
        /// </summary>
        public override void GetStatistics()
        {
            if (IsInRam && this.IsFullyWindowed())
            {
                // The in-memory version of this is a little faster, so use it, but only if we can.
                base.GetStatistics();
                return;
            }
            // If we get here, we either need to check the file because no data is in memory or because
            // the window that is in memory does not have all the values.

            FileStream fs = new FileStream(Filename, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            fs.Seek(HeaderSize, SeekOrigin.Begin);
            ProgressMeter pm = new ProgressMeter(ProgressHandler,
                                                 "Calculating Statistics for the entire raster " + Filename,
                                                 NumRowsInFile);
            int count = 0;
            T min = MinValue;
            T max = MaxValue;

            double total = 0;
            double sqrTotal = 0;
            T noDataValue = ConvertTo<T>(NoDataValue);
            for (int row = 0; row < NumRowsInFile; row++)
            {
                for (int col = 0; col < NumColumnsInFile; col++)
                {
                    T val = br.Read<T>();

                    if (!EqualityComparer<T>.Default.Equals(val, noDataValue))
                    {
                        if (Operator.GreaterThan(val, max)) max = val;
                        if (Operator.LessThan(val, min)) min = val;
                        double valAsDouble = Convert.ToDouble(val);
                        total += valAsDouble;
                        sqrTotal += valAsDouble * valAsDouble;
                        count++;
                    }
                }
                pm.CurrentValue = row;
            }

            Value.Updated = false;
            Minimum = Convert.ToDouble(min);
            Maximum = Convert.ToDouble(max);
            Mean = total / count;
            NumValueCells = count;
            StdDeviation = (float)Math.Sqrt((sqrTotal / NumValueCells) - (total / NumValueCells) * (total / NumValueCells));

            br.Close();
        }

        /// <summary>
        /// This creates a window from this raster.  The window will still save to the same
        /// source file, but only has access to a small window of data, so it can be loaded like a buffer.
        /// The georeferenced extents will be for the new window, not the original raster.  startRow and endRow
        /// will exist in the new raster, however, so that it knows how to copy values back to the original raster.
        /// </summary>
        /// <param name="startRow">The 0 based integer index of the top row to get from this raster.  If this raster is itself a window, 0 represents the startRow from the file.</param>
        /// <param name="endRow">The integer index of the bottom row to get from this raster.  The largest allowed value is NumRows - 1.</param>
        /// <param name="startColumn">The 0 based integer index of the leftmost column to get from this raster.  If this raster is a window, 0 represents the startColumn from the file.</param>
        /// <param name="endColumn">The 0 based integer index of the rightmost column to get from this raster.  The largest allowed value is NumColumns - 1</param>
        /// <param name="inRam">Boolean.  If this is true and the window is small enough, a copy of the values will be loaded into memory.</param>
        /// <returns>An implementation of IRaster</returns>
        public new IRaster GetWindow(int startRow, int endRow, int startColumn, int endColumn, bool inRam)
        {
            int numCols = endColumn - startColumn + 1;
            int numRows = endRow - startRow + 1;
            var result = new BinaryRaster<T>
            {
                Filename = Filename,
                Projection = Projection,
                DataType = typeof (int),
                NumRows = endRow - startRow + 1,
                NumColumns = endColumn - startColumn + 1,
                NumRowsInFile = NumRowsInFile,
                NumColumnsInFile = NumColumnsInFile,
                NoDataValue = NoDataValue,
                StartColumn = startColumn + StartColumn,
                StartRow = startRow + StartRow,
                EndColumn = endColumn + StartColumn,
                EndRow = EndRow + StartRow
            };

            // Reposition the "raster" so that it matches the window, not the whole raster
            var ac = new AffineTransform(Bounds.AffineCoefficients).TransfromToCorner(startColumn, startRow);
            result.Bounds = new RasterBounds(result.NumRows, result.NumColumns, ac);
            
            // Now we can copy any values currently in memory.
            if (IsInRam)
            {
                //result.ReadHeader(Filename);
                result.Data = new T[numRows][];
                ProgressMeter pm = new ProgressMeter(ProgressHandler, DataStrings.CopyingValues, endRow)
                {
                    StartValue = startRow
                };
                // copy values directly using both data structures
                for (int row = 0; row < numRows; row++)
                {
                    result.Data[row] = new T[numCols];
                    for (int col = 0; col < numCols; col++)
                    {
                        result.Data[row][col] = Data[startRow + row][startColumn + col];
                    }
                    pm.CurrentValue = row;
                }
                pm.Reset();
            }
            else
                result.OpenWindow(Filename, startRow, endRow, startColumn, endColumn, inRam);
            result.Value = new ValueGrid<T>(result);
            return result;
        }

        /// <summary>
        /// Obtains only the statistics for the small window specified by startRow, endRow etc.
        /// </summary>
        public new void GetWindowStatistics()
        {
            if (IsInRam)
            {
                // don't bother to do file calculations if the whole raster is in memory
                base.GetWindowStatistics();
                return;
            }

            // The window was not in memory, so go ahead and get statistics for the window from the file.
            FileStream fs = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.Read, NumColumns * ByteSize);
            BinaryReader br = new BinaryReader(fs);
            fs.Seek(HeaderSize, SeekOrigin.Begin);
            ProgressMeter pm = new ProgressMeter(ProgressHandler,
                                                 "Calculating Statistics for the entire raster " + Filename, NumRows);

            double total = 0;
            double sqrTotal = 0;
            int count = 0;
            int byteSize = ByteSize; // cache this for faster calcs
            int min = int.MaxValue;
            int max = int.MinValue;
            fs.Seek(StartRow * ByteSize * NumColumnsInFile, SeekOrigin.Current); // To top edge of the Window

            int noDataValue = Convert.ToInt32(NoDataValue);
            for (int row = 0; row < NumRows; row++)
            {
                fs.Seek(StartColumn * byteSize, SeekOrigin.Current); // to the left edge of the window
                for (int col = 0; col < NumColumns; col++)
                {
                    int val = br.ReadInt32();

                    if (val == noDataValue || val <= -100000) continue;
                    if (val > max) max = val;
                    if (val < min) min = val;
                    double dblVal = val;
                    total += dblVal;
                    sqrTotal += dblVal * dblVal;
                    count++;
                }
                fs.Seek(NumColumnsInFile - EndRow - 1, SeekOrigin.Current); // skip to the end of this row.
                pm.CurrentValue = row;
            }
            Minimum = min;
            Maximum = max;
            NumValueCells = count;
            StdDeviation = (float)Math.Sqrt((sqrTotal / NumValueCells) - (total / NumValueCells) * (total / NumValueCells));
            br.Close();
        }

        /// <summary>
        /// Opens the specified file into this raster.
        /// </summary>
        public override void Open()
        {
            Open(Filename, true);
        }

        /// <summary>
        /// Opens a new instance of the BinaryRaster
        /// </summary>
        /// <param name="fileName">The string fileName of the raster file to open</param>
        /// <param name="inRam">Boolean, indicates whether or not the values for the raster should be loaded into memory</param>
        public virtual void Open(string fileName, bool inRam)
        {
            Filename = fileName;
            ReadHeader(fileName);
            if (inRam)
            {
                if (NumRowsInFile * NumColumnsInFile < 64000000)
                {
                    IsInRam = true;
                    Read();
                }
            }
            Value = new ValueGrid<T>(this);
        }

        /// <summary>
        /// This converts this object into a raster defined by the specified window dimensions.
        /// </summary>
        /// <param name="fileName">The string fileName to open</param>
        /// <param name="startRow">The integer row index to become the first row to load into this raster.</param>
        /// <param name="endRow">The 0 based integer row index to become the last row included in this raster.</param>
        /// <param name="startColumn">The 0 based integer column index for the first column of the raster.</param>
        /// <param name="endColumn">The 0 based integer column index for the last column to include in this raster.</param>
        /// <param name="inRam">Boolean.  If this is true and the window is small enough, this will load the window into ram.</param>
        public virtual void OpenWindow(string fileName, int startRow, int endRow, int startColumn, int endColumn,
                                       bool inRam)
        {
            Filename = fileName;
            ReadHeader(fileName);
            NumRows = endRow - startRow + 1;
            NumColumns = endColumn - startColumn + 1;

            StartColumn = startColumn;
            StartRow = startRow;
            EndColumn = endColumn;
            EndRow = EndRow;

            // Reposition the "raster" so that it matches the window, not the whole raster
            Bounds.AffineCoefficients = new AffineTransform(Bounds.AffineCoefficients).TransfromToCorner(startColumn, startRow);
            if (inRam)
            {
                if (NumRows * NumColumns < 64000000)
                {
                    IsInRam = true;
                    Read();
                }
            }

            Value = new ValueGrid<T>(this);
        }

        /// <summary>
        /// Copies the contents from the specified sourceRaster into this sourceRaster.  If both rasters are InRam, this does not affect the files.
        /// </summary>
        /// <param name="sourceRaster">The raster of values to paste into this raster.  If the CellWidth and CellHeight values do not match between the files,
        /// an exception will be thrown.  If the sourceRaster overlaps with the edge of this raster, only the intersecting region will be
        /// pasted.</param>
        /// <param name="startRow">Specifies the row in this raster where the top row of the sourceRaster will be pasted </param>
        /// <param name="startColumn">Specifies the column in this raster where the left column of the sourceRaster will be pasted.</param>
        public void PasteRaster(Raster<T> sourceRaster, int startRow, int startColumn)
        {
            int byteSize = ByteSize;

            if (sourceRaster.DataType != typeof(int))
            {
                throw new ArgumentException(
                    DataStrings.ArgumentOfWrongType_S1_S2.Replace("%S1", "sourceRaster").Replace("%S2", "BinaryRaster"));
            }

            if (startRow + sourceRaster.NumRows <= 0) return; // sourceRaster is above this raster
            if (startColumn + sourceRaster.NumColumns <= 0) return; // sourceRaster is left of this raster
            if (startRow > NumRows) return; // sourceRaster is below this raster
            if (startColumn > NumColumns) return; // sourceRaster is to the right of this raster
            if (sourceRaster.CellWidth != CellWidth || sourceRaster.CellHeight != CellHeight)
                throw new ArgumentException(DataStrings.RastersNeedSameCellSize);

            // These are specified in coordinates that match the source raster
            int sourceStartColumn = 0;
            int sourceStartRow = 0;
            int destStartColumn = startColumn;
            int destStartRow = startRow;

            int numPasteColumns = sourceRaster.NumColumns;
            int numPasteRows = sourceRaster.NumRows;

            // adjust range to cover only the overlapping sections
            if (startColumn < 0)
            {
                sourceStartColumn = -startColumn;
                destStartColumn = 0;
            }
            if (startRow < 0)
            {
                sourceStartRow = -startRow;
                destStartRow = 0;
            }

            if (numPasteRows + destStartRow > NumRows) numPasteRows = (NumRows - destStartRow);
            if (numPasteColumns + destStartColumn > NumColumns) numPasteColumns = (NumColumns - destStartRow);

            if (IsInRam)
            {
                // ---------------------- RAM BASED ------------------------------------------------------
                if (sourceRaster.IsInRam)
                {
                    // both members are inram, so directly copy values.
                    for (int row = 0; row < numPasteRows; row++)
                    {
                        for (int col = 0; col < numPasteColumns; col++)
                        {
                            // since we are copying direct, we don't have to do a type check on T
                            Data[destStartRow + row][destStartColumn + col] =
                                sourceRaster.Data[sourceStartRow + row][sourceStartColumn + col];
                        }
                    }
                }
                else
                {
                    FileStream fs = new FileStream(sourceRaster.Filename, FileMode.Open, FileAccess.Write,
                                                   FileShare.None, (numPasteColumns) * byteSize);
                    ProgressMeter pm = new ProgressMeter(ProgressHandler,
                                                         DataStrings.ReadingValuesFrom_S.Replace("%S",
                                                                                                 sourceRaster.
                                                                                                     Filename),
                                                         numPasteRows);
                    fs.Seek(HeaderSize, SeekOrigin.Begin);

                    // Position the binary reader at the top of the "sourceRaster"
                    fs.Seek(sourceStartRow * sourceRaster.NumColumnsInFile * byteSize, SeekOrigin.Current);
                    BinaryReader br = new BinaryReader(fs);

                    for (int row = 0; row < numPasteRows; row++)
                    {
                        // Position the binary reader at the beginning of the sourceRaster
                        fs.Seek(byteSize * sourceStartColumn, SeekOrigin.Current);

                        for (int col = 0; col < numPasteColumns; col++)
                        {
                            Data[destStartRow + row][destStartColumn + col] = br.Read<T>();
                        }
                        pm.CurrentValue = row;
                        fs.Seek(byteSize * (NumColumnsInFile - sourceStartColumn - numPasteColumns), SeekOrigin.Current);
                    }

                    br.Close();
                }
                // The statistics will have changed with the newly pasted data involved
                GetStatistics();
            }
            else
            {
                // ----------------------------------------- FILE BASED ---------------------------------
                FileStream writefs = new FileStream(Filename, FileMode.Open, FileAccess.Write, FileShare.None,
                                                    NumColumns * byteSize);
                BinaryWriter bWriter = new BinaryWriter(writefs);

                ProgressMeter pm = new ProgressMeter(ProgressHandler,
                                                     DataStrings.WritingValues_S.Replace("%S", Filename),
                                                     numPasteRows);

                writefs.Seek(HeaderSize, SeekOrigin.Begin);
                writefs.Seek(destStartRow * NumColumnsInFile * byteSize, SeekOrigin.Current);
                // advance to top of paste window area

                if (sourceRaster.IsInRam)
                {
                    // we can just write values

                    for (int row = 0; row < numPasteColumns; row++)
                    {
                        // Position the binary reader at the beginning of the sourceRaster
                        writefs.Seek(byteSize * destStartColumn, SeekOrigin.Current);

                        for (int col = 0; col < numPasteColumns; col++)
                        {
                            T val = sourceRaster.Data[sourceStartRow + row][sourceStartColumn + col];
                            bWriter.Write(val);
                        }
                        pm.CurrentValue = row;
                        writefs.Seek(byteSize * (NumColumnsInFile - destStartColumn - numPasteColumns), SeekOrigin.Current);
                    }
                }
                else
                {
                    // Since everything is handled from a file, we don't have to type check.  Just copy the bytes.

                    FileStream readfs = new FileStream(sourceRaster.Filename, FileMode.Open, FileAccess.Read,
                                                       FileShare.Read, numPasteColumns * byteSize);
                    BinaryReader bReader = new BinaryReader(readfs);
                    readfs.Seek(HeaderSize, SeekOrigin.Begin);
                    readfs.Seek(sourceStartRow * sourceRaster.NumColumnsInFile * byteSize, SeekOrigin.Current);
                    // advances to top of paste window area

                    for (int row = 0; row < numPasteRows; row++)
                    {
                        readfs.Seek(sourceStartColumn * byteSize, SeekOrigin.Current);
                        writefs.Seek(destStartColumn * byteSize, SeekOrigin.Current);
                        byte[] rowData = bReader.ReadBytes(numPasteColumns * byteSize);
                        bWriter.Write(rowData);
                        readfs.Seek(sourceRaster.NumColumnsInFile - sourceStartColumn - numPasteColumns,
                                    SeekOrigin.Current);
                        writefs.Seek(NumColumnsInFile - destStartColumn - numPasteColumns, SeekOrigin.Current);
                    }
                    bReader.Close();
                }
                bWriter.Close();
            }
        }

        private U ConvertTo<U>(double value)
        {
            // we don't consider whether the current culture might cause an incorrect conversion.
            object ret = null;

            if (typeof(U) == typeof(Byte))
                ret = Convert.ToByte(value);
            else if (typeof(U) == typeof(Double))
                ret = Convert.ToDouble(value);
            else if (typeof(U) == typeof(Decimal))
                ret = Convert.ToDecimal(value);
            else if (typeof(U) == typeof(Int16))
                ret = Convert.ToInt16(value);
            else if (typeof(U) == typeof(Int32))
                ret = Convert.ToInt32(value);
            else if (typeof(U) == typeof(Int64))
                ret = Convert.ToInt64(value);
            else if (typeof(U) == typeof(Single))
                ret = Convert.ToSingle(value);
            else if (typeof(U) == typeof(Boolean))
                ret = Convert.ToBoolean(value);

            if (ret == null)
                throw new NotSupportedException("Unable to convert type - " + typeof(U));

            return (U)ret;
        }

        /// <summary>
        /// Reads the the contents for the "window" specified by the start and end values
        /// for the rows and columns.
        /// </summary>
        public void Read()
        {
            FileStream fs = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.Read, NumColumns * ByteSize);
            ProgressMeter pm = new ProgressMeter(ProgressHandler, DataStrings.ReadingValuesFrom_S.Replace("%S", Filename), NumRows);
            fs.Seek(HeaderSize, SeekOrigin.Begin);

            // Position the binary reader at the top of the "window"
            fs.Seek(StartRow * NumColumnsInFile * ByteSize, SeekOrigin.Current);
            BinaryReader br = new BinaryReader(fs);

            Data = new T[NumRows][];

            T min = MinValue;
            T max = MaxValue;

            double total = 0;
            double sqrTotal = 0;
            T noDataValue = ConvertTo<T>(NoDataValue);
            for (int row = 0; row < NumRows; row++)
            {
                Data[row] = new T[NumColumns];

                // Position the binary reader at the beginning of the window
                fs.Seek(4 * StartColumn, SeekOrigin.Current);

                for (int col = 0; col < NumColumns; col++)
                {
                    T val = br.Read<T>();

                    if (!EqualityComparer<T>.Default.Equals(val, noDataValue))
                    {
                        if (Operator.GreaterThan(val, max)) max = val;
                        if (Operator.LessThan(val, min)) min = val;
                        double valAsDouble = Convert.ToDouble(val);
                        total += valAsDouble;
                        sqrTotal += valAsDouble * valAsDouble;
                        NumValueCells++;
                    }

                    Data[row][col] = val;
                }
                pm.CurrentValue = row;
                fs.Seek(ByteSize * (NumColumnsInFile - EndColumn - 1), SeekOrigin.Current);
            }
            Maximum = Convert.ToDouble(max);
            Minimum = Convert.ToDouble(min);

            StdDeviation = Math.Sqrt((sqrTotal / NumValueCells) - (total / NumValueCells) * (total / NumValueCells));

            br.Close();
        }

        // http://stackoverflow.com/questions/4418636/c-sharp-generics-how-to-use-x-maxvalue-x-minvalue-int-float-double-in-a
        private static T ReadStaticField(string name)
        {
            FieldInfo field = typeof(T).GetField(name, BindingFlags.Public | BindingFlags.Static);
            if (field == null)
            {
                // There's no TypeArgumentException, unfortunately.
                throw new InvalidOperationException("Invalid type argument: " + typeof(T).Name);
            }
            return (T)field.GetValue(null);
        }

        /// <summary>
        /// Writes the header, regardless of which subtype of binary raster this is written for
        /// </summary>
        /// <param name="fileName">The string fileName specifying what file to load</param>
        public void ReadHeader(string fileName)
        {
            BinaryReader br = new BinaryReader(new FileStream(fileName, FileMode.Open));
            StartColumn = 0;
            NumColumns = br.ReadInt32();
            NumColumnsInFile = NumColumns;
            EndColumn = NumColumns - 1;
            StartRow = 0;
            NumRows = br.ReadInt32();
            NumRowsInFile = NumRows;
            EndRow = NumRows - 1;
            Bounds = new RasterBounds(NumRows, NumColumns, new[] { 0.0, 1.0, 0.0, NumRows, 0.0, -1.0 });

            CellWidth = br.ReadDouble();
            Bounds.AffineCoefficients[5] = -br.ReadDouble(); // dy
            Xllcenter = br.ReadDouble();
            Yllcenter = br.ReadDouble();
            RasterDataType dataType = (RasterDataType)br.ReadInt32();
            if (dataType != RasterDataType.INTEGER)
            {
                throw new ArgumentException(
                    DataStrings.ArgumentOfWrongType_S1_S2.Replace("%S1", fileName).Replace("%S2", "BinaryShortRaster"));
            }
            NoDataValue = br.ReadInt32();
            string proj = Encoding.ASCII.GetString(br.ReadBytes(255)).Replace('\0', ' ').Trim();
            Projection = ProjectionInfo.FromProj4String(proj);
            Notes = Encoding.ASCII.GetString(br.ReadBytes(255)).Replace('\0', ' ').Trim();
            if (Notes.Length == 0) Notes = null;
            br.Close();
        }

        /// <summary>
        /// The string fileName where this will begin to write data by clearing the existing file
        /// </summary>
        /// <param name="fileName">a fileName to write data to</param>
        public void WriteHeader(string fileName)
        {
            using (var bw = new BinaryWriter(new FileStream(fileName, FileMode.OpenOrCreate)))
            {
                bw.Write(NumColumnsInFile);
                bw.Write(NumRowsInFile);
                bw.Write(CellWidth);
                bw.Write(CellHeight);
                bw.Write(Xllcenter);
                bw.Write(Yllcenter);
                bw.Write((int) RasterDataType.INTEGER);
                bw.Write(Convert.ToInt32(NoDataValue));

                // These are each 256 bytes because they are ASCII encoded, not the standard DotNet Unicode
                byte[] proj = new byte[255];
                if (Projection != null)
                {
                    byte[] temp = Encoding.ASCII.GetBytes(Projection.ToProj4String());
                    int len = Math.Min(temp.Length, 255);
                    for (int i = 0; i < len; i++)
                    {
                        proj[i] = temp[i];
                    }
                }
                bw.Write(proj);
                byte[] note = new byte[255];
                if (Notes != null)
                {
                    byte[] temp = Encoding.ASCII.GetBytes(Notes);
                    int len = Math.Min(temp.Length, 255);
                    for (int i = 0; i < len; i++)
                    {
                        note[i] = temp[i];
                    }
                }

                bw.Write(note);
            }
        }

        /// <summary>
        /// This would be a horrible choice for any kind of serious process, but is provided as
        /// a way to write values directly to the file.
        /// </summary>
        /// <param name="row">The 0 based integer row index for the file to write to.</param>
        /// <param name="column">The 0 based column index for the file to write to.</param>
        /// <param name="value">The actual value to write.</param>
        public void WriteValue(int row, int column, int value)
        {
            using (var fs = new FileStream(Filename, FileMode.Open, FileAccess.Write, FileShare.None))
            {
                fs.Seek(HeaderSize, SeekOrigin.Begin);
                fs.Seek(row*NumColumnsInFile*ByteSize, SeekOrigin.Current);
                fs.Seek(column*ByteSize, SeekOrigin.Current);
                using (var bw = new BinaryWriter(fs))
                {
                    bw.Write(value);
                }
            }
        }

        /// <summary>
        /// Saves the values in memory to the disk.
        /// </summary>
        public override void Save()
        {
            Write(Filename);
        }

        /// <summary>
        /// If no file exists, this writes the header and no-data values.  If a file exists, it will assume
        /// that data already has been filled in the file and will attempt to insert the data values
        /// as a window into the file.  If you want to create a copy of the file and values, just use
        /// System.IO.File.Copy, it almost certainly would be much more optimized.
        /// </summary>
        /// <param name="fileName">The string fileName to write values to.</param>
        public void Write(string fileName)
        {
            FileStream fs;
            BinaryWriter bw;
            ProgressMeter pm = new ProgressMeter(ProgressHandler, "Writing values to " + fileName, NumRows);
            long expectedByteCount = NumRows * NumColumns * ByteSize;
            if (expectedByteCount < 1000000) pm.StepPercent = 5;
            if (expectedByteCount < 5000000) pm.StepPercent = 10;
            if (expectedByteCount < 100000) pm.StepPercent = 50;

            if (File.Exists(fileName))
            {
                FileInfo fi = new FileInfo(fileName);
                // if the following test fails, then the target raster doesn't fit the bill for pasting into, so clear it and write a new one.
                if (fi.Length == HeaderSize + ByteSize * NumColumnsInFile * NumRowsInFile)
                {
                    WriteHeader(fileName);
                    // assume that we already have a file set up for us, and just write the window of values into the appropriate place.
                    fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None,
                                        ByteSize * NumColumns);
                    fs.Seek(HeaderSize, SeekOrigin.Begin);
                    fs.Seek(ByteSize * StartRow, SeekOrigin.Current);
                    bw = new BinaryWriter(fs); // encoding doesn't matter because we don't have characters

                    for (int row = 0; row < NumRows; row++)
                    {
                        fs.Seek(StartColumn * ByteSize, SeekOrigin.Current);
                        for (int col = 0; col < NumColumns; col++)
                        {
                            // this is the only line that is type dependant, but I don't want to type check on every value
                            bw.Write(Data[row][col]);
                        }
                        fs.Flush(); // Since I am buffering, make sure that I write the buffered data before seeking
                        fs.Seek((NumColumnsInFile - EndColumn - 1) * ByteSize, SeekOrigin.Current);
                        pm.CurrentValue = row;
                    }

                    pm.Reset();
                    bw.Close();
                    return;
                }

                // If we got here, either the file didn't exist or didn't match the specifications correctly, so write a new one.

                Debug.WriteLine("The size of the file was " + fi.Length + " which didn't match the expected " +
                                HeaderSize + ByteSize * NumColumnsInFile * NumRowsInFile);
                File.Delete(fileName);
            }

            if (File.Exists(fileName)) File.Delete(fileName);
            WriteHeader(fileName);

            // Open as append and it will automatically skip the header for us.
            fs = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None, ByteSize * NumColumnsInFile);
            bw = new BinaryWriter(fs);
            // the row and column counters here are relative to the whole file, not just the window that is currently in memory.
            pm.EndValue = NumRowsInFile;

            int noDataValue = Convert.ToInt32(NoDataValue);
            for (int row = 0; row < NumRowsInFile; row++)
            {
                for (int col = 0; col < NumColumnsInFile; col++)
                {
                    if (row < StartRow || row > EndRow || col < StartColumn || col > EndColumn)
                        bw.Write(Convert.ToInt32(noDataValue));
                    else
                        bw.Write(Data[row - StartRow][col - StartColumn]);
                }
                pm.CurrentValue = row;
            }

            fs.Flush(); // flush anything that hasn't gotten written yet.
            pm.Reset();
            bw.Close();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the size of T in bytes.  This should be overridden, but
        /// exists as a "just-in-case" implementation that works for structs,
        /// but definitely won't work correctly for objects.
        /// </summary>
        public override int ByteSize
        {
            get { return Marshal.SizeOf(typeof(T)); }
        }

        /// <summary>
        /// All the binary rasters use the Binary file type
        /// </summary>
        public override RasterFileType FileType
        {
            get { return RasterFileType.BINARY; }
        }

        /// <summary>
        /// This is always 1 band
        /// </summary>
        public override int NumBands
        {
            get { return 1; }
        }

        /// <summary>
        /// Gets the size of the header.  There is one no-data value in the header.
        /// </summary>
        public virtual int HeaderSize
        {
            get { return 554 + ByteSize; }
        }

        #endregion
    }
}