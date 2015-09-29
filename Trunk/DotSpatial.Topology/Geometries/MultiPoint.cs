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
using System.Collections.Generic;

namespace DotSpatial.Topology.Geometries
{
    /// <summary>
    /// Models a collection of <c>Point</c>s.
    /// </summary>
    [Serializable]
    public class MultiPoint : GeometryCollection, IMultiPoint
    {
        #region Fields

        /// <summary>
        /// Represents an empty <c>MultiPoint</c>.
        /// </summary>
        public static new readonly IMultiPoint Empty = new GeometryFactory().CreateMultiPoint(new IPoint[] { });

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a <c>MultiPoint</c>.
        /// </summary>
        /// <param name="points">
        /// The <c>Point</c>s for this <c>MultiPoint</c>
        ///, or <c>null</c> or an empty array to create the empty point.
        /// Elements may be empty <c>Point</c>s, but not <c>null</c>s.
        /// </param>
        /// <param name="factory"></param>
        public MultiPoint(IEnumerable<Coordinate> points, IGeometryFactory factory) : base(CastPoints(points), factory) { }

        /// <summary>
        /// This will attempt to create a new MultiPoint from the specified basic geometry.
        /// </summary>
        /// <param name="inBasicGeometry">A Point or MultiPoint</param>
        public MultiPoint(IGeometry inBasicGeometry)
            : base(inBasicGeometry, DefaultFactory)
        {
        }

        /// <summary>
        /// Creates new Multipoint using interface points
        /// </summary>
        /// <param name="points"></param>
        public MultiPoint(IEnumerable<Coordinate> points) : base(CastPoints(points), DefaultFactory) { }

        /// <summary>
        /// Creates new Multipoint using interface points
        /// </summary>
        /// <param name="points"></param>
        public MultiPoint(IEnumerable<ICoordinate> points) : base(CastPoints(points), DefaultFactory) { }
      
	    /// <summary>
        /// Constructs a <c>MultiPoint</c>.
        /// </summary>
        /// <param name="points">
        /// The <c>Point</c>s for this <c>MultiPoint</c>
        /// , or <c>null</c> or an empty array to create the empty point.
        /// Elements may be empty <c>Point</c>s, but not <c>null</c>s.
        /// </param>
        /// <param name="factory"></param>
        public MultiPoint(IEnumerable<IPoint> points, IGeometryFactory factory) : base(points, factory) { }

        /// <summary>
        /// Constructs a <c>MultiPoint</c>.
        /// </summary>
        /// <param name="points">
        /// The <c>Point</c>s for this <c>MultiPoint</c>
        /// , or <c>null</c> or an empty array to create the empty point.
        /// Elements may be empty <c>Point</c>s, but not <c>null</c>s.
        /// </param>
        /// <remarks>
        /// For create this <see cref="Geometry"/> is used a standard <see cref="GeometryFactory"/> 
        /// with <see cref="PrecisionModel" /> <c> == </c> <see cref="PrecisionModels.Floating"/>.
        /// </remarks>
        public MultiPoint(IEnumerable<IPoint> points) : this(points, DefaultFactory) { }
        #endregion

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public override IGeometry Boundary
        {
            get
            {
                return Factory.CreateGeometryCollection(null);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override Dimension BoundaryDimension
        {
            get
            {
                return Dimension.False;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override Dimension Dimension 
        {
            get
            {
                return Dimension.Point;
            }
        }

        /// <summary>
        /// Returns the name of this object's interface.
        /// </summary>
        /// <returns>"MultiPoint"</returns>
        public override string GeometryType
        {
            get
            {
                return "MultiPoint";
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return true;
            }
        }

        public override OgcGeometryType OgcGeometryType
        {
            get { return OgcGeometryType.MultiPoint; }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets or sets the point that resides at the specified index
        /// </summary>
        /// <param name="index">A zero-based integer index specifying the point to get or set</param>
        /// <returns></returns>
        public new IPoint this[int index]
        {
            get
            {
                return base[index] as IPoint;
            }
            set
            {
                base[index] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts an array of point interface variables into local points.
        /// Eventually I hope to reduce the amount of "casting" necessary, in order
        /// to allow as much as possible to occur via an interface.
        /// </summary>
        /// <param name="rawPoints"></param>
        /// <returns></returns>
        private static IEnumerable<IGeometry> CastPoints(IEnumerable<Coordinate> rawPoints)
        {
            List<IGeometry> result = new List<IGeometry>();
            foreach (Coordinate rawPoint in rawPoints)
            {
                result.Add(new Point(rawPoint));
            }
            return result;
        }

        /// <summary>
        /// Converts an array of point interface variables into local points.
        /// Eventually I hope to reduce the amount of "casting" necessary, in order
        /// to allow as much as possible to occur via an interface.
        /// </summary>
        /// <param name="rawPoints"></param>
        /// <returns></returns>
        private static IEnumerable<IGeometry> CastPoints(IEnumerable<ICoordinate> rawPoints)
        {
            List<IGeometry> result = new List<IGeometry>();
            foreach (ICoordinate rawPoint in rawPoints)
            {
                result.Add(new Point(rawPoint));
            }
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="other"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public override bool EqualsExact(IGeometry other, double tolerance)
        {
            if (!IsEquivalentClass(other))
                return false;
            return base.EqualsExact(other, tolerance);
        }

        /// <summary>
        /// Returns the <c>Coordinate</c> at the given position.
        /// </summary>
        /// <param name="n">The index of the <c>Coordinate</c> to retrieve, beginning at 0.
        /// </param>
        /// <returns>The <c>n</c>th <c>Coordinate</c>.</returns>
        protected Coordinate GetCoordinate(int n) 
        {
            return Geometries[n].Coordinate;
        }

        #endregion
    }
}