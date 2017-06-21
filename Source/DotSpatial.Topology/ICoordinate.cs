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

using System;
using System.ComponentModel;

namespace DotSpatial.Topology
{
    /// <summary>
    /// Adds any topology functions to the basic Vector ICoordinate
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public interface ICoordinate : ICloneable, IComparable
    {
        #region Methods

        /// <summary>
        /// Return HashCode.
        /// </summary>
        int GetHashCode();

        #endregion

        #region Properties

        /// <summary>
        /// A Measure coordinate
        /// </summary>
        double M { get; set; }

        /// <summary>
        /// A 1D coordinate only has a valid X.  A 2D coordinate has X and Y, while a 3D coordinate
        /// has X, Y, and Z.  Presumably this is open ended and could support higher coordinates,
        /// but this coordinate is not responsible for storing values beyond its dimension and
        /// may cause exceptions if a value higher than the dimension is used.
        /// </summary>
        int NumOrdinates
        {
            get;
        }

        /// <summary>
        /// Gets a double value for the specified zero based ordinate.
        /// </summary>
        /// <param name="index">The zero based integer ordinate.</param>
        /// <returns>A double value.</returns>
        double this[int index]
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the values as a one dimensional array of doubles.
        /// </summary>
        double[] Values
        {
            get;
            set;
        }

        /// <summary>
        /// The X coordinate
        /// </summary>
        double X { get; set; }

        /// <summary>
        /// The Y coordinate
        /// </summary>
        double Y { get; set; }

        /// <summary>
        /// The Z coordinate
        /// </summary>
        double Z { get; set; }

        #endregion
    }
}