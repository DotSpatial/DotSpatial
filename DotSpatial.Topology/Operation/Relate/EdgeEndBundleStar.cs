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
using DotSpatial.Topology.GeometriesGraph;

namespace DotSpatial.Topology.Operation.Relate
{
    /// <summary>
    /// An ordered list of <c>EdgeEndBundle</c>s around a <c>RelateNode</c>.
    /// They are maintained in CCW order (starting with the positive x-axis) around the node
    /// for efficient lookup and topology building.
    /// </summary>
    public class EdgeEndBundleStar : EdgeEndStar
    {
        /// <summary>
        /// Insert a EdgeEnd in order in the list.
        /// If there is an existing EdgeStubBundle which is parallel, the EdgeEnd is
        /// added to the bundle.  Otherwise, a new EdgeEndBundle is created
        /// to contain the EdgeEnd.
        /// </summary>
        /// <param name="e"></param>
        public override void Insert(EdgeEnd e)
        {
            EdgeEndBundle eb = (EdgeEndBundle)EdgeMap[e];
            if (eb == null)
            {
                eb = new EdgeEndBundle(e);
                InsertEdgeEnd(e, eb);
            }
            else
                eb.Insert(e);
        }

        /// <summary>
        /// Update the IM with the contribution for the EdgeStubs around the node.
        /// </summary>
        /// <param name="im"></param>
        public virtual void UpdateIm(IntersectionMatrix im)
        {
            for (IEnumerator it = GetEnumerator(); it.MoveNext(); )
            {
                EdgeEndBundle esb = (EdgeEndBundle)it.Current;
                esb.UpdateIm(im);
            }
        }
    }
}