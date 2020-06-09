// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Tools
{
    /// <summary>
    /// This tool calculates the euclidean distance from each raster cell to the nearest target cell.
    /// </summary>
    public class RasterDistance : Tool
    {
        #region Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterDistance"/> class.
        /// </summary>
        public RasterDistance()
        {
            Name = TextStrings.RasterDistanceproximity;
            Category = TextStrings.RasterOverlay;
            Description = TextStrings.Calculateseuclideandistance;
            ToolTip = TextStrings.RasterDistanceproximity;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the input paramater array.
        /// </summary>
        public override Parameter[] InputParameters => _inputParam;

        /// <summary>
        /// Gets the output paramater array.
        /// </summary>
        public override Parameter[] OutputParameters => _outputParam;

        #endregion

        #region Methods

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if successful.
        /// </summary>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>Boolean, true if the execute was successful.</returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IRaster inputRaster = _inputParam[0].Value as IRaster;

            IRaster outputRaster = _outputParam[0].Value as IRaster;
            double maximumDistance = (double)_inputParam[1].Value;

            return Execute(inputRaster, outputRaster, maximumDistance, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the Raster distance calculation
        /// Ping Yang deleted static for external testing 10/2010.
        /// </summary>
        /// <param name="input">The input raster.</param>
        /// <param name="output">The output raster.</param>
        /// <param name="maxDistance">The maximum distance value. Cells with a larger distance to nearest
        /// target cell than maxDistance will be assigned 'no data' value.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>true if execution was successful, false otherwise.</returns>
        public bool Execute(IRaster input, IRaster output, double maxDistance, ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (input == null || output == null)
            {
                return false;
            }

            // Creates the output raster with the same bounds as the input
            RasterBounds bounds = (RasterBounds)input.Bounds.Copy();

            output = Raster.CreateRaster(output.Filename, string.Empty, bounds.NumColumns, bounds.NumRows, 1, typeof(int), new[] { string.Empty });
            output.Bounds = bounds;

            // internally we reference output as an integer type raster.
            if (output is Raster<int> outRaster)
            {
                int numColumns = outRaster.NumColumns;
                int numRows = outRaster.NumRows;
                int lastUpdate = 0;

                // declare two jagged arrays for storing the (Rx, Ry) vectors.
                // rX is distance from current cell to nearest target cell measured in the x-direction
                // rY is distance from current cell to nearest target cell measured in the y-direction
                // the actual distance can be calculated as sqrt(rX^2 + rY^2).
                // in the resulting distance raster we store the squared distance as well as the rX, rY relative coordinates
                // to improve computation speed
                int[][] aRx = new int[numRows][];
                int[][] aRy = new int[numRows][];
                int[][] aSqDist = new int[numRows][];

                const int InfD = int.MaxValue;
                const int TargetVal = 0;

                // initialize the arrays
                for (int i = 0; i < numRows; i++)
                {
                    aRx[i] = new int[numColumns];
                    aRy[i] = new int[numColumns];
                    aSqDist[i] = new int[numColumns];
                    ReadInputRow(input, i, aSqDist[i], TargetVal, InfD);
                }

                // *******************************************************************
                // raster distance calculation pass one - top to bottom, left to right
                // *******************************************************************
                // int[] row1 = new int[numColumns];
                // int[] row2 = new int[numColumns];
                int[] aNcels = new int[4]; // the values of four neighbouring cells (W, NW, N, NE)
                int[] aDiff = new int[4]; // the | y coordinate distances to nearest target cell

                for (int row = 1; row < numRows; row++)
                {
                    //// read the row from input raster
                    // ReadInputRow(input, rowIndex, row2, targetVal, infD);
                    for (int col = 1; col < numColumns - 1; col++)
                    {
                        int val = aSqDist[row][col];

                        // Continue processing only if the current cell is not a target
                        if (val == TargetVal)
                        {
                            continue;
                        }

                        // read the values of the cell's neighbours
                        aNcels[0] = aSqDist[row][col - 1]; // W
                        aNcels[1] = aSqDist[row - 1][col - 1]; // NW
                        aNcels[2] = aSqDist[row - 1][col]; // N
                        aNcels[3] = aSqDist[row - 1][col + 1]; // NE

                        // calculate the squared euclidean distances to each neighbouring cell and to the nearest target cell
                        aDiff[0] = (aNcels[0] < InfD) ? aNcels[0] + (2 * aRx[row][col - 1]) + 1 : InfD;
                        aDiff[1] = (aNcels[1] < InfD) ? aNcels[1] + (2 * (aRx[row - 1][col - 1] + aRy[row - 1][col - 1] + 1)) : InfD;
                        aDiff[2] = (aNcels[2] < InfD) ? aNcels[2] + (2 * aRy[row - 1][col]) + 1 : InfD;
                        aDiff[3] = (aNcels[3] < InfD) ? aNcels[3] + (2 * (aRx[row - 1][col + 1] + aRy[row - 1][col + 1] + 1)) : InfD;

                        // find neighbouring cell with minimum distance difference
                        int minDiff = aDiff[0];
                        int minDiffCell = 0;
                        for (int i = 1; i < 4; i++)
                        {
                            if (aDiff[i] < minDiff)
                            {
                                minDiff = aDiff[i];
                                minDiffCell = i;
                            }
                        }

                        // if a neighbouring cell with known distance was found:
                        if (minDiff < InfD)
                        {
                            // assign the minimum euclidean distance
                            aSqDist[row][col] = minDiff;

                            // update the (rX, rY) cell-to-nearest-target vector
                            switch (minDiffCell)
                            {
                                case 0: // W
                                    aRx[row][col] = aRx[row][col - 1] + 1;
                                    aRy[row][col] = aRy[row][col - 1];
                                    break;
                                case 1: // NW
                                    aRx[row][col] = aRx[row - 1][col - 1] + 1;
                                    aRy[row][col] = aRy[row - 1][col - 1] + 1;
                                    break;
                                case 2: // N
                                    aRx[row][col] = aRx[row - 1][col];
                                    aRy[row][col] = aRy[row - 1][col] + 1;
                                    break;
                                case 3: // NE
                                    aRx[row][col] = aRx[row - 1][col + 1] + 1;
                                    aRy[row][col] = aRy[row - 1][col + 1] + 1;
                                    break;
                            }
                        }

                        // end of update (rX, rY) cell-to-nearest-target vector
                    }

                    // end or current row processing - report progress
                    int percent = (int)(row / (double)numRows * 100f);
                    if (percent > lastUpdate)
                    {
                        lastUpdate += 1;
                        cancelProgressHandler.Progress(string.Empty, lastUpdate, TextStrings.Pass1 + lastUpdate + TextStrings.progresscompleted);
                        if (cancelProgressHandler.Cancel)
                        {
                            return false;
                        }
                    }
                }

                // *******************************************************************
                // end of first pass for loop
                // *******************************************************************

                // *******************************************************************
                // raster distance calculation PASS TWO - bottom to top, right to left
                // *******************************************************************
                lastUpdate = 0;
                for (int row = numRows - 2; row > 0; row--)
                {
                    for (int col = numColumns - 2; col > 0; col--)
                    {
                        int val = aSqDist[row][col];

                        // Continue processing only if the current cell is not a target
                        if (val == TargetVal)
                        {
                            continue;
                        }

                        // read the values of the cell's neighbours
                        aNcels[0] = aSqDist[row][col + 1]; // E
                        aNcels[1] = aSqDist[row + 1][col + 1]; // SE
                        aNcels[2] = aSqDist[row + 1][col]; // S
                        aNcels[3] = aSqDist[row + 1][col - 1]; // SW

                        // calculate the squared euclidean distances to each neighbouring cell and to the nearest target cell
                        aDiff[0] = (aNcels[0] < InfD) ? aNcels[0] + (2 * aRx[row][col + 1]) + 1 : InfD;
                        aDiff[1] = (aNcels[1] < InfD) ? aNcels[1] + (2 * (aRx[row + 1][col + 1] + aRy[row + 1][col + 1] + 1)) : InfD;
                        aDiff[2] = (aNcels[2] < InfD) ? aNcels[2] + (2 * aRy[row + 1][col]) + 1 : InfD;
                        aDiff[3] = (aNcels[3] < InfD) ? aNcels[3] + (2 * (aRx[row + 1][col - 1] + aRy[row + 1][col - 1] + 1)) : InfD;

                        // find neighbouring cell with minimum distance difference
                        int minDiff = aDiff[0];
                        int minDiffCell = 0;
                        for (int i = 1; i < 4; i++)
                        {
                            if (aDiff[i] < minDiff)
                            {
                                minDiff = aDiff[i];
                                minDiffCell = i;
                            }
                        }

                        // if a neighbouring cell with known distance smaller than current known distance was found:
                        if (minDiff < val)
                        {
                            // assign the minimum euclidean distance
                            aSqDist[row][col] = minDiff;

                            // update the (rX, rY) cell-to-nearest-target vector
                            switch (minDiffCell)
                            {
                                case 0: // E
                                    aRx[row][col] = aRx[row][col + 1] + 1;
                                    aRy[row][col] = aRy[row][col + 1];
                                    break;
                                case 1: // SE
                                    aRx[row][col] = aRx[row + 1][col + 1] + 1;
                                    aRy[row][col] = aRy[row + 1][col + 1] + 1;
                                    break;
                                case 2: // S
                                    aRx[row][col] = aRx[row + 1][col];
                                    aRy[row][col] = aRy[row + 1][col] + 1;
                                    break;
                                case 3: // SW
                                    aRx[row][col] = aRx[row + 1][col - 1] + 1;
                                    aRy[row][col] = aRy[row + 1][col - 1] + 1;
                                    break;
                            }
                        }

                        // end of update (rX, rY) cell-to-nearest-target vector
                    }

                    // Write row to output raster
                    WriteOutputRow(outRaster, row, aSqDist[row]);

                    // Report progress
                    int percent = (int)(row / (double)numRows * 100f);
                    if (percent > lastUpdate)
                    {
                        lastUpdate += 1;
                        cancelProgressHandler.Progress(string.Empty, lastUpdate, TextStrings.Pass2 + lastUpdate + TextStrings.progresscompleted);
                        if (cancelProgressHandler.Cancel)
                        {
                            return false;
                        }
                    }
                }
            }

            // *******************************************************************
            // end of second pass proximity calculation loop
            // *******************************************************************

            // save output raster
            output.Save();

            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here.
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new RasterParam(TextStrings.inputRaster)
                                 {
                                     HelpText = TextStrings.InputRastercontainingtargetcells
                                 };
            _inputParam[1] = new DoubleParam(TextStrings.Maximumdistance, 100.0)
                                 {
                                     HelpText = TextStrings.Maximumdistancetobecalculated
                                 };
            _outputParam = new Parameter[2];
            _outputParam[0] = new RasterParam(TextStrings.OutputRaster)
                                  {
                                      HelpText = TextStrings.SelectresultrasterfileName
                                  };
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        /// <summary>
        /// Reads and converts the specified row from input raster to an array of integer
        /// It assigns noDataVal to 'noData' values and assigns dataVal to other values.
        /// </summary>
        /// <param name="input">Raster from which the row is read.</param>
        /// <param name="rowNumber">Number of the row that gets converted.</param>
        /// <param name="rowArray">The array where the row is saved. This array must be correctly dimensioned.</param>
        /// <param name="dataVal">New value which will be assigned to value cells.</param>
        /// <param name="noDataVal">New value which will be assigned to 'no data value' cells.</param>
        private static void ReadInputRow(IRaster input, int rowNumber, int[] rowArray, int dataVal, int noDataVal)
        {
            // IValueGrid vals = input.Value;
            double inputNoDataVal = input.NoDataValue;

            int numColumns = input.NumColumns;

            for (int column = 0; column < numColumns; column++)
            {
                if (input.Value[rowNumber, column] == inputNoDataVal)
                {
                    rowArray[column] = noDataVal;
                }
                else
                {
                    rowArray[column] = dataVal;
                }
            }
        }

        /// <summary>
        /// Writes the integer row array to the output raster. The square distance is
        /// converted to a normal distance. Unknown distance is converted to 'no data' value.
        /// </summary>
        /// <param name="output">The raster the row array is written to.</param>
        /// <param name="rowNumber">Number of the row.</param>
        /// <param name="rowArray">The array that contains the row. This array must be correctly dimensioned.</param>
        private static void WriteOutputRow(Raster<int> output, int rowNumber, int[] rowArray)
        {
            int noDataVal = (int)output.NoDataValue;

            for (int col = 0; col < rowArray.Length; col++)
            {
                int val = rowArray[col];
                if (val == int.MaxValue)
                {
                    output.Data[rowNumber][col] = noDataVal;
                }
                else
                {
                    output.Data[rowNumber][col] = (int)Math.Sqrt(val);
                }
            }
        }

        #endregion
    }
}