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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/25/2010 4:20:33 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// |   Ted Dunsford       |  6/30/2010  |  Moved to DotSpatial
// ********************************************************************************************************

using System;
using DotSpatial.Data;

namespace DotSpatial.Analysis
{
    /// <summary>
    /// A class for supporting methods to calculate the slope.
    /// </summary>
    public static class Slope
    {
        /// <summary>
        /// Executes the slope generation raster.
        /// </summary>
        /// <param name="raster">The input altitude raster.</param>
        /// <param name="inZFactor">The double precision multiplicative scaling factor for elevation values.</param>
        /// <param name="slopeInPercent">A boolean parameter that clarifies the nature of the slope values.  If this is true, the values represent percent slope.</param>
        /// <param name="result">The output slope raster.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>A boolean value, true if the process was successful.</returns>
        public static bool GetSlope(IRaster raster, double inZFactor, bool slopeInPercent, ref IRaster result,
                                    ICancelProgressHandler cancelProgressHandler)
        {
            //Validates the input and output data
            if (raster == null || result == null)
            {
                return false;
            }
            int noOfCol = raster.NumColumns;
            int noOfRow = raster.NumRows;

            //Create the new raster with the appropriate dimensions
            IRaster temp = Raster.CreateRaster("SlopeRaster.bgd", string.Empty, noOfCol, noOfRow, 1, typeof(double),
                                               new[] { string.Empty });
            temp.NoDataValue = raster.NoDataValue;
            temp.Bounds = raster.Bounds;
            temp.Projection = raster.Projection;

            ProgressMeter progMeter = null;
            try
            {
                if (cancelProgressHandler != null)
                    progMeter = new ProgressMeter(cancelProgressHandler, "Calculating Slope", temp.NumRows);

                for (int i = 0; i < temp.NumRows; i++)
                {

                    if (cancelProgressHandler != null)
                    {
                        progMeter.Next();
                        if ((i % 100) == 0)
                        {
                            progMeter.SendProgress();

                            // HACK: DoEvents messes up the normal flow of your application. 
                            System.Windows.Forms.Application.DoEvents();
                        }

                    }
                    for (int j = 0; j < temp.NumColumns; j++)
                    {
                        if (i > 0 && i < temp.NumRows - 1 && j > 0 && j < temp.NumColumns - 1)
                        {
                            double z1 = raster.Value[i - 1, j - 1];
                            double z2 = raster.Value[i - 1, j];
                            double z3 = raster.Value[i - 1, j + 1];
                            double z4 = raster.Value[i, j - 1];
                            double z5 = raster.Value[i, j + 1];
                            double z6 = raster.Value[i + 1, j - 1];
                            double z7 = raster.Value[i + 1, j];
                            double z8 = raster.Value[i + 1, j + 1];

                            //3rd Order Finite Difference slope algorithm
                            double dZdX = inZFactor * ((z3 - z1) + (2 * (z5 - z4)) + (z8 - z6)) / (8 * raster.CellWidth);
                            double dZdY = inZFactor * ((z1 - z6) + (2 * (z2 - z7)) + (z3 - z8)) / (8 * raster.CellHeight);

                            double slope = Math.Atan(Math.Sqrt((dZdX * dZdX) + (dZdY * dZdY))) * (180 / Math.PI);

                            //change to radius and in percentage
                            if (slopeInPercent)
                            {
                                slope = (Math.Tan(slope * Math.PI / 180)) * 100;
                            }

                            temp.Value[i, j] = slope;

                            if (cancelProgressHandler != null && cancelProgressHandler.Cancel)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            temp.Value[i, j] = temp.NoDataValue;
                        }

                        if (cancelProgressHandler != null && cancelProgressHandler.Cancel)
                        {
                            return false;
                        }
                    }
                }

                result = temp;
                if (result.IsFullyWindowed())
                {
                    result.Save();
                    return true;
                }
                return false;
            }
            finally
            {
                if (progMeter != null)
                {
                    progMeter.Reset();
                    System.Windows.Forms.Application.DoEvents();
                }
            }
        }
    }
}