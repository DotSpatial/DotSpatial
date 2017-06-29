// *******************************************************************************************************
// Product: DotSpatial.Symbology.Forms.ColorCategoryActions
// Description: Implementation of IColorCategoryActions
// Copyright & License: See www.DotSpatial.org.
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
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
        /// <param name="e"></param>
        public void ShowEdit(IColorCategory e)
        {
            using (var frm = new ColorPicker(e))
            {
                ShowDialog(frm);
            }
        }
    }
}