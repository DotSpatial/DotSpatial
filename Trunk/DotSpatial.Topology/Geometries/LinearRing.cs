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
// *********************************************************************************************************

using System;
using System.Collections.Generic;
using DotSpatial.Serialization;
using DotSpatial.Topology.Algorithm;

namespace DotSpatial.Topology.Geometries
{
    /// <summary>
    /// Models an OGC SFS <c>LinearRing</c>.
    /// </summary>
    /// <remarks>
    /// A <c>LinearRing</c> is a <see cref="LineString"/> which is both closed and simple.
    /// In other words,
    /// the first and last coordinate in the ring must be equal,
    /// and the interior of the ring must not self-intersect.
    /// Either orientation of the ring is allowed.
    /// <para>
    /// A ring must have either 0 or 4 or more points.
    /// The first and last points must be equal (in 2D).
    /// If these conditions are not met, the constructors throw
    /// an <see cref="ArgumentException"/></para>
    /// </remarks>
    [Serializable]
    public class LinearRing : LineString, ILinearRing
    {
        #region Constant Fields

        /// <summary>
        /// The minimum number of vertices allowed in a valid non-empty ring (= 4).
        /// Empty rings with 0 vertices are also valid.
        /// </summary>
        public const int MinimumValidSize = 4;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a <c>LinearRing</c> with the given points.
        /// </summary>
        /// <param name="points">
        /// Points forming a closed and simple linestring, or
        /// <c>null</c> or an empty array to create the empty point.
        /// This array must not contain <c>null</c> elements.
        /// </param>
        /// <param name="factory"></param>
        /// <exception cref="ArgumentException">If the ring is not closed, or has too few points</exception>
        public LinearRing(ICoordinateSequence points, IGeometryFactory factory)
            : base(points, factory)
        {
            ValidateConstruction();
        }

        /// <summary>
        /// Creates a new instance of a linear ring where the enumerable collection of
        /// coordinates represents the set of coordinates to add to the ring.
        /// </summary>
        /// <param name="coordinates"></param>
        public LinearRing(IEnumerable<Coordinate> coordinates)
            : base(coordinates)
        {
            ValidateConstruction();
        }

        /// <summary>
        /// Creates a new instance of a linear ring where the enumerable collection of
        /// coordinates represents the set of coordinates to add to the ring.
        /// </summary>
        /// <param name="coordinates"></param>
        public LinearRing(IEnumerable<ICoordinate> coordinates)
            : base(coordinates)
        {
            ValidateConstruction();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="linestringbase"></param>
        public LinearRing(IBasicLineString linestringbase)
            : base(linestringbase)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns <c>Dimensions.False</c>, since by definition LinearRings do not have a boundary.
        /// </summary>
        public override Dimension BoundaryDimension
        {
            get
            {
                return Dimension.False;
            }
        }

        /// <summary>
        /// This will always contain Line, even if it is technically empty
        /// </summary>
        public override FeatureType FeatureType
        {
            get
            {
                return FeatureType.Line;
            }
        }

        /// <summary>
        /// Returns the name of this object's interface.
        /// </summary>
        /// <returns>"LinearRing"</returns>
        public override string GeometryType
        {
            get { return "LinearRing"; }
        }

        public bool IsCounterClockwise { get { return CGAlgorithms.IsCounterClockwise(CoordinateSequence); } }

        /// <summary>
        /// Gets a boolean that is true if the EndPoint is geometrically equal to the StartPoint in 2 Dimensions.
        /// </summary>
        public override bool IsClosed
        {
            get
            {
                if (IsEmpty)
                {
                    // empty LinearRings are closed by definition
                    return true;
                }
                return base.IsClosed;
            }
        }

        #endregion

        #region Methods

        public override IGeometry Reverse()
        {
            var sequence = CoordinateSequence.Reversed();
            return Factory.CreateLinearRing(sequence);
        }

        /// <summary>
        /// Correct constructions with non-closed sequences.
        /// </summary>
        private void ValidateConstruction()
        {
            if (!IsEmpty && !base.IsClosed)
            {
                // The sequence is not closed, so add the first point again to close it.
                Coordinates.Add(Coordinates[0].Copy());
            } //TODO ich fürchte Coordinates entspricht nicht mehr dem was es vorher war und die hinzugefügte Coordinate wird ignoriert.
            if (CoordinateSequence.Count >= 1 && CoordinateSequence.Count < MinimumValidSize)
                throw new ArgumentException("Number of points must be 0 or >3");
        }

        #endregion
    }
}