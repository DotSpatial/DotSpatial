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

using System.Collections.Generic;

namespace DotSpatial.Topology.Geometries
{
    /// <summary>
    /// Full powered topology style Polygon
    /// </summary>
    public interface IPolygon :  ISurface, IPolygonal
    {
        #region Properties

        ILineString ExteriorRing { get; set; }

        /// <summary>
        /// Gets the list of Interior Rings in the form of ILineStringBase objects
        /// </summary>
        ILinearRing[] Holes
        {
            get;
            set;
        }

        ILineString[] InteriorRings { get; }

        /// <summary>
        /// Gets the count of holes or interior rings
        /// </summary>
        int NumHoles { get; }

        int NumInteriorRings { get; }

        /// <summary>
        /// Gets the exterior ring of the polygon as an ILineStringBase.
        /// </summary>
        ILinearRing Shell
        {
            get;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a specific <see cref="ILinearRing">ILinearRing</see> identified by the 0 based index n
        /// </summary>
        /// <param name="n">A 0 based integer index enumerating the rings</param>
        /// <returns><see cref="ILinearRing">ILinearRing</see> outlining the n'th hole in the polygon</returns>
        ILineString GetInteriorRingN(int n);

        #endregion
    }
}