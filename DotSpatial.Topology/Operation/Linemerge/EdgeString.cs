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

namespace DotSpatial.Topology.Operation.Linemerge
{
    /// <summary>
    /// A sequence of <c>LineMergeDirectedEdge</c>s forming one of the lines that will
    /// be output by the line-merging process.
    /// </summary>
    public class EdgeString
    {
        private readonly IList _directedEdges = new ArrayList();
        private readonly IGeometryFactory _factory;
        private Coordinate[] _coordinates;

        /// <summary>
        /// Constructs an EdgeString with the given factory used to convert this EdgeString
        /// to a LineString.
        /// </summary>
        /// <param name="factory"></param>
        public EdgeString(IGeometryFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        ///
        /// </summary>
        private Coordinate[] Coordinates
        {
            get
            {
                if (_coordinates == null)
                {
                    int forwardDirectedEdges = 0;
                    int reverseDirectedEdges = 0;
                    CoordinateList coordinateList = new CoordinateList();
                    IEnumerator i = _directedEdges.GetEnumerator();
                    while (i.MoveNext())
                    {
                        LineMergeDirectedEdge directedEdge = (LineMergeDirectedEdge)i.Current;
                        if (directedEdge.EdgeDirection)
                            forwardDirectedEdges++;
                        else reverseDirectedEdges++;
                        coordinateList.Add(((LineMergeEdge)directedEdge.Edge).Line.Coordinates, false, directedEdge.EdgeDirection);
                    }
                    _coordinates = coordinateList.ToCoordinateArray();
                    if (reverseDirectedEdges > forwardDirectedEdges)
                        CoordinateArrays.Reverse(_coordinates);
                }
                return _coordinates;
            }
        }

        /// <summary>
        /// Adds a directed edge which is known to form part of this line.
        /// </summary>
        /// <param name="directedEdge"></param>
        public virtual void Add(LineMergeDirectedEdge directedEdge)
        {
            _directedEdges.Add(directedEdge);
        }

        /// <summary>
        /// Converts this EdgeString into a LineString.
        /// </summary>
        public virtual ILineString ToLineString()
        {
            return _factory.CreateLineString(Coordinates);
        }
    }
}