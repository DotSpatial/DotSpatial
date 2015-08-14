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
using Wintellect.PowerCollections;

namespace DotSpatial.Topology.Noding
{
    /// <summary>
    /// Dissolves a noded collection of <see cref="ISegmentString" />s to produce
    /// a set of merged linework with unique segments.
    /// </summary>
    /// <remarks>
    /// A custom <see cref="ISegmentStringMerger"/> merging strategy
    /// can be supplied.  
    /// This strategy will be called when two identical (up to orientation)
    /// strings are dissolved together.
    /// The default merging strategy is simply to discard one of the merged strings.
    ///<para>
    /// A common use for this class is to merge noded edges
    /// while preserving topological labelling.
    /// This requires a custom merging strategy to be supplied 
    /// to merge the topology labels appropriately.
    /// </para>
    ///</remarks>
    public class SegmentStringDissolver
    {
        #region Fields

        private readonly ISegmentStringMerger _merger;

        private readonly IDictionary<OrientedCoordinateArray, ISegmentString> _ocaMap =
            new OrderedDictionary<OrientedCoordinateArray, ISegmentString>();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a dissolver with a user-defined merge strategy.
        /// </summary>
        /// <param name="merger"></param>
        public SegmentStringDissolver(ISegmentStringMerger merger)
        {
            _merger = merger;
        }

        /// <summary>
        /// Creates a dissolver with the default merging strategy.
        /// </summary>
        public SegmentStringDissolver() : this(null) { }

        #endregion

        #region Interfaces

        /// <summary>
        /// A merging strategy which can be used to update the context data of <see cref="ISegmentString"/>s 
        /// which are merged during the dissolve process.
        /// </summary>
        /// <author>mbdavis</author>
        public interface ISegmentStringMerger
        {
            #region Methods

            /// <summary>
            /// Updates the context data of a <see cref="ISegmentString" />
            /// when an identical (up to orientation) one is found during dissolving.
            /// </summary>
            /// <param name="mergeTarget">The segment string to update.</param>
            /// <param name="ssToMerge">The segment string being dissolved.</param>
            /// <param name="isSameOrientation">
            /// <c>true</c> if the strings are in the same direction,
            /// <c>false</c> if they are opposite.
            /// </param>
            void Merge(ISegmentString mergeTarget, ISegmentString ssToMerge, bool isSameOrientation);

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the collection of dissolved (i.e. unique) <see cref="ISegmentString" />s
        /// </summary>
        public ICollection<ISegmentString> Dissolved
        {
            get
            {
                return _ocaMap.Values;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Dissolve all <see cref="ISegmentString" />s in the input <see cref="IEnumerable{ISegmentString}"/>.
        /// </summary>
        /// <param name="segStrings"></param>
        public void Dissolve(IEnumerable<ISegmentString> segStrings)
        {
            foreach (var obj in segStrings)
                Dissolve(obj);
        }

        /// <summary>
        /// Dissolve the given <see cref="ISegmentString" />.
        /// </summary>
        /// <param name="segString"></param>
        public void Dissolve(ISegmentString segString)
        {
            OrientedCoordinateArray oca = new OrientedCoordinateArray(segString.Coordinates);
            ISegmentString existing = FindMatching(oca);
            if (existing == null)
                Add(oca, segString);
            else if (_merger != null)
            {
                bool isSameOrientation = Equals(existing.Coordinates, segString.Coordinates);
                _merger.Merge(existing, segString, isSameOrientation);
            }

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="oca"></param>
        /// <param name="segString"></param>
        private void Add(OrientedCoordinateArray oca, ISegmentString segString)
        {
            _ocaMap.Add(oca, segString);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="oca"></param>
        /// <returns></returns>
        private ISegmentString FindMatching(OrientedCoordinateArray oca)
        {
            ISegmentString ret;
            return _ocaMap.TryGetValue(oca, out ret) ? ret : null;
        }

        #endregion
    }
}