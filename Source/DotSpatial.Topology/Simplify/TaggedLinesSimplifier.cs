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

namespace DotSpatial.Topology.Simplify
{
    /// <summary>
    /// Simplifies a collection of TaggedLineStrings, preserving topology
    /// (in the sense that no new intersections are introduced).
    /// </summary>
    public class TaggedLinesSimplifier
    {
        private readonly LineSegmentIndex _inputIndex = new LineSegmentIndex();
        private readonly LineSegmentIndex _outputIndex = new LineSegmentIndex();
        private double _distanceTolerance;

        /// <summary>
        /// Gets/Sets the distance tolerance for the simplification.
        /// Points closer than this tolerance to a simplified segment may
        /// be removed.
        /// </summary>
        public virtual double DistanceTolerance
        {
            get
            {
                return _distanceTolerance;
            }
            set
            {
                _distanceTolerance = value;
            }
        }

        /// <summary>
        /// Simplify a collection of <c>TaggedLineString</c>s.
        /// </summary>
        /// <param name="taggedLines">The collection of lines to simplify.</param>
        public virtual void Simplify(IList taggedLines)
        {
            for (IEnumerator i = taggedLines.GetEnumerator(); i.MoveNext(); )
                _inputIndex.Add((TaggedLineString)i.Current);
            for (IEnumerator i = taggedLines.GetEnumerator(); i.MoveNext(); )
            {
                TaggedLineStringSimplifier tlss
                              = new TaggedLineStringSimplifier(_inputIndex, _outputIndex);
                tlss.DistanceTolerance = _distanceTolerance;
                tlss.Simplify((TaggedLineString)i.Current);
            }
        }
    }
}