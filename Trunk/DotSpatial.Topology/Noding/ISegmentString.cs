using System.Collections.Generic;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Noding
{
    ///<summary>
    /// An interface for classes which represent a sequence of contiguous line segments.
    /// SegmentStrings can carry a context object, which is useful
    /// for preserving topological or parentage information.
    ///</summary>
    public interface ISegmentString
    {
        #region Properties

        ///<summary>
        /// Gets/Sets the user-defined data for this segment string.
        ///</summary>
        object Context { get; set; }

        ///<summary>
        /// Points that make up ISegmentString
        ///</summary>
        IList<Coordinate> Coordinates { get; }

        ///<summary>
        /// Size of Coordinate Sequence
        ///</summary>
        int Count { get; }

        /// <summary>
        /// States whether ISegmentString is closed
        /// </summary>
        bool IsClosed { get; }

        #endregion

        #region Indexers

        LineSegment this[int index] { get; set; }

        #endregion
    }
}