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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/1/2010 2:12:57 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// SegmentSet
    /// </summary>
    public class SegmentRange : IEnumerable<Segment>
    {
        #region IEnumerable<Segment> Members

        /// <inheritdoc />
        public IEnumerator<Segment> GetEnumerator()
        {
            return new SegmentRangeEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Nested type: SegmentRangeEnumerator

        /// <summary>
        /// Cycles through the points, creating segments.  If the feature type is a polygon, then this will
        /// loop around again at the end of the part to create a segment from the first and last vertex.
        /// </summary>
        public class SegmentRangeEnumerator : IEnumerator<Segment>
        {
            private readonly int _numVertices;
            private readonly SegmentRange _range;
            private readonly int _start;
            private readonly double[] _verts;
            private Segment _current;
            private int _index;

            /// <summary>
            /// Creates a new enumerator given the SegmentRange
            /// </summary>
            /// <param name="parent"></param>
            public SegmentRangeEnumerator(SegmentRange parent)
            {
                _range = parent;
                _start = _range.Part.StartIndex;
                _numVertices = _range.Part.NumVertices;
                _index = -1;
                _verts = parent.Part.Vertices;
            }

            #region IEnumerator<Segment> Members

            /// <summary>
            /// Gets the current segment
            /// </summary>
            public Segment Current
            {
                get { return _current; }
            }

            object IEnumerator.Current
            {
                get { return _current; }
            }

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
                    Vertex p1 = new Vertex(_verts[_start * 2], _verts[_start * 2 + 1]);
                    Vertex p2 = new Vertex(_verts[_start * 2 + 2], _verts[_start * 2 + 3]);
                    _current = new Segment(p1, p2);
                    return true;
                }
                if (_index == _numVertices - 1)
                {
                    // We have reached the last vertex, but if it is a polygon we wrap this around
                    if (_range.FeatureType != FeatureType.Polygon) return false;
                    Vertex p1 = _current.P2;
                    Vertex p2 = new Vertex(_verts[_start * 2], _verts[_start * 2 + 1]);
                    _current = new Segment(p1, p2);
                    return true;
                }
                if (_index > 0 && _index < _numVertices - 1)
                {
                    Vertex p1 = _current.P2;
                    Vertex p2 = new Vertex(_verts[2 * (_start + _index + 1)], _verts[2 * (_start + _index + 1) + 1]);
                    _current = new Segment(p1, p2);
                    return true;
                }
                return false;
            }

            /// <inheritdoc />
            public void Reset()
            {
                _current = null;
                _index = -1;
            }

            #endregion
        }

        #endregion

        #region Private Variables

        private readonly FeatureType _featureType;
        private readonly PartRange _part;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SegmentSet
        /// </summary>
        public SegmentRange(PartRange part, FeatureType featureType)
        {
            _featureType = featureType;
            _part = part;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets the feature type
        /// </summary>
        public FeatureType FeatureType
        {
            get { return _featureType; }
        }

        /// <summary>
        /// Gets the part
        /// </summary>
        public PartRange Part
        {
            get { return _part; }
        }

        #endregion
    }
}