using System;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.IO;

namespace DotSpatial.Topology.Triangulate
{
    /// <summary>
    /// Indicates a failure during constraint enforcement.
    /// </summary>
    /// <author>Martin Davis</author>
    /// <version>1.0</version>
    public class ConstraintEnforcementException : Exception
    {
        #region Fields

        private readonly Coordinate _pt;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance with a given message.
        /// </summary>
        /// <param name="msg">a string</param>
        public ConstraintEnforcementException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Creates a new instance with a given message and approximate location.
        /// </summary>
        /// <param name="msg">a string</param>
        /// <param name="pt">the location of the error</param>
        public ConstraintEnforcementException(String msg, Coordinate pt)
            : base(MsgWithCoord(msg, pt))
        {
            _pt = new Coordinate(pt);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the approximate location of this error.
        /// </summary>
        /// <remarks>a location</remarks>
        public Coordinate Coordinate
        {
            get
            {
                return _pt;
            }
        }

        #endregion

        #region Methods

        private static String MsgWithCoord(String msg, Coordinate pt) {
            if (pt != null)
                return msg + " [ " + WKTWriter.ToPoint(pt) + " ]";
            return msg;
        }

        #endregion
    }
}