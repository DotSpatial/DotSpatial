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

using System.Collections;
using System.IO;
using DotSpatial.Topology.GeometriesGraph;

namespace DotSpatial.Topology.Operation.Relate
{
    /// <summary>
    /// A collection of EdgeStubs which obey the following invariant:
    /// They originate at the same node and have the same direction.
    /// Contains all <c>EdgeEnd</c>s which start at the same point and are parallel.
    /// </summary>
    public class EdgeEndBundle : EdgeEnd
    {
        private readonly IList _edgeEnds = new ArrayList();

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        public EdgeEndBundle(EdgeEnd e)
            : base(e.Edge, e.Coordinate, e.DirectedCoordinate, new Label(e.Label))
        {
            Insert(e);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual IList EdgeEnds
        {
            get
            {
                return _edgeEnds;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator GetEnumerator()
        {
            return _edgeEnds.GetEnumerator();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        public void Insert(EdgeEnd e)
        {
            // Assert: start point is the same
            // Assert: direction is the same
            _edgeEnds.Add(e);
        }

        /// <summary>
        /// This computes the overall edge label for the set of
        /// edges in this EdgeStubBundle.  It essentially merges
        /// the ON and side labels for each edge.
        /// These labels must be compatible
        /// </summary>
        public override void ComputeLabel()
        {
            // create the label.  If any of the edges belong to areas,
            // the label must be an area label
            bool isArea = false;
            for (IEnumerator it = GetEnumerator(); it.MoveNext(); )
            {
                EdgeEnd e = (EdgeEnd)it.Current;
                if (e.Label.IsArea())
                    isArea = true;
            }
            if (isArea)
            {
                Label = new Label(LocationType.Null, LocationType.Null, LocationType.Null);
            }
            else
            {
                Label = new Label(LocationType.Null);
            }

            // compute the On label, and the side labels if present
            for (int i = 0; i < 2; i++)
            {
                ComputeLabelOn(i);
                if (isArea)
                    ComputeLabelSides(i);
            }
        }

        /// <summary>
        /// Compute the overall ON location for the list of EdgeStubs.
        /// (This is essentially equivalent to computing the self-overlay of a single Geometry)
        /// edgeStubs can be either on the boundary (eg Polygon edge)
        /// OR in the interior (e.g. segment of a LineString)
        /// of their parent Geometry.
        /// In addition, GeometryCollections use the mod-2 rule to determine
        /// whether a segment is on the boundary or not.
        /// Finally, in GeometryCollections it can still occur that an edge is both
        /// on the boundary and in the interior (e.g. a LineString segment lying on
        /// top of a Polygon edge.) In this case as usual the Boundary is given precendence.
        /// These observations result in the following rules for computing the ON location:
        ///  if there are an odd number of Bdy edges, the attribute is Bdy
        ///  if there are an even number >= 2 of Bdy edges, the attribute is Int
        ///  if there are any Int edges, the attribute is Int
        ///  otherwise, the attribute is Null.
        /// </summary>
        /// <param name="geomIndex"></param>
        private void ComputeLabelOn(int geomIndex)
        {
            // compute the On location value
            int boundaryCount = 0;
            bool foundInterior = false;
            LocationType loc;

            for (IEnumerator it = GetEnumerator(); it.MoveNext(); )
            {
                EdgeEnd e = (EdgeEnd)it.Current;
                loc = e.Label.GetLocation(geomIndex);
                if (loc == LocationType.Boundary)
                    boundaryCount++;
                if (loc == LocationType.Interior)
                    foundInterior = true;
            }

            loc = LocationType.Null;
            if (foundInterior)
                loc = LocationType.Interior;
            if (boundaryCount > 0)
                loc = GeometryGraph.DetermineBoundary(boundaryCount);
            Label.SetLocation(geomIndex, loc);
        }

        /// <summary>
        /// Compute the labelling for each side
        /// </summary>
        /// <param name="geomIndex"></param>
        private void ComputeLabelSides(int geomIndex)
        {
            ComputeLabelSide(geomIndex, PositionType.Left);
            ComputeLabelSide(geomIndex, PositionType.Right);
        }

        /// <summary>
        /// To compute the summary label for a side, the algorithm is:
        /// FOR all edges
        /// IF any edge's location is Interior for the side, side location = Interior
        /// ELSE IF there is at least one Exterior attribute, side location = Exterior
        /// ELSE  side location = Null
        /// Notice that it is possible for two sides to have apparently contradictory information
        /// i.e. one edge side may indicate that it is in the interior of a point, while
        /// another edge side may indicate the exterior of the same point.  This is
        /// not an incompatibility - GeometryCollections may contain two Polygons that touch
        /// along an edge.  This is the reason for Interior-primacy rule above - it
        /// results in the summary label having the Geometry interior on both sides.
        /// </summary>
        /// <param name="geomIndex"></param>
        /// <param name="side"></param>
        private void ComputeLabelSide(int geomIndex, PositionType side)
        {
            for (IEnumerator it = GetEnumerator(); it.MoveNext(); )
            {
                EdgeEnd e = (EdgeEnd)it.Current;
                if (!e.Label.IsArea()) continue;
                LocationType loc = e.Label.GetLocation(geomIndex, side);
                if (loc == LocationType.Interior)
                {
                    Label.SetLocation(geomIndex, side, LocationType.Interior);
                    return;
                }
                if (loc == LocationType.Exterior)
                    Label.SetLocation(geomIndex, side, LocationType.Exterior);
            }
        }

        /// <summary>
        /// Update the IM with the contribution for the computed label for the EdgeStubs.
        /// </summary>
        /// <param name="im"></param>
        public virtual void UpdateIm(IntersectionMatrix im)
        {
            Edge.UpdateIm(Label, im);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="outstream"></param>
        public override void Write(StreamWriter outstream)
        {
            outstream.WriteLine("EdgeEndBundle--> Label: " + Label);
            for (IEnumerator it = GetEnumerator(); it.MoveNext(); )
            {
                EdgeEnd ee = (EdgeEnd)it.Current;
                ee.Write(outstream);
                outstream.WriteLine();
            }
        }
    }
}