// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

using DotSpatial.Data;

using NetTopologySuite.Geometries;

namespace DotSpatial.Analysis
{
    /// <summary>
    /// This class resamples the given raster cells.
    /// </summary>
    public static class ResampleCells
    {
        #region Methods

        /// <summary>
        /// This will resample the cells.
        /// If the cell size is zero, this will default to the shorter of the width or height
        /// divided by 256.
        /// </summary>
        /// <param name="input1">The input raster.</param>
        /// <param name="cellHeight">The new cell height.</param>
        /// <param name="cellWidth">The new cell width.</param>
        /// <param name="outputFileName">The destination file name.</param>
        /// <returns>Resampled raster name.</returns>
        public static IRaster Resample(IRaster input1, double cellHeight, double cellWidth, string outputFileName)
        {
            return Resample(input1, cellHeight, cellWidth, outputFileName, null);
        }

        /// <summary>
        /// This will resample the cells.
        /// If the cell size is zero, this will default to the shorter of the width or height
        /// divided by 256.
        /// </summary>
        /// <param name="input1">the input raster.</param>
        /// <param name="cellHeight">The new cell height or null.</param>
        /// <param name="cellWidth">The new cell width or null.</param>
        /// <param name="outputFileName">The string name of the output raster.</param>
        /// <param name="progressHandler">An interface for handling the progress messages.</param>
        /// <returns>The resampled raster.</returns>
        public static IRaster Resample(IRaster input1, double cellHeight, double cellWidth, string outputFileName, IProgressHandler progressHandler)
        {
            if (input1 == null)
            {
                return null;
            }

            Extent envelope = input1.Bounds.Extent;

            if (cellHeight == 0)
            {
                cellHeight = envelope.Height / 256;
            }

            if (cellWidth == 0)
            {
                cellWidth = envelope.Width / 256;
            }

            // Calculate new number of columns and rows
            int noOfCol = Convert.ToInt32(Math.Abs(envelope.Width / cellWidth));
            int noOfRow = Convert.ToInt32(Math.Abs(envelope.Height / cellHeight));

            IRaster output = Raster.CreateRaster(outputFileName, string.Empty, noOfCol, noOfRow, 1, input1.DataType, new[] { string.Empty });
            RasterBounds bound = new(noOfRow, noOfCol, envelope);
            output.Bounds = bound;

            output.NoDataValue = input1.NoDataValue;

            int max = output.Bounds.NumRows;
            ProgressMeter pm = new(progressHandler, "ReSize Cells", max);

            // Loop through every cell for new value
            for (int i = 0; i < max; i++)
            {
                for (int j = 0; j < output.Bounds.NumColumns; j++)
                {
                    // Project the cell position to Map
                    Coordinate cellCenter = output.CellToProj(i, j);
                    var index1 = input1.ProjToCell(cellCenter);

                    double val;
                    if (index1.Row <= input1.EndRow && index1.Column <= input1.EndColumn && index1.Row > -1 && index1.Column > -1)
                    {
                        val = input1.Value[index1.Row, index1.Column] == input1.NoDataValue ? output.NoDataValue : input1.Value[index1.Row, index1.Column];
                    }
                    else
                    {
                        val = output.NoDataValue;
                    }

                    output.Value[i, j] = val;
                }

                pm.CurrentValue = i;
            }

            output.Save();
            pm.Reset();
            return output;
        }

        #endregion
    }
}