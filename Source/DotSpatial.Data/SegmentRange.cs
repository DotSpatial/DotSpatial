// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// SegmentRange
    /// </summary>
    public class SegmentRange : IEnumerable<Segment>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentRange"/> class.
        /// </summary>
        /// <param name="part">The part range.</param>
        /// <param name="featureType">The feature type.</param>
        public SegmentRange(PartRange part, FeatureType featureType)
        {
            FeatureType = featureType;
            Part = part;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the feature type
        /// </summary>
        public FeatureType FeatureType { get; }

        /// <summary>
        /// Gets the part
        /// </summary>
        public PartRange Part { get; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public IEnumerator<Segment> GetEnumerator()
        {
            return new SegmentRangeEnumerator(this);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Classes

        /// <summary>
        /// Cycles through the points, creating segments. If the feature type is a polygon, then this will
        /// loop around again at the end of the part to create a segment from the first and last vertex.
        /// </summary>
        private class SegmentRangeEnumerator : IEnumerator<Segment>
        {
            #region Fields

            private readonly int _numVertices;
            private readonly SegmentRange _range;
            private readonly int _start;
            private readonly double[] _verts;
            private int _index;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="SegmentRangeEnumerator"/> class.
            /// </summary>
            /// <param name="parent">The segment range.</param>
            public SegmentRangeEnumerator(SegmentRange parent)
            {
                _range = parent;
                _start = _range.Part.StartIndex;
                _numVertices = _range.Part.NumVertices;
                _index = -1;
                _verts = parent.Part.Vertices;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets the current segment.
            /// </summary>
            public Segment Current { get; private set; }

            object IEnumerator.Current => Current;

            #endregion

            #region Methods

            /// <summary>
            /// Does nothing
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// Advances the enumerator to the next member
            /// </summary>
            /// <returns>True if a member is found, false if there are no more members</returns>
            public bool MoveNext()
            {
                _index++;
                if (_index == 0)
                {
                    Vertex p1 = new Vertex(_verts[_start * 2], _verts[(_start * 2) + 1]);
                    Vertex p2 = new Vertex(_verts[(_start * 2) + 2], _verts[(_start * 2) + 3]);
                    Current = new Segment(p1, p2);
                    return true;
                }

                if (_index == _numVertices - 1)
                {
                    // We have reached the last vertex, but if it is a polygon we wrap this around
                    if (_range.FeatureType != FeatureType.Polygon) return false;

                    Vertex p1 = Current.P2;
                    Vertex p2 = new Vertex(_verts[_start * 2], _verts[(_start * 2) + 1]);
                    Current = new Segment(p1, p2);
                    return true;
                }

                if (_index > 0 && _index < _numVertices - 1)
                {
                    Vertex p1 = Current.P2;
                    Vertex p2 = new Vertex(_verts[2 * (_start + _index + 1)], _verts[(2 * (_start + _index + 1)) + 1]);
                    Current = new Segment(p1, p2);
                    return true;
                }

                return false;
            }

            /// <inheritdoc />
            public void Reset()
            {
                Current = null;
                _index = -1;
            }

            #endregion
        }

        #endregion
    }
}