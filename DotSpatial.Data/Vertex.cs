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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/11/2009 10:28:29 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Runtime.InteropServices;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// A vertex is a two dimensional structure with an X and a Y value.  This is deliberately kept as small as possibl.e
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vertex
    {
        /// <summary>
        /// The tolerance for testing equality
        /// </summary>
        public static double Epsilon = double.Epsilon;

        /// <summary>
        /// An X coordinate
        /// </summary>
        public double X;

        /// <summary>
        /// The Y coordinate
        /// </summary>
        public double Y;

        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Vertex
        /// </summary>
        public Vertex(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Casts this vertex to a coordinate
        /// </summary>
        /// <returns></returns>
        public Coordinate ToCoordinate()
        {
            return new Coordinate(X, Y);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Vertex other)
        {
            return other.X == X && other.Y == Y;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator ==(Vertex v1, Vertex v2)
        {
            return Math.Abs(v2.X - v1.X) < Epsilon && Math.Abs(v2.Y - v1.Y) < Epsilon;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator !=(Vertex v1, Vertex v2)
        {
            return !(Math.Abs(v2.X - v1.X) < Epsilon && Math.Abs(v2.Y - v1.Y) < Epsilon);
        }

        #endregion

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