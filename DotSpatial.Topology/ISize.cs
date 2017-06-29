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
// *********************************************************************************************************

using System.ComponentModel;

namespace DotSpatial.Topology
{
    /// <summary>
    /// ISize interface for expressing a length in the X, Y or Z directions
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public interface ISize
    {
        #region Properties

        /// <summary>
        /// Gets or sets the size in the x direction or longitude
        /// </summary>
        double XSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size in the y direction or latitude
        /// </summary>
        double YSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size in the z direction or altitude
        /// </summary>
        double ZSize
        {
            get;
            set;
        }

        #endregion
    }
}