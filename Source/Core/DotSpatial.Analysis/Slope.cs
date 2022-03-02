// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Windows.Forms;

using DotSpatial.Data;

namespace DotSpatial.Analysis
{
    /// <summary>
    /// A class for supporting methods to calculate the slope.
    /// </summary>
    public static class Slope
    {
        #region Methods

        /// <summary>
        /// Executes the slope generation raster.
        /// </summary>
        /// <param name="raster">The input altitude raster.</param>
        /// <param name="inZFactor">The double precision multiplicative scaling factor for elevation values.</param>
        /// <param name="slopeInPercent">A boolean parameter that clarifies the nature of the slope values.  If this is true, the values represent percent slope.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>The output slope raster, or null if the process was unsuccessful.</returns>
        public static IRaster GetSlope(IRaster raster, double inZFactor, bool slopeInPercent, ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (raster == null)
            {
                return null;
            }

            int noOfCol = raster.NumColumns;
            int noOfRow = raster.NumRows;

            // Create the new raster with the appropriate dimensions
            var result = Raster.CreateRaster("SlopeRaster.bgd", string.Empty, noOfCol, noOfRow, 1, typeof(double), new[] { string.Empty });
            result.NoDataValue = raster.NoDataValue;
            result.Bounds = raster.Bounds;
            result.Projection = raster.Projection;

            ProgressMeter progMeter = null;
            try
            {
                if (cancelProgressHandler != null) progMeter = new ProgressMeter(cancelProgressHandler, "Calculating Slope", result.NumRows);

                // Cache cell size for faster access
                var cellWidth = raster.CellWidth;
                var cellHeight = raster.CellHeight;
                for (int i = 0; i < result.NumRows; i++)
                {
                    if (cancelProgressHandler != null)
                    {
                        progMeter.Next();
                        if ((i % 100) == 0)
                        {
                            progMeter.SendProgress();

                            // HACK: DoEvents messes up the normal flow of your application.
                            Application.DoEvents();
                        }
                    }

                    for (int j = 0; j < result.NumColumns; j++)
                    {
                        if (i > 0 && i < result.NumRows - 1 && j > 0 && j < result.NumColumns - 1)
                        {
                            double z1 = raster.Value[i - 1, j - 1];
                            double z2 = raster.Value[i - 1, j];
                            double z3 = raster.Value[i - 1, j + 1];
                            double z4 = raster.Value[i, j - 1];
                            double z5 = raster.Value[i, j + 1];
                            double z6 = raster.Value[i + 1, j - 1];
                            double z7 = raster.Value[i + 1, j];
                            double z8 = raster.Value[i + 1, j + 1];

                            // 3rd Order Finite Difference slope algorithm
                            double dZdX = inZFactor * ((z3 - z1) + (2 * (z5 - z4)) + (z8 - z6)) / (8 * cellWidth);
                            double dZdY = inZFactor * ((z1 - z6) + (2 * (z2 - z7)) + (z3 - z8)) / (8 * cellHeight);

                            double slope = Math.Atan(Math.Sqrt((dZdX * dZdX) + (dZdY * dZdY))) * (180 / Math.PI);

                            // change to radius and in percentage
                            if (slopeInPercent)
                            {
                                slope = Math.Tan(slope * Math.PI / 180) * 100;
                            }

                            result.Value[i, j] = slope;

                            if (cancelProgressHandler != null && cancelProgressHandler.Cancel)
                            {
                                return null;
                            }
                        }
                        else
                        {
                            result.Value[i, j] = result.NoDataValue;
                        }

                        if (cancelProgressHandler != null && cancelProgressHandler.Cancel)
                        {
                            return null;
                        }
                    }
                }

                if (result.IsFullyWindowed())
                {
                    result.Save();
                    return result;
                }

                return null;
            }
            finally
            {
                if (progMeter != null)
                {
                    progMeter.Reset();
                    Application.DoEvents();
                }
            }
        }

        #endregion
    }
}