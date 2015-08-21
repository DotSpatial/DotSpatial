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

using System.Collections.Generic;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Noding;

namespace DotSpatial.Topology.GeometriesGraph
{
    /// <summary>
    /// Validates that a collection of <see cref="Edge"/> is correctly noded.
    /// Throws an appropriate exception if an noding error is found.
    /// </summary>
    public class EdgeNodingValidator
    {
        #region Fields

        private readonly FastNodingValidator _nv;

        #endregion

        #region Constructors

        ///<summary>
       /// Creates a new validator for the given collection of <see cref="Edge"/>s.
       /// </summary> 
       public EdgeNodingValidator(IEnumerable<Edge> edges)
        {
            _nv = new FastNodingValidator(ToSegmentStrings(edges));
        }

        #endregion

        #region Methods

        ///<summary>
        /// Checks whether the supplied <see cref="Edge"/>s are correctly noded. 
        ///</summary>
        /// <param name="edges">an enumeration of Edges.</param>
        /// <exception cref="TopologyException">If the SegmentStrings are not correctly noded</exception>
        public static void CheckValid(IEnumerable<Edge> edges)
        {
            EdgeNodingValidator validator = new EdgeNodingValidator(edges);
            validator.CheckValid();
        }

        /// <summary>
        /// Checks whether the supplied edges are correctly noded. 
        /// </summary>
       /// <exception cref="TopologyException">If the SegmentStrings are not correctly noded</exception>
        public void CheckValid()
        {
            _nv.CheckValid();
        }

        public static IEnumerable<ISegmentString> ToSegmentStrings(IEnumerable<Edge> edges)
        {
            // convert Edges to SegmentStrings
            IList<ISegmentString> segStrings = new List<ISegmentString>();
            foreach (Edge e in edges)
                segStrings.Add(new BasicSegmentString(e.Coordinates, e));
            return segStrings;
        }

        #endregion
    }
}