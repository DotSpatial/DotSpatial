// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// Default implementation of IColorCategoryActions
    /// </summary>
    public class ColorCategoryActions : LegendItemActionsBase, IColorCategoryActions
    {
        /// <summary>
        /// Show the color category editor form.
        /// </summary>
        /// <param name="category">The color category.</param>
        public void ShowEdit(IColorCategory category)
        {
            using (var frm = new ColorPicker(category))
            {
                ShowDialog(frm);
            }
        }
    }
}