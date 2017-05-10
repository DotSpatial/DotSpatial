// *******************************************************************************************************
// Product: DotSpatial.Tools.RasterMagic.cs
// Description:  Used to consolidate duplicate code across multiple tools.
//
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders.
// -------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
// -------------------|--------------------|--------------------------------------------------------------
// ********************************************************************************************************

using System;
using DotSpatial.Data;
using GeoAPI.Geometries;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Used to consolidate duplicate code across multiple tools.
    /// </summary>
    internal class RasterMagic
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterMagic"/> class.
        /// </summary>
        public RasterMagic()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterMagic"/> class.
        /// </summary>
        /// <param name="operation">Operation the RasterMagic should perform in RasterMath.</param>
        public RasterMagic(OperationDelegate operation)
        {
            Operation = operation;
        }

        #endregion

        /// <summary>
        /// Gets or sets the operation the RasterMagic should perform in RasterMath.
        /// </summary>
        /// <param name="val1">First operand.</param>
        /// <param name="val2">Second operand.</param>
        /// <returns>Result of the operation.</returns>
        public delegate double OperationDelegate(double val1, double val2);

        #region Properties

        /// <summary>
        /// Gets or sets the operation the RasterMagic should perform in RasterMath.
        /// </summary>
        public OperationDelegate Operation { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Performs the operation on the input rasters.
        /// </summary>
        /// <param name="input1">First input raster.</param>
        /// <param name="input2">Second input raster.</param>
        /// <param name="output">The output raster.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True, if the operation was performed successfully.</returns>
        public bool RasterMath(IRaster input1, IRaster input2, IRaster output, ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (input1 == null || input2 == null || output == null) return false;

            // Figures out which raster has smaller cells
            var smallestCellRaster = input1.CellWidth < input2.CellWidth ? input1 : input2;
            var envelope = UnionEnvelope(input1, input2);
            envelope.MinX = envelope.MinX + (smallestCellRaster.CellWidth / 2);
            envelope.MinY = envelope.MinY - (smallestCellRaster.CellHeight / 2);
            envelope.MaxX = envelope.MaxX + (smallestCellRaster.CellWidth / 2);
            envelope.MaxY = envelope.MaxY - (smallestCellRaster.CellHeight / 2);

            // Given the envelope of the two rasters we calculate the number of columns / rows
            int noOfCol = Convert.ToInt32(Math.Abs(envelope.Width / smallestCellRaster.CellWidth));
            int noOfRow = Convert.ToInt32(Math.Abs(envelope.Height / smallestCellRaster.CellHeight));

            // create output raster
            output = Raster.CreateRaster(output.Filename, string.Empty, noOfCol, noOfRow, 1, typeof(int), new[] { string.Empty });
            var bound = new RasterBounds(noOfRow, noOfCol, envelope);
            output.Bounds = bound;
            output.NoDataValue = input1.NoDataValue;

            int previous = 0;
            int max = output.Bounds.NumRows + 1;
            for (int i = 0; i < output.Bounds.NumRows; i++)
            {
                for (int j = 0; j < output.Bounds.NumColumns; j++)
                {
                    Coordinate cellCenter = output.CellToProj(i, j);
                    var v1 = input1.ProjToCell(cellCenter);
                    double val1;
                    if (v1.Row <= input1.EndRow && v1.Column <= input1.EndColumn && v1.Row > -1 && v1.Column > -1)
                    {
                        val1 = input1.Value[v1.Row, v1.Column];
                    }
                    else
                    {
                        val1 = input1.NoDataValue;
                    }

                    var v2 = input2.ProjToCell(cellCenter);
                    double val2;
                    if (v2.Row <= input2.EndRow && v2.Column <= input2.EndColumn && v2.Row > -1 && v2.Column > -1)
                    {
                        val2 = input2.Value[v2.Row, v2.Column];
                    }
                    else
                    {
                        val2 = input2.NoDataValue;
                    }

                    if (val1 == input1.NoDataValue || val2 == input2.NoDataValue)
                    {
                        output.Value[i, j] = output.NoDataValue;
                    }
                    else
                    {
                        output.Value[i, j] = Operation(val1, val2);
                    }

                    if (cancelProgressHandler.Cancel)
                    {
                        return false;
                    }
                }

                int current = Convert.ToInt32(Math.Round(i * 100D / max));

                // only update when increment in persentage
                if (current > previous)
                {
                    cancelProgressHandler.Progress(string.Empty, current, current + TextStrings.progresscompleted);
                }

                previous = current;
            }

            output.Save();
            return true;
        }

        /// <summary>
        /// Expands the first envelope to include the second.
        /// </summary>
        /// <param name="input1">The first input raster to union the envelope for.</param>
        /// <param name="input2">The second input raster to union the envelope for.</param>
        /// <returns>The expanded envelope.</returns>
        private static Extent UnionEnvelope(IRaster input1, IRaster input2)
        {
            Extent e1 = input1.Bounds.Extent;
            Extent e2 = input2.Bounds.Extent;
            e1.ExpandToInclude(e2);
            return e1;
        }

        #endregion
    }
}