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
using System.Collections.Generic;

namespace DotSpatial.Topology.Utilities
{
    /// <summary>
    /// A framework for processes which transform an input <c>Geometry</c> into
    /// an output <c>Geometry</c>, possibly changing its structure and type(s).
    /// This class is a framework for implementing subclasses
    /// which perform transformations on
    /// various different Geometry subclasses.
    /// It provides an easy way of applying specific transformations
    /// to given point types, while allowing unhandled types to be simply copied.
    /// Also, the framework handles ensuring that if subcomponents change type
    /// the parent geometries types change appropriately to maintain valid structure.
    /// Subclasses will override whichever <c>TransformX</c> methods
    /// they need to to handle particular Geometry types.
    /// A typically usage would be a transformation that may transform Polygons into
    /// Polygons, LineStrings
    /// or Points.  This class would likely need to override the TransformMultiPolygon
    /// method to ensure that if input Polygons change type the result is a GeometryCollection,
    /// not a MultiPolygon.
    /// The default behaviour of this class is to simply recursively transform
    /// each Geometry component into an identical object by copying.
    /// Notice that all <c>TransformX</c> methods may return <c>null</c>,
    /// to avoid creating empty point objects. This will be handled correctly
    /// by the transformer.
    /// The Transform method itself will always
    /// return a point object.
    /// </summary>
    public class GeometryTransformer
    {
        /*
        * Possible extensions:
        * GetParent() method to return immediate parent e.g. of LinearRings in Polygons
        */

        // these could eventually be exposed to clients
        /// <summary>
        /// <c>true</c> if empty geometries should not be included in the result.
        /// </summary>
        private const bool PRUNE_EMPTY_GEOMETRY = true;

        /// <summary>
        /// <c>true</c> if the type of the input should be preserved.
        /// </summary>
        private const bool PRESERVE_TYPE = false;

        /// <summary>
        ///
        /// </summary>
        private readonly GeometryFactory _factory = Geometry.DefaultFactory;

        private IGeometry _inputGeom;

        /// <summary>
        ///
        /// </summary>
        public virtual IGeometry InputGeometry
        {
            get
            {
                return _inputGeom;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="anInputGeom"></param>
        /// <returns></returns>
        public virtual IGeometry Transform(IGeometry anInputGeom)
        {
            _inputGeom = anInputGeom;
            //this.factory = anInputGeom.Factory;
            if (anInputGeom is IPoint)
                return TransformPoint((Point)anInputGeom);
            if (anInputGeom is IMultiPoint)
                return TransformMultiPoint((IMultiPoint)anInputGeom);
            if (anInputGeom is ILinearRing)
                return TransformLineString((LinearRing)anInputGeom);
            if (anInputGeom is ILineString)
                return TransformLineString((LineString)anInputGeom);
            if (anInputGeom is IMultiLineString)
                return TransformMultiLineString((IMultiLineString)anInputGeom);
            if (anInputGeom is IPolygon)
                return TransformPolygon((Polygon)anInputGeom, null);
            if (anInputGeom is IMultiPolygon)
                return TransformMultiPolygon((IMultiPolygon)anInputGeom);
            if (anInputGeom is IGeometryCollection)
                return TransformGeometryCollection((IGeometryCollection)anInputGeom, null);
            throw new ArgumentException("Unknown Geometry subtype: " + anInputGeom.GeometryType);
        }

        /// <summary>
        /// Convenience method which provides standard way of
        /// creating a <c>CoordinateSequence</c>.
        /// </summary>
        /// <param name="coords">The coordinate array to copy.</param>
        /// <returns>A coordinate sequence for the array.</returns>
        protected virtual ICoordinateSequence CreateCoordinateSequence(Coordinate[] coords)
        {
            return _factory.CoordinateSequenceFactory.Create(coords);
        }

        /// <summary>
        /// Convenience method which provides statndard way of copying {CoordinateSequence}s
        /// </summary>
        /// <param name="seq">The sequence to copy.</param>
        /// <returns>A deep copy of the sequence.</returns>
        protected virtual IList<Coordinate> Copy(IList<Coordinate> seq)
        {
            return seq.CloneList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="coords"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        protected virtual IList<Coordinate> TransformCoordinates(IList<Coordinate> coords, IGeometry parent)
        {
            return Copy(coords);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        protected virtual IGeometry TransformPoint(IPoint geom)
        {
            return _factory.CreatePoint(TransformCoordinates(geom.Coordinates, geom)[0]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        protected virtual IGeometry TransformMultiPoint(IMultiPoint geom)
        {
            ArrayList transGeomList = new ArrayList();
            for (int i = 0; i < geom.NumGeometries; i++)
            {
                IGeometry transformGeom = TransformPoint((Point)geom.GetGeometryN(i));
                if (transformGeom == null) continue;
                if (transformGeom.IsEmpty) continue;
                transGeomList.Add(transformGeom);
            }
            return _factory.BuildGeometry(transGeomList);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        protected virtual IGeometry TransformLinearRing(ILinearRing geom)
        {
            IList<Coordinate> seq = TransformCoordinates(geom.Coordinates, geom);
            int seqSize = seq.Count;
            // ensure a valid LinearRing
            if (seqSize > 0 && seqSize < 4 && !PRESERVE_TYPE)
                return _factory.CreateLineString(seq);
            return _factory.CreateLinearRing(seq);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        protected virtual IGeometry TransformLineString(ILineString geom)
        {
            // should check for 1-point sequences and downgrade them to points
            return _factory.CreateLineString(TransformCoordinates(geom.Coordinates, geom));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        protected virtual IGeometry TransformMultiLineString(IMultiLineString geom)
        {
            ArrayList transGeomList = new ArrayList();
            for (int i = 0; i < geom.NumGeometries; i++)
            {
                IGeometry transformGeom = TransformLineString((ILineString)geom.GetGeometryN(i));
                if (transformGeom == null) continue;
                if (transformGeom.IsEmpty) continue;
                transGeomList.Add(transformGeom);
            }
            return _factory.BuildGeometry(transGeomList);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        protected virtual IGeometry TransformPolygon(IPolygon geom, IGeometry parent)
        {
            bool isAllValidLinearRings = true;
            IGeometry shell = TransformLinearRing(geom.Shell);

            if (shell == null || !(shell is LinearRing) || shell.IsEmpty)
                isAllValidLinearRings = false;

            ArrayList holes = new ArrayList();
            for (int i = 0; i < geom.NumHoles; i++)
            {
                IGeometry hole = TransformLinearRing((ILinearRing)geom.GetInteriorRingN(i));
                if (hole == null || hole.IsEmpty) continue;
                if (!(hole is LinearRing))
                    isAllValidLinearRings = false;
                holes.Add(hole);
            }

            if (isAllValidLinearRings)
                return _factory.CreatePolygon((ILinearRing)shell,
                                             (ILinearRing[])holes.ToArray(typeof(ILinearRing)));
            ArrayList components = new ArrayList();
            if (shell != null)
                components.Add(shell);
            foreach (object hole in holes)
                components.Add(hole);
            return _factory.BuildGeometry(components);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        protected virtual IGeometry TransformMultiPolygon(IMultiPolygon geom)
        {
            ArrayList transGeomList = new ArrayList();
            for (int i = 0; i < geom.NumGeometries; i++)
            {
                IGeometry transformGeom = TransformPolygon((Polygon)geom.GetGeometryN(i), geom);
                if (transformGeom == null) continue;
                if (transformGeom.IsEmpty) continue;
                transGeomList.Add(transformGeom);
            }
            return _factory.BuildGeometry(transGeomList);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        protected virtual IGeometry TransformGeometryCollection(IGeometryCollection geom, IGeometry parent)
        {
            ArrayList transGeomList = new ArrayList();
            for (int i = 0; i < geom.NumGeometries; i++)
            {
                IGeometry transformGeom = Transform(geom.GetGeometryN(i));
                if (transformGeom == null) continue;
                if (PRUNE_EMPTY_GEOMETRY && transformGeom.IsEmpty) continue;
                transGeomList.Add(transformGeom);
            }
            return _factory.CreateGeometryCollection(GeometryFactory.ToGeometryArray(transGeomList));
        }
    }
}