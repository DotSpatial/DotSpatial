// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// Generic interface for category controls.
    /// </summary>
    public interface ICategoryControl
    {
        /// <summary>
        /// Initializes the specified layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        void Initialize(ILayer layer);

        /// <summary>
        /// Applies the changes.
        /// </summary>
        void ApplyChanges();

        /// <summary>
        /// Cancels changes.
        /// </summary>
        void Cancel();
    }
}