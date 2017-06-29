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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/2/2010 10:56:27 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// The FeatureSetPack contains a MultiPoint, Line and Polygon FeatureSet which can
    /// handle the various types of geometries that can arrive from a mixed geometry.
    /// </summary>
    public class FeatureSetPack : IEnumerable<IFeatureSet>
    {
        /// <summary>
        /// The featureset with all the lines
        /// </summary>
        public IFeatureSet Lines;

        /// <summary>
        /// The featureset with all the points
        /// </summary>
        public IFeatureSet Points;

        /// <summary>
        /// The featureset with all the polygons
        /// </summary>
        public IFeatureSet Polygons;

        private int _lineLength;
        private List<double[]> _lineVertices;
        private int _pointLength;
        private List<double[]> _pointVertices;
        private int _polygonLength;

        /// <summary>
        /// Stores the raw vertices from the different shapes in a list while being created.
        /// That way, the final array can be created one time.
        /// </summary>
        private List<double[]> _polygonVertices;

        /// <summary>
        /// Creates a new instance of the FeatureSetPack
        /// </summary>
        public FeatureSetPack()
        {
            Polygons = new FeatureSet(FeatureType.Polygon);
            Polygons.IndexMode = true;
            Points = new FeatureSet(FeatureType.MultiPoint);
            Points.IndexMode = true;
            Lines = new FeatureSet(FeatureType.Line);
            Lines.IndexMode = true;
            _polygonVertices = new List<double[]>();
            _pointVertices = new List<double[]>();
            _lineVertices = new List<double[]>();
        }

        #region IEnumerable<IFeatureSet> Members

        /// <inheritdoc />
        public IEnumerator<IFeatureSet> GetEnumerator()
        {
            return new FeatureSetPackEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Clears the vertices and sets up new featuresets.
        /// </summary>
        public void Clear()
        {
            _polygonVertices = new List<double[]>();
            _pointVertices = new List<double[]>();
            _lineVertices = new List<double[]>();
            Polygons = new FeatureSet(FeatureType.Polygon);
            Polygons.IndexMode = true;
            Points = new FeatureSet(FeatureType.MultiPoint);
            Points.IndexMode = true;
            Lines = new FeatureSet(FeatureType.Line);
            Lines.IndexMode = true;
            _lineLength = 0;
            _polygonLength = 0;
            _pointLength = 0;
        }

        /// <summary>
        /// Adds the shape.  Assumes that the "part" indices are created with a 0 base, and the number of
        /// vertices is specified.  The start range of each part will be updated with the new shape range.
        /// The vertices array itself iwll be updated during hte stop editing step.
        /// </summary>
        /// <param name="shapeVertices"></param>
        /// <param name="shape"></param>
        public void Add(double[] shapeVertices, ShapeRange shape)
        {
            if (shape.FeatureType == FeatureType.Point || shape.FeatureType == FeatureType.MultiPoint)
            {
                _pointVertices.Add(shapeVertices);
                shape.StartIndex = _pointLength / 2; // point offset, not double offset
                Points.ShapeIndices.Add(shape);
                _pointLength += shapeVertices.Length;
            }
            if (shape.FeatureType == FeatureType.Line)
            {
                _lineVertices.Add(shapeVertices);
                shape.StartIndex = _lineLength / 2; // point offset
                Lines.ShapeIndices.Add(shape);
                _lineLength += shapeVertices.Length;
            }
            if (shape.FeatureType == FeatureType.Polygon)
            {
                _polygonVertices.Add(shapeVertices);
                shape.StartIndex = _polygonLength / 2; // point offset
                Polygons.ShapeIndices.Add(shape);
                _polygonLength += shapeVertices.Length / 2;
            }
        }

        /// <summary>
        /// Finishes the featuresets by converting the lists
        /// </summary>
        public void StopEditing()
        {
            Points.Vertex = Combine(_pointVertices, _pointLength);
            Points.UpdateExtent();
            Lines.Vertex = Combine(_lineVertices, _lineLength);
            Lines.UpdateExtent();
            Polygons.Vertex = Combine(_polygonVertices, _polygonLength);
            Polygons.UpdateExtent();
        }

        /// <summary>
        /// Combines the vertices, finalizing the creation
        /// </summary>
        /// <param name="verts"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static double[] Combine(IEnumerable<double[]> verts, int length)
        {
            double[] result = new double[length * 2];
            int offset = 0;
            foreach (double[] shape in verts)
            {
                Array.Copy(shape, 0, result, offset, shape.Length);
                offset += shape.Length;
            }
            return result;
        }

        #region Nested type: FeatureSetPackEnumerator

        /// <summary>
        /// Enuemratres the FeatureSetPack in Polygon, Line, Point order.  If any member
        /// is null, it skips that member.
        /// </summary>
        public class FeatureSetPackEnumerator : IEnumerator<IFeatureSet>
        {
            private readonly FeatureSetPack _parent;
            private IFeatureSet _current;
            private int _index;

            /// <summary>
            /// Creates the FeatureSetPackEnumerator based on the specified FeaturSetPack
            /// </summary>
            /// <param name="parent">The Pack</param>
            public FeatureSetPackEnumerator(FeatureSetPack parent)
            {
                _parent = parent;
                _index = -1;
            }

            #region IEnumerator<IFeatureSet> Members

            /// <inheritdoc />
            public IFeatureSet Current
            {
                get { return _current; }
            }

            object IEnumerator.Current
            {
                get { return _current; }
            }

            /// <inheritdoc />
            public void Dispose()
            {
            }

            /// <inheritdoc />
            public bool MoveNext()
            {
                _current = null;
                while (_current == null || _current.Vertex == null || _current.Vertex.Length == 0)
                {
                    _index++;
                    if (_index > 2) return false;
                    switch (_index)
                    {
                        case 0:
                            _current = _parent.Polygons;
                            break;
                        case 1:
                            _current = _parent.Lines;
                            break;
                        case 2:
                            _current = _parent.Points;
                            break;
                    }
                }
                return true;
            }

            /// <inheritdoc />
            public void Reset()
            {
                _index = -1;
                _current = null;
            }

            #endregion
        }

        #endregion
    }
}