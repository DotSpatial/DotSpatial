// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/11/2009 10:28:29 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Runtime.InteropServices;
using GeoAPI.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// A vertex is a two dimensional structure with an X and a Y value. This is deliberately kept as small as possible.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vertex
    {
        /// <summary>
        /// The tolerance for testing equality.
        /// </summary>
        public static double Epsilon = double.Epsilon;

        /// <summary>
        /// The X coordinate.
        /// </summary>
        public double X;

        /// <summary>
        /// The Y coordinate.
        /// </summary>
        public double Y;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Vertex"/> struct.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        public Vertex(double x, double y)
        {
            X = x;
            Y = y;
        }

        #endregion

        /// <summary>
        /// Checks whether the given vertices are equal.
        /// </summary>
        /// <param name="v1">The first vertex.</param>
        /// <param name="v2">The second vertex.</param>
        /// <returns>True, if the difference is smaller than epsilon.</returns>
        public static bool operator ==(Vertex v1, Vertex v2)
        {
            return Math.Abs(v2.X - v1.X) < Epsilon && Math.Abs(v2.Y - v1.Y) < Epsilon;
        }

        /// <summary>
        /// Checks whether the given vertices are equal.
        /// </summary>
        /// <param name="v1">The first vertex.</param>
        /// <param name="v2">The second vertex.</param>
        /// <returns>True, if the difference is bigger than epsilon.</returns>
        public static bool operator !=(Vertex v1, Vertex v2)
        {
            return !(v1 == v2);
        }

        /// <summary>
        /// Casts this vertex to a coordinate.
        /// </summary>
        /// <returns>This as coordinate.</returns>
        public Coordinate ToCoordinate()
        {
            return new Coordinate(X, Y);
        }

        /// <summary>
        /// Checks whether this equals other.
        /// </summary>
        /// <param name="other">The other vertex to check against.</param>
        /// <returns>True, if both are equal.</returns>
        public bool Equals(Vertex other)
        {
            return other.X == X && other.Y == Y;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Vertex)) return false;

            return Equals((Vertex)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }
    }
}