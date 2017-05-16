// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using GeoAPI.Geometries;

// The Original Code is from a code project example:
// http://www.codeproject.com/KB/recipes/fortunevoronoi.aspx
// which is protected under the Code Project Open License
// http://www.codeproject.com/info/cpol10.aspx
namespace DotSpatial.NTSExtension.Voronoi
{
    /// <summary>
    /// A vector class, implementing all interesting features of vectors
    /// </summary>
    public struct Vector2
    {
        #region Fields

        /// <summary>
        /// This double controls the test for equality so that values that are smaller than this value will be considered equal.
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

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2"/> struct by reading a long array of vertices and assigning the vector based on that.
        /// </summary>
        /// <param name="xyvertices">Vertices to assign.</param>
        /// <param name="offset">Offset that indicates where the x value is.</param>
        public Vector2(double[] xyvertices, int offset)
        {
            X = xyvertices[offset];
            Y = xyvertices[offset + 1];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2"/> struct.
        /// </summary>
        /// <param name="x">The elements of the vector</param>
        public Vector2(params double[] x)
        {
            X = x[0];
            Y = x[1];
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the dot product of this vector with itself
        /// </summary>
        public double SquaredLength => this * this;

        #endregion

        #region Operators

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
        /// Overrides equality to use the tolerant equality
        /// </summary>
        /// <param name="a">First vector to check.</param>
        /// <param name="b">Second vector to check.</param>
        /// <returns>True, if both are equal.</returns>
        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Overrides equality to use the tolerant equality test
        /// </summary>
        /// <param name="a">First vector to check.</param>
        /// <param name="b">Second vector to check.</param>
        /// <returns>True, if both are not equal.</returns>
        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return !a.Equals(b);
        }

        /// <summary>
        /// Get the scalar product of two vectors
        /// </summary>
        /// <param name="a">First vector to multiply.</param>
        /// <param name="b">Second vector to multiply.</param>
        /// <returns>The resulting value.</returns>
        public static double operator *(Vector2 a, Vector2 b)
        {
            return (a.X * b.X) + (a.Y * b.Y);
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

        #endregion

        #region Methods

        /// <summary>
        /// True if any of the double values is not a number.
        /// </summary>
        /// <returns>bool if either X or Y is nan.</returns>
        public bool ContainsNan()
        {
            return double.IsNaN(X) || double.IsNaN(Y);
        }

        /// <summary>
        /// Calculates the euclidean distance from this cell to another
        /// </summary>
        /// <param name="other">Second cell for distance calculation.</param>
        /// <returns>Vector2 stuff</returns>
        public double Distance(Vector2 other)
        {
            double dx = X - other.X;
            double dy = Y - other.Y;
            return Math.Sqrt((dx * dx) + (dy * dy));
        }

        /// <summary>
        /// Compares this vector with another one.
        /// </summary>
        /// <param name="obj">Selecond vector for comparing.</param>
        /// <returns>True, if the vectors are equal by tolerance.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Vector2 b = (Vector2)obj;
            return TolerantEqual(X, b.X) && TolerantEqual(Y, b.Y);
        }

        /// <summary>
        /// Retrieves a hashcode that is dependent on the elements.
        /// </summary>
        /// <returns>The hashcode</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() * Y.GetHashCode();
        }

        /// <summary>
        /// Transforms the vector into a coordinate with an x and y value.
        /// </summary>
        /// <returns>Returns a coordinate with this vectors X and Y values.</returns>
        public Coordinate ToCoordinate()
        {
            return new Coordinate(X, Y);
        }

        /// <summary>
        /// Convert the vector into a reconstructable string representation
        /// </summary>
        /// <returns>A string from which the vector can be rebuilt</returns>
        public override string ToString()
        {
            return "(" + X + "," + Y + ")";
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

        #endregion
    }
}