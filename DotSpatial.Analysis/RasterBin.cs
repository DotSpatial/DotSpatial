// ********************************************************************************************************
// Product Name: DotSpatial.Analysis.dll
// Description:  The analysis libraries provide a programming API for the processes wrapped by tools.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created August, 2007
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// |                      |             |
// ********************************************************************************************************

using System;
using System.IO;
using DotSpatial.Data;

namespace DotSpatial.Analysis
{
    /// <summary>
    /// A class for creating "ranges" of raster values.
    /// </summary>
    public class RasterBin
    {
        /// <summary>
        /// Initializes a new instance of the RasterBin class.
        /// </summary>
        public RasterBin()
        {
            BinSize = 10;
        }

        /// <summary>
        /// Gets or sets the "origin" of the bins.  Bins may occur above or below this,
        /// in increments of "BinSize".  The default is 0.
        /// </summary>
        public double BaseValue { get; set; }

        /// <summary>
        /// Gets or sets the double value separating the bins for the raster.  The default is 10.
        /// </summary>
        public double BinSize { get; set; }

        /// <summary>
        /// This uses the BaseValue and BinSize properties in order to categorize the values
        /// according to the source.  The cells in the bin will receive a value that is equal
        /// to the midpoint between the range.  So a range from 0 to 10 will all have the value
        /// of 5.  Values with no data continue to be marked as NoData.
        /// </summary>
        /// <param name="source">The source raster.</param>
        /// <param name="destName">The output filename.</param>
        /// <param name="progressHandler">The progress handler for messages.</param>
        /// <returns>The IRaster of binned values from the original source.</returns>
        public bool BinRaster(IRaster source, string destName, ICancelProgressHandler progressHandler)
        {
            IRaster result;
            try
            {
                result = Raster.Create(destName, String.Empty, source.NumColumns, source.NumRows, source.NumBands,
                                       source.DataType, new string[] { });
                result.Bounds = source.Bounds.Copy();
                result.Projection = source.Projection;
            }
            catch (Exception)
            {
                File.Copy(source.Filename, destName);
                result = Raster.Open(destName);
            }
            bool finished;

            if (source.DataType == typeof(int))
            {
                finished = BinRaster(source.ToRaster<int>(), result.ToRaster<int>(), progressHandler);
            }
            else if (source.DataType == typeof(float))
            {
                finished = BinRaster(source.ToRaster<float>(), result.ToRaster<float>(), progressHandler);
            }
            else if (source.DataType == typeof(short))
            {
                finished = BinRaster(source.ToRaster<short>(), result.ToRaster<short>(), progressHandler);
            }
            else if (source.DataType == typeof(byte))
            {
                finished = BinRaster(source.ToRaster<byte>(), result.ToRaster<byte>(), progressHandler);
            }
            else if (source.DataType == typeof(double))
            {
                finished = BinRaster(source.ToRaster<double>(), result.ToRaster<double>(), progressHandler);
            }
            else
            {
                finished = BinRasterSlow(source, result, progressHandler);
            }
            if (finished)
            {
                result.Save();
            }
            return finished;
        }

        private bool BinRaster<T>(Raster<T> source, Raster<T> result, ICancelProgressHandler progressHandler)
            where T : IEquatable<T>, IComparable<T>
        {
            ProgressMeter pm = new ProgressMeter(progressHandler, "Calculating values", source.NumRows);
            for (int row = 0; row < source.NumRows; row++)
            {
                for (int col = 0; col < source.NumColumns; col++)
                {
                    double value = (double)Convert.ChangeType(source.Data[row][col], typeof(double));
                    int sign = Math.Sign(value);
                    value = (value - BaseValue) / BinSize;
                    value = Math.Floor(value);
                    value = BinSize * value + sign * BinSize / 2;
                    result.Data[row][col] = (T)Convert.ChangeType(value, typeof(T));
                }
                pm.Next();
                if (progressHandler.Cancel) return false;
            }
            return true;
        }

        private bool BinRasterSlow(IRaster source, IRaster result, ICancelProgressHandler progressHandler)
        {
            ProgressMeter pm = new ProgressMeter(progressHandler, "Calculating values", source.NumRows);
            for (int row = 0; row < source.NumRows; row++)
            {
                for (int col = 0; col < source.NumColumns; col++)
                {
                    double value = source.Value[row, col];
                    int sign = Math.Sign(value);
                    value = (value - BaseValue) / BinSize;
                    value = Math.Floor(value);
                    value = BinSize * value + sign * BinSize / 2;
                    result.Value[row, col] = value;
                }
                pm.Next();
                if (progressHandler.Cancel) return false;
            }
            pm.Reset();
            return true;
        }
    }
}