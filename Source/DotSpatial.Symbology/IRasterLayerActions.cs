// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This interface provides ability to use in RasterLayer any custom actions (including GUI-dependent dialogs)
    /// </summary>
    public interface IRasterLayerActions
    {
        /// <summary>
        /// SHow properties dialog
        /// </summary>
        /// <param name="e">Raster layer</param>
        void ShowProperties(IRasterLayer e);

        /// <summary>
        /// Show export dialog
        /// </summary>
        /// <param name="e">Raster</param>
        void ExportData(IRaster e);
    }
}