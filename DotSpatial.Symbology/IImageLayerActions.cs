// *******************************************************************************************************
// Product:  DotSpatial.Symbology.IImageLayerActions
// Description:  Contains methods which can be used in ImageLayer (e.g. in ContextMenu)
// Copyright & License: See www.DotSpatial.org.
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// Max Miroshnikov    |  3/2013            |  Initial commit
// *******************************************************************************************************

using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This interface provides ability to use in ImageLayer any custom actions (including GUI-dependent dialogs)
    /// </summary>
    public interface IImageLayerActions
    {
        /// <summary>
        /// Show properties dialog
        /// </summary>
        /// <param name="e">Image layer</param>
        void ShowProperties(IImageLayer e);

        /// <summary>
        /// Show export dialog
        /// </summary>
        /// <param name="e">Image data</param>
        void ExportData(IImageData e);
    }
}