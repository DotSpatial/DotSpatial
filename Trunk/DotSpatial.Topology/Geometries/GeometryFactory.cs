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
using DotSpatial.Topology.Geometries.Implementation;
using DotSpatial.Topology.Geometries.Utilities;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Geometries
{
    /// <summary>
    /// Supplies a set of utility methods for building Geometry objects 
    /// from lists of Coordinates.
    /// </summary>            
    /// <remarks>
    /// Note that the factory constructor methods do <b>not</b> change the input coordinates in any way.
    /// In particular, they are not rounded to the supplied <c>PrecisionModel</c>.
    /// It is assumed that input Coordinates meet the given precision.
    /// </remarks>
    [Serializable]
    public class GeometryFactory : IGeometryFactory
    {
        #region Fields

        /// <summary>
        /// A predefined <see cref="GeometryFactory" /> with <see cref="PrecisionModel" /> 
        /// <c> == </c> <see cref="PrecisionModels.Floating" />.
        /// </summary>
        public static readonly IGeometryFactory Default = new GeometryFactory();

        /// <summary>
        /// A predefined <see cref="GeometryFactory" /> with <see cref="PrecisionModel" /> 
        /// <c> == </c> <see cref="PrecisionModels.Fixed" />.
        /// </summary>
        public static readonly IGeometryFactory Fixed = new GeometryFactory(new PrecisionModel(PrecisionModels.Fixed));

        /// <summary>
        /// A predefined <see cref="GeometryFactory" /> with <see cref="PrecisionModel" /> 
        /// <c> == </c> <see cref="PrecisionModels.Floating" />.
        /// </summary>
        /// <remarks>A shortcut for <see cref="GeometryFactory.Default" />.</remarks>
        public static readonly IGeometryFactory Floating = Default;

        /// <summary>
        /// A predefined <see cref="GeometryFactory" /> with <see cref="PrecisionModel" /> 
        /// <c> == </c> <see cref="PrecisionModels.FloatingSingle" />.
        /// </summary>
        public static readonly IGeometryFactory FloatingSingle = new GeometryFactory(new PrecisionModel(PrecisionModels.FloatingSingle));

        private readonly ICoordinateSequenceFactory _coordinateSequenceFactory;
        private readonly IPrecisionModel _precisionModel;
        private readonly int _srid;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a GeometryFactory that generates Geometries having the given
        /// PrecisionModel, spatial-reference ID, and CoordinateSequence implementation.
        /// </summary>        
        public GeometryFactory(IPrecisionModel precisionModel, int srid, ICoordinateSequenceFactory coordinateSequenceFactory)
        {
            _precisionModel = precisionModel;
            _coordinateSequenceFactory = coordinateSequenceFactory;
            _srid = srid;
        }

        /// <summary>
        /// Constructs a GeometryFactory object from any valid IGeometryFactory interface
        /// </summary>
        /// <param name="gf"></param>
        public GeometryFactory(IGeometryFactory gf)
        {
            _precisionModel = new PrecisionModel(gf.PrecisionModel);
            _coordinateSequenceFactory = GetDefaultCoordinateSequenceFactory();
            _srid = gf.SRID;
        }

        /// <summary>
        /// Constructs a GeometryFactory pertaining to a specific _coordinateSequenceFactory
        /// using any valid IGeometryFactory and ICoordinateSequenceFactory interface
        /// </summary>
        /// <param name="gf">An IGeometryFactory Interface</param>
        /// <param name="coordinateSequenceFactory">An ICoordianteSequenceFactory interface</param>
        public GeometryFactory(IGeometryFactory gf, ICoordinateSequenceFactory coordinateSequenceFactory)
        {
            _precisionModel = new PrecisionModel(gf.PrecisionModel);
            _coordinateSequenceFactory = coordinateSequenceFactory;
            _srid = gf.SRID;
        }

        /// <summary>
        /// Constructs a GeometryFactory that generates Geometries having the given
        /// CoordinateSequence implementation, a double-precision floating PrecisionModel and a
        /// spatial-reference ID of 0.
        /// </summary>
        public GeometryFactory(ICoordinateSequenceFactory coordinateSequenceFactory) :
            this(new PrecisionModel(), 0, coordinateSequenceFactory) { }

        /// <summary>
        /// Constructs a GeometryFactory that generates Geometries having the given
        /// {PrecisionModel} and the default CoordinateSequence
        /// implementation.
        /// </summary>
        /// <param name="precisionModel">The PrecisionModel to use.</param>
        public GeometryFactory(IPrecisionModel precisionModel) :
            this(precisionModel, 0, GetDefaultCoordinateSequenceFactory()) { }

        /// <summary>
        /// Constructs a GeometryFactory that generates Geometries having the given
        /// <c>PrecisionModel</c> and spatial-reference ID, and the default CoordinateSequence
        /// implementation.
        /// </summary>
        /// <param name="precisionModel">The PrecisionModel to use.</param>
        /// <param name="srid">The SRID to use.</param>
        public GeometryFactory(IPrecisionModel precisionModel, int srid) :
            this(precisionModel, srid, GetDefaultCoordinateSequenceFactory()) { }

        /// <summary>
        /// Constructs a GeometryFactory that generates Geometries having a floating
        /// PrecisionModel and a spatial-reference ID of 0.
        /// </summary>
        public GeometryFactory() : this(new PrecisionModel(), 0) { }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public ICoordinateSequenceFactory CoordinateSequenceFactory
        {
            get { return _coordinateSequenceFactory; }
        }

        /// <summary>
        /// Returns the PrecisionModel that Geometries created by this factory
        /// will be associated with.
        /// </summary>
        public IPrecisionModel PrecisionModel
        {
            get { return _precisionModel; }
        }

        /// <summary>
        /// The SRID value defined for this factory.
        /// </summary>
        public int SRID
        {
            get { return _srid; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Build an appropriate <c>Geometry</c>, <c>MultiGeometry</c>, or
        /// <c>GeometryCollection</c> to contain the <c>Geometry</c>s in
        /// it.
        /// </summary>
        /// <remarks>
        ///  If <c>geomList</c> contains a single <c>Polygon</c>,
        /// the <c>Polygon</c> is returned.<br/>
        ///  If <c>geomList</c> contains several <c>Polygon</c>s, a
        /// <c>MultiPolygon</c> is returned.<br/>
        ///  If <c>geomList</c> contains some <c>Polygon</c>s and
        /// some <c>LineString</c>s, a <c>GeometryCollection</c> is
        /// returned.<br/>
        ///  If <c>geomList</c> is empty, an empty <c>GeometryCollection</c>
        /// is returned.
        /// Note that this method does not "flatten" Geometries in the input, and hence if
        /// any MultiGeometries are contained in the input a GeometryCollection containing
        /// them will be returned.
        /// </remarks>
        /// <param name="geomList">The <c>Geometry</c> to combine.</param>
        /// <returns>
        /// A <see cref="IGeometry"/> of the "smallest", "most type-specific" 
        /// class that can contain the elements of <c>geomList</c>.
        /// </returns>
        public IGeometry BuildGeometry(ICollection<IGeometry> geomList)
        {
            /**
             * Determine some facts about the geometries in the list
             */
            Type geomClass = null;
            bool isHeterogeneous = false;
            bool hasGeometryCollection = false;

            IGeometry geom0 = null;
            foreach (IGeometry geom in geomList)
            {
                if (geom == null) continue;
                geom0 = geom;

                Type partClass = geom.GetType();
                if (geomClass == null)
                    geomClass = partClass;
                if (partClass != geomClass)
                    isHeterogeneous = true;
                if (geom is IGeometryCollection)
                    hasGeometryCollection = true;
            }

            /**
             * Now construct an appropriate geometry to return
             */

            // for the empty point, return an empty GeometryCollection
            if (geomClass == null)
                return CreateGeometryCollection(null);

            // for heterogenous collection of geometries or if it contains a GeometryCollection, return a GeometryCollection
            if (isHeterogeneous || hasGeometryCollection)
                return CreateGeometryCollection(ToGeometryArray(geomList));

            // at this point we know the collection is homogenous.
            // Determine the type of the result from the first Geometry in the list
            // this should always return a point, since otherwise an empty collection would have already been returned
            bool isCollection = geomList.Count > 1;

            if (isCollection)
            {
                if (geom0 is IPolygon)
                    return CreateMultiPolygon(ToPolygonArray(geomList));
                if (geom0 is ILineString)
                    return CreateMultiLineString(ToLineStringArray(geomList));
                if (geom0 is IPoint)
                    return CreateMultiPoint(ToPointArray(geomList));
                Assert.ShouldNeverReachHere("Unhandled class: " + geom0.GetType().FullName);
            }
            return geom0;
        }

        /// <summary>
        /// Creates a deep copy of the input <see cref="IGeometry"/>.
        /// The <see cref="ICoordinateSequenceFactory"/> defined for this factory
        /// is used to copy the <see cref="ICoordinateSequence"/>s
        /// of the input geometry.
        /// <para/>
        /// This is a convenient way to change the <tt>CoordinateSequence</tt>
        /// used to represent a geometry, or to change the 
        /// factory used for a geometry.
        /// <para/>
        /// <see cref="IGeometry.Clone()"/> can also be used to make a deep copy,
        /// but it does not allow changing the CoordinateSequence type.
        /// </summary>
        /// <param name="g">The geometry</param>
        /// <returns>A deep copy of the input geometry, using the CoordinateSequence type of this factory</returns>
        /// <seealso cref="IGeometry.Clone"/>
        public IGeometry CreateGeometry(IGeometry g)
        {
            // NOTE: don't move lambda to a separate variable!
            //       make a variable and you've broke WinPhone build.       
            var operation = new GeometryEditor.CoordinateSequenceOperation((x, y) => _coordinateSequenceFactory.Create(x));
            GeometryEditor editor = new GeometryEditor(this);
            return editor.Edit(g, operation);
        }

        /// <summary>
        /// Creates a <c>GeometryCollection</c> using the given <c>Geometries</c>; a null or empty
        /// array will create an empty GeometryCollection.
        /// </summary>
        /// <param name="geometries">Geometries, each of which may be empty but not null.</param>
        public virtual IGeometryCollection CreateGeometryCollection(IGeometry[] geometries)
        {
            return new GeometryCollection(geometries, this);
        }

        /// <summary>
        /// Creates a <c>LinearRing</c> using the given <c>Coordinates</c>; a null or empty array will
        /// create an empty LinearRing. The points must form a closed and simple
        /// linestring. Consecutive points must not be equal.
        /// </summary>
        /// <param name="coordinates">An array without null elements, or an empty array, or null.</param>
        public virtual ILinearRing CreateLinearRing(IEnumerable<Coordinate> coordinates)
        {
            return CreateLinearRing(coordinates != null ? CoordinateSequenceFactory.Create(coordinates) : null);
        }

        /// <summary> 
        /// Creates a <c>LinearRing</c> using the given <c>CoordinateSequence</c>; a null or empty CoordinateSequence
        /// creates an empty LinearRing. The points must form a closed and simple
        /// linestring. Consecutive points must not be equal.
        /// </summary>
        /// <param name="coordinates">A CoordinateSequence (possibly empty), or null.</param>
        /// <returns>A <see cref="ILinearRing"/> object</returns>
        /// <exception cref="ArgumentException"> If the ring is not closed, or has too few points</exception>
        public ILinearRing CreateLinearRing(ICoordinateSequence coordinates)
        {
            return new LinearRing(coordinates, this);
        }

        /// <summary>
        /// Creates a LineString using the given Coordinates; a null or empty array will
        /// create an empty LineString. Consecutive points must not be equal.
        /// </summary>
        /// <param name="coordinates">An array without null elements, or an empty array, or null.</param>
        /// <returns></returns>
        public virtual ILineString CreateLineString(IList<Coordinate> coordinates)
        {
            return CreateLineString(coordinates != null ? CoordinateSequenceFactory.Create(coordinates) : null);
        }

        /// <summary>
        /// Creates a LineString using the given CoordinateSequence.
        /// A null or empty CoordinateSequence creates an empty LineString.
        /// </summary>
        /// <param name="coordinates">A CoordinateSequence (possibly empty), or null.</param>
        /// <returns>A <see cref="ILineString"/> object</returns>
        public ILineString CreateLineString(ICoordinateSequence coordinates)
        {
            return new LineString(coordinates, this);
        }

        /// <summary>
        /// Creates a <c>MultiLineString</c> using the given <c>LineStrings</c>; a null or empty
        /// array will create an empty MultiLineString.
        /// </summary>
        /// <param name="lineStrings">LineStrings, each of which may be empty but not null-</param>
        public virtual IMultiLineString CreateMultiLineString(ILineString[] lineStrings)
        {
            if (lineStrings == null)
            {
                return new MultiLineString();
            }
            int count = lineStrings.Length;
            LineString[] ls = new LineString[count];
            for (int i = 0; i < count; i++)
            {
                ls[i] = new LineString(lineStrings[i]);
            }
            MultiLineString temp = new MultiLineString(ls);
            return temp;
        }

        /// <summary>
        /// Creates a MultiPoint using the given Points; a null or empty array will
        /// create an empty MultiPoint.
        /// </summary>
        /// <param name="point">An array without null elements, or an empty array, or null.</param>
        public virtual IMultiPoint CreateMultiPoint(IEnumerable<Coordinate> point)
        {
            return new MultiPoint(point, this);
        }

        public IMultiPoint CreateMultiPoint(IEnumerable<IPoint> point)
        {
            return new MultiPoint(point, this);
        }

        /// <summary>
        /// Creates an object that implements DotSpatial.Geometries.IMultiPoint from an array of objects that implement DotSpatial.Geometries.ICoordinate
        /// </summary>
        /// <param name="coordinates">An array of objects that implement DotSpatial.Geometries.ICoordinate</param>
        /// <returns>An object that implements DotSpatial.Geometries.IMultiPoint</returns>
        public IMultiPoint CreateMultiPoint(IEnumerable<ICoordinate> coordinates)
        {
            return CreateMultiPoint(coordinates != null ? CoordinateSequenceFactory.Create(coordinates) : null);
        }

        /// <summary> 
        /// Creates a <see cref="IMultiPoint"/> using the given CoordinateSequence.
        /// A null or empty CoordinateSequence will create an empty MultiPoint.
        /// </summary>
        /// <param name="coordinates">A CoordinateSequence (possibly empty), or <c>null</c>.</param>
        /// <returns>A <see cref="IMultiPoint"/> object</returns>
        public virtual IMultiPoint CreateMultiPoint(ICoordinateSequence coordinates)
        {
            if (coordinates == null)
                coordinates = CoordinateSequenceFactory.Create(new Coordinate[] { });

            List<IPoint> points = new List<IPoint>();
            for (int i = 0; i < coordinates.Count; i++)
            {
                ICoordinateSequence seq = CoordinateSequenceFactory.Create(1, coordinates.Ordinates);
                CoordinateSequences.Copy(coordinates, i, seq, 0, 1);
                points.Add(CreatePoint(seq));
            }
            return CreateMultiPoint(points.ToArray());
        }

        /// <summary>
        /// Creates a <c>MultiPolygon</c> using the given <c>Polygons</c>; a null or empty array
        /// will create an empty Polygon. The polygons must conform to the
        /// assertions specified in the <see href="http://www.opengis.org/techno/specs.htm"/> OpenGIS Simple Features
        /// Specification for SQL.
        /// </summary>
        /// <param name="polygons">Polygons, each of which may be empty but not null.</param>
        /// <returns>A <see cref="IMultiPolygon"/> object</returns>
        public virtual IMultiPolygon CreateMultiPolygon(IPolygon[] polygons)
        {
            return new MultiPolygon(polygons, this);
        }

        /// <summary>
        /// Creates a Point using the given Coordinate; a null Coordinate will create
        /// an empty Geometry.
        /// </summary>
        /// <param name="coordinate"></param>
        public virtual IPoint CreatePoint(Coordinate coordinate)
        {
            return CreatePoint(coordinate != null ? CoordinateSequenceFactory.Create(new[] { coordinate }) : null);
        }

        public IPoint CreatePoint(ICoordinateSequence coordinates)
        {
            return new Point(coordinates, this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="exemplar"></param>
        /// <returns></returns>
        public static IPoint CreatePointFromInternalCoord(Coordinate coord, IGeometry exemplar)
        {
            exemplar.PrecisionModel.MakePrecise(coord);
            return exemplar.Factory.CreatePoint(coord);
        }

        /// <summary>
        /// Constructs a <c>Polygon</c> with the given exterior boundary and
        /// interior boundaries.
        /// </summary>
        /// <param name="shell">
        /// The outer boundary of the new <c>Polygon</c>, or
        /// <c>null</c> or an empty <c>LinearRing</c> if
        /// the empty point is to be created.
        /// </param>
        /// <param name="holes">
        /// The inner boundaries of the new <c>Polygon</c>, or
        /// <c>null</c> or empty <c>LinearRing</c> s if
        /// the empty point is to be created.        
        /// </param>
        /// <returns>A <see cref="IPolygon"/> object</returns>
        public virtual IPolygon CreatePolygon(ILinearRing shell, ILinearRing[] holes)
        {
            return new Polygon(shell, holes, this);
        }

        /// <summary>
        /// Constructs a <c>Polygon</c> with the given exterior boundary.
        /// </summary>
        /// <param name="coordinates">the outer boundary of the new <c>Polygon</c>, or
        /// <c>null</c> or an empty <c>LinearRing</c> if
        /// the empty geometry is to be created.</param>
        /// <returns>A <see cref="IPolygon"/> object</returns>
        /// <exception cref="ArgumentException">If the boundary ring is invalid</exception>
        public virtual IPolygon CreatePolygon(ICoordinateSequence coordinates)
        {
            return CreatePolygon(CreateLinearRing(coordinates));
        }

        public virtual IPolygon CreatePolygon(IEnumerable<Coordinate> coordinates)
        {
            return CreatePolygon(CreateLinearRing(coordinates));
        }

        /// <summary>
        /// Constructs a <c>Polygon</c> with the given exterior boundary.
        /// </summary>
        /// <param name="coordinates">the outer boundary of the new <c>Polygon</c>, or
        /// <c>null</c> or an empty <c>LinearRing</c> if
        /// the empty geometry is to be created.</param>
        /// <returns>A <see cref="IPolygon"/> object</returns>
        /// <exception cref="ArgumentException">If the boundary ring is invalid</exception>
        public virtual IPolygon CreatePolygon(Coordinate[] coordinates)
        {
            return CreatePolygon(CreateLinearRing(coordinates));
        }

        /// <summary>
        /// Constructs a <c>Polygon</c> with the given exterior boundary.
        /// </summary>
        /// <param name="shell">the outer boundary of the new <c>Polygon</c>, or
        /// <c>null</c> or an empty <c>LinearRing</c> if
        /// the empty geometry is to be created.</param>
        /// <returns>the created Polygon</returns>
        /// <exception cref="ArgumentException">If the boundary ring is invalid</exception>
        public virtual IPolygon CreatePolygon(ILinearRing shell)
        {
            return CreatePolygon(shell, null);
        }

        private static ICoordinateSequenceFactory GetDefaultCoordinateSequenceFactory()
        {
            return CoordinateArraySequenceFactory.Instance;
        }

        /// <summary>
        /// If the <c>Envelope</c> is a null <c>Envelope</c>, returns an
        /// empty <c>Point</c>. If the <c>Envelope</c> is a point, returns
        /// a non-empty <c>Point</c>. If the <c>Envelope</c> is a
        /// rectangle, returns a <c>Polygon</c> whose points are (minx, miny),
        /// (maxx, miny), (maxx, maxy), (minx, maxy), (minx, miny).
        /// </summary>
        /// <param name="envelope">The <c>Envelope</c> to convert to a <c>Geometry</c>.</param>
        /// <returns>
        /// An empty <c>Point</c> (for null <c>Envelope</c>
        /// s), a <c>Point</c> (when min x = max x and min y = max y) or a
        /// <c>Polygon</c> (in all other cases)
        /// throws a <c>TopologyException</c> if <c>coordinates</c>
        /// is not a closed linestring, that is, if the first and last coordinates
        /// are not equal.
        /// </returns>
        public virtual IGeometry ToGeometry(IEnvelope envelope)
        {
            if (envelope.IsNull)
                return CreatePoint((Coordinate)null);

            if (envelope.Minimum.X == envelope.Maximum.X && envelope.Minimum.Y == envelope.Maximum.Y)
                return CreatePoint(new Coordinate(envelope.Minimum.X, envelope.Minimum.Y));

            // vertical or horizontal line?
            if (envelope.Minimum.X == envelope.Maximum.X
                    || envelope.Minimum.Y == envelope.Maximum.Y)
            {
                return CreateLineString(new[] 
                    {
                        new Coordinate(envelope.Minimum.X, envelope.Minimum.Y),
                        new Coordinate(envelope.Maximum.X, envelope.Maximum.Y)
                    });
            }
            return CreatePolygon(
                CreateLinearRing(new[]
                                     {
                                         new Coordinate(envelope.Minimum.X, envelope.Minimum.Y),
                                         new Coordinate(envelope.Maximum.X, envelope.Minimum.Y),
                                         new Coordinate(envelope.Maximum.X, envelope.Maximum.Y),
                                         new Coordinate(envelope.Minimum.X, envelope.Maximum.Y),
                                         new Coordinate(envelope.Minimum.X, envelope.Minimum.Y),
                                     }),
                null);
        }

        /// <summary>
        /// Converts the <c>ICollection</c> to an array.
        /// </summary>
        /// <param name="geometries">The <c>ICollection</c> of <c>Geometry</c>'s to convert.</param>
        /// <returns>The <c>ICollection</c> in array format.</returns>
        public static IGeometry[] ToGeometryArray(ICollection<IGeometry> geometries)
        {
            IGeometry[] list = new IGeometry[geometries.Count];
            int i = 0;
            foreach (IGeometry g in geometries)
                list[i++] = g;
            return list;
        }

        /// <summary>
        /// Converts the <c>ICollection</c> to an array.
        /// </summary>
        /// <param name="linearRings">The <c>ICollection</c> of LinearRings to convert.</param>
        /// <returns>The <c>ICollection</c> in array format.</returns>
        public static ILinearRing[] ToLinearRingArray(ICollection<IGeometry> linearRings)
        {
            ILinearRing[] list = new ILinearRing[linearRings.Count];
            int i = 0;
            foreach (ILinearRing lr in linearRings)
                list[i++] = lr;
            return list;
        }

        /// <summary>
        /// Converts the <c>ICollection</c> to an array.
        /// </summary>
        /// <param name="lineStrings">The <c>ICollection</c> of LineStrings to convert.</param>
        /// <returns>The <c>ICollection</c> in array format.</returns>
        public static ILineString[] ToLineStringArray(ICollection<IGeometry> lineStrings)
        {
            ILineString[] list = new ILineString[lineStrings.Count];
            int i = 0;
            foreach (ILineString ls in lineStrings)
                list[i++] = ls;
            return list;
        }

        /// <summary>
        /// Converts the <c>ICollection</c> to an array.
        /// </summary>
        /// <param name="multiLineStrings">The <c>ICollection</c> of MultiLineStrings to convert.</param>
        /// <returns>The <c>ICollection</c> in array format.</returns>
        public static IMultiLineString[] ToMultiLineStringArray(ICollection<IGeometry> multiLineStrings)
        {
            IMultiLineString[] list = new IMultiLineString[multiLineStrings.Count];
            int i = 0;
            foreach (IMultiLineString mls in multiLineStrings)
                list[i++] = mls;
            return list;
        }

        /// <summary>
        /// Converts the <c>ICollection</c> to an array.
        /// </summary>
        /// <param name="multiPoints">The <c>ICollection</c> of MultiPoints to convert.</param>
        /// <returns>The <c>ICollection</c> in array format.</returns>
        public static IMultiPoint[] ToMultiPointArray(ICollection<IGeometry> multiPoints)
        {
            IMultiPoint[] list = new IMultiPoint[multiPoints.Count];
            int i = 0;
            foreach (IMultiPoint mp in multiPoints)
                list[i++] = mp;
            return list;
        }

        /// <summary>
        /// Converts the <c>ICollection</c> to an array.
        /// </summary>
        /// <param name="multiPolygons">The <c>ICollection</c> of MultiPolygons to convert.</param>
        /// <returns>The <c>ICollection</c> in array format.</returns>
        public static IMultiPolygon[] ToMultiPolygonArray(ICollection<IGeometry> multiPolygons)
        {
            IMultiPolygon[] list = new IMultiPolygon[multiPolygons.Count];
            int i = 0;
            foreach (IMultiPolygon mp in multiPolygons)
                list[i++] = mp;
            return list;
        }

        /// <summary>
        /// Converts the <c>ICollection</c> to an array.
        /// </summary>
        /// <param name="points">The <c>ICollection</c> of Points to convert.</param>
        /// <returns>The <c>ICollection</c> in array format.</returns>
        public static IPoint[] ToPointArray(ICollection<IGeometry> points)
        {
            IPoint[] list = new IPoint[points.Count];
            int i = 0;
            foreach (IPoint p in points)
                list[i++] = p;
            return list;
        }

        /// <summary>
        /// Converts the <c>ICollection</c> to an array.
        /// </summary>
        /// <param name="polygons">The <c>ICollection</c> of Polygons to convert.</param>
        /// <returns>The <c>ICollection</c> in array format.</returns>
        public static IPolygon[] ToPolygonArray(ICollection<IGeometry> polygons)
        {
            IPolygon[] list = new IPolygon[polygons.Count];
            int i = 0;
            foreach (IPolygon p in polygons)
                list[i++] = p;
            return list;
        }
        #endregion
    }
}