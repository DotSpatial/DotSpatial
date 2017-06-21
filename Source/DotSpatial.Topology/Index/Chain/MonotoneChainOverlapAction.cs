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

namespace DotSpatial.Topology.Index.Chain
{
    /// <summary>
    /// The action for the internal iterator for performing
    /// overlap queries on a MonotoneChain.
    /// </summary>
    public class MonotoneChainOverlapAction
    {
        private Envelope _tempEnv1 = new Envelope();
        private Envelope _tempEnv2 = new Envelope();

        /// <summary>
        /// This envelope is used during the MonotoneChain search process.
        /// </summary>
        public Envelope TempEnv1
        {
            get { return _tempEnv1; }
            set { _tempEnv1 = value; }
        }

        /// <summary>
        /// This envelope is used during the MonotoneChain search process.
        /// </summary>
        public Envelope TempEnv2
        {
            get { return _tempEnv2; }
            set { _tempEnv2 = value; }
        }

        /// <summary>
        /// One overlapping segment.
        /// </summary>
        protected LineSegment OverlapSeg1 { get; set; }

        /// <summary>
        /// The other overlapping segment.
        /// </summary>
        protected LineSegment OverlapSeg2 { get; set; }

        /// <summary>
        /// This function can be overridden if the original chains are needed.
        /// </summary>
        /// <param name="mc1"></param>
        /// <param name="start1">The index of the start of the overlapping segment from mc1.</param>
        /// <param name="mc2"></param>
        /// <param name="start2">The index of the start of the overlapping segment from mc2.</param>
        public virtual void Overlap(MonotoneChain mc1, int start1, MonotoneChain mc2, int start2)
        {
            OverlapSeg1 = mc1.GetLineSegment(start1);
            OverlapSeg2 = mc2.GetLineSegment(start2);
            Overlap(OverlapSeg1, OverlapSeg2);
        }

        /// <summary>
        /// This is a convenience function which can be overridden to obtain the actual
        /// line segments which overlap.
        /// </summary>
        /// <param name="seg1"></param>
        /// <param name="seg2"></param>
        public virtual void Overlap(LineSegment seg1, LineSegment seg2) { }
    }
}