// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
// The Original Code is from MapWindow.dll version 6.0
// The Initial Developer of this Original Code is Ted Dunsford.  Created 11/25/2010 8:55 AM
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// ********************************************************************************************************

using DotSpatial.Projections;

namespace DotSpatial.Data
{
    /// <summary>
    /// This interface supports the basic Reprojection content.  CanReproject tests to see if the
    /// DotSpatial.Projections library exists and will return false if it does not exist.  In
    /// such a case the
    /// </summary>
    public interface IReproject
    {
        /// <summary>
        /// Gets a value indicating whether the DotSpatial.Projections assembly is loaded.  If
        /// not, this returns false, and neither ProjectionInfo nor the Reproject() method should
        /// be used.
        /// </summary>
        /// <returns>Boolean, true if the value can reproject.</returns>
        bool CanReproject { get; }

        /// <summary>
        /// Gets or sets the projection information for this dataset
        /// </summary>
        ProjectionInfo Projection { get; set; }

        /// <summary>
        /// Gets or sets the proj4 string for this dataset.  This exists in
        /// case the Projections library is not loaded.  If the projection
        /// library is loaded, this also updates the Projection property.
        /// Setting this behaves like defining the projection and will not
        /// reproject the values.
        /// </summary>
        string ProjectionString { get; set; }

        /// <summary>
        /// Reprojects all of the in-ram vertices of vectors, or else this
        /// simply updates the "Bounds" of image and raster objects
        /// This will also update the projection to be the specified projection.
        /// </summary>
        /// <param name="targetProjection">
        /// The projection information to reproject the coordinates to.
        /// </param>
        void Reproject(ProjectionInfo targetProjection);
    }
}