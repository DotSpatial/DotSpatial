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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/27/2009 4:45:30 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name              |   Date             |   Comments
// ------------------|--------------------|---------------------------------------------------------
// Benjamin Dittes   | August 10, 2005    |  Authored original code for working with laser data
// Ted Dunsford      | August 26, 2009    |  Ported and cleaned up the raw source from code project
// ********************************************************************************************************

using System;

namespace DotSpatial.NTSExtension.Voronoi
{
    /// <summary>
    /// The Voronoi Edge represents a bisector between two of the original datapoints. The
    /// LeftData and RightData represent those original points. VVertexA and VVertexB represent
    /// the endpoints of the segment created using those original points.
    /// </summary>
    public class VoronoiEdge
    {
        #region Properties

        /// <summary>
        /// Gets a vector direction that indicates the direction of this
        /// </summary>
        public Vector2 DirectionVector
        {
            get
            {
                if (!IsPartlyInfinite) return (VVertexB - VVertexA) * (1.0 / VVertexA.Distance(VVertexB));
                if (LeftData.X == RightData.X)
                {
                    if (LeftData.Y < RightData.Y) return new Vector2(-1, 0);
                    return new Vector2(1, 0);
                }

                Vector2 erg = new Vector2(-(RightData.Y - LeftData.Y) / (RightData.X - LeftData.X), 1);
                if (RightData.X < LeftData.X) erg = erg * -1;
                erg = erg * (1.0 / Math.Sqrt(erg.SquaredLength));
                return erg;
            }
        }

        /// <summary>
        /// Gets the center between the left and right data points.
        /// </summary>
        public Vector2 FixedPoint
        {
            get
            {
                if (IsInfinite) return (LeftData + RightData) * 0.5;
                if (VVertexA != Fortune.VVInfinite) return VVertexA;
                return VVertexB;
            }
        }

        /// <summary>
        /// Gets a value indicating whether both the VertexA and VertexB are infinite vectors
        /// </summary>
        public bool IsInfinite => VVertexA == Fortune.VVInfinite && VVertexB == Fortune.VVInfinite;

        /// <summary>
        /// Gets a value indicating whether one of the voronoi vertices for this edge is known, but no
        /// intersection is found to bound the other edge, and it should extend to the
        /// bounding box.
        /// </summary>
        public bool IsPartlyInfinite => VVertexA == Fortune.VVInfinite || VVertexB == Fortune.VVInfinite;

        /// <summary>
        /// Gets or sets the other original point in the dataset
        /// </summary>
        public Vector2 LeftData { get; set; }

        /// <summary>
        /// Gets the length of this edge
        /// </summary>
        public double Length
        {
            get
            {
                if (IsPartlyInfinite) return double.PositiveInfinity;
                return VVertexA.Distance(VVertexB);
            }
        }

        /// <summary>
        /// Gets or sets one of the original points in the dataset
        /// </summary>
        public Vector2 RightData { get; set; }

        /// <summary>
        /// Gets or sets one of the endpoints for the segment that defines this edge
        /// </summary>
        public Vector2 VVertexA { get; set; } = Fortune.VVUnkown;

        /// <summary>
        /// Gets or sets the other endpoint for the segment that defines this edge
        /// </summary>
        public Vector2 VVertexB { get; set; } = Fortune.VVUnkown;

        /// <summary>
        /// Gets or sets a value indicating whether some cleanup operations are done.
        /// </summary>
        internal bool Done { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds VertexA and VertexB sequentially, so that if VertexA is defined,
        /// then VertexB will become the specified vertex.
        /// </summary>
        /// <param name="v">Vector that gets assigned to either VertexA or VertexB</param>
        /// <exception cref="Exception">Thrown if VertexA and VertexB are both already set.</exception>
        public void AddVertex(Vector2 v)
        {
            if (VVertexA == Fortune.VVUnkown) VVertexA = v;
            else if (VVertexB == Fortune.VVUnkown) VVertexB = v;
            else throw new Exception("Tried to add third vertex!");
        }

        #endregion
    }
}