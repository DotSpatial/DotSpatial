using System;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Triangulate.QuadEdge
{
    public class LocateFailureException : Exception
    {
        #region Constructors

        public LocateFailureException(String msg)
            :base(msg)
        {
        }

        public LocateFailureException(String msg, LineSegment seg)
            :base(MsgWithSpatial(msg, seg))
        {
            this.Segment = new LineSegment(seg);
        }

        public LocateFailureException(LineSegment seg)
            :base("Locate failed to converge (at edge: "
                + seg
                + ").  Possible causes include invalid Subdivision topology or very close sites")
        {
            this.Segment = new LineSegment(seg);
        }

        #endregion

        #region Properties

        public LineSegment Segment { get; private set; }

        #endregion

        #region Methods

        private static String MsgWithSpatial(String msg, LineSegment seg)
        {
            if (seg != null)
                return msg + " [ " + seg + " ]";
            return msg;
        }

        #endregion
    }
}