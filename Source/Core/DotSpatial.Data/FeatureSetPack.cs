// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// The FeatureSetPack contains a MultiPoint, Line and Polygon FeatureSet which can
    /// handle the various types of geometries that can arrive from a mixed geometry.
    /// </summary>
    public class FeatureSetPack : IEnumerable<IFeatureSet>
    {
        #region Variables
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
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSetPack"/> class.
        /// </summary>
        public FeatureSetPack()
        {
            Polygons = new FeatureSet(FeatureType.Polygon) { IndexMode = true };
            Points = new FeatureSet(FeatureType.MultiPoint) { IndexMode = true };
            Lines = new FeatureSet(FeatureType.Line) { IndexMode = true };
            _polygonVertices = new List<double[]>();
            _pointVertices = new List<double[]>();
            _lineVertices = new List<double[]>();
        }

        #region Properties

        /// <summary>
        /// Gets or sets the featureset with all the lines.
        /// </summary>
        public IFeatureSet Lines { get; set; }

        /// <summary>
        /// Gets or sets the featureset with all the points.
        /// </summary>
        public IFeatureSet Points { get; set; }

        /// <summary>
        /// Gets or sets the featureset with all the polygons.
        /// </summary>
        public IFeatureSet Polygons { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Combines the vertices, finalizing the creation.
        /// </summary>
        /// <param name="verts">List of the vertices that get combined.</param>
        /// <param name="length">Number of all the vertices inside verts.</param>
        /// <returns>A double array containing all vertices.</returns>
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

        /// <summary>
        /// Adds the shape. Assumes that the "part" indices are created with a 0 base, and the number of
        /// vertices is specified. The start range of each part will be updated with the new shape range.
        /// The vertices array itself iwll be updated during hte stop editing step.
        /// </summary>
        /// <param name="shapeVertices">Vertices of the shape that gets added.</param>
        /// <param name="shape">Shape that gets added.</param>
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
        /// Clears the vertices and sets up new featuresets.
        /// </summary>
        public void Clear()
        {
            _polygonVertices = new List<double[]>();
            _pointVertices = new List<double[]>();
            _lineVertices = new List<double[]>();
            Polygons = new FeatureSet(FeatureType.Polygon) { IndexMode = true };
            Points = new FeatureSet(FeatureType.MultiPoint) { IndexMode = true };
            Lines = new FeatureSet(FeatureType.Line) { IndexMode = true };
            _lineLength = 0;
            _polygonLength = 0;
            _pointLength = 0;
        }

        /// <inheritdoc />
        public IEnumerator<IFeatureSet> GetEnumerator()
        {
            return new FeatureSetPackEnumerator(this);
        }

        /// <summary>
        /// Finishes the featuresets by converting the lists.
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

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Classes

        /// <summary>
        /// Enuemratres the FeatureSetPack in Polygon, Line, Point order. If any member is null, it skips that member.
        /// </summary>
        private class FeatureSetPackEnumerator : IEnumerator<IFeatureSet>
        {
            private readonly FeatureSetPack _parent;

            private int _index;

            /// <summary>
            /// Initializes a new instance of the <see cref="FeatureSetPackEnumerator"/> class based on the specified FeaturSetPack.
            /// </summary>
            /// <param name="parent">The Pack.</param>
            public FeatureSetPackEnumerator(FeatureSetPack parent)
            {
                _parent = parent;
                _index = -1;
            }

            /// <inheritdoc />
            public IFeatureSet Current { get; private set; }

            object IEnumerator.Current => Current;

            /// <inheritdoc />
            public void Dispose()
            {
            }

            /// <inheritdoc />
            public bool MoveNext()
            {
                Current = null;
                while (Current?.Vertex == null || Current.Vertex.Length == 0)
                {
                    _index++;
                    if (_index > 2) return false;
                    switch (_index)
                    {
                        case 0:
                            Current = _parent.Polygons;
                            break;
                        case 1:
                            Current = _parent.Lines;
                            break;
                        case 2:
                            Current = _parent.Points;
                            break;
                    }
                }

                return true;
            }

            /// <inheritdoc />
            public void Reset()
            {
                _index = -1;
                Current = null;
            }
        }

        #endregion
    }
}