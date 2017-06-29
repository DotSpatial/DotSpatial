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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/16/2009 1:28:16 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    public sealed class ShapeRange : ICloneable
    {
        #region Private Variables

        /// <summary>
        /// The feature type
        /// </summary>
        public FeatureType FeatureType { get; private set; }

        /// <summary>
        /// The content length
        /// </summary>
        public int ContentLength { get; set; }

        /// <summary>
        /// Control the epsilon to use for the intersect calculations
        /// </summary>
        public const double Epsilon = double.Epsilon;

        /// <summary>
        /// If this is null, then there is only one part for this ShapeIndex.
        /// </summary>
        public List<PartRange> Parts { get; private set; }

        /// <summary>
        /// The record number (for .shp files usually 1-based)
        /// </summary>
        public int RecordNumber { get; set; }

        private Extent _extent;
        private int _numParts;
        private int _numPoints;
        private ShapeType _shapeType;
        private int _startIndex;

        /// <summary>
        /// The starting index for the entire shape range.
        /// </summary>
        public int StartIndex
        {
            get { return _startIndex; }
            set
            {
                NumPoints = 0;
                foreach (PartRange part in Parts)
                {
                    part.ShapeOffset = value;
                    NumPoints += part.NumVertices;
                }
                _startIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets the extent of this shape range.  Setting this will prevent
        /// the automatic calculations.
        /// </summary>
        public Extent Extent
        {
            get
            {
                if (_extent == null) _extent = CalculateExtents();
                return _extent;
            }
            set
            {
                _extent = value;
            }
        }

        /// <summary>
        /// The shape type for the header of this shape
        /// </summary>
        public ShapeType ShapeType
        {
            get
            {
                return _shapeType;
            }
            set
            {
                _shapeType = value;
                UpgradeExtent();
            }
        }

        /// <summary>
        /// The number of points in the entire shape
        /// </summary>
        public int NumPoints
        {
            get
            {
                if (_numPoints < 0)
                {
                    int n = 0;
                    foreach (PartRange part in Parts)
                    {
                        n += part.NumVertices;
                    }
                    return n;
                }
                return _numPoints;
            }
            set { _numPoints = value; }
        }

        /// <summary>
        /// Creates a shallow copy of everything except the parts list and extent, which are deep copies.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            ShapeRange copy = (ShapeRange)MemberwiseClone();
            copy.Parts = new List<PartRange>(Parts.Count);
            foreach (PartRange part in Parts)
            {
                copy.Parts.Add(part.Copy());
            }
            copy.Extent = Extent.Copy();
            return copy;
        }

        /// <summary>
        /// Considers the ShapeType and upgrades the extent class to accommodate M and Z.
        /// This is automatically called form the setter of ShapeType.
        /// </summary>
        public void UpgradeExtent()
        {
            if (_shapeType == ShapeType.MultiPointZ || _shapeType == ShapeType.PointZ ||
                _shapeType == ShapeType.PolygonZ || _shapeType == ShapeType.PolyLineZ)
            {
                IExtentZ zTest = Extent as IExtentZ;
                if (zTest == null)
                {
                    Extent ext = new ExtentMZ();
                    if (_extent != null) ext.CopyFrom(_extent);
                    _extent = ext;
                }
                // Already implements M and Z
            }
            else if (_shapeType == ShapeType.MultiPointM || _shapeType == ShapeType.PointM ||
                     _shapeType == ShapeType.PolygonM || _shapeType == ShapeType.PolyLineM)
            {
                IExtentM mTest = Extent as IExtentM;
                if (mTest == null)
                {
                    Extent ext = new ExtentMZ();
                    if (_extent != null) ext.CopyFrom(_extent);
                    _extent = ext;
                }
                // already at least implements M
            }
            // No upgrade necessary
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a blank instance of a shaperange where vertices can be assigned later.
        /// <param name="type">the feature type clarifies point, line, or polygon.</param>
        /// <param name="coordType">The coordinate type clarifies whether M or Z values exist.</param>
        /// </summary>
        public ShapeRange(FeatureType type, CoordinateType coordType = CoordinateType.Regular)
        {
            FeatureType = type;
            Parts = new List<PartRange>();
            _numParts = -1; // default to relying on the parts list instead of the cached value.
            _numPoints = -1; // rely on accumulation from parts instead of a solid number

            switch (coordType)
            {
                case CoordinateType.Z:
                    _extent = new ExtentMZ();
                    break;
                case CoordinateType.M:
                    _extent = new ExtentM();
                    break;
                default:
                    _extent = new Extent();
                    break;
            }
        }

        /// <summary>
        /// Creates a new "point" shape that has only the one point.
        /// </summary>
        /// <param name="v"></param>
        public ShapeRange(Vertex v)
        {
            FeatureType = FeatureType.Point;
            Parts = new List<PartRange>();
            _numParts = -1;
            double[] coords = new double[2];
            coords[0] = v.X;
            coords[1] = v.Y;
            PartRange prt = new PartRange(coords, 0, 0, FeatureType.Point);
            prt.NumVertices = 1;
            Extent = new Extent(v.X, v.Y, v.X, v.Y);
            Parts.Add(prt);
        }

        /// <summary>
        /// Initializes a new instance of the ShapeRange class.
        /// </summary>
        /// <param name="env">The envelope to turn into a shape range.</param>
        public ShapeRange(IEnvelope env)
            :this(env.ToExtent())
        {
        }

        /// <summary>
        /// This creates a polygon shape from an extent.
        /// </summary>
        /// <param name="ext">The extent to turn into a polygon shape.</param>
        public ShapeRange(Extent ext)
        {
            Extent = ext;
            Parts = new List<PartRange>();
            _numParts = -1;
            // Counter clockwise
            // 1 2
            // 4 3
            double[] coords = new double[8];
            // C1
            coords[0] = ext.MinX;
            coords[1] = ext.MaxY;
            // C2
            coords[2] = ext.MaxX;
            coords[3] = ext.MaxY;
            // C3
            coords[4] = ext.MaxX;
            coords[5] = ext.MinY;
            // C4
            coords[6] = ext.MinX;
            coords[7] = ext.MinY;

            FeatureType = FeatureType.Polygon;
            ShapeType = ShapeType.Polygon;
            PartRange pr = new PartRange(coords, 0, 0, FeatureType.Polygon);
            pr.NumVertices = 4;
            Parts.Add(pr);
        }

        #endregion

        #region Methods

        /// <summary>
        /// If this is set, then it will cache an integer count that is independant from Parts.Count.
        /// If this is not set, (or set to a negative value) then getting this will return Parts.Count
        /// </summary>
        public int NumParts
        {
            get
            {
                if (_numParts < 0) return Parts.Count;
                return _numParts;
            }
            set
            {
                _numParts = value;
            }
        }

        /// <summary>
        /// Forces each of the parts to adopt an extent equal to a calculated extents.
        /// The extents for the shape will expand to include those.
        /// </summary>
        public Extent CalculateExtents()
        {
            Extent ext = new Extent();
            foreach (PartRange part in Parts)
            {
                ext.ExpandToInclude(part.CalculateExtent());
            }
            Extent = ext;
            return ext;
        }

        /// <summary>
        /// Gets the first vertex from the first part.
        /// </summary>
        /// <returns></returns>
        public Vertex First()
        {
            double[] verts = Parts[0].Vertices;
            Vertex result = new Vertex(verts[StartIndex], verts[StartIndex + 1]);
            return result;
        }

        /// <summary>
        /// Tests the intersection with an extents
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public bool Intersects(Extent ext)
        {
            return Intersects(new Shape(ext).Range);
        }

        /// <summary>
        /// Tests the intersection using an envelope
        /// </summary>
        /// <param name="envelope"></param>
        /// <returns></returns>
        public bool Intersects(IEnvelope envelope)
        {
            return Intersects(new Shape(envelope).Range);
        }

        /// <summary>
        /// Tests the intersection with a coordinate
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public bool Intersects(Coordinate coord)
        {
            return Intersects(new Shape(coord).Range);
        }

        /// <summary>
        /// Tests the intersection with a vertex
        /// </summary>
        /// <param name="vert"></param>
        /// <returns></returns>
        public bool Intersects(Vertex vert)
        {
            return Intersects(new Shape(vert).Range);
        }

        /// <summary>
        /// Tests the intersection with a shape
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        public bool Intersects(Shape shape)
        {
            return Intersects(shape.Range);
        }

        /// <summary>
        /// This calculations processes the intersections
        /// </summary>
        /// <param name="shape">The shape to do intersection calculations with.</param>
        public bool Intersects(ShapeRange shape)
        {
            // Extent check first.  If the extents don't intersect, then this doesn't intersect.
            if (!Extent.Intersects(shape.Extent)) return false;

            switch (FeatureType)
            {
                case FeatureType.Polygon:
                    PolygonShape.Epsilon = Epsilon;
                    return PolygonShape.Intersects(this, shape);
                case FeatureType.Line:
                    LineShape.Epsilon = Epsilon;
                    return LineShape.Intersects(this, shape);
                case FeatureType.Point:
                    PointShape.Epsilon = Epsilon;
                    return PointShape.Intersects(this, shape);
                default:
                    return false;
            }
        }

        /// <summary>
        /// This sets the vertex array by cycling through each part index and updates.
        /// </summary>
        /// <param name="vertices">The double array of vertices that should be referenced by the parts.</param>
        public void SetVertices(double[] vertices)
        {
            foreach (PartRange prt in Parts)
            {
                prt.Vertices = vertices;
            }
        }

        /// <summary>
        /// Given a vertex, this will determine the part that the vertex is within.
        /// </summary>
        /// <param name="vertexOffset"></param>
        /// <returns></returns>
        public int PartIndex(int vertexOffset)
        {
            int i = 0;
            foreach (PartRange part in Parts)
            {
                if (part.StartIndex <= vertexOffset && part.EndIndex >= vertexOffset) return i;
                i++;
            }
            return -1;
        }

        /// <summary>
        /// gets the integer end index as calculated from the number of points and the start index
        /// </summary>
        /// <returns></returns>
        public int EndIndex()
        {
            return StartIndex + NumPoints - 1;
        }

        #endregion
    }
}