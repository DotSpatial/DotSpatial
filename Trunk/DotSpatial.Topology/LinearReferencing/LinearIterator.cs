//using System.Collections;

using System;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.LinearReferencing
{
    /// <summary>
    /// An iterator over the components and coordinates of a linear geometry
    /// (<see cref="LineString" />s and <see cref="MultiLineString" />s.
    /// </summary>
    public class LinearIterator //: IEnumerator<LinearIterator.LinearElement>,
    //    IEnumerable<LinearIterator.LinearElement>
    {
        #region Fields

        private readonly IGeometry _linearGeom;
        private readonly int _numLines;
        private int _componentIndex;

        /// <summary>
        /// Invariant: currentLine &lt;&gt; null if the iterator is pointing at a valid coordinate
        /// </summary>
        private ILineString _currentLine;

        private int _vertexIndex;

        #endregion

        #region Constructors

        //// Used for avoid the first call to Next() in MoveNext()
        //private bool _atStart;

        //// Returned by Ienumerator.Current
        //private LinearElement _current;

        //// Cached start values - for Reset() call
        //private readonly int _startComponentIndex;
        //private readonly int _startVertexIndex;

        /// <summary>
        /// Creates an iterator initialized to the start of a linear <see cref="Geometry" />.
        /// </summary>
        /// <param name="linearGeom">The linear geometry to iterate over.</param>
        /// <exception cref="ArgumentException"> if <paramref name="linearGeom"/> is not <see cref="ILineal"/></exception>
        public LinearIterator(IGeometry linearGeom) : this(linearGeom, 0, 0) { }

        /// <summary>
        /// Creates an iterator starting at a <see cref="LinearLocation" /> on a linear <see cref="Geometry" />.
        /// </summary>
        /// <param name="linearGeom">The linear geometry to iterate over.</param>
        /// <param name="start">The location to start at.</param>
        /// <exception cref="ArgumentException"> if <paramref name="linearGeom"/> is not <see cref="ILineal"/></exception>
        public LinearIterator(IGeometry linearGeom, LinearLocation start) :
            this(linearGeom, start.ComponentIndex, SegmentEndVertexIndex(start)) { }

        /// <summary>
        /// Creates an iterator starting at
        /// a component and vertex in a linear <see cref="Geometry" />.
        /// </summary>
        /// <param name="linearGeom">The linear geometry to iterate over.</param>
        /// <param name="componentIndex">The component to start at.</param>
        /// <param name="vertexIndex">The vertex to start at.</param>
        /// <exception cref="ArgumentException"> if <paramref name="linearGeom"/> is not <see cref="ILineal"/></exception>
        public LinearIterator(IGeometry linearGeom, int componentIndex, int vertexIndex)
        {
            if (!(linearGeom is ILineal))
                throw new ArgumentException("Lineal geometry is required.");
            _linearGeom = linearGeom;
            _numLines = linearGeom.NumGeometries;

            _componentIndex = componentIndex;
            _vertexIndex = vertexIndex;

            LoadCurrentLine();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The component index of the vertex the iterator is currently at.
        /// </summary>
        public int ComponentIndex
        {
            get
            {
                return _componentIndex;
            }
        }

        /// <summary>
        /// Checks whether the iterator cursor is pointing to the
        /// endpoint of a component <see cref="ILineString"/>.
        /// </summary>
        public bool IsEndOfLine
        {
            get
            {
                if (_componentIndex >= _numLines)
                    return false;
                if (_vertexIndex < _currentLine.NumPoints - 1)
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Gets the <see cref="LineString" /> component the iterator is current at.
        /// </summary>
        public ILineString Line
        {
            get
            {
                return _currentLine;
            }
        }

        /// <summary>
        /// Gets the second <see cref="Coordinate" /> of the current segment
        /// (the coordinate of the next vertex).
        /// If the iterator is at the end of a line, <c>null</c> is returned.
        /// </summary>
        public Coordinate SegmentEnd
        {
            get
            {
                if (_vertexIndex < Line.NumPoints - 1)
                    return _currentLine.GetCoordinateN(_vertexIndex + 1);
                return null;
            }
        }

        /// <summary>
        /// Gets the first <see cref="Coordinate" /> of the current segment
        /// (the coordinate of the current vertex).
        /// </summary>
        public Coordinate SegmentStart
        {
            get
            {
                return _currentLine.GetCoordinateN(_vertexIndex);
            }
        }

        /// <summary>
        /// The vertex index of the vertex the iterator is currently at.
        /// </summary>
        public int VertexIndex
        {
            get
            {
                return _vertexIndex;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tests whether there are any vertices left to iterator over.
        /// Specifically, <c>HasNext()</c> returns <tt>true</tt> if the
        /// current state of the iterator represents a valid location
        /// on the linear geometry. 
        /// </summary>
        /// <returns><c>true</c> if there are more vertices to scan.</returns>
        public bool HasNext()
        {
            if (_componentIndex >= _numLines)
                return false;
            if (_componentIndex == _numLines - 1 &&
                _vertexIndex >= _currentLine.NumPoints)
                return false;
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        private void LoadCurrentLine()
        {
            if (_componentIndex >= _numLines)
            {
                _currentLine = null;
                return;
            }
            _currentLine = (ILineString)_linearGeom.GetGeometryN(_componentIndex);
        }

        /// <summary>
        /// Jump to the next element of the iteration.
        /// </summary>
        public void Next()
        {
            if (!HasNext())
                return;

            _vertexIndex++;
            if (_vertexIndex >= _currentLine.NumPoints)
            {
                _componentIndex++;
                LoadCurrentLine();
                _vertexIndex = 0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public static int SegmentEndVertexIndex(LinearLocation loc)
        {
            if (loc.SegmentFraction > 0.0)
                return loc.SegmentIndex + 1;
            return loc.SegmentIndex;
        }

        #endregion
    }
}