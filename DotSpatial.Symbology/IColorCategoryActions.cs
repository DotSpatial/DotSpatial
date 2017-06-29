// *******************************************************************************************************
// Product:  DotSpatial.Symbology.IColorCategoryActions
// Description:  Contains methods which can be used in ColorCategory (e.g. in ContextMenu)
// Copyright & License: See www.DotSpatial.org.
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// Max Miroshnikov    |  3/2013            |  Initial commit
// *******************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This interface provides ability to use in ColorCategory any custom actions (including GUI-dependent dialogs)
    /// </summary>
    public interface IColorCategoryActions
    {
        /// <summary>
        /// Show edit dialog
        /// </summary>
        /// <param name="e">Instance of ColorCategory</param>
        void ShowEdit(IColorCategory e);
    }
}