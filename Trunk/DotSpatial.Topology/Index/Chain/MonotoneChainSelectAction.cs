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

using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Index.Chain
{
    /// <summary>
    /// The action for the internal iterator for performing
    /// envelope select queries on a MonotoneChain.
    /// </summary>
    public class MonotoneChainSelectAction
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        public LineSegment SelectedSegment = new LineSegment();

        /// <summary>
        /// These envelopes are used during the MonotoneChain search process.
        /// </summary>
        public Envelope TempEnv1 = new Envelope();

        #endregion

        #region Methods

        /// <summary> 
        /// This method is overridden to process a segment 
        /// in the context of the parent chain.
        /// </summary>
        /// <param name="mc">The parent chain</param>
        /// <param name="startIndex">The index of the start vertex of the segment being processed</param>
        public virtual void Select(MonotoneChain mc, int startIndex)
        {
            mc.GetLineSegment(startIndex, ref SelectedSegment);
            // call this routine in case select(segmenet) was overridden
            Select(SelectedSegment);
        }

        /// <summary>
        /// This is a convenience method which can be overridden to obtain the actual
        /// line segment which is selected.
        /// </summary>
        /// <param name="seg"></param>
        public virtual void Select(LineSegment seg) { }

        #endregion
    }
}