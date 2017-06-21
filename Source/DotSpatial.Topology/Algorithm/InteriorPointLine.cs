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
using System.Linq;

namespace DotSpatial.Topology.Algorithm
{
    /// <summary>
    /// Computes a point in the interior of an linear point.
    /// Algorithm:
    /// Find an interior vertex which is closest to
    /// the centroid of the linestring.
    /// If there is no interior vertex, find the endpoint which is
    /// closest to the centroid.
    /// </summary>
    public class InteriorPointLine
    {
        private readonly Coordinate _centroid;
        private Coordinate _interiorPoint;
        private double _minDistance = Double.MaxValue;

        /// <summary>
        ///
        /// </summary>
        /// <param name="g"></param>
        public InteriorPointLine(IGeometry g)
        {
            _centroid = new Coordinate(g.Centroid);
            AddInterior(g);

            if (_interiorPoint == null)
                AddEndpoints(g);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Coordinate InteriorPoint
        {
            get
            {
                return _interiorPoint;
            }
        }

        /// <summary>
        /// Tests the interior vertices (if any)
        /// defined by a linear Geometry for the best inside point.
        /// If a Geometry is not of dimension 1 it is not tested.
        /// </summary>
        /// <param name="geom">The point to add.</param>
        private void AddInterior(IGeometry geom)
        {
            if (geom is ILineString)
                AddInterior(geom.Coordinates);
            else if (geom is GeometryCollection)
            {
                GeometryCollection gc = (GeometryCollection)geom;
                foreach (Geometry geometry in gc.Geometries)
                    AddInterior(geometry);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pts"></param>
        private void AddInterior(IEnumerable<Coordinate> pts)
        {
            foreach (Coordinate pt in pts)
            {
                Add(pt);
            }
        }

        /// <summary>
        /// Tests the endpoint vertices
        /// defined by a linear Geometry for the best inside point.
        /// If a Geometry is not of dimension 1 it is not tested.
        /// </summary>
        /// <param name="geom">The point to add.</param>
        private void AddEndpoints(IGeometry geom)
        {
            if (geom is LineString)
                AddEndpoints(geom.Coordinates);
            else if (geom is GeometryCollection)
            {
                GeometryCollection gc = (GeometryCollection)geom;
                foreach (Geometry geometry in gc.Geometries)
                    AddEndpoints(geometry);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pts"></param>
        private void AddEndpoints(IEnumerable<Coordinate> pts)
        {
            Add(pts.First());
            Add(pts.Last());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="point"></param>
        private void Add(Coordinate point)
        {
            double dist = point.Distance(_centroid);
            if (dist < _minDistance)
            {
                _interiorPoint = new Coordinate(point);
                _minDistance = dist;
            }
        }
    }
}