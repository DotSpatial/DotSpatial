using System;
using System.Collections.Generic;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Geometries.Implementation;
using DotSpatial.Topology.IO;

namespace DotSpatial.Topology.Noding
{
    ///<summary>
    /// Represents a list of contiguous line segments,
    /// and supports noding the segments.
    /// The line segments are represented by an array of <see cref="Coordinate" />s.
    /// Intended to optimize the noding of contiguous segments by
    /// reducing the number of allocated objects.
    /// SegmentStrings can carry a context object, which is useful
    /// for preserving topological or parentage information.
    /// All noded substrings are initialized with the same context object.
    ///</summary>
    public class BasicSegmentString : ISegmentString
    {
        #region Fields

        private readonly IList<Coordinate> _pts;

        #endregion

        #region Constructors

        ///<summary>
        /// Creates a new segment string from a list of vertices.
        ///</summary>
        ///<param name="pts">the vertices of the segment string</param>
        ///<param name="data">the user-defined data of this segment string (may be null)</param>
        public BasicSegmentString(IList<Coordinate> pts, object data)
        {
            _pts = pts;
            Context = data;
        }

        #endregion

        #region Properties

        public IList<Coordinate> Coordinates { get { return _pts; } }

        public int Count
        {
            get { return _pts.Count; }
        }

        public bool IsClosed
        {
            get { return _pts[0].Equals2D(_pts[_pts.Count]); }
        }

        ///<summary>Gets the user-defined data for this segment string.
        ///</summary>
        public object Context { get; set; }

        #endregion

        #region Indexers

        public LineSegment this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException("index", index, TopologyText.NodedSegmentString_WrongParameterSize);

                return new LineSegment(_pts[index], _pts[index + 1]);
            }
            set
            {
                throw new NotSupportedException(TopologyText.NodedSegmentString_UnsupportedSettingLinesegments);
            }
        }

        #endregion

        #region Methods

        ///<summary>
        /// Gets the octant of the segment starting at vertex <code>index</code>
        ///</summary>
        ///<param name="index">the index of the vertex starting the segment. Must not be the last index in the vertex list</param>
        ///<returns>octant of the segment at the vertex</returns>
        public OctantDirection GetSegmentOctant(int index)
        {
            return index == _pts.Count - 1 ? OctantDirection.Null : Octant.GetOctant(_pts[index], _pts[index + 1]);
        }

        public override string ToString()
        {
            return WKTWriter.ToLineString(new CoordinateArraySequence(_pts));
        }

        #endregion
    }
}