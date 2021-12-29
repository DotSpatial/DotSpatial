// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using NetTopologySuite.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// A new model, now that we support 3.5 framework and extension methods that are essentially
    /// derived characteristics away from the IRaster interface, essentially reducing it
    /// to the simplest interface possible for future implementers, while extending the most
    /// easy-to-find functionality to the users.
    /// </summary>
    public static class RasterExt
    {
        #region Methods

        /// <summary>
        /// Determines if the shape is partially inside grid extents.
        /// </summary>
        /// <param name="raster">Raster that should be checked.</param>
        /// <param name="shape">Shape that should be checked.</param>
        /// <returns>false, if the shape is completely outside grid extents
        /// true, if it's at least partially inside. </returns>
        public static bool ContainsFeature(this IRaster raster, IFeature shape)
        {
            IRasterBounds bounds = raster.Bounds;
            Extent shapeExtent = shape.Geometry.EnvelopeInternal.ToExtent();
            return !(shapeExtent.MinX > bounds.Extent.MaxX) && !(shapeExtent.MinY > bounds.Extent.MaxY) && !(shapeExtent.MaxX < bounds.Extent.MinX) && !(shapeExtent.MaxY < bounds.Extent.MinY);
        }

        /// <summary>
        /// Gets a boolean that is true if the Window extents contain are all the information for the raster.
        /// In otherwords, StartRow = StartColumn = 0, EndRow = NumRowsInFile - 1, and EndColumn = NumColumnsInFile - 1.
        /// </summary>
        /// <param name="raster">Raster that should be checked.</param>
        /// <returns>True, if the whole raster is inside the window.</returns>
        public static bool IsFullyWindowed(this IRaster raster)
        {
            if (raster.StartRow != 0) return false;
            if (raster.StartColumn != 0) return false;

            if (raster.EndRow != raster.NumRowsInFile - 1) return false;
            if (raster.EndColumn != raster.NumColumnsInFile - 1) return false;
            return true;
        }

        #region GeoReference

        /// <summary>
        /// This doesn't change the data, but instead performs a translation where the upper left coordinate
        /// is specified in world coordinates.
        /// </summary>
        /// <param name="raster">Moves this raster so that the upper left coordinate will match the specified position. The skew and cellsize will remain unaltered.</param>
        /// <param name="position">The location to move the upper left corner of the raster to in world coordinates.</param>
        public static void MoveTo(this IRaster raster, Coordinate position)
        {
            double[] vals = raster.Bounds.AffineCoefficients;
            vals[0] = position.X;
            vals[3] = position.Y;
        }

        /// <summary>
        /// Rotates the geospatial reference points for this image by rotating the affine coordinates.
        /// The center for this rotation will be the center of the image.
        /// </summary>
        /// <param name="raster">The raster to rotate.</param>
        /// <param name="degrees">The angle in degrees to rotate the image counter clockwise.</param>
        public static void Rotate(this IRaster raster, float degrees)
        {
            Matrix m = raster.Bounds.GetAffineMatrix();
            m.Rotate(degrees);
            raster.Bounds.SetAffineMatrix(m);
        }

        /// <summary>
        /// Rotates the geospatial reference points for this image by rotating the affine coordinates.
        /// The center for this rotation will be the center of the image.
        /// </summary>
        /// <param name="raster">The raster to rotate about the specified coordinate.</param>
        /// <param name="degrees">The angle in degrees to rotate the image counterclockwise.</param>
        /// <param name="center">The point that marks the center of the desired rotation in geographic coordiantes.</param>
        public static void RotateAt(this IRaster raster, float degrees, Coordinate center)
        {
            Matrix m = raster.Bounds.GetAffineMatrix();
            m.RotateAt(degrees, new PointF(Convert.ToSingle(center.X), Convert.ToSingle(center.Y)));
            raster.Bounds.SetAffineMatrix(m);
        }

        /// <summary>
        /// This method uses a matrix transform to adjust the scale. The precision of using
        /// a Drawing2D transform is float precision, so some accuracy may be lost.
        /// </summary>
        /// <param name="raster">The raster to apply the scale transform to.</param>
        /// <param name="scaleX">The multiplier to adjust the geographic extents of the raster in the X direction.</param>
        /// <param name="scaleY">The multiplier to adjust the geographic extents of the raster in the Y direction.</param>
        public static void Scale(this IRaster raster, float scaleX, float scaleY)
        {
            Matrix m = raster.Bounds.GetAffineMatrix();
            m.Scale(scaleX, scaleY);
            raster.Bounds.SetAffineMatrix(m);
        }

        /// <summary>
        /// This method uses a matrix transform to adjust the shear. The precision of using
        /// a Drawing2D transform is float precision, so some accuracy may be lost.
        /// </summary>
        /// <param name="raster">The raster to apply the transform to.</param>
        /// <param name="shearX">The floating point horizontal shear factor.</param>
        /// <param name="shearY">The floating ponit vertical shear factor.</param>
        public static void Shear(this IRaster raster, float shearX, float shearY)
        {
            Matrix m = raster.Bounds.GetAffineMatrix();
            m.Shear(shearX, shearY);
            raster.Bounds.SetAffineMatrix(m);
        }

        /// <summary>
        /// Applies a translation transform to the georeferenced coordinates on this raster.
        /// </summary>
        /// <param name="raster">The raster to apply the translation to.</param>
        /// <param name="shift">An ICoordinate with shear values.</param>
        public static void Translate(this IRaster raster, Coordinate shift)
        {
            double[] affine = raster.Bounds.AffineCoefficients;
            affine[0] += shift.X;
            affine[3] += shift.Y;
        }

        #endregion

        #region Nearest Values

        /// <summary>
        /// Retrieves the data from the cell that is closest to the specified coordinates. This will
        /// return a No-Data value if the specified coordintes are outside of the grid.
        /// </summary>
        /// <param name="raster">The raster to get the value from.</param>
        /// <param name="location">A valid implementation of Icoordinate specifying the geographic location.</param>
        /// <returns>The value of type T of the cell that has a center closest to the specified coordinates.</returns>
        public static double GetNearestValue(this IRaster raster, Coordinate location)
        {
            RcIndex position = raster.ProjToCell(location.X, location.Y);
            if (position.Row < 0 || position.Row >= raster.NumRows) return raster.NoDataValue;
            if (position.Column < 0 || position.Column >= raster.NumColumns) return raster.NoDataValue;
            return raster.Value[position.Row, position.Column];
        }

        /// <summary>
        /// Retrieves the data from the cell that is closest to the specified coordinates. This will
        /// return a No-Data value if the specified coordintes are outside of the grid.
        /// </summary>
        /// <param name="raster">The raster to get the value from.</param>
        /// <param name="x">The longitude or horizontal coordinate.</param>
        /// <param name="y">The latitude or vertical coordinate.</param>
        /// <returns>The double value of the cell that has a center closest to the specified coordinates.</returns>
        public static double GetNearestValue(this IRaster raster, double x, double y)
        {
            RcIndex position = raster.ProjToCell(x, y);
            if (position.Row < 0 || position.Row >= raster.NumRows) return raster.NoDataValue;
            if (position.Column < 0 || position.Column >= raster.NumColumns) return raster.NoDataValue;
            return raster.Value[position.Row, position.Column];
        }

        /// <summary>
        /// Retrieves the location from the cell that is closest to the specified coordinates. This will
        /// do nothing if the specified coordinates are outside of the raster.
        /// </summary>
        /// <param name="raster">The IRaster to set the value for.</param>
        /// <param name="x">The longitude or horizontal coordinate.</param>
        /// <param name="y">The latitude or vertical coordinate.</param>
        /// <param name="value">The value to assign to the nearest cell to the specified location.</param>
        public static void SetNearestValue(this IRaster raster, double x, double y, double value)
        {
            RcIndex position = raster.ProjToCell(x, y);
            if (position.Row < 0 || position.Row >= raster.NumRows) return;
            if (position.Column < 0 || position.Column >= raster.NumColumns) return;
            raster.Value[position.Row, position.Column] = value;
        }

        /// <summary>
        /// Retrieves the location from the cell that is closest to the specified coordinates. This will
        /// do nothing if the specified coordinates are outside of the raster.
        /// </summary>
        /// <param name="raster">The IRaster to set the value for.</param>
        /// <param name="location">An Icoordinate specifying the location.</param>
        /// <param name="value">The value to assign to the nearest cell to the specified location.</param>
        public static void SetNearestValue(this IRaster raster, Coordinate location, double value)
        {
            RcIndex position = raster.ProjToCell(location.X, location.Y);
            if (position.Row < 0 || position.Row >= raster.NumRows) return;
            if (position.Column < 0 || position.Column >= raster.NumColumns) return;
            raster.Value[position.Row, position.Column] = value;
        }

        #endregion

        #region Projection

        /// <summary>
        /// Extends the IRaster interface to return the coordinate of the center of a row column position.
        /// </summary>
        /// <param name="raster">The raster interface to extend.</param>
        /// <param name="position">The zero based integer index of the row and column of the cell to locate.</param>
        /// <returns>The geographic location of the center of the specified cell.</returns>
        public static Coordinate CellToProj(this IRaster raster, RcIndex position)
        {
            return raster?.Bounds?.CellCenterToProj(position.Row, position.Column);
        }

        /// <summary>
        /// Extends the IRaster interface to return the coordinate of the center of a row column position.
        /// </summary>
        /// <param name="raster">The raster interface to extend.</param>
        /// <param name="row">The zero based integer index of the row of the cell to locate.</param>
        /// <param name="col">The zero based integer index of the column of the cell to locate.</param>
        /// <returns>The geographic location of the center of the specified cell.</returns>
        public static Coordinate CellToProj(this IRaster raster, int row, int col)
        {
            return raster?.Bounds?.CellCenterToProj(row, col);
        }

        /// <summary>
        /// Extends the IRaster interface to return the zero based integer row and column indices.
        /// </summary>
        /// <param name="raster">The raster interface to extend.</param>
        /// <param name="location">The geographic coordinate describing the latitude and longitude.</param>
        /// <returns>The RcIndex that describes the zero based integer row and column indices.</returns>
        public static RcIndex ProjToCell(this IRaster raster, Coordinate location)
        {
            if (raster?.Bounds == null) return RcIndex.Empty;
            return raster.Bounds.ProjToCell(location);
        }

        /// <summary>
        /// Extends the IRaster interface to return the zero based integer row and column indices.
        /// </summary>
        /// <param name="raster">The raster interface to extend.</param>
        /// <param name="x">A double precision floating point describing the longitude.</param>
        /// <param name="y">A double precision floating point describing the latitude.</param>
        /// <returns>The RcIndex that describes the zero based integer row and column indices.</returns>
        public static RcIndex ProjToCell(this IRaster raster, double x, double y)
        {
            if (raster?.Bounds == null) return RcIndex.Empty;
            return raster.Bounds.ProjToCell(new Coordinate(x, y));
        }

        #endregion

        #endregion
    }
}