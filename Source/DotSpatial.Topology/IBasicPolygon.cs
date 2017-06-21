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

using System.Collections.Generic;

namespace DotSpatial.Topology
{
    /// <summary>
    /// This supports some of the basic data-related capabilities of a polygon, but no topology functions.
    /// Each of these uses the specifically different nomenclature so that the parallel concepts in a
    /// full Polygon can return the appropriate datatype.  Since Polygon will Implement IPolygonBase, it
    /// is the responsibility of the developer to perform the necessary casts when returning this
    /// set from the more complete topology classes.
    /// </summary>
    public interface IBasicPolygon : IBasicGeometry
    {
        /// <summary>
        /// Gets the list of Interior Rings in the form of ILineStringBase objects
        /// </summary>
        ICollection<IBasicLineString> Holes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the exterior ring of the polygon as an ILineStringBase.
        /// </summary>
        IBasicLineString Shell
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the count of holes or interior rings
        /// </summary>
        int NumHoles { get; }
    }
}