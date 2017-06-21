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
using System.Collections;

namespace DotSpatial.Topology
{
    /// <summary>
    /// Basic implementation of <c>MultiPolygon</c>.
    /// </summary>
    [Serializable]
    public class MultiPolygon : GeometryCollection, IMultiPolygon
    {
        /// <summary>
        /// Represents an empty <c>MultiPolygon</c>.
        /// </summary>
        public static new readonly IMultiPolygon Empty = new GeometryFactory().CreateMultiPolygon(null);

        /// <summary>
        /// Constructs a <c>MultiPolygon</c>.
        /// </summary>
        /// <param name="polygons">
        /// The <c>Polygon</c>s for this <c>MultiPolygon</c>
        ///, or <c>null</c> or an empty array to create the empty point.
        /// Elements may be empty <c>Polygon</c>s, but not <c>null</c>
        /// s. The polygons must conform to the assertions specified in the
        /// <see href="http://www.opengis.org/techno/specs.htm"/> OpenGIS Simple Features
        /// Specification for SQL.
        /// </param>
        /// <remarks>
        /// For create this <see cref="Geometry"/> is used a standard <see cref="GeometryFactory"/>
        /// with <see cref="PrecisionModel" /> <c> == </c> <see cref="PrecisionModelType.Floating"/>.
        /// </remarks>
        public MultiPolygon(Polygon[] polygons) : this(polygons, DefaultFactory) { }

        /// <summary>
        /// This was added by Ted Dunsford to allow the construction of MultiPolygons
        /// from an array of basic polygon interfaces.
        /// </summary>
        /// <param name="polygons"></param>
        public MultiPolygon(IBasicPolygon[] polygons) : base(polygons, DefaultFactory) { }

        /// <summary>
        /// This will attempt to create a new MultiPolygon from the specified basic geometry.
        /// </summary>
        /// <param name="inBasicGeometry">A Polygon or MultiPolygon</param>
        public MultiPolygon(IBasicGeometry inBasicGeometry)
            : base(inBasicGeometry, DefaultFactory)
        {
        }

        /// <summary>
        /// This will attempt to create a new MultiPolygon from the specified basic geometry.
        /// </summary>
        /// <param name="inBasicGeometry">A Polygon or MultiPolygon</param>
        /// <param name="inFactory">An implementation of the IGeometryFactory interface</param>
        public MultiPolygon(IBasicGeometry inBasicGeometry, IGeometryFactory inFactory)
            : base(inBasicGeometry, inFactory)
        {
        }

        /// <summary>
        /// Constructs a <c>MultiPolygon</c>.
        /// </summary>
        /// <param name="polygons">
        /// The <c>Polygon</c>s for this <c>MultiPolygon</c>
        ///, or <c>null</c> or an empty array to create the empty point.
        /// Elements may be empty <c>Polygon</c>s, but not <c>null</c>
        /// s. The polygons must conform to the assertions specified in the
        /// <see href="http://www.opengis.org/techno/specs.htm"/> OpenGIS Simple Features
        /// Specification for SQL.
        /// </param>
        /// <param name="factory"></param>
        public MultiPolygon(IPolygon[] polygons, IGeometryFactory factory) : base(polygons, factory) { }

        #region IMultiPolygon Members

        /// <summary>
        ///
        /// </summary>
        public override DimensionType Dimension
        {
            get
            {
                return DimensionType.Surface;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override DimensionType BoundaryDimension
        {
            get
            {
                return DimensionType.Curve;
            }
        }

        /// <summary>
        /// Always returns "MultiPolygon"
        /// </summary>
        public override string GeometryType
        {
            get
            {
                return "MultiPolygon";
            }
        }

        /// <summary>
        /// Always Polygon
        /// </summary>
        public override FeatureType FeatureType
        {
            get
            {
                return FeatureType.Polygon;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override bool IsSimple
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override IGeometry Boundary
        {
            get
            {
                if (IsEmpty) return Factory.CreateGeometryCollection(null);
                ArrayList allRings = new ArrayList();
                for (int i = 0; i < Geometries.Length; i++)
                {
                    Polygon polygon = (Polygon)Geometries[i];
                    Geometry rings = (Geometry)polygon.Boundary;
                    for (int j = 0; j < rings.NumGeometries; j++)
                        allRings.Add(rings.GetGeometryN(j));
                }
                return Factory.CreateMultiLineString((LineString[])allRings.ToArray(typeof(LineString)));
            }
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

        #endregion

        /// <summary>
        /// Presuming that the specified basic geometry describes a MultiPolygon, this will perform the necessary
        /// casting in order to create a MultiPolygon.  If, in fact, it is only a BasicMultiPolygon, this will
        /// create a new, fully functional MultiPolygon based on the same coordinates.
        /// </summary>
        /// <param name="inGeometry">The IBasicGeometry to turn into a MultiPolygon. </param>
        public static new IMultiPolygon FromBasicGeometry(IBasicGeometry inGeometry)
        {
            // Multipolygons cast directly
            IMultiPolygon result = inGeometry as IMultiPolygon;
            if (result != null) return result;

            // Polygons are just wrapped in a Multipolygon with the one polygon as an element
            IPolygon p = (IPolygon)inGeometry;
            if (p != null)
            {
                return new MultiPolygon(new[] { p });
            }

            IBasicPolygon bp = (IBasicPolygon)inGeometry;
            if (bp != null)
            {
                return new MultiPolygon(new[] { bp });
            }

            IPolygon[] polygonArray = new IPolygon[inGeometry.NumGeometries];

            // assume that we have some kind of MultiGeometry of IBasicPolygon objects
            for (int i = 0; i < inGeometry.NumGeometries; i++)
            {
                IBasicPolygon ibp = (IBasicPolygon)inGeometry.GetBasicGeometryN(i);
                polygonArray[i] = new Polygon(ibp);
            }

            return new MultiPolygon(polygonArray);
        }
    }
}