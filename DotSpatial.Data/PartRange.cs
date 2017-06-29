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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/16/2009 1:24:22 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// Vertices require methods to work quickly with data.  These are not designed to be inherited,
    /// overridden or equiped with interfaces and properties.
    /// </summary>
    public sealed class PartRange : VertexRange, ICloneable
    {
        private readonly SegmentRange _segments;

        #region Constructors

        /// <summary>
        /// StartRange, count and vertices will be declared later.
        /// </summary>
        /// <param name="featureType"></param>
        public PartRange(FeatureType featureType)
        {
            // _featureType = featureType;
        }

        /// <summary>
        /// Creates a new instance of Part
        /// </summary>
        /// <param name="allVertices">An array of all the vertex locations</param>
        /// <param name="shapeOffset">The point index of the shape. </param>
        /// <param name="partOffset">The ponit index of the part.</param>
        /// <param name="featureType">The type of features.</param>
        public PartRange(double[] allVertices, int shapeOffset, int partOffset, FeatureType featureType) :
            base(allVertices, shapeOffset, partOffset)
        {
            _segments = new SegmentRange(this, featureType);
            //_featureType = featureType;
        }

        /// <summary>
        /// Gets a segment range enumerator.
        /// </summary>
        public SegmentRange Segments
        {
            get
            {
                return _segments;
            }
        }

        /// <summary>
        /// This returns a shallow copy, and expects any resetting of the vertex arrays to occur at a
        /// higher level.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// Calculates the area and if the area is negative, this is considered a hole.
        /// </summary>
        /// <returns>Boolean, true if this has a negative area and should be thought of as a hole</returns>
        public bool IsHole()
        {
            // If the area is actually half of the "area" calculated below.
            // If the area is negative, then the polygon is clockwise.
            // http://en.wikipedia.org/wiki/Polygon
            double area = 0;
            //We will triangulate the polygon
            //into triangles with points p[0], p[i], p[i+1]
            for (int i = StartIndex; i <= EndIndex; i++)
            {
                double x1 = Vertices[i * 2];
                double y1 = Vertices[i * 2 + 1];
                double x2, y2;
                if (i < EndIndex)
                {
                    x2 = Vertices[(i + 1) * 2];
                    y2 = Vertices[(i + 1) * 2 + 1];
                }
                else
                {
                    x2 = Vertices[StartIndex];
                    y2 = Vertices[StartIndex + 1];
                }
                double trapArea = (x1 * y2) - (x2 * y1);
                area += trapArea;
            }
            return area >= 0;
        }

        /// <summary>
        /// This creates a new extent and cylces through the coordinates to calculate what it is.
        /// Since the vertices may change so easilly, this is not cached.
        /// </summary>
        /// <returns></returns>
        public Extent CalculateExtent()
        {
            // Create an extent for faster checking in most cases
            Extent ext = new Extent();

            // Do this once, and then we can re-use it for each other part.
            foreach (Vertex point in this)
            {
                if (ext.MaxX < point.X) ext.MaxX = point.X;
                if (ext.MinX > point.X) ext.MinX = point.X;
                if (ext.MinY > point.Y) ext.MinY = point.Y;
                if (ext.MaxY < point.Y) ext.MaxY = point.Y;
            }
            return ext;
        }

        #endregion

        //private FeatureType _featureType;
    }
}