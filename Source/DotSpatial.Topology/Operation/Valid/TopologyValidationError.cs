// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System;

namespace DotSpatial.Topology.Operation.Valid
{
    /// <summary>
    /// Contains information about the nature and location of a <c>Geometry</c>
    /// validation error.
    /// </summary>
    public class TopologyValidationError
    {
        // Notice: modified for "safe" assembly in Sql 2005
        // Added readonly!

        /// <summary>
        /// These messages must synch up with the indexes above
        /// </summary>
        private static readonly string[] ErrMsg =
        {
            "Topology Validation Error",
            "Repeated Point",
            "Hole lies outside shell",
            "Holes are nested",
            "Interior is disconnected",
            "Self-intersection",
            "Ring Self-intersection",
            "Nested shells",
            "Duplicate Rings",
            "Too few points in geometry component",
            "Invalid Coordinate"
        };

        private readonly TopologyValidationErrorType _errorType;
        private readonly Coordinate _pt;

        /// <summary>
        ///
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="pt"></param>
        public TopologyValidationError(TopologyValidationErrorType errorType, Coordinate pt)
        {
            _errorType = errorType;
            if (pt != null)
                _pt = (Coordinate)pt.Clone();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="errorType"></param>
        public TopologyValidationError(TopologyValidationErrorType errorType) : this(errorType, null) { }

        /// <summary>
        ///
        /// </summary>
        public virtual Coordinate Coordinate
        {
            get
            {
                return _pt;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual TopologyValidationErrorType ErrorType
        {
            get
            {
                return _errorType;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual String Message
        {
            get
            {
                return ErrMsg[(int)_errorType];
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Message + " at or near point " + _pt;
        }
    }

    /// <summary>
    /// Contains information about the nature and location of
    /// a <see cref="Geometry" /> validation error.
    /// </summary>
    public enum TopologyValidationErrorType
    {
        /// <summary>
        /// Not used.
        /// </summary>
        [Obsolete("Not used")]
        Error = 0,

        /// <summary>
        /// No longer used:
        /// repeated points are considered valid as per the SFS.
        /// </summary>
        [Obsolete("No longer used: repeated points are considered valid as per the SFS")]
        RepeatedPoint = 1,

        /// <summary>
        /// Indicates that a hole of a polygon lies partially
        /// or completely in the exterior of the shell.
        /// </summary>
        HoleOutsideShell = 2,

        /// <summary>
        /// Indicates that a hole lies
        /// in the interior of another hole in the same polygon.
        /// </summary>
        NestedHoles = 3,

        /// <summary>
        /// Indicates that the interior of a polygon is disjoint
        /// (often caused by set of contiguous holes splitting
        /// the polygon into two parts).
        /// </summary>
        DisconnectedInteriors = 4,

        /// <summary>
        /// Indicates that two rings of a polygonal geometry intersect.
        /// </summary>
        SelfIntersection = 5,

        /// <summary>
        /// Indicates that a ring self-intersects.
        /// </summary>
        RingSelfIntersection = 6,

        /// <summary>
        /// Indicates that a polygon component of a
        /// <see cref="MultiPolygon" /> lies inside another polygonal component.
        /// </summary>
        NestedShells = 7,

        /// <summary>
        /// Indicates that a polygonal geometry
        /// contains two rings which are identical.
        /// </summary>
        DuplicateRings = 8,

        /// <summary>
        /// Indicates that either:
        /// - A <see cref="LineString" /> contains a single point.
        /// - A <see cref="LinearRing" /> contains 2 or 3 points.
        /// </summary>
        TooFewPoints = 9,

        /// <summary>
        /// Indicates that the <c>X</c> or <c>Y</c> ordinate of
        /// a <see cref="Coordinate" /> is not a valid
        /// numeric value (e.g. <see cref="Double.NaN" />).
        /// </summary>
        InvalidCoordinate = 10,

        /// <summary>
        /// Indicates that a ring is not correctly closed
        /// (the first and the last coordinate are different).
        /// </summary>
        RingNotClosed = 11,
    }
}