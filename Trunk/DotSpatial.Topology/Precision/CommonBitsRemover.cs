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

using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Precision
{
    /// <summary>
    /// Removes common most-significant mantissa bits
    /// from one or more <see cref="IGeometry"/>s.
    /// <para/>
    /// The CommonBitsRemover "scavenges" precision
    /// which is "wasted" by a large displacement of the geometry
    /// from the origin.
    /// For example, if a small geometry is displaced from the origin
    /// by a large distance,
    /// the displacement increases the significant figures in the coordinates,
    /// but does not affect the <i>relative</i> topology of the geometry.
    /// Thus the geometry can be translated back to the origin
    /// without affecting its topology.
    /// In order to compute the translation without affecting
    /// the full precision of the coordinate values,
    /// the translation is performed at the bit level by
    /// removing the common leading mantissa bits.
    /// <para/>
    /// If the geometry envelope already contains the origin,
    /// the translation procedure cannot be applied.
    /// In this case, the common bits value is computed as zero.
    /// <para/>
    /// If the geometry crosses the Y axis but not the X axis
    /// (and <i>mutatis mutandum</i>),
    /// the common bits for Y are zero,
    /// but the common bits for X are non-zero.
    /// </summary>
    public class CommonBitsRemover
    {
        #region Fields

        private readonly CommonCoordinateFilter _ccFilter = new CommonCoordinateFilter();
        private Coordinate _commonCoord;

        #endregion

        #region Properties

        /// <summary>
        /// The common bits of the Coordinates in the supplied Geometries.
        /// </summary>
        public Coordinate CommonCoordinate
        {
            get { return _commonCoord; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a point to the set of geometries whose common bits are
        /// being computed.  After this method has executed the
        /// common coordinate reflects the common bits of all added
        /// geometries.
        /// </summary>
        /// <param name="geom">A Geometry to test for common bits.</param>
        public void Add(IGeometry geom)
        {
            geom.Apply(_ccFilter);
            _commonCoord = _ccFilter.CommonCoordinate;
        }

        /// <summary>
        /// Adds the common coordinate bits back into a Geometry.
        /// The coordinates of the Geometry are changed.
        /// </summary>
        /// <param name="geom">The Geometry to which to add the common coordinate bits.</param>
        public void AddCommonBits(IGeometry geom)
        {
            var trans = new Translater(_commonCoord);
            geom.Apply(trans);
            geom.GeometryChanged();
        }

        /// <summary>
        /// Removes the common coordinate bits from a Geometry.
        /// The coordinates of the Geometry are changed.
        /// </summary>
        /// <param name="geom">The Geometry from which to remove the common coordinate bits.</param>
        /// <returns>The shifted Geometry.</returns>
        public IGeometry RemoveCommonBits(IGeometry geom)
        {
            if (_commonCoord.X == 0.0 && _commonCoord.Y == 0.0)
                return geom;
            Coordinate invCoord = new Coordinate(_commonCoord);
            invCoord.X = -invCoord.X;
            invCoord.Y = -invCoord.Y;
            Translater trans = new Translater(invCoord);
            geom.Apply(trans);
            geom.GeometryChanged();
            return geom;
        }

        #endregion

        #region Classes

        /// <summary>
        ///
        /// </summary>
        public class CommonCoordinateFilter : ICoordinateFilter
        {
            #region Fields

            private readonly CommonBits _commonBitsX = new CommonBits();
            private readonly CommonBits _commonBitsY = new CommonBits();

            #endregion

            #region Properties

            /// <summary>
            ///
            /// </summary>
            public Coordinate CommonCoordinate
            {
                get
                {
                    return new Coordinate(_commonBitsX.Common, _commonBitsY.Common);
                }
            }

            #endregion

            #region Methods

            /// <summary>
            ///
            /// </summary>
            /// <param name="coord"></param>
            public void Filter(Coordinate coord)
            {
                _commonBitsX.Add(coord.X);
                _commonBitsY.Add(coord.Y);
            }

            #endregion
        }

        /// <summary>
        ///
        /// </summary>
        private class Translater : ICoordinateFilter
        {
            #region Fields

            private readonly Coordinate _trans;

            #endregion

            #region Constructors

            /// <summary>
            ///
            /// </summary>
            /// <param name="trans"></param>
            public Translater(Coordinate trans)
            {
                _trans = trans;
            }

            #endregion

            #region Methods

            /// <summary>
            ///
            /// </summary>
            /// <param name="coord"></param>
            public void Filter(Coordinate coord)
            {
                coord.X += _trans.X;
                coord.Y += _trans.Y;
            }

            #endregion
        }

        #endregion
    }
}