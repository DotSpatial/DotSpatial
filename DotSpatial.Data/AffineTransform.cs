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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/25/2008 9:34:48 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// |   Ted Dunsford       |  6/30/2010  |  Moved to DotSpatial
// ********************************************************************************************************

using System;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// AffineTransform
    /// </summary>
    public class AffineTransform
    {
        #region Private Variables

        private double[] _coefficients;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of AffineTransform
        /// </summary>
        public AffineTransform(double[] inCoefficients)
        {
            _coefficients = inCoefficients;
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
            double x = _coefficients[0] + _coefficients[1] * column + _coefficients[2] * row;
            double y = _coefficients[3] + _coefficients[4] * column + _coefficients[5] * row;
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
            double x = _coefficients[0] + _coefficients[1] * (column - 0.5) + _coefficients[2] * (row - 0.5);
            double y = _coefficients[3] + _coefficients[4] * (column - 0.5) + _coefficients[5] * (row - 0.5);
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
            double x = _coefficients[0] + _coefficients[1] * (column + 0.5) + _coefficients[2] * (row - 0.5);
            double y = _coefficients[3] + _coefficients[4] * (column + 0.5) + _coefficients[5] * (row - 0.5);
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
            double x = _coefficients[0] + _coefficients[1] * (column - 0.5) + _coefficients[2] * (row + 0.5);
            double y = _coefficients[3] + _coefficients[4] * (column - 0.5) + _coefficients[5] * (row + 0.5);
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
            double x = _coefficients[0] + _coefficients[1] * (column + 0.5) + _coefficients[2] * (row + 0.5);
            double y = _coefficients[3] + _coefficients[4] * (column + 0.5) + _coefficients[5] * (row + 0.5);
            return new Coordinate(x, y);
        }

        /// <summary>
        ///  Given the row and column, this returns new affine coefficients transformed to that cell
        /// </summary>
        /// <param name="startRow">The integer row index from 0 to numRows - 1</param>
        /// <param name="startColumn">The integer column index from 0 to numColumns - 1</param>
        /// <returns>Transoformed affine coefficients</returns>
        public double[] TransfromToCorner(int startColumn, int startRow)
        {
            // X = [0] + [1] * column + [2] * row;
            // Y = [3] + [4] * column + [5] * row;
            var ba = new double[6];
            ba[0] = _coefficients[0] + _coefficients[1] * startColumn + _coefficients[2] * startRow;
            ba[1] = _coefficients[1];
            ba[2] = _coefficients[2];
            ba[3] = _coefficients[3] + _coefficients[4] * startColumn + _coefficients[5] * startRow;
            ba[4] = _coefficients[4];
            ba[5] = _coefficients[5];
            return ba;
        }

        /// <summary>
        /// Returns the row and column index.
        /// </summary>
        /// <param name="location">Gets or sets the ICoordinate</param>
        /// <returns>An RcIndex that shows the best row or column index for the specified coordinate.</returns>
        public RcIndex ProjToCell(Coordinate location)
        {
            double[] c = _coefficients;
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
            var iRow = (int)Math.Floor(rw);
            var iCol = (int)Math.Floor(cl);
            return new RcIndex(iRow, iCol);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the array of coefficients for this transform
        /// </summary>
        public double[] Coefficients
        {
            get { return _coefficients; }
            set { _coefficients = value; }
        }

        #endregion
    }
}