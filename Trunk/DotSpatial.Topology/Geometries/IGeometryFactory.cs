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

using System.Collections;
using System.Collections.Generic;

namespace DotSpatial.Topology.Geometries
{
    /// <summary>
    /// Supplies a set of utility methods for building Geometry objects 
    /// from lists of Coordinates.
    /// </summary>
    public interface IGeometryFactory
    {
        #region Properties

        /// <summary>
        /// Gets the coordinate sequence factory to use when creating geometries.
        /// </summary>
        ICoordinateSequenceFactory CoordinateSequenceFactory { get; }

        /// <summary>
        /// Gets the PrecisionModel that Geometries created by this factory
        /// will be associated with.
        /// </summary>
        IPrecisionModel PrecisionModel { get; }

        /// <summary>
        /// Gets the spatial reference id to assign when creating geometries
        /// </summary>
        int Srid
        {
            get;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generic constructor that parses a list and tries to form a working
        /// object that implements MapWindow.Interfaces.IGeometry
        /// </summary>
        /// <param name="geomList">some list of things</param>
        /// <returns>An object that implements DotSpatial.Geometries.IGeometry</returns>
        IGeometry BuildGeometry(IList geomList);

        /// <summary>
        /// Creates an object that implements DotSpatial.Geometries.IGeometry that is a copy
        /// of the specified object that implements DotSpatial.Geometries.IGeometry
        /// </summary>
        /// <param name="g">An object that implements DotSpatial.Geometries.IGeometry</param>
        /// <returns>An copy of the original object that implements DotSpatial.Geometries.IGeometry</returns>
        IGeometry CreateGeometry(IGeometry g);

        /// <summary>
        /// Creates an object that implements DotSpatial.Geometries.IGeometryCollection
        /// from an array of objects that implement DotSpatial.Geometries.IGeometry
        /// </summary>
        /// <param name="geometries">An array of objects that implement DotSpatial.Geometries.IGeometry</param>
        /// <returns>A new object that implements DotSpatial.Geometries.IGeometryCollection</returns>
        IGeometryCollection CreateGeometryCollection(IGeometry[] geometries);

        /// <summary>
        /// Creates an object that implements DotSpatial.Geometries.ILinearRing from an array of DotSpatial.Geometries.ICoordinates
        /// </summary>
        /// <param name="coordinates">An array of objects that implement ICoordinate</param>
        /// <returns>An object that implements DotSpatial.Geometries.ILinearRing</returns>
        ILinearRing CreateLinearRing(IEnumerable<Coordinate> coordinates);

        /// <summary> 
        /// Creates a <c>LinearRing</c> using the given <c>CoordinateSequence</c>; a null or empty CoordinateSequence will
        /// create an empty LinearRing. The points must form a closed and simple
        /// linestring. Consecutive points must not be equal.
        /// </summary>
        /// <param name="coordinates">A CoordinateSequence possibly empty, or null.</param>
        ILinearRing CreateLinearRing(ICoordinateSequence coordinates);

        /// <summary> 
        /// Creates a LineString using the given Coordinates; a null or empty array will
        /// create an empty LineString. Consecutive points must not be equal.
        /// </summary>
        /// <param name="coordinates">An array without null elements, or an empty array, or null.</param>
        /// <returns>A DotSpatial.Geometries.ILineString</returns>
        ILineString CreateLineString(IList<Coordinate> coordinates);

        /// <summary> 
        /// Creates a LineString using the given Coordinates; a null or empty array will
        /// create an empty LineString. Consecutive points must not be equal.
        /// </summary>
        /// <param name="coordinates">An array without null elements, or an empty array, or null.</param>
        /// <returns>A LineString</returns>
        ILineString CreateLineString(ICoordinateSequence coordinates);

        /// <summary>
        /// Creates a new object that implements DotSpatial.Geometries.MultiLineString
        /// given an array of objects that implement DotSpatial.Geometries.ILineStringBase
        /// </summary>
        /// <param name="lineStrings">The Array of objects that implement DotSpatial.Geometries.IlineStringBase </param>
        /// <returns>A new MultiLineString that implements IMultiLineString</returns>
        IMultiLineString CreateMultiLineString(IBasicLineString[] lineStrings);

        /// <summary>
        /// Creates an object that implements DotSpatial.Geometries.IMultiPoint from an array of objects that implement DotSpatial.Geometries.ICoordinate
        /// </summary>
        /// <param name="coordinates">An array of objects that implement DotSpatial.Geometries.ICoordinate</param>
        /// <returns>An object that implements DotSpatial.Geometries.IMultiPoint</returns>
        IMultiPoint CreateMultiPoint(IEnumerable<ICoordinate> coordinates);

        /// <summary>
        /// Creates an object that implements DotSpatial.Geometries.IMultiPoint from an array of objects that implement DotSpatial.Geometries.ICoordinate
        /// </summary>
        /// <param name="coordinates">An array of objects that implement DotSpatial.Geometries.ICoordinate</param>
        /// <returns>An object that implements DotSpatial.Geometries.IMultiPoint</returns>
        IMultiPoint CreateMultiPoint(IEnumerable<Coordinate> coordinates);

        /// <summary> 
        /// Creates a <see cref="IMultiPoint"/> using the given Points.
        /// A null or empty array will  create an empty MultiPoint.
        /// </summary>
        /// <param name="point">An array (without null elements), or an empty array, or <c>null</c>.</param>
        /// <returns>A <see cref="IMultiPoint"/> object</returns>
        IMultiPoint CreateMultiPoint(IEnumerable<IPoint> point);

        /// <summary> 
        /// Creates a <see cref="IMultiPoint"/> using the given CoordinateSequence.
        /// A null or empty CoordinateSequence will create an empty MultiPoint.
        /// </summary>
        /// <param name="coordinates">A CoordinateSequence (possibly empty), or <c>null</c>.</param>
        IMultiPoint CreateMultiPoint(ICoordinateSequence coordinates);

        /// <summary>
        /// Creates a <c>MultiPolygon</c> using the given <c>Polygons</c>; a null or empty array
        /// will create an empty Polygon. The polygons must conform to the
        /// assertions specified in the <see href="http://www.opengis.org/techno/specs.htm"/> OpenGIS Simple Features
        /// Specification for SQL.
        /// </summary>
        /// <param name="polygons">Polygons, each of which may be empty but not null.</param>
        /// <returns>An object that implements DotSpatial.Geometries.IMultiPolygon</returns>
        IMultiPolygon CreateMultiPolygon(IPolygon[] polygons);

        /// <summary>
        /// Creates a Point using the given Coordinate; a null Coordinate will create
        /// an empty Geometry.
        /// </summary>
        /// <param name="coordinate">The coordinate</param>
        /// <returns>An object that implements DotSpatial.Geometries.IPoint</returns>
        IPoint CreatePoint(Coordinate coordinate);

        /// <summary>
        /// Creates a <c>Point</c> using the given <c>CoordinateSequence</c>; a null or empty
        /// CoordinateSequence will create an empty Point.
        /// </summary>
        /// <param name="coordinates">The coordiante sequence.</param>
        /// <returns>A Point</returns>
        IPoint CreatePoint(ICoordinateSequence coordinates);

        /// <summary>
        /// Creates an object that implements DotSpatial.Geometries.IPolygon given a specified
        /// DotSpatial.Geometries.ILinearRing shell and an array of
        /// DotSpatial.Geometries.ILinearRing that represent the holes
        /// </summary>
        /// <param name="shell">The outer perimeter of the polygon, represented by an object that implements DotSpatial.Geometries.ILinearRing</param>
        /// <param name="holes">The interior holes in the polygon, represented by an array of objects that implements DotSpatial.Geometries.ILinearRing</param>
        /// <returns>An object that implements DotSpatial.Geometries.IPolygon</returns>
        IPolygon CreatePolygon(ILinearRing shell, ILinearRing[] holes);

        /// <summary>
        /// Constructs a <c>Polygon</c> with the given exterior boundary.
        /// </summary>
        /// <param name="coordinates">the outer boundary of the new <c>Polygon</c>, or
        /// <c>null</c> or an empty <c>LinearRing</c> if
        /// the empty geometry is to be created.</param>
        /// <returns>The polygon</returns>
        IPolygon CreatePolygon(ICoordinateSequence coordinates);

        /// <summary>
        /// Constructs a <c>Polygon</c> with the given exterior boundary.
        /// </summary>
        /// <param name="coordinates">the outer boundary of the new <c>Polygon</c>, or
        /// <c>null</c> or an empty <c>LinearRing</c> if
        /// the empty geometry is to be created.</param>
        /// <returns>The polygon</returns>
        IPolygon CreatePolygon(IEnumerable<Coordinate> coordinates);

        /// <summary>
        /// Constructs a <c>Polygon</c> with the given exterior boundary.
        /// </summary>
        /// <param name="shell">the outer boundary of the new <c>Polygon</c>, or
        /// <c>null</c> or an empty <c>LinearRing</c> if
        /// the empty geometry is to be created.</param>
        /// <returns>The polygon</returns>
        IPolygon CreatePolygon(ILinearRing shell);

        /// <summary>
        /// Creates a <see cref="IGeometry"/> with the same extent as the given envelope.
        /// </summary>
        IGeometry ToGeometry(IEnvelope envelopeInternal);
        #endregion
    }
}