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
namespace DotSpatial.Topology.Index.Strtree
{
    /// <summary>
    /// A spatial object in an AbstractSTRtree.
    /// </summary>
    public interface IBoundable
    {
        /// <summary>
        /// Returns a representation of space that encloses this Boundable, preferably
        /// not much bigger than this Boundable's boundary yet fast to test for intersection
        /// with the bounds of other Boundables. The class of object returned depends
        /// on the subclass of AbstractSTRtree.
        /// </summary>
        /// <returns>
        /// An Envelope (for STRtrees), an Interval (for SIRtrees), or other object
        /// (for other subclasses of AbstractSTRtree).
        /// </returns>
        object Bounds { get; }
    }
}