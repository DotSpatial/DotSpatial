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
    /// Base class for <see cref="INoder" />s which make a single pass to find intersections.
    /// This allows using a custom <see cref="ISegmentIntersector" />
    /// (which for instance may simply identify intersections, rather than insert them).
    /// </summary>
    public abstract class SinglePassNoder : INoder
    {
        private ISegmentIntersector _segInt;

        /// <summary>
        /// Initializes a new instance of the <see cref="SinglePassNoder"/> class.
        /// </summary>
        protected SinglePassNoder() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SinglePassNoder"/> class.
        /// </summary>
        /// <param name="segInt">The <see cref="ISegmentIntersector" /> to use.</param>
        protected SinglePassNoder(ISegmentIntersector segInt)
        {
            _segInt = segInt;
        }

        /// <summary>
        /// Gets/sets the <see cref="ISegmentIntersector" /> to use with this noder.
        /// A <see cref="ISegmentIntersector" />  will normally add intersection nodes
        /// to the input segment strings, but it may not - it may
        /// simply record the presence of intersections.
        /// However, some <see cref="INoder" />s may require that intersections be added.
        /// </summary>
        public ISegmentIntersector SegmentIntersector
        {
            get
            {
                return _segInt;
            }
            set
            {
                _segInt = value;
            }
        }

        #region INoder Members

        /// <summary>
        /// Computes the noding for a collection of <see cref="SegmentString"/>s.
        /// Some Noders may add all these nodes to the input <see cref="SegmentString"/>s;
        /// others may only add some or none at all.
        /// </summary>
        /// <param name="segStrings"></param>
        public abstract void ComputeNodes(IList segStrings);

        /// <summary>
        /// Returns a <see cref="IList"/> of fully noded <see cref="SegmentString"/>s.
        /// The <see cref="SegmentString"/>s have the same context as their parent.
        /// </summary>
        /// <returns></returns>
        public abstract IList GetNodedSubstrings();

        #endregion
    }
}