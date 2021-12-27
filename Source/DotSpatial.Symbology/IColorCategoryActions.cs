// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This interface provides ability to use in ColorCategory any custom actions (including GUI-dependent dialogs).
    /// </summary>
    public interface IColorCategoryActions
    {
        /// <summary>
        /// Show edit dialog.
        /// </summary>
        /// <param name="e">Instance of ColorCategory.</param>
        void ShowEdit(IColorCategory e);
    }
}