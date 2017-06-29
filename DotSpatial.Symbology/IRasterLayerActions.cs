// *******************************************************************************************************
// Product:  DotSpatial.Symbology.IRasterLayerActions
// Description:  Contains methods which can be used in RasterLayer (e.g. in ContextMenu)
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