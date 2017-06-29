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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/6/2009 10:14:34 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DotSpatial.Data
{
    /// <summary>
    /// BgdRaster
    /// </summary>
    public class BgdRaster<T> : Raster<T> where T : IComparable<T>, IEquatable<T>
    {
        #region Constructors

        /// <summary>
        /// A BgdRaster in created this way probably expects to open a file using the "Open" method,
        /// which allows for progress handlers or other things to be set before what might be a
        /// time consuming read-value process.
        /// </summary>
        public BgdRaster()
        {
        }

        /// <summary>
        /// Creates a new instance of a BGD raster, attempting to store the entire structure in memory if possible.
        /// </summary>
        /// <param name="numRows"></param>
        /// <param name="numColumns"></param>
        public BgdRaster(int numRows, int numColumns)
            : base(numRows, numColumns)
        {
        }

        /// <summary>
        /// This creates a new BGD raster.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="numRows"></param>
        /// <param name="numColumns"></param>
        public BgdRaster(string fileName, int numRows, int numColumns)
            : base(numRows, numColumns)
        {
            if (File.Exists(fileName)) File.Delete(fileName);
            base.Filename = fileName;
            base.NumRowsInFile = numRows;
            base.NumColumnsInFile = numColumns;
            //base.IsInRam = false;
            WriteHeader(fileName);
        }

        #endregion

        #region Methods

        /// <summary>
        /// This Method should be overrridden by classes, and provides the primary ability.
        /// </summary>
        /// <param name="xOff">The horizontal offset of the area to read values from.</param>
        /// <param name="yOff">The vertical offset of the window to read values from.</param>
        /// <param name="sizeX">The number of values to read into the buffer.</param>
        /// <param name="sizeY">The vertical size of the window to read into the buffer.</param>
        /// <returns>A jagged array of type T.</returns>
        public override T[][] ReadRaster(int xOff, int yOff, int sizeX, int sizeY)
        {
            FileStream fs = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.Read, NumColumns * ByteSize);
            ProgressMeter pm = new ProgressMeter(ProgressHandler,
                                                 DataStrings.ReadingValuesFrom_S.Replace("%S", Filename), NumRows);
            fs.Seek(HeaderSize, SeekOrigin.Begin);

            // Position the binary reader at the top of the "window"
            fs.Seek(yOff * NumColumnsInFile * ByteSize, SeekOrigin.Current);
            BinaryReader br = new BinaryReader(fs);

            T[][] result = new T[NumRows][];
            int endX = xOff + sizeX;

            for (int row = 0; row < sizeY; row++)
            {
                result[row] = new T[sizeX];
                // Position the binary reader at the beginning of the window
                fs.Seek(ByteSize * xOff, SeekOrigin.Current);
                byte[] values = br.ReadBytes(sizeX * ByteSize);
                Buffer.BlockCopy(values, 0, result[row], 0, ByteSize * sizeX);
                pm.CurrentValue = row;
                fs.Seek(ByteSize * (NumColumnsInFile - endX), SeekOrigin.Current);
            }
            br.Close();
            return result;
        }

        /// <summary>
        /// Most reading is optimized to read in a block at a time and process it.  This method is designed
        /// for seeking through the file.  It should work faster than the buffered methods in cases where
        /// an unusually arranged collection of values are required.  Sorting the list before calling
        /// this should significantly improve performance.
        /// </summary>
        /// <param name="indices">A list or array of long values that are (Row * NumRowsInFile + Column)</param>
        public override List<T> GetValuesT(IEnumerable<long> indices)
        {
            if (IsInRam) return base.GetValuesT(indices);
#if DEBUG
            var sw = new Stopwatch();
            sw.Start();
#endif
            var result = new List<T>();
            using (var fs = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.Read, ByteSize))
            {
                fs.Seek(HeaderSize, SeekOrigin.Begin);
                using (var br = new BinaryReader(fs))
                {
                    foreach (long index in indices)
                    {
                        var offset = HeaderSize + index*ByteSize;
                        // Position the binary reader at the top of the "window"
                        fs.Seek(offset, SeekOrigin.Begin);
                        var values = br.ReadBytes(ByteSize);
                        var x = new T[1];
                        Buffer.BlockCopy(values, 0, x, 0, ByteSize);
                        result.Add(x[0]);
                    }
                }
            }

#if DEBUG
            sw.Stop();
            Debug.WriteLine("Time to read values from file:" + sw.ElapsedMilliseconds);
#endif

            return result;
        }

        /// <summary>
        /// Writes the bgd content from the specified jagged array of values to the file.
        /// </summary>
        /// <param name="buffer">The data</param>
        /// <param name="xOff">The horizontal offset</param>
        /// <param name="yOff">The vertical offset</param>
        /// <param name="xSize">The number of values to write horizontally</param>
        /// <param name="ySize">The number of values to write vertically</param>
        public override void WriteRaster(T[][] buffer, int xOff, int yOff, int xSize, int ySize)
        {
            var pm = new ProgressMeter(ProgressHandler,
                    DataStrings.ReadingValuesFrom_S.Replace("%S", Filename), NumRowsInFile) { StartValue = yOff };

            using (var fs = new FileStream(Filename, FileMode.Open, FileAccess.Write, FileShare.Write,
                    NumColumns*ByteSize))
            {
                
                fs.Seek(HeaderSize, SeekOrigin.Begin);

                // Position the binary reader at the top of the "window"
                long offset = yOff*(long) NumColumnsInFile*ByteSize;
                fs.Seek(offset, SeekOrigin.Current);
                using (var br = new BinaryWriter(fs))
                {
                    int endX = xOff + xSize;
                    for (int row = 0; row < ySize; row++)
                    {
                        // Position the binary reader at the beginning of the window
                        fs.Seek(ByteSize*xOff, SeekOrigin.Current);
                        byte[] values = new byte[xSize*ByteSize];
                        Buffer.BlockCopy(buffer[row], 0, values, 0, xSize*ByteSize);
                        br.Write(values);
                        pm.CurrentValue = row + yOff;
                        fs.Seek(ByteSize*(NumColumnsInFile - endX), SeekOrigin.Current);
                    }
                }
            }
        }

        /// <summary>
        /// Copies the raster header, and if copyValues is true, the values to the specified file </summary>
        /// <param name="fileName">The full path of the file to copy content to to</param>
        /// <param name="copyValues">Boolean, true if this should copy values as well as just header information</param>
        public override void Copy(string fileName, bool copyValues)
        {
            if (copyValues)
            {
                Write(fileName);
            }
            else
            {
                WriteHeader(fileName);
                T[] blank = new T[NumColumnsInFile];
                T val = (T)Convert.ChangeType(NoDataValue, typeof(T));
                for (int col = 0; col < NumColumnsInFile; col++)
                {
                    blank[col] = val;
                }
                for (int row = 0; row < NumRowsInFile; row++)
                {
                    WriteRow(blank, row);
                }
            }
        }

        /// <summary>
        /// Opens the specified file
        /// </summary>
        public override void Open()
        {
            NoDataValue = Global.ToDouble(Global.MinimumValue<T>()); // Sets it to the appropriate minimum for the int datatype
            
            ReadHeader(Filename);

            StartRow = 0;
            EndRow = NumRows - 1;
            StartColumn = 0;
            EndColumn = NumColumns - 1;
            NumColumnsInFile = NumColumns;
            NumRowsInFile = NumRows;
            NumValueCells = 0;
            Value = new ValueGrid<T>(this);
            
            DataType = typeof(T);

            if (base.NumColumnsInFile * base.NumRowsInFile < 64000000)
            {
                base.IsInRam = true;
                Data = ReadRaster();
                base.GetStatistics();
            }
            else
            {
                base.IsInRam = false;
                // Don't read in any data at this point.  Let the user use ReadRaster for specific blocks.
            }
        }

        /// <summary>
        /// Saves the content from this file using the current fileName and header information
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
        private void Write(string fileName)
        {
            ProgressMeter pm = new ProgressMeter(ProgressHandler, "Writing values to " + Filename, NumRows);
            long expectedByteCount = NumRows * NumColumns * ByteSize;
            if (expectedByteCount < 1000000) pm.StepPercent = 5;
            if (expectedByteCount < 5000000) pm.StepPercent = 10;
            if (expectedByteCount < 100000) pm.StepPercent = 50;

            if (File.Exists(fileName))
            {
                FileInfo fi = new FileInfo(Filename);
                // if the following test fails, then the target raster doesn't fit the bill for pasting into, so clear it and write a new one.
                if (fi.Length == HeaderSize + ByteSize * NumColumnsInFile * NumRowsInFile)
                {
                    WriteHeader(fileName);
                    WriteRaster(Data);
                    return;
                }

                // If we got here, either the file didn't exist or didn't match the specifications correctly, so write a new one.

                Debug.WriteLine("The size of the file was " + fi.Length + " which didn't match the expected " +
                                HeaderSize + ByteSize * NumColumnsInFile * NumRowsInFile);
            }

            if (File.Exists(Filename)) File.Delete(Filename);
            WriteHeader(fileName);

            // Open as append and it will automatically skip the header for us.
            using (var bw =new BinaryWriter(new FileStream(Filename, FileMode.Append, FileAccess.Write, FileShare.None,ByteSize*NumColumnsInFile)))
            {
                // the row and column counters here are relative to the whole file, not just the window that is currently in memory.
                pm.EndValue = NumRowsInFile;

                for (int row = 0; row < NumRowsInFile; row++)
                {
                    byte[] rawBytes = new byte[NumColumnsInFile*ByteSize];
                    T[] nd = new T[1];
                    nd[0] = (T) Convert.ChangeType(NoDataValue, typeof (T));
                    Buffer.BlockCopy(Data[row - StartRow], 0, rawBytes, StartColumn*ByteSize, NumColumns*ByteSize);
                    for (int col = 0; col < StartColumn; col++)
                    {
                        Buffer.BlockCopy(nd, 0, rawBytes, col*ByteSize, ByteSize);
                    }
                    for (int col = EndColumn + 1; col < NumColumnsInFile; col++)
                    {
                        Buffer.BlockCopy(nd, 0, rawBytes, col*ByteSize, ByteSize);
                    }
                    bw.Write(rawBytes);
                    pm.CurrentValue = row;
                }
            }
            pm.Reset();
        }

        /// <summary>
        /// Writes the header to the fileName
        /// </summary>
        public override void WriteHeader()
        {
            WriteHeader(Filename);
        }

        /// <summary>
        /// The string fileName where this will begin to write data by clearing the existing file
        /// </summary>
        public void WriteHeader(string fileName)
        {
            var dt = GetRasterDataType();
            if (dt == RasterDataType.INVALID)
            {
                throw new Exception("Invalid DataType");
            }

            using (var bw = new BinaryWriter(new FileStream(fileName, FileMode.OpenOrCreate)))
            {
                bw.Write(NumColumnsInFile);
                bw.Write(NumRowsInFile);
                bw.Write(CellWidth);
                bw.Write(CellHeight);
                bw.Write(Xllcenter);
                bw.Write(Yllcenter);
                bw.Write((int) dt);
                var nd = new byte[ByteSize];
                var nds = new[] {(T) Convert.ChangeType(NoDataValue, typeof (T))};
                Buffer.BlockCopy(nds, 0, nd, 0, ByteSize);
                bw.Write(nd);

                // These are each 256 bytes because they are ASCII encoded, not the standard DotNet Unicode
                byte[] proj = new byte[255];
                if (Projection != null)
                {
                    byte[] temp = Encoding.Default.GetBytes(Projection.ToProj4String());
                    int len = Math.Min(temp.Length, 255);
                    for (int i = 0; i < len; i++)
                    {
                        proj[i] = temp[i];
                    }
                    string prj = Path.ChangeExtension(Filename, ".prj");
                    if (File.Exists(prj))
                    {
                        File.Delete(prj);
                    }
                    var fi = new FileInfo(prj);
                    using (var tw = fi.CreateText())
                    {
                        tw.WriteLine(Projection.ToEsriString());
                    }
                }
                bw.Write(proj);
                byte[] note = new byte[255];
                if (Notes != null)
                {
                    byte[] temp = Encoding.Default.GetBytes(Notes);
                    int len = Math.Min(temp.Length, 255);
                    for (int i = 0; i < len; i++)
                    {
                        note[i] = temp[i];
                    }
                }

                bw.Write(note);
            }
        }

        private RasterDataType GetRasterDataType()
        {
            if (DataType == typeof(byte))
                return RasterDataType.BYTE;
            if (DataType == typeof(short))
                return RasterDataType.SHORT;
            if (DataType == typeof (int))
                return RasterDataType.INTEGER;
            if (DataType == typeof(long))
                return RasterDataType.LONG;
            if (DataType == typeof(float))
                return RasterDataType.SINGLE;
            if (DataType == typeof(double))
                return RasterDataType.DOUBLE;
            if (DataType == typeof(sbyte))
                return RasterDataType.SBYTE;
            if (DataType == typeof(ushort))
                return RasterDataType.USHORT;
            if (DataType == typeof(uint))
                return RasterDataType.UINTEGER;
            if (DataType == typeof(ulong))
                return RasterDataType.ULONG;
            if (DataType == typeof(bool))
                return RasterDataType.BOOL;

            return RasterDataType.INVALID;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the size of the header.  There is one no-data value in the header.
        /// </summary>
        public virtual int HeaderSize
        {
            get { return 554 + ByteSize; }
        }

        /// <summary>
        /// Writes the header, regardless of which subtype of binary raster this is written for
        /// </summary>
        /// <param name="fileName">The string fileName specifying what file to load</param>
        public void ReadHeader(string fileName)
        {
            using (var br = new BinaryReader(new FileStream(fileName, FileMode.Open)))
            {
                StartColumn = 0;
                NumColumns = br.ReadInt32();
                NumColumnsInFile = NumColumns;
                EndColumn = NumColumns - 1;
                StartRow = 0;
                NumRows = br.ReadInt32();
                NumRowsInFile = NumRows;
                EndRow = NumRows - 1;
                Bounds = new RasterBounds(NumRows, NumColumns, new[] {0.0, 1.0, 0.0, NumRows, 0.0, -1.0});

                CellWidth = br.ReadDouble();
                Bounds.AffineCoefficients[5] = -br.ReadDouble(); // dy
                Xllcenter = br.ReadDouble();
                Yllcenter = br.ReadDouble();
                br.ReadInt32(); //  Read RasterDataType only to skip it since we know the type already.
                byte[] noDataBytes = br.ReadBytes(ByteSize);
                var nd = new T[1];
                Buffer.BlockCopy(noDataBytes, 0, nd, 0, ByteSize);
                NoDataValue = Global.ToDouble(nd[0]);
                string proj = Encoding.Default.GetString(br.ReadBytes(255)).Replace('\0', ' ').Trim();
                ProjectionString = proj;

                Notes = Encoding.Default.GetString(br.ReadBytes(255)).Replace('\0', ' ').Trim();
                if (Notes.Length == 0) Notes = null;
            }

            string prj = Path.ChangeExtension(Filename, ".prj");
            if (File.Exists(prj))
            {
                using (var sr = new StreamReader(prj))
                {
                    ProjectionString = sr.ReadToEnd();
                }
            }
        }

        #endregion
    }
}