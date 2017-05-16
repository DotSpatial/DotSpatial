// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Projections;

namespace DotSpatial.Data
{
    /// <summary>
    /// This interface supports the basic Reprojection content. CanReproject tests to see if the
    /// DotSpatial.Projections library exists and will return false if it does not exist. In
    /// such a case the
    /// </summary>
    public interface IReproject
    {
        /// <summary>
        /// Gets a value indicating whether the DotSpatial.Projections assembly is loaded. If
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
        /// Gets or sets the proj4 string for this dataset. This exists in
        /// case the Projections library is not loaded. If the projection
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