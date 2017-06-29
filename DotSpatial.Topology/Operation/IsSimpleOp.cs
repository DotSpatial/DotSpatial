// ********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is protected by the GNU Lesser Public License
// http://dotspatial.codeplex.com/license and the sourcecode for the Net Topology Suite
// can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.GeometriesGraph;
using DotSpatial.Topology.GeometriesGraph.Index;

namespace DotSpatial.Topology.Operation
{
    /// <summary>
    /// Tests whether a <c>Geometry</c> is simple.
    /// Only <c>Geometry</c>s whose definition allows them
    /// to be simple or non-simple are tested.  (E.g. Polygons must be simple
    /// by definition, so no test is provided.  To test whether a given Polygon is valid,
    /// use <c>Geometry.IsValid</c>)
    /// </summary>
    public class IsSimpleOp
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public virtual bool IsSimple(LineString geom)
        {
            return IsSimpleLinearGeometry(geom);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public virtual bool IsSimple(MultiLineString geom)
        {
            return IsSimpleLinearGeometry(geom);
        }

        /// <summary>
        /// A MultiPoint is simple if it has no repeated points.
        /// </summary>
        public virtual bool IsSimple(MultiPoint mp)
        {
            if (mp.IsEmpty)
                return true;
            var points = new HashSet<Coordinate>();
            for (int i = 0; i < mp.NumGeometries; i++)
            {
                Point pt = (Point)mp.GetGeometryN(i);
                Coordinate p = pt.Coordinate;
                if (points.Contains(p))
                    return false;
                points.Add(p);
            }
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        private static bool IsSimpleLinearGeometry(IGeometry geom)
        {
            if (geom.IsEmpty)
                return true;
            GeometryGraph graph = new GeometryGraph(0, geom);
            LineIntersector li = new RobustLineIntersector();
            SegmentIntersector si = graph.ComputeSelfNodes(li, true);
            // if no self-intersection, must be simple
            if (!si.HasIntersection) return true;
            if (si.HasProperIntersection) return false;
            if (HasNonEndpointIntersection(graph)) return false;
            if (HasClosedEndpointIntersection(graph)) return false;
            return true;
        }

        /// <summary>
        /// For all edges, check if there are any intersections which are NOT at an endpoint.
        /// The Geometry is not simple if there are intersections not at endpoints.
        /// </summary>
        /// <param name="graph"></param>
        private static bool HasNonEndpointIntersection(GeometryGraph graph)
        {
            for (IEnumerator i = graph.GetEdgeEnumerator(); i.MoveNext(); )
            {
                Edge e = (Edge)i.Current;
                int maxSegmentIndex = e.MaximumSegmentIndex;
                for (IEnumerator eiIt = e.EdgeIntersectionList.GetEnumerator(); eiIt.MoveNext(); )
                {
                    EdgeIntersection ei = (EdgeIntersection)eiIt.Current;
                    if (!ei.IsEndPoint(maxSegmentIndex))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Test that no edge intersection is the
        /// endpoint of a closed line.  To check this we compute the
        /// degree of each endpoint. The degree of endpoints of closed lines
        /// must be exactly 2.
        /// </summary>
        /// <param name="graph"></param>
        private static bool HasClosedEndpointIntersection(GeometryGraph graph)
        {
            IDictionary endPoints = new SortedList();
            for (IEnumerator i = graph.GetEdgeEnumerator(); i.MoveNext(); )
            {
                Edge e = (Edge)i.Current;
                bool isClosed = e.IsClosed;
                Coordinate p0 = e.GetCoordinate(0);
                AddEndpoint(endPoints, p0, isClosed);
                Coordinate p1 = e.GetCoordinate(e.NumPoints - 1);
                AddEndpoint(endPoints, p1, isClosed);
            }
            for (IEnumerator i = endPoints.Values.GetEnumerator(); i.MoveNext(); )
            {
                EndpointInfo eiInfo = (EndpointInfo)i.Current;
                if (eiInfo.IsClosed && eiInfo.Degree != 2)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Add an endpoint to the map, creating an entry for it if none exists.
        /// </summary>
        /// <param name="endPoints"></param>
        /// <param name="p"></param>
        /// <param name="isClosed"></param>
        private static void AddEndpoint(IDictionary endPoints, Coordinate p, bool isClosed)
        {
            EndpointInfo eiInfo = (EndpointInfo)endPoints[p];
            if (eiInfo == null)
            {
                eiInfo = new EndpointInfo(p);
                endPoints.Add(p, eiInfo);
            }
            eiInfo.AddEndpoint(isClosed);
        }

        #region Nested type: EndpointInfo

        /// <summary>
        ///
        /// </summary>
        public class EndpointInfo
        {
            private int _degree;
            private bool _isClosed;
            private Coordinate _pt;

            /// <summary>
            ///
            /// </summary>
            /// <param name="pt"></param>
            public EndpointInfo(Coordinate pt)
            {
                _pt = pt;
                _isClosed = false;
                _degree = 0;
            }

            /// <summary>
            ///
            /// </summary>
            public virtual Coordinate Point
            {
                get { return _pt; }
                set { _pt = value; }
            }

            /// <summary>
            ///
            /// </summary>
            public virtual bool IsClosed
            {
                get { return _isClosed; }
                set { _isClosed = value; }
            }

            /// <summary>
            ///
            /// </summary>
            public virtual int Degree
            {
                get { return _degree; }
                set { _degree = value; }
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="isClosed"></param>
            public virtual void AddEndpoint(bool isClosed)
            {
                Degree++;
                IsClosed |= isClosed;
            }
        }

        #endregion
    }
}