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

namespace DotSpatial.Topology.Noding
{
    /// <summary>
    /// Computes all intersections between segments in a set of <see cref="SegmentString" />s.
    /// Intersections found are represented as <see cref="SegmentNode" />s and added to the
    /// <see cref="SegmentString" />s in which they occur.
    /// As a final step in the noding a new set of segment strings split at the nodes may be returned.
    /// </summary>
    public interface INoder
    {
        /// <summary>
        /// Computes the noding for a collection of <see cref="SegmentString" />s.
        /// Some Noders may add all these nodes to the input <see cref="SegmentString" />s;
        /// others may only add some or none at all.
        /// </summary>
        /// <param name="segStrings"></param>
        void ComputeNodes(IList segStrings);

        /// <summary>
        /// Returns a <see cref="IList" /> of fully noded <see cref="SegmentString" />s.
        /// The <see cref="SegmentString" />s have the same context as their parent.
        /// </summary>
        /// <returns></returns>
        IList GetNodedSubstrings();
    }
}