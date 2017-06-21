// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from a code project example:
// http://www.codeproject.com/KB/recipes/fortunevoronoi.aspx
// which is protected under the Code Project Open License
// http://www.codeproject.com/info/cpol10.aspx
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name              |   Date             |   Comments
// ------------------|--------------------|---------------------------------------------------------
// Benjamin Dittes   | August 10, 2005    |  Authored an original "Vector" class which enumerated an array of doubles
// Ted Dunsford      | August 26, 2009    |  Reformatted some code and use public structures for X and Y instead
// ********************************************************************************************************

using System;

namespace DotSpatial.Topology.Voronoi
{
    /// <summary>
    /// A vector class, implementing all interesting features of vectors
    /// </summary>
    public struct Vector2
    {
        /// <summary>
        /// This double controls the test for equality so that values that
        /// are smaller than this value will be considered equal.
        /// </summary>
        public static double Tolerance;

        /// <summary>
        /// The x coordinate
        /// </summary>
        public readonly double X;

        /// <summary>
        /// The y coordinate
        /// </summary>
        public readonly double Y;

        /// <summary>
        /// Creates a vector by reading a long array of vertices and assigning the vector based on that
        /// </summary>
        /// <param name="xyvertices"></param>
        /// <param name="offset"></param>
        public Vector2(double[] xyvertices, int offset)
        {
            X = xyvertices[offset];
            Y = xyvertices[offset + 1];
        }

        /// <summary>
        /// Build a new vector
        /// </summary>
        /// <param name="x">The elements of the vector</param>
        public Vector2(params double[] x)
        {
            X = x[0];
            Y = x[1];
        }

        /// <summary>
        /// Gets the dot product of this vector with itself
        /// </summary>
        public double SquaredLength
        {
            get { return this * this; }
        }

        /// <summary>
        /// Transforms the vector into a coordinate with an x and y value
        /// </summary>
        /// <returns></returns>
        public Coordinate ToCoordinate()
        {
            return new Coordinate(X, Y);
        }

        /// <summary>
        /// True if any of the double values is not a number
        /// </summary>
        public bool ContainsNan()
        {
            if (double.IsNaN(X)) return true;
            if (double.IsNaN(Y)) return true;
            return false;
        }

        /// <summary>
        /// Convert the vector into a reconstructable string representation
        /// </summary>
        /// <returns>A string from which the vector can be rebuilt</returns>
        public override string ToString()
        {
            return "(" + X + "," + Y + ")";
        }

        /// <summary>
        /// Compares this vector with another one
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Vector2 b = (Vector2)obj;
            return TolerantEqual(X, b.X) && TolerantEqual(Y, b.Y);
        }

        /// <summary>
        /// Calculates the euclidean distance from this cell to another
        /// </summary>
        /// <returns>Vector2 stuff</returns>
        public double Distance(Vector2 other)
        {
            double dx = X - other.X;
            double dy = Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private static bool TolerantEqual(double a, double b)
        {
            // both being NaN counts as equal
            if (double.IsNaN(a) && double.IsNaN(b)) return true;

            // If one is NaN but the other isn't, then the vector is not equal
            if (double.IsNaN(a) || double.IsNaN(b)) return false;

            // Allow the default, operating system controlled equality check for two doubles
            if (Tolerance == 0) return a == b;

            // If a tolerance has been specified, check equality using that tolerance level.
            return Math.Abs(a - b) <= Tolerance;
        }

        /// <summary>
        /// Retrieves a hashcode that is dependent on the elements
        /// </summary>
        /// <returns>The hashcode</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() * Y.GetHashCode();
        }

        /// <summary>
        /// Get the scalar product of two vectors
        /// </summary>
        public static double operator *(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        /// <summary>
        /// Overrides equality to use the tolerant equality
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Overrides equality to use the tolerant equality test
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return !a.Equals(b);
        }

        /// <summary>
        /// Calculates the vector sum of these two vectors
        /// </summary>
        /// <param name="a">One vector to add</param>
        /// <param name="b">The second vector to add</param>
        /// <returns>The vector sum of the specified vectors</returns>
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Calculates the vector sum of these two vectors
        /// </summary>
        /// <param name="a">One vector to add</param>
        /// <param name="b">The second vector to add</param>
        /// <returns>The vector sum of the specified vectors</returns>
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Multiplies the vector by a scalar
        /// </summary>
        /// <param name="a">The vector to modify</param>
        /// <param name="scale">The double scale to multiply</param>
        /// <returns>A new Vector2</returns>
        public static Vector2 operator *(Vector2 a, double scale)
        {
            return new Vector2(a.X * scale, a.Y * scale);
        }

        /// <summary>
        /// Multiplies the vector by a scalar.
        /// </summary>
        /// <param name="scale">The double scale to multiply.</param>
        /// <param name="a">The vector to modify.</param>
        /// <returns>A new Vector2.</returns>
        public static Vector2 operator *(double scale, Vector2 a)
        {
            return new Vector2(a.X * scale, a.Y * scale);
        }
    }
}