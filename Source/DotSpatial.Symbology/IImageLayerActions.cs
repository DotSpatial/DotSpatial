// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This interface provides ability to use in ImageLayer any custom actions (including GUI-dependent dialogs)
    /// </summary>
    public interface IImageLayerActions
    {
        #region Methods

        /// <summary>
        /// Show export dialog
        /// </summary>
        /// <param name="e">Image data</param>
        void ExportData(IImageData e);

        /// <summary>
        /// Show properties dialog
        /// </summary>
        /// <param name="e">Image layer</param>
        void ShowProperties(IImageLayer e);

        #endregion
    }
}