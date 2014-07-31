// ********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/12/2009 7:11:21 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Topology
{
    /// <summary>
    /// My concept for the Envelope is that there are several methods that are derived calculations.  These are
    /// so straight forward that it is unlikey that "new" implementations of those derived values will be needed
    /// or wanted.  However, we want lots of different kinds of things to be able to "become" an envelope
    /// with a minimum of fuss.
    /// </summary>
    public static class EnvelopeExt
    {
        /// <summary>
        /// Calculates the area of this envelope.  Because the word Area,
        /// like Volume, is dimension specific, this method only looks
        /// at the X and Y ordinates, and requires at least 2 ordinates.
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        /// <returns>The 2D area as a double value.</returns>
        public static double Area(this IEnvelope self)
        {
            if (self == null) return -1;
            return self.Width * self.Height;
        }

        /// <summary>
        /// Gets a linear ring built clockwise around the border, starting from the TopLeft.
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        public static ILinearRing Border(this IEnvelope self)
        {
            if (self == null) return null;
            return self.ToLinearRing();
        }

        /// <summary>
        /// Gets an array of 4 line segments working clockwise from the top segment.
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        public static ILineSegment[] BorderSegments(this IEnvelope self)
        {
            ILineSegment[] result = new ILineSegment[4];
            result[0] = self.TopBorder();
            result[1] = self.RightBorder();
            result[2] = self.BottomBorder();
            result[3] = self.LeftBorder();
            return result;
        }

        /// <summary>
        /// Gets the minY, which is Y - Height.
        /// </summary>
        /// <param name="self">The <c>IEnvelope</c> that this calculation is for.</param>
        /// <returns></returns>
        public static double Bottom(this IEnvelope self)
        {
            return self.Y - self.Height;
        }

        /// <summary>
        /// Gets an ILineSegment from the top right corner to the bottom right corner
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        public static ILineSegment BottomBorder(this IEnvelope self)
        {
            return new LineSegment(self.BottomRight(), self.BottomLeft());
        }

        /// <summary>
        /// Gets the bottom left corner of this rectangle as a 2D coordinate.
        /// </summary>
        /// <param name="self">The <c>IEnvelope</c> that this calculation is for.</param>
        /// <returns>An <c>ICoordinate</c> at the lower left corner of this rectangle</returns>
        public static Coordinate BottomLeft(this IEnvelope self)
        {
            return new Coordinate(Left(self), Bottom(self));
        }

        /// <summary>
        /// Gets the bottom right corner of this rectangle as a 2D coordinate
        /// </summary>
        /// <param name="self">The <c>IEnvelope</c> that this calculation is for.</param>
        /// <returns>An <c>ICoordinate</c> at the lower right corner of this rectangle</returns>
        public static Coordinate BottomRight(this IEnvelope self)
        {
            return new Coordinate(Right(self), Bottom(self));
        }

        /// <summary>
        /// Gets the coordinate defining the center of this envelope
        /// in all of the dimensions of this envelope.
        /// </summary>
        /// <param name="self">The <c>IEnvelope</c> to find the center for</param>
        /// <returns>An ICoordinate</returns>
        public static Coordinate Center(this IEnvelope self)
        {
            if (self == null) return null;
            if (self.IsNull) return null;
            Coordinate result = new Coordinate();
            for (int i = 0; i < self.NumOrdinates; i++)
            {
                result[i] = (self.Minimum[i] + self.Maximum[i]) / 2;
            }
            return result;
        }

        /// <summary>
        /// Gets the left value for this rectangle.  This should be the
        /// X coordinate, but is added for clarity.
        /// </summary>
        /// <param name="self">The <c>IEnvelope</c> that this calculation is for.</param>
        /// <returns></returns>
        public static double Left(this IEnvelope self)
        {
            return self.X;
        }

        /// <summary>
        /// Gets an ILineSegment from the bottom left to the top left corners
        /// </summary>
        /// <param name="self">The <c>IEnvelope</c> that is being extended by this method</param>
        public static ILineSegment LeftBorder(this IEnvelope self)
        {
            return new LineSegment(BottomLeft(self), TopLeft(self));
        }

        /// <summary>
        /// Gets the right value, which is X + Width.
        /// </summary>
        /// <param name="self">The <c>IEnvelope</c> that this calculation is for.</param>
        /// <returns></returns>
        public static double Right(this IEnvelope self)
        {
            return self.X + self.Width;
        }

        /// <summary>
        /// Gets an ILineSegment from the bottom right corner to the bottom left corner
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        public static ILineSegment RightBorder(this IEnvelope self)
        {
            if (self == null) return null;
            return new LineSegment(self.TopRight(), self.BottomRight());
        }

        ///// <summary>
        ///// Gets a floating point version of the 2D Rectangular extensts.
        ///// </summary>
        ///// <param name="self">The IEnvelope to use with this method</param>
        //public static System.Drawing.RectangleF ToRectangleF(this IEnvelope self)
        //{
        //    if (self == null || self.IsNull)
        //    {
        //        return System.Drawing.RectangleF.Empty;
        //    }

        //    System.Drawing.RectangleF result = new System.Drawing.RectangleF();
        //    result.X = Convert.ToSingle(self.Minimum.X);
        //    result.Y = Convert.ToSingle(self.Minimum.Y);
        //    result.Width = Convert.ToSingle(self.Maximum.X - self.Minimum.X);
        //    result.Height = Convert.ToSingle(self.Maximum.Y - self.Minimum.Y);
        //    return result;

        //}

        /// <summary>
        /// Converts this envelope into a linear ring.
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        /// <returns>A Linear ring describing the border of this envelope.</returns>
        public static ILinearRing ToLinearRing(this IEnvelope self)
        {
            // Holes are counter clockwise, so this should create
            // a clockwise linear ring.
            CoordinateListSequence coords = new CoordinateListSequence();
            coords.Add(self.TopLeft());
            coords.Add(self.TopRight());
            coords.Add(self.BottomRight());
            coords.Add(self.BottomLeft());
            coords.Add(self.TopLeft()); // close the polygon
            return new LinearRing(coords);
        }

        /// <summary>
        /// Gets an ILineSegment from the top left to the top right corners
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        public static ILineSegment TopBorder(this IEnvelope self)
        {
            return new LineSegment(TopLeft(self), TopRight(self));
        }

        /// <summary>
        /// Calculates the TopLeft 2D coordinate from this envelope.
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        /// <returns></returns>
        public static Coordinate TopLeft(this IEnvelope self)
        {
            return new Coordinate(self.Minimum.X, self.Maximum.Y);
        }

        /// <summary>
        /// Technically an Evelope object is not actually a geometry.
        /// This creates a polygon from the extents.
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        /// <returns>A Polygon, which technically qualifies as an IGeometry</returns>
        public static IPolygon ToPolygon(this IEnvelope self)
        {
            return new Polygon(ToLinearRing(self));
        }

        /// <summary>
        /// Calcualte the TopRight 2D Coordinate from this envelope
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        /// <returns>An ICoordinate in the position of maximum X, Maximum Y</returns>
        public static Coordinate TopRight(this IEnvelope self)
        {
            return new Coordinate(self.Maximum.X, self.Maximum.Y);
        }

        /// <summary>
        /// Changes the envelope extent by the specified amount relative
        /// to it's current extent in that dimension (preserving the aspect ratio).
        /// So Zoom(10) on a 100 unit wide envelope creates a 110 unit wide envlope,
        /// while Zoom(-10) on a 100 unit wide envelope creates a 90 unit wide envelope.
        /// Zoom(-100) on an envelope makes it 100% smaller, or effectively a point.
        /// Tragically, a point cannot be "zoomed" back in, so a check should be used
        /// to ensure that the envelope is not currently a point before attempting
        /// to zoom in.
        /// </summary>
        /// <param name="self">The IEnvelope that this zoom method modifies</param>
        /// <param name="percent">
        /// Positive 50 makes the envelope 50% larger
        /// Negative 50 makes the envelope 50% smaller
        /// </param>
        /// <example>
        ///  perCent = -50 compact the envelope a 50% (make it smaller).
        ///  perCent = 200 enlarge envelope by 2.
        /// </example>
        public static void Zoom(this IEnvelope self, double percent)
        {
            if (self == null) return;
            if (self.IsNull) return;
            double ratio = percent / 100;
            Coordinate size = new Coordinate();
            for (int i = 0; i < self.NumOrdinates; i++)
            {
                double oldSize = self.Maximum[i] - self.Minimum[i];
                size[i] = oldSize + ratio * oldSize;
            }
            SetCenter(self, self.Center(), size);
        }

        #region Union

        /// <summary>
        /// Returns a new envelope that is a copy of this envelope, but modified
        /// to include the specified coordinate.
        /// </summary>
        public static IEnvelope Union(this IEnvelope self, Coordinate coord)
        {
            IEnvelope env = self.Copy();
            env.ExpandToInclude(coord);
            return env;
        }

        /// <summary>
        /// Calculates the union of the current box and the given box.
        /// </summary>
        public static IEnvelope Union(this IEnvelope self, IEnvelope box)
        {
            if (box == null) return self.Copy();
            if (box.IsNull) return self.Copy();
            if (self == null) return box.Copy();
            if (self.IsNull) return box.Copy();
            IEnvelope result = self.Copy();
            result.ExpandToInclude(box);
            return result;
        }

        #endregion

        #region SetCenter

        /// <summary>
        /// Moves the envelope to the indicated coordinate.
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        /// <param name="center">The new center coordinate.</param>
        public static void SetCenter(this IEnvelope self, Coordinate center)
        {
            SetCenter(self, center, null);
        }

        /// <summary>
        /// Resizes the envelope to the indicated point.
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        /// <param name="width">The new width.</param>
        /// <param name="height">The new height.</param>
        public static void SetCenter(this IEnvelope self, double width, double height)
        {
            SetCenter(self, null, new Coordinate(width, height));
        }

        /// <summary>
        /// Moves and resizes the current envelope.
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        /// <param name="center">The new centre coordinate.</param>
        /// <param name="width">The new width.</param>
        /// <param name="height">The new height.</param>
        public static void SetCenter(this IEnvelope self, Coordinate center, double width, double height)
        {
            Coordinate size = new Coordinate(width, height);
            SetCenter(self, center, size);
        }

        /// <summary>
        /// This handles setting the center and scale in N dimensions.  The size is equivalent to the
        /// span in each dimension, while the center is the central position in each dimension.  The
        /// envelope will have values in each dimension where either the existing envelope or both the
        /// specified center and size values have been specified.  If only the existing envelope
        /// and a size is specified for dimension 3, for example, the existing center will be used.
        /// </summary>
        /// <param name="self">The envelope to modify</param>
        /// <param name="center">The center position.  This can also be null.</param>
        /// <param name="size">The size (or span) in each dimension.  This can also be null.</param>
        public static void SetCenter(this IEnvelope self, Coordinate center, Coordinate size)
        {
            int centerDimensions = 0;
            int sizeDimensions = 0;

            if (self == null) return;
            if (center != null) centerDimensions = center.NumOrdinates;
            if (size != null) sizeDimensions = size.NumOrdinates;
            int numDimensionsCenterAndSize = Math.Min(centerDimensions, sizeDimensions);

            int numDimensions = Math.Max(self.NumOrdinates, numDimensionsCenterAndSize);
            Coordinate min = new Coordinate();
            Coordinate max = new Coordinate();
            for (int i = 0; i < numDimensions; i++)
            {
                if (centerDimensions > i && sizeDimensions > i)
                {
                    // we have completely new information for this dimension
                    if (center != null)
                    {
                        if (size != null)
                        {
                            min[i] = center[i] - size[i] / 2;
                            max[i] = center[i] + size[i] / 2;
                        }
                    }
                }
                if (centerDimensions <= i && sizeDimensions <= i)
                {
                    // we have no new information at all, so just use the existing envelope
                    min[i] = self.Minimum[i];
                    max[i] = self.Maximum[i];
                }
                if (centerDimensions > i && sizeDimensions <= i)
                {
                    // We have a center specified in this dimension, but not a size, so keep the old size
                    double oldSize = self.Maximum[i] - self.Minimum[i];
                    if (center != null)
                    {
                        min[i] = center[i] - oldSize / 2;
                        max[i] = center[i] + oldSize / 2;
                    }
                }
                if (centerDimensions <= i && sizeDimensions > i)
                {
                    // We have a size specified in this dimension, but not any center information, so use the old center.
                    double oldCenter = (self.Minimum[i] + self.Maximum[i]) / 2;
                    if (size != null)
                    {
                        min[i] = oldCenter - size[i];
                        max[i] = oldCenter + size[i];
                    }
                }
            }
            self.Init(min, max);
        }

        #endregion

        #region SetExtents

        /// <summary>
        /// Despite the naming of the extents, this will force the larger of the two x values
        /// to become Xmax etc.
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        /// <param name="minX">An X coordinate</param>
        /// <param name="minY">A Y coordinate</param>
        /// <param name="minZ">A Z coordinate</param>
        /// <param name="maxX">Another X coordinate</param>
        /// <param name="maxY">Another Y coordinate</param>
        /// <param name="maxZ">Another Z coordinate</param>
        public static void SetExtents(this IEnvelope self, double minX, double minY, double minZ, double maxX, double maxY, double maxZ)
        {
            if (self == null) return;
            Coordinate min = new Coordinate(minX, minY, minZ);
            Coordinate max = new Coordinate(maxX, maxY, maxZ);
            self.Init(min, max);
        }

        /// <summary>
        /// The two dimensional overload for consistency with other code.
        /// Despite the names, this will force the smallest X coordinate given
        /// to become maxX.
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        /// <param name="minX">An X coordinate</param>
        /// <param name="minY">A Y coordinate</param>
        /// <param name="maxX">Another X coordinate</param>
        /// <param name="maxY">Another Y coordinate</param>
        public static void SetExtents(this IEnvelope self, double minX, double minY, double maxX, double maxY)
        {
            if (self == null) return;
            Coordinate min = new Coordinate(minX, minY);
            Coordinate max = new Coordinate(maxX, maxY);
            self.Init(min, max);
        }

        /// <summary>
        /// Gets the maxY value, which should be Y.
        /// </summary>
        /// <param name="self">The <c>IEnvelope</c> that this calculation is for.</param>
        /// <returns>The double value representing the Max Y value of this rectangle</returns>
        public static double Top(this IEnvelope self)
        {
            return self.Y;
        }

        #endregion

        #region

        /// <summary>
        /// Translates this envelope by given amounts in the X and Y direction.
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        /// <param name="shiftX">The amount to translate along the X axis.</param>
        /// <param name="shiftY">The amount to translate along the Y axis.</param>
        public static void Translate(this IEnvelope self, double shiftX, double shiftY)
        {
            Translate(self, new Coordinate(shiftX, shiftY));
        }

        /// <summary>
        /// Translates the envelope by given amounts up to the minimum dimension between
        /// the envelope and the shift coordinate.  (e.g., A 2D envelope will only
        /// be shifted in 2 dimensions because it has no Z, while a 2D coordinate
        /// can only shift a cube based on the X and Y positions, leaving the Z
        /// info alone.
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        /// <param name="shift"></param>
        /// <remarks>This does nothing to a "NULL" envelope</remarks>
        public static void Translate(this IEnvelope self, Coordinate shift)
        {
            if (self == null) return;
            if (self.IsNull) return;
            if (shift == null) return;
            Coordinate min = self.Minimum;
            Coordinate max = self.Maximum;
            int numDimensions = Math.Min(self.NumOrdinates, shift.NumOrdinates);
            for (int i = 0; i < numDimensions; i++)
            {
                min[i] += shift[i];
                max[i] += shift[i];
            }
            self.Init(min, max);
        }

        #endregion

        #region Overlaps

        /// <summary>
        /// Overlaps is defined as intersecting, but having some part of each envelope that is also outside
        /// of the other envelope.  Therefore it would amount to saying "Intersects And Not Contains And Not Within"
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool Overlaps(this IEnvelope self, IEnvelope other)
        {
            if (self.Intersects(other) == false) return false;
            if (self.Contains(other)) return false;
            if (other.Contains(self)) return false;
            return true;
        }

        /// <summary>
        /// For a point, or coordinate, this is a degenerate case and
        /// will simply perform an intersection test instead.
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool Overlaps(this IEnvelope self, Coordinate p)
        {
            return self.Intersects(p);
        }

        /// <summary>
        /// For a point, or coordinate, this is a degenerate case and
        /// will simply perform an intersection test instead.
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <returns>Boolean, true if the specified point intersects with this envelope</returns>
        public static bool Overlaps(this IEnvelope self, double x, double y)
        {
            return self.Intersects(new Coordinate(x, y));
        }

        #endregion

        #region Contains

        /// <summary>
        /// Evaluates if the specified coordinate is found inside or on the border.
        /// This will test all the dimensions shared by both the envelope and point.
        /// </summary>
        public static bool Contains(this IEnvelope self, Coordinate p)
        {
            if (self == null) return false;
            if (self.IsNull) return false;
            int numDimensions = Math.Min(self.NumOrdinates, p.NumOrdinates);
            for (int i = 0; i < numDimensions; i++)
            {
                if (p[i] < self.Minimum[i] || p[i] > self.Maximum[i]) return false;
            }
            return true;
        }

        /// <summary>
        /// Returns <c>true</c> if the given point lies in or on the envelope.
        /// </summary>
        /// <param name="self">The IEnvelope that this is for</param>
        /// <param name="x"> the x-coordinate of the point which this <c>Envelope</c> is
        /// being checked for containing.</param>
        /// <param name="y"> the y-coordinate of the point which this <c>Envelope</c> is
        /// being checked for containing.</param>
        /// <returns><c>true</c> if <c>(x, y)</c> lies in the interior or
        /// on the boundary of this <c>Envelope</c>.</returns>
        public static bool Contains(this IEnvelope self, double x, double y)
        {
            return Contains(self, new Coordinate(x, y));
        }

        /// <summary>
        /// Returns true if the other <c>IEnvelope</c> is within this envelope in all dimensions.
        /// If the boundaries touch, this will true.  This will test the number of
        /// dimensions that is the smallest dimensionality.  This will ignore M values.
        /// </summary>
        /// <param name="envelope">The first envelope (this object when extending)</param>
        /// <param name="other"> the <c>Envelope</c> which this <c>Envelope</c> is being checked for containing.</param>
        /// <returns><c>true</c> if <c>other</c> is contained in this <c>Envelope</c>.</returns>
        public static bool Contains(this IEnvelope envelope, IEnvelope other)
        {
            if (envelope == null || other == null) return false;
            if (envelope.IsNull || other.IsNull)
                return false;
            int numOrds = Math.Min(envelope.NumOrdinates, other.NumOrdinates);
            for (int i = 0; i < numOrds; i++)
            {
                if (other.Minimum[i] < envelope.Minimum[i] || other.Maximum[i] > envelope.Maximum[i]) return false;
            }
            return true;
        }

        #endregion

        #region Distance

        /// <summary>
        /// Computes the distance between this and another Envelope.
        /// The distance between overlapping Envelopes is 0.  Otherwise, the
        /// distance is the hyper Euclidean distance between the closest points.
        /// M values are ignored, but every dimension is considered, up to
        /// the minimum of the number of ordinates between the two envelopes.
        /// Use Distance with a dimension specified to force 2D distance calculations.
        /// </summary>
        /// <param name="self">The first envelope (this object when extending)</param>
        /// <param name="other">The other envelope to calculate the distance to</param>
        /// <returns>The distance between this and another <c>Envelope</c>.</returns>
        /// <remarks>Null cases produce a distance of -1</remarks>
        public static double Distance(this IEnvelope self, IEnvelope other)
        {
            // because we are reading NumOrdinates, we need to do a null check first
            if (self == null) return -1;
            if (other == null) return -1;
            return DoDistance(self, other, Math.Min(self.NumOrdinates, other.NumOrdinates));
        }

        /// <summary>
        /// Sometimes only two dimensions are important, and the full dimensionality is not needed.
        /// The Distance 2D performs the same distance check but only in X and Y, regardless
        /// of how many dimensions exist in the envelopes.  Both envelopes should have at least
        /// two ordinates however.  (Not a criteria of the Distance() method)
        /// </summary>
        /// <param name="self">The first envelope (this object when extending)</param>
        /// <param name="other">The other envelope to calculate the distance to</param>
        /// <returns></returns>
        public static double Distance2D(this IEnvelope self, IEnvelope other)
        {
            return DoDistance(self, other, 2);
        }

        /// <summary>
        /// Calculates the distance specifically in 3D, even if the envelope
        /// exists in higher dimensions.  The NumOrdinates of both envelopes
        /// must be at least 3.
        /// </summary>
        /// <param name="self">The first envelope (this object when extending)</param>
        /// <param name="other">The other envelope to calculate the distance to</param>
        /// <returns></returns>
        public static double Distance3D(this IEnvelope self, IEnvelope other)
        {
            return DoDistance(self, other, 3);
        }

        // Actually calculates the distance, given the specified number of dimensions.
        private static double DoDistance(IEnvelope self, IEnvelope other, int numDimensions)
        {
            if (self == null || other == null) return -1;
            if (self.IsNull || other.IsNull) return -1;
            if (self.NumOrdinates < numDimensions && other.NumOrdinates < numDimensions)
            {
                throw new InsufficientDimensionsException("both envelopes");
            }
            if (self.NumOrdinates < numDimensions)
            {
                throw new InsufficientDimensionsException("this envelope"); // assuming used as an extension
            }
            if (self.NumOrdinates < numDimensions)
            {
                throw new InsufficientDimensionsException("the other envelope");
            }
            if (self.Intersects(other)) return 0;

            Coordinate c = new Coordinate();

            for (int i = 0; i < c.NumOrdinates; i++)
            {
                if (self.Maximum[i] < other.Minimum[i])
                    c[i] = other.Minimum.X - self.Maximum.X;
                if (self.Minimum[i] > other.Maximum[i])
                    c[i] = self.Minimum[i] - other.Minimum[i];
            }

            double sumSquares = 0;
            for (int i = 0; i < numDimensions; i++)
            {
                sumSquares += c[i] * c[i];
                if (c[i] == 0) return 0; // an extra check.  If any distance is zero, the distance is zero.
            }

            return Math.Sqrt(sumSquares);
        }

        #endregion

        #region Expand

        /// <summary>
        /// Uses the specified distance to expand the envelope by that amount in all dimensions.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="distance"></param>
        public static void ExpandBy(this IEnvelope self, double distance)
        {
            if (self == null) return;
            int numDimensions = self.NumOrdinates;
            Coordinate min = self.Minimum;
            Coordinate max = self.Maximum;
            for (int i = 0; i < numDimensions; i++)
            {
                min[i] -= distance;
                max[i] += distance;
            }
            self.Init(min, max); // this is important in case negative values are used, and
            // the maximum is now the minimum etc.
        }

        /// <summary>
        /// Uses the dimensions of the specified distances coordinate to
        /// specify the amount to expand the envelopes in each ordinate.
        /// This will apply the method to the minimum dimensions between
        /// the distances coordinate and this envelope.  (eg. A 2D
        /// distances coordinate will not affect Z values in the envelope).
        /// </summary>
        /// <param name="self">The first envelope (this object when extending)</param>
        /// <param name="distances">The distance to expand the envelope.</param>
        public static void ExpandBy(this IEnvelope self, Coordinate distances)
        {
            if (self == null) return;
            int numDimensions = Math.Min(self.NumOrdinates, distances.NumOrdinates);
            Coordinate min = self.Minimum;
            Coordinate max = self.Maximum;
            for (int i = 0; i < numDimensions; i++)
            {
                min[i] -= distances[i];
                max[i] += distances[i];
            }
            self.Init(min, max); // this is important in case negative values are used, and
            // the maximum is now the minimum etc.
        }

        /// <summary>
        /// Expands this envelope by a given distance in all directions.
        /// Both positive and negative distances are supported.
        /// </summary>
        /// <param name="self">The first envelope (this object when extending)</param>
        /// <param name="deltaX">The distance to expand the envelope along the the X axis.</param>
        /// <param name="deltaY">The distance to expand the envelope along the the Y axis.</param>
        public static void ExpandBy(this IEnvelope self, double deltaX, double deltaY)
        {
            if (self == null) return;
            Coordinate c = new Coordinate(deltaX, deltaY);
            ExpandBy(self, c);
        }

        /// <summary>
        /// Enlarges the boundary of the <c>Envelope</c> so that it contains (p).
        /// Does nothing if (p) is already on or within the boundaries.
        /// This executes to the minimum of dimensions between p and this envelope.
        /// </summary>
        /// <param name="self">The first envelope (this object when extending)</param>
        /// <param name="p">The Coordinate.</param>
        public static void ExpandToInclude(this IEnvelope self, Coordinate p)
        {
            if (self == null) return;
            if (self.IsNull)
            {
                self.Init(p, p);
                return;
            }
            int numDimensions = Math.Min(self.NumOrdinates, p.NumOrdinates);
            Coordinate min = self.Minimum;
            Coordinate max = self.Maximum;
            for (int i = 0; i < numDimensions; i++)
            {
                if (p[i] < min[i]) min[i] = p[i];
                if (p[i] > max[i]) max[i] = p[i];
            }
            self.Init(min, max); // this is to ensure we don't break the min/max relationships.
        }

        /// <summary>
        /// Enlarges the boundary of the <c>Envelope</c> so that it contains
        /// (x, y). Does nothing if (x, y) is already on or within the boundaries.
        /// </summary>
        /// <param name="self">The first envelope (this object when extending)</param>
        /// <param name="x">The value to lower the minimum x to or to raise the maximum x to.</param>
        /// <param name="y">The value to lower the minimum y to or to raise the maximum y to.</param>
        public static void ExpandToInclude(this IEnvelope self, double x, double y)
        {
            ExpandToInclude(self, new Coordinate(x, y));
        }

        /// <summary>
        /// Enlarges the boundary of the <c>Envelope</c> so that it contains
        /// <c>other</c>. Does nothing if <c>other</c> is wholly on or
        /// within the boundaries.
        /// </summary>
        /// <param name="self">The first envelope (this object when extending)</param>
        /// <param name="other">the <c>Envelope</c> to merge with.</param>
        public static void ExpandToInclude(this IEnvelope self, IEnvelope other)
        {
            if (self == null) return;
            if (other == null) return;
            if (other.IsNull)
                return;
            if (self.IsNull)
            {
                self.Init(other.Minimum, other.Maximum);
                return;
            }
            int numDimensions = Math.Min(self.NumOrdinates, other.NumOrdinates);

            Coordinate min = self.Minimum;
            Coordinate max = self.Maximum;
            for (int i = 0; i < numDimensions; i++)
            {
                if (other.Minimum[i] < min[i]) min[i] = other.Minimum[i];
                if (other.Maximum[i] > max[i]) max[i] = other.Maximum[i];
            }
            self.Init(min, max); // re-initialize to prevent sign errors and indicate a change.
        }

        #endregion

        #region Intersect

        /// <summary>
        /// Returns the intersection of the specified segment with this bounding box.  If there is no intersection,
        /// then this returns null.  If the intersection is a corner, then the LineSegment will be degenerate,
        /// that is both the coordinates will be the same.  Otherwise, the segment will be returned so that the
        /// direction is the same as the original segment.
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method</param>
        /// <param name="segment">The LineSegment to intersect.</param>
        /// <returns>An ILineSegment that is cropped to fit within the bounding box.</returns>
        public static ILineSegment Intersection(this IEnvelope self, ILineSegment segment)
        {
            if (self == null) return null;
            if (self.IsNull) return null;
            if (segment == null) return null;
            // If the line segment is completely contained by this envelope, simply return the original.
            if (self.Contains(segment.P0) && self.Contains(segment.P1)) return segment;
            int count = 0;
            Coordinate[] borderPoints = new Coordinate[2];
            ILineSegment[] border = self.BorderSegments();
            for (int i = 0; i < 4; i++)
            {
                borderPoints[count] = border[i].Intersection(segment);
                if (borderPoints[count] != null)
                {
                    count++;
                    if (count > 1)
                        break;
                }
            }

            // If there are two intersections, the line crosses this envelope
            if (count == 2)
            {
                Vector v = new Vector(segment.P0, segment.P1);
                Vector t = new Vector(borderPoints[0], borderPoints[1]);
                return t.Dot(v) < 0 ? new LineSegment(borderPoints[1], borderPoints[0]) : new LineSegment(borderPoints[0], borderPoints[1]);
            }

            // if there is only one intersection, we probably have one point contained and one point not-contained
            if (count == 1)
            {
                if (self.Contains(segment.P0))
                {
                    // P1 got cropped, so make a line from p0 to the cropped point
                    return new LineSegment(segment.P0, borderPoints[0]);
                }
                return self.Contains(segment.P1) ? new LineSegment(borderPoints[0], segment.P1) : new LineSegment(borderPoints[0], borderPoints[0]);
            }

            return null;
        }

        /// <summary>
        /// Finds an envelope that represents the intersection between this
        /// envelope and the specified <c>IEnvelope</c>.  The number of dimensions of the
        /// returned envelope is the maximum of the NumOrdinates between the two envelopes,
        /// since a 2D envelope will only constrain a cube in 2 dimensions, and allow the
        /// z bounds to remain unaltered.
        /// </summary>
        /// <param name="self">The <c>IEnvelope</c> that is being extended by this method</param>
        /// <param name="env">An <c>IEnvelope</c> to compare against</param>
        /// <returns>an <c>IEnvelope</c> that bounds the intersection area</returns>
        public static IEnvelope Intersection(this IEnvelope self, IEnvelope env)
        {
            if (self.IsNull || env.IsNull || !self.Intersects(env))
                return new Envelope();

            IEnvelope bigDimension;
            IEnvelope smallDimension;
            if (env.NumOrdinates > self.NumOrdinates)
            {
                bigDimension = env;
                smallDimension = self;
            }
            else
            {
                bigDimension = self;
                smallDimension = env;
            }
            Coordinate min = bigDimension.Minimum.Copy();
            Coordinate max = bigDimension.Maximum.Copy();
            for (int i = 0; i < smallDimension.NumOrdinates; i++)
            {
                if (smallDimension.Minimum[i] > min[i]) min[i] = smallDimension.Minimum[i];
                if (smallDimension.Maximum[i] < max[i]) max[i] = smallDimension.Maximum[i];
            }
            return new Envelope(min, max);
        }

        /// <summary>
        /// Using an envelope intersection has some optimizations by checking against the envelope
        /// of the geometry.  In the worst case scenario, the envelope crops the geometry, and a new geometry is created.
        /// This will be much faster if the envelope contains the geometries envelope, however, simply returning
        /// the original geometry.
        /// </summary>
        /// <param name="self">The <c>IEnvelope</c> that is being extended by this method</param>
        /// <param name="geom">A geometric intersection against the area of this envelope</param>
        /// <returns>A geometry, cropped to the space of this envelope if necessary.</returns>
        public static IBasicGeometry Intersection(this IEnvelope self, IBasicGeometry geom)
        {
            if (self == null || geom == null) return null;
            if (self.IsNull) return null;
            IEnvelope env = geom.Envelope;
            if (env.Intersects(self) == false) return null;
            if (self.Contains(env)) return geom;
            IGeometry g = Geometry.FromBasicGeometry(geom);
            return g.Intersection(self.ToPolygon());
        }

        /// <summary>
        /// Check if the point <c>p</c> overlaps (lies inside) the region of this <c>Envelope</c>.
        /// </summary>
        /// <param name="self">The <c>IEnvelope</c> that is being extended by this method</param>
        /// <param name="p"> the <c>Coordinate</c> to be tested.</param>
        /// <returns><c>true</c> if the point overlaps this <c>Envelope</c>.</returns>
        public static bool Intersects(this IEnvelope self, Coordinate p)
        {
            if (self == null || p == null) return false;
            if (self.IsNull) return false;
            // A 2D envelope only tests in 2D, even if the point is 3d.
            // A 2D point can only be tested in 2D, even if the envelope is a cube.
            int numDimensions = Math.Min(self.NumOrdinates, p.NumOrdinates);

            for (int i = 0; i < numDimensions; i++)
            {
                if (p[i] < self.Minimum[i] || p[i] > self.Maximum[i]) return false;
            }
            return true;
        }

        /// <summary>
        /// Check if the point <c>(x, y)</c> overlaps (lies inside) the region of this <c>Envelope</c>.
        /// </summary>
        /// <param name="self">The <c>IEnvelope</c> that is being extended by this method</param>
        /// <param name="x"> the x-ordinate of the point.</param>
        /// <param name="y"> the y-ordinate of the point.</param>
        /// <returns><c>true</c> if the point overlaps this <c>Envelope</c>.</returns>
        public static bool Intersects(this IEnvelope self, double x, double y)
        {
            return Intersects(self, new Coordinate(x, y));
        }

        /// <summary>
        /// This should only be used if the Intersection is not actually required because it uses the Intersection method
        /// and returns false if the return value is null.
        /// </summary>
        /// <param name="self">The <c>IEnvelope</c> that is being extended by this method</param>
        /// <param name="segment">The segment to be tested against this envelope.</param>
        /// <returns>The line segment to compare against.</returns>
        public static bool Intersects(this IEnvelope self, ILineSegment segment)
        {
            if (self.Intersection(segment) != null) return true;
            return false;
        }

        /// <summary>
        /// Tests for intersection (any overlap) between the two envelopes.  In cases of unequal
        /// dimensions, the smaller dimension is used (e.g., if a 2D rectangle doesn't intersect
        /// a cube in its own plane, this would return false.)
        /// </summary>
        /// <param name="self">The <c>IEnvelope</c> that is being extended by this method</param>
        /// <param name="other"> the <c>Envelope</c> which this <c>Envelope</c> is
        /// being checked for overlapping.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <c>Envelope</c>s overlap.
        /// </returns>
        public static bool Intersects(this IEnvelope self, IEnvelope other)
        {
            if (self == null || other == null) return false;
            if (self.IsNull || other.IsNull)
                return false;
            int numDimensions = Math.Min(self.NumOrdinates, other.NumOrdinates);
            for (int i = 0; i < numDimensions; i++)
            {
                if (other.Minimum[i] > self.Maximum[i] || other.Maximum[i] < self.Minimum[i]) return false;
            }
            return true;
        }

        #endregion
    }
}