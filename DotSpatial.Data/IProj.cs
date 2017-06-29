// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description: The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/5/2009 1:12:10 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;

namespace DotSpatial.Data
{
    /// <summary>
    /// The Projection interface that acts as a useful tool for calculating pixel coordinates
    /// to geographic coordinates and vise-versa.
    /// </summary>
    public interface IProj
    {
        #region Properties

        /// <summary>
        /// The Rectangle representation of the geographic extents in image coordinates
        /// </summary>
        Rectangle ImageRectangle
        {
            get;
        }

        /// <summary>
        /// The geographic extents used for projection.
        /// </summary>
        Extent GeographicExtents
        {
            get;
        }

        #endregion
    }
}