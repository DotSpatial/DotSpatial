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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/21/2010 3:15:10 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;

namespace DotSpatial.Data
{
    /// <summary>
    ///
    /// </summary>
    public class ProjectionHelper : IProj
    {
        /// <summary>
        /// Initializes a new instance of the ProjectionHelper class.
        /// </summary>
        /// <param name="geographicExtents">The geographic extents to project to and from.</param>
        /// <param name="viewRectangle">The view rectangle in pixels to transform with.</param>
        public ProjectionHelper(Extent geographicExtents, Rectangle viewRectangle)
        {
            GeographicExtents = geographicExtents;
            ImageRectangle = viewRectangle;
        }

        #region IProj Members

        /// <summary>
        /// Gets or sets the geographic extent to use.
        /// </summary>
        public Extent GeographicExtents { get; set; }

        /// <summary>
        /// Gets or sets the rectangular pixel region to use.
        /// </summary>
        public Rectangle ImageRectangle { get; set; }

        #endregion
    }
}