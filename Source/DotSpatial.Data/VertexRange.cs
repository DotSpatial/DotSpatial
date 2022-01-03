// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// VertexRange.
    /// </summary>
    public class VertexRange : IEnumerable<Vertex>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexRange"/> class that can have the vertices and number of vertices assigned later.
        /// </summary>
        public VertexRange()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexRange"/> class.
        /// </summary>
        /// <param name="allVertices">An array of all the vertex locations.</param>
        /// <param name="shapeOffset">The shape offset.</param>
        /// <param name="partOffset">The part offset.</param>
        public VertexRange(double[] allVertices, int shapeOffset, int partOffset)
        {
            ShapeOffset = shapeOffset;
            PartOffset = partOffset;
            Vertices = allVertices;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the integer index of the last vertex included in this range.
        /// </summary>
        public int EndIndex => StartIndex + NumVertices - 1;

        /// <summary>
        /// Gets or sets the number of vertices. This will also set the EndIndex relative to the start position.
        /// </summary>
        public int NumVertices { get; set; }

        /// <summary>
        /// Gets or sets the part offset. For parts, controlling the part offset is perhaps more useful that controlling the shape offset.
        /// </summary>
        public int PartOffset { get; set; }

        /// <summary>
        /// Gets or sets the shape offset. The StartIndex is the sum of the shape offset and the part offset. Controlling them separately
        /// allows the entire shape offset to be adjusted independently after the part is created.
        /// </summary>
        public int ShapeOffset { get; set; }

        /// <summary>
        /// Gets the index of the first vertex included in this range. This is overridden
        /// in the case of part ranges to also take into account the shape start index.
        /// </summary>
        public int StartIndex => ShapeOffset + PartOffset;

        /// <summary>
        /// Gets or sets the vertices.
        /// </summary>
        public double[] Vertices { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets an enumerator. This exists to make it easier to cycle values,
        /// but in truth should not be used because it re-adds the property accessor
        /// which slows down the data access, which is the whole point of putting
        /// all the vertices into a jagged array of doubles in the first place.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<Vertex> GetEnumerator()
        {
            return new VertexRangeEnumerator(Vertices, StartIndex, EndIndex);
        }

        /// <summary>
        /// Gets an enumerator. This exists to make it easier to cycle values,
        /// but in truth should not be used because it re-adds the property accessor
        /// which slows down the data access, which is the whole point of putting
        /// all the vertices into a jagged array of doubles in the first place.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Classes

        /// <summary>
        /// The enumerator is here to provide an easy method for cycling vertex values
        /// in each range. This sort of defeats the point because it adds
        /// two method calls for advancing each step (one to MoveNext and one to
        /// access the property. The whole point of loading everything
        /// into a single array of vertices in the first place is to avoid
        /// property accessors slowing down the process. However, it's here
        /// if someone wants it.
        /// </summary>
        private class VertexRangeEnumerator : IEnumerator<Vertex>
        {
            #region Fields

            private readonly int _end;
            private readonly int _start;
            private readonly double[] _vertices;
            private int _index;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="VertexRangeEnumerator"/> class.
            /// </summary>
            /// <param name="vertices">The vertices to create.</param>
            /// <param name="start">The integer index of the first included vertex. </param>
            /// <param name="end">The integer index of the last included vertex.</param>
            public VertexRangeEnumerator(double[] vertices, int start, int end)
            {
                _start = start;
                _end = end;
                _index = start - 1;
                _vertices = vertices;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets the current value.
            /// </summary>
            public Vertex Current { get; private set; }

            object IEnumerator.Current => Current;

            #endregion

            #region Methods

            /// <summary>
            /// This does nothing.
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// Advances the enumerator to the next position.
            /// </summary>
            /// <returns>True if it was moved.</returns>
            public bool MoveNext()
            {
                _index++;
                if (_index > _end) return false;

                Current = new Vertex(_vertices[_index * 2], _vertices[(_index * 2) + 1]);
                return true;
            }

            /// <summary>
            /// Resets this enumerator to the beginning of the range of vertices.
            /// </summary>
            public void Reset()
            {
                _index = _start - 1;
            }

            #endregion
        }

        #endregion

        // Internally keep track of the vertices array.
    }
}