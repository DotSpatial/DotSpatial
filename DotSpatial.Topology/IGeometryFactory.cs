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

namespace DotSpatial.Topology
{
    /// <summary>
    /// Factory for Geometry stuff
    /// </summary>
    public interface IGeometryFactory
    {
        #region Public Properties

        /// <summary>
        /// Floating reference
        /// </summary>
        IGeometryFactory Floating
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IGeometryFactory FloatingSingle
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        int Srid
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        PrecisionModelType PrecisionModel
        {
            get;
        }

        #endregion

        /// <summary>
        /// CoordinateSequenceFactory that can manufacture a coordinate sequence
        /// </summary>
        ICoordinateSequenceFactory CoordinateSequenceFactory { get; }

        /// <summary>
        /// Generic constructor that parses a list and tries to form a working
        /// object that implements MapWindow.Interfaces.IGeometry
        /// </summary>
        /// <param name="geomList">some list of things</param>
        /// <returns>An object that implements DotSpatial.Geometries.IGeometry</returns>
        IGeometry BuildGeometry(IList geomList);

        /// <summary>
        /// Method to produce a point given a coordinate
        /// </summary>
        /// <param name="coord">An object that implements DotSpatial.Geometries.ICoordinate</param>
        /// <returns>An object that implements DotSpatial.Geometries.IPoint</returns>
        IPoint CreatePoint(Coordinate coord);

        /// <summary>
        /// Creates a new object that implements DotSpatial.Geometries.MultiLineString
        /// given an array of objects that implement DotSpatial.Geometries.ILineStringBase
        /// </summary>
        /// <param name="lineStrings">The Array of objects that implement DotSpatial.Geometries.IlineStringBase </param>
        /// <returns>A new MultiLineString that implements IMultiLineString</returns>
        IMultiLineString CreateMultiLineString(IBasicLineString[] lineStrings);

        /// <summary>
        /// Creates an object that implements DotSpatial.Geometries.IGeometryCollection
        /// from an array of objects that implement DotSpatial.Geometries.IGeometry
        /// </summary>
        /// <param name="geometries">An array of objects that implement DotSpatial.Geometries.IGeometry</param>
        /// <returns>A new object that implements DotSpatial.Geometries.IGeometryCollection</returns>
        IGeometryCollection CreateGeometryCollection(IGeometry[] geometries);

        /// <summary>
        /// Creates an object that implements DotSpatial.Geometries.IMultiPolygon from an array of
        /// objects that implement DotSpatial.Geometries.IPolygon
        /// </summary>
        /// <param name="polygons">An Array of objects that implement DotSpatial.Geometries.IPolygon</param>
        /// <returns>An object that implements DotSpatial.Geometries.IMultiPolygon</returns>
        IMultiPolygon CreateMultiPolygon(IPolygon[] polygons);

        /// <summary>
        /// Creates an object that implements DotSpatial.Geometries.ILinearRing from an array of DotSpatial.Geometries.ICoordinates
        /// </summary>
        /// <param name="coordinates">An array of objects that implement ICoordinate</param>
        /// <returns>An object that implements DotSpatial.Geometries.ILinearRing</returns>
        ILinearRing CreateLinearRing(IList<Coordinate> coordinates);

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
        /// Creates an object that implements DotSpatial.Geometries.ILineString from an array of objects that implement DotSpatial.Geometries.ICoordinate
        /// </summary>
        /// <param name="coordinates">An array of objects that implement DotSpatial.Geometries.ICoordinate</param>
        /// <returns>A DotSpatial.Geometries.ILineString</returns>
        ILineString CreateLineString(IList<Coordinate> coordinates);

        /// <summary>
        /// Creates an object that implements DotSpatial.Geometries.IGeometry that is a copy
        /// of the specified object that implements DotSpatial.Geometries.IGeometry
        /// </summary>
        /// <param name="g">An object that implements DotSpatial.Geometries.IGeometry</param>
        /// <returns>An copy of the original object that implements DotSpatial.Geometries.IGeometry</returns>
        IGeometry CreateGeometry(IGeometry g);

        /// <summary>
        /// Creates an object that implements DotSpatial.Geometries.IPolygon given a specified
        /// DotSpatial.Geometries.ILinearRing shell and an array of
        /// DotSpatial.Geometries.ILinearRing that represent the holes
        /// </summary>
        /// <param name="shell">The outer perimeter of the polygon, represented by an object that implements DotSpatial.Geometries.ILinearRing</param>
        /// <param name="holes">The interior holes in the polygon, represented by an array of objects that implements DotSpatial.Geometries.ILinearRing</param>
        /// <returns>An object that implements DotSpatial.Geometries.IPolygon</returns>
        IPolygon CreatePolygon(ILinearRing shell, ILinearRing[] holes);
    }
}