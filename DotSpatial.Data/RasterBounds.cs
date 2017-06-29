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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/1/2008 11:23:05 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;

namespace DotSpatial.Data
{
    /// <summary>
    /// RasterBounds
    /// </summary>
    public class RasterBounds : IRasterBounds
    {
        #region Private Variables

        private double[] _affine;
        private readonly int _numColumns;
        private readonly int _numRows;
        private string _worldFile;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new RasterBounds
        /// </summary>
        public RasterBounds()
        {
            _affine = new double[6];
        }

        /// <summary>
        /// Attempts to read the very simple 6 number world file associated with an image
        /// </summary>
        /// <param name="numRows">The number of rows in this raster</param>
        /// <param name="numColumns">The number of columns in this raster</param>
        /// <param name="worldFileName">A world file to attempt to read</param>
        public RasterBounds(int numRows, int numColumns, string worldFileName)
        {
            _numRows = numRows;
            _numColumns = numColumns;
            _affine = new double[6];
            this.OpenWorldFile(worldFileName);
        }

        /// <summary>
        /// Creates a new instance of the RasterBounds class
        /// </summary>
        /// <param name="numRows">The number of rows for this raster</param>
        /// <param name="numColumns">The number of columns for this raster</param>
        /// <param name="affineCoefficients">The affine coefficients describing the location of this raster.</param>
        public RasterBounds(int numRows, int numColumns, double[] affineCoefficients)
        {
            _affine = affineCoefficients;
            _numRows = numRows;
            _numColumns = numColumns;
        }

        /// <summary>
        /// Creates a new raster bounds that is georeferenced to the specified envelope.
        /// </summary>
        /// <param name="numRows">The number of rows</param>
        /// <param name="numColumns">The number of columns</param>
        /// <param name="bounds">The bounding envelope</param>
        public RasterBounds(int numRows, int numColumns, Extent bounds)
        {
            _affine = new double[6];
            _numRows = numRows;
            _numColumns = numColumns;
            Extent = bounds;
        }

        #endregion

        #region Methods

        IRasterBounds IRasterBounds.Copy()
        {
            return Copy();
        }

        /// <summary>
        /// Attempts to load the data from the file listed in WorldFile
        /// </summary>
        public virtual void Open(string fileName)
        {
            this.OpenWorldFile(fileName);
        }

        /// <summary>
        /// Attempts to save the data to the file listed in WorldFile
        /// </summary>
        public virtual void Save()
        {
            this.SaveWorldFile();
        }

        /// <summary>
        /// Returns a duplicate of this object as an object.
        /// </summary>
        /// <returns>A duplicate of this object as an object.</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// Creates a duplicate of this RasterBounds class.
        /// </summary>
        /// <returns>A RasterBounds that has the same properties but does not point to the same internal array.</returns>
        public RasterBounds Copy()
        {
            var result = (RasterBounds)MemberwiseClone();
            result.AffineCoefficients = new double[6];
            for (int i = 0; i < 6; i++)
            {
                result.AffineCoefficients[i] = _affine[i];
            }
            return result;
        }

        #endregion

        #region IRasterBounds Members

        /// <summary>
        /// Gets or sets the double affine coefficients that control the world-file
        /// positioning of this image.  X' and Y' are real world coords.
        /// X' = [0] + [1] * Column + [2] * Row
        /// Y' = [3] + [4] * Column + [5] * Row
        /// </summary>
        [Category("GeoReference"), Description("X' = [0] + [1] * Column + [2] * Row, Y' = [3] + [4] * Column + [5] * Row")]
        public virtual double[] AffineCoefficients
        {
            get { return _affine; }
            set
            {
                _affine = value;
            }
        }

        /// <summary>
        /// Gets or sets the desired width per cell.  This will keep the skew the same, but
        /// will adjust both the column based and row based width coefficients in order
        /// to match the specified cell width.  This can be thought of as the width
        /// of a bounding box that contains an entire grid cell, no matter if it is skewed.
        /// </summary>
        public double CellWidth
        {
            get
            {
                double[] affine = AffineCoefficients;
                // whatever sign the coefficients are, they only increase the cell width
                return Math.Abs(affine[1]) + Math.Abs(affine[2]);
            }
            set
            {
                double[] affine = AffineCoefficients;
                double columnFactor = affine[1] / CellWidth;
                double rowFactor = affine[2] / CellWidth;
                affine[1] = Math.Sign(affine[1]) * value * columnFactor;
                affine[2] = Math.Sign(affine[2]) * value * rowFactor;
                AffineCoefficients = affine; // use the setter for overriding classes
            }
        }

        /// <summary>
        /// Gets or sets the desired height per cell.  This will keep the skew the same, but
        /// will adjust both the column based and row based height coefficients in order
        /// to match the specified cell height.  This can be thought of as the height
        /// of a bounding box that contains an entire grid cell, no matter if it is skewed.
        /// </summary>
        public double CellHeight
        {
            get
            {
                double[] affine = AffineCoefficients;
                // whatever sign the coefficients are, they only increase the cell hight
                return Math.Abs(affine[4]) + Math.Abs(affine[5]);
            }
            set
            {
                double[] affine = AffineCoefficients;
                double columnFactor = affine[4] / CellWidth;
                double rowFactor = affine[5] / CellWidth;
                affine[4] = Math.Sign(affine[4]) * value * columnFactor;
                affine[5] = Math.Sign(affine[5]) * value * rowFactor;
                AffineCoefficients = affine; // use the setter for overriding classes
            }
        }

        /// <summary>
        /// Gets or sets the rectangular bounding box for this raster.
        /// </summary>
        public Extent Extent
        {
            get
            {
                double[] affine = AffineCoefficients;
                if (affine[1] == 0 || affine[5] == 0) return null;
                return new Extent(this.Left(), this.Bottom(), this.Right(), this.Top());
            }
            set
            {
                // Preserve the skew, but translate and scale to fit the envelope.
                if (value != null)
                {
                    X = value.X;
                    Y = value.Y;
                    Width = value.Width;
                    Height = value.Height;
                }
            }
        }

        /// <summary>
        /// Gets or sets the height of the entire bounds.  This is derived by considering both the
        /// column and row based contributions to the overall height.  Changing this will keep
        /// the skew ratio the same, but adjust both portions so that the overall height
        /// will match the specified height.
        /// </summary>
        public double Height
        {
            get { return Math.Abs(_affine[4]) * NumColumns + Math.Abs(_affine[5]) * NumRows; }
            set
            {
                if (Height == 0 && _numRows > 0)
                {
                    _affine[5] = -(value / _numRows);
                    _affine[4] = 0;
                    return;
                }
                double columnFactor = NumColumns * Math.Abs(_affine[4]) / Height;
                double rowFactor = NumRows * Math.Abs(_affine[5]) / Height;
                double newColumnHeight = value * columnFactor;
                double newRowHeight = value * rowFactor;
                _affine[4] = Math.Sign(_affine[4]) * newColumnHeight / NumColumns;
                _affine[5] = Math.Sign(_affine[5]) * newRowHeight / NumRows;
            }
        }

        /// <summary>
        /// Gets the number of rows in the raster.
        /// </summary>
        [Category("General"), Description("Gets the number of rows in the underlying raster.")]
        public virtual int NumRows
        {
            get { return _numRows; }
        }

        /// <summary>
        /// Gets the number of columns in the raster.
        /// </summary>
        [Category("General"), Description("Gets the number of columns in the raster.")]
        public virtual int NumColumns
        {
            get { return _numColumns; }
        }

        /// <summary>
        /// Gets or sets the geographic width of this raster.  This will include the skew term
        /// in the width estimate, so it will adjust both the width and the skew coefficient,
        /// but preserve the ratio of skew to cell width.
        /// </summary>
        public double Width
        {
            get
            {
                return NumColumns * Math.Abs(_affine[1]) + NumRows * Math.Abs(_affine[2]);
            }
            set
            {
                if (Width == 0 && _numColumns > 0)
                {
                    _affine[1] = value / _numColumns;
                    _affine[2] = 0;
                    return;
                }
                double columnFactor = NumColumns * Math.Abs(_affine[1]) / Width;
                double rowFactor = NumRows * Math.Abs(_affine[2]) / Width;
                double newColumnWidth = value * columnFactor;
                double newRowWidth = value * rowFactor;
                _affine[1] = Math.Sign(_affine[1]) * newColumnWidth / NumColumns;
                _affine[2] = Math.Sign(_affine[2]) * newRowWidth / NumRows;
            }
        }

        /// <summary>
        /// Gets or sets the fileName of the wordfile that describes the geographic coordinates of this raster.
        /// </summary>
        [Category("GeoReference"), Description("Returns the Geographic width of the envelope that completely contains this raster.")]
        public string WorldFile
        {
            get { return _worldFile; }
            set { _worldFile = value; }
        }

        /// <summary>
        /// Gets or sets the horizontal placement of the upper left corner of this bounds.  Because
        /// of the skew, this upper left position may not actually be the same as the upper left
        /// corner of the image itself (_affine[0]).  Instead, this is the top left corner of
        /// the rectangular extent for this raster.
        /// </summary>
        public double X
        {
            get
            {
                double xMin = double.MaxValue;
                double[] affine = AffineCoefficients; // in case this is an overridden property
                double nr = NumRows;
                double nc = NumColumns;

                // Because these coefficients can be negative, we can't make assumptions about what corner is furthest left.
                if (affine[0] < xMin) xMin = affine[0]; // TopLeft;
                if (affine[0] + nc * affine[1] < xMin) xMin = affine[0] + nc * affine[1]; // TopRight;
                if (affine[0] + nr * affine[2] < xMin) xMin = affine[0] + nr * affine[2]; // BottomLeft;
                if (affine[0] + nc * affine[1] + nr * affine[2] < xMin) xMin = affine[0] + nc * affine[1] + nr * affine[2]; // BottomRight

                // the coordinate thus far is the center of the cell.  The actual left is half a cell further left.
                return xMin - Math.Abs(affine[1]) / 2 - Math.Abs(affine[2]) / 2;
            }
            set
            {
                double dx = value - X;
                _affine[0] = _affine[0] + dx; // resetting affine[0] will shift everything else
            }
        }

        /// <summary>
        /// Gets or sets the vertical placement of the upper left corner of this bounds, which is the
        /// same as the top.  The top left corner of the actual image may not be in this position
        /// because of skew, but this represents the maximum Y value of the rectangular extents
        /// that contains the image.
        /// </summary>
        public double Y
        {
            get
            {
                double yMax = double.MinValue;
                double[] affine = AffineCoefficients; // in case this is an overridden property
                double nr = NumRows;
                double nc = NumColumns;

                // Because these coefficients can be negative, we can't make assumptions about what corner is furthest left.
                if (affine[3] > yMax) yMax = affine[3]; // TopLeft;
                if (affine[3] + nc * affine[4] > yMax) yMax = affine[3] + nc * affine[4]; // TopRight;
                if (affine[3] + nr * affine[5] > yMax) yMax = affine[3] + nr * affine[5]; // BottomLeft;
                if (affine[3] + nc * affine[4] + nr * affine[5] > yMax) yMax = affine[3] + nc * affine[4] + nr * affine[5]; // BottomRight

                // the value thus far is at the center of the cell.  Return a value half a cell further
                return yMax + Math.Abs(affine[4]) / 2 + Math.Abs(affine[5]) / 2;
            }
            set
            {
                double dy = value - Y;
                _affine[3] += dy; // resets the dY
            }
        }

        #endregion
    }
}