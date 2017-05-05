// *******************************************************************************************************
// Product: DotSpatial.Symbology.Forms.ColorCategoryActions
// Description: Implementation of IColorCategoryActions
//
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders.
// -------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
// -------------------|--------------------|--------------------------------------------------------------
// Max Miroshnikov    |  3/2013            |  Initial commit
// *******************************************************************************************************

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