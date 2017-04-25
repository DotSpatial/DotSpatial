// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/25/2008 9:34:48 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// |   Ted Dunsford       |  6/30/2010  |  Moved to DotSpatial
// ********************************************************************************************************

using System;
using GeoAPI.Geometries;

namespace DotSpatial.Data
{
    public class AffineTransform
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of AffineTransform
        /// </summary>
        public AffineTransform(double[] inCoefficients)
        {
            Coefficients = inCoefficients;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Given any input row or column, this returns the appropriate geographic location for the
        /// position of the center of the cell.
        /// </summary>
        /// <param name="row">The integer row index from 0 to numRows - 1</param>
        /// <param name="column">The integer column index from 0 to numColumns - 1</param>
        /// <returns>The geographic position of the center of the specified cell</returns>
        public Coordinate CellCenter_ToProj(int row, int column)
        {
            double x = Coefficients[0] + Coefficients[1] * column + Coefficients[2] * row;
            double y = Coefficients[3] + Coefficients[4] * column + Coefficients[5] * row;
            return new Coordinate(x, y);
        }

        /// <summary>
        /// Given the row and column, this returns the geographic position of the top left corner of the cell
        /// </summary>
        /// <param name="row">The integer row index from 0 to numRows - 1</param>
        /// <param name="column">The integer column index from 0 to numColumns - 1</param>
        /// <returns>The geographic position of the top left corner of the specified cell</returns>
        public Coordinate CellTopLeft_ToProj(int row, int column)
        {
            double x = Coefficients[0] + Coefficients[1] * (column - 0.5) + Coefficients[2] * (row - 0.5);
            double y = Coefficients[3] + Coefficients[4] * (column - 0.5) + Coefficients[5] * (row - 0.5);
            return new Coordinate(x, y);
        }

        /// <summary>
        /// Given the row and column, this returns the geographic position of the top right corner of the cell
        /// </summary>
        /// <param name="row">The integer row index from 0 to numRows - 1</param>
        /// <param name="column">The integer column index from 0 to numColumns - 1</param>
        /// <returns>The geographic position of the top right corner of the specified cell</returns>
        public Coordinate CellTopRight_ToProj(int row, int column)
        {
            double x = Coefficients[0] + Coefficients[1] * (column + 0.5) + Coefficients[2] * (row - 0.5);
            double y = Coefficients[3] + Coefficients[4] * (column + 0.5) + Coefficients[5] * (row - 0.5);
            return new Coordinate(x, y);
        }

        /// <summary>
        /// Given the row and column, this returns the geographic position of the bottom left corner of the cell
        /// </summary>
        /// <param name="row">The integer row index from 0 to numRows - 1</param>
        /// <param name="column">The integer column index from 0 to numColumns - 1</param>
        /// <returns>The geographic position of the bottom left corner of the specified cell</returns>
        public Coordinate CellBottomLeft_ToProj(int row, int column)
        {
            double x = Coefficients[0] + Coefficients[1] * (column - 0.5) + Coefficients[2] * (row + 0.5);
            double y = Coefficients[3] + Coefficients[4] * (column - 0.5) + Coefficients[5] * (row + 0.5);
            return new Coordinate(x, y);
        }

        /// <summary>
        /// Given the row and column, this returns the geographic position of the bottom right corner of the cell
        /// </summary>
        /// <param name="row">The integer row index from 0 to numRows - 1</param>
        /// <param name="column">The integer column index from 0 to numColumns - 1</param>
        /// <returns>The geographic position of the bottom right corner of the specified cell</returns>
        public Coordinate CellBottomRight_ToProj(int row, int column)
        {
            double x = Coefficients[0] + Coefficients[1] * (column + 0.5) + Coefficients[2] * (row + 0.5);
            double y = Coefficients[3] + Coefficients[4] * (column + 0.5) + Coefficients[5] * (row + 0.5);
            return new Coordinate(x, y);
        }

        /// <summary>
        ///  Given the row and column, this returns new affine coefficients transformed to that cell
        /// </summary>
        /// <param name="startRow">The row index from 0 to numRows - 1</param>
        /// <param name="startColumn">The column index from 0 to numColumns - 1</param>
        /// <returns>Transformed affine coefficients</returns>
        public double[] TransfromToCorner(double startColumn, double startRow)
        {
            // X = [0] + [1] * column + [2] * row;
            // Y = [3] + [4] * column + [5] * row;
            var ba = new double[6];
            ba[0] = Coefficients[0] + Coefficients[1] * startColumn + Coefficients[2] * startRow;
            ba[1] = Coefficients[1];
            ba[2] = Coefficients[2];
            ba[3] = Coefficients[3] + Coefficients[4] * startColumn + Coefficients[5] * startRow;
            ba[4] = Coefficients[4];
            ba[5] = Coefficients[5];
            return ba;
        }

        /// <summary>
        /// Returns the row and column index.
        /// </summary>
        /// <param name="location">Gets or sets the ICoordinate</param>
        /// <returns>An RcIndex that shows the best row or column index for the specified coordinate.</returns>
        public RcIndex ProjToCell(Coordinate location)
        {
            double[] c = Coefficients;
            double rw, cl;
            if (c[2] == 0 && c[4] == 0)
            {
                // If we don't have skew terms, then the x and y terms are independent.
                // also both of the other tests will have divide by zero errors and fail.
                cl = (location.X - c[0]) / c[1];
                rw = (location.Y - c[3]) / c[5];
            }
            else if (c[2] != 0)
            {
                // this equation will have divide by zero if c[2] is zero, but works if c[4] is zero
                double div = (c[5] * c[1]) / c[2] - c[4];
                cl = (c[3] + (c[5] * location.X) / c[2] - (c[5] * c[0]) / c[2] - location.Y) / div;
                rw = (location.X - c[0] - c[1] * cl) / c[2];
            }
            else
            {
                double div = (c[4] * c[2] / c[1] - c[5]);
                rw = (c[3] + c[4] * location.X / c[1] - c[4] * c[0] / c[1] - location.Y) / div;
                cl = (location.X - c[2] * rw - c[0]) / c[1];
            }
            var iRow = (int)Math.Round(rw);
            var iCol = (int)Math.Round(cl);
            return new RcIndex(iRow, iCol);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the array of coefficients for this transform
        /// </summary>
        public double[] Coefficients { get; set; }

        #endregion
    }
}