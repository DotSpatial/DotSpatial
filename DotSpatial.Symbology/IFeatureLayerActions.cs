// *******************************************************************************************************
// Product:  DotSpatial.Symbology.IFeatureLayerActions
// Description:  Contains methods which can be used in FeatureLayer (e.g. in ContextMenu)
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
    /// This interface provides ability to use in FeatureLayer any custom actions (including GUI-dependent dialogs)
    /// </summary>
    public interface IFeatureLayerActions
    {
        /// <summary>
        /// Show properties
        /// </summary>
        /// <param name="e">Feature layer</param>
        void ShowProperties(IFeatureLayer e);

        /// <summary>
        /// Show Join Excel
        /// </summary>
        /// <param name="e">Feature set</param>
        void ExcelJoin(IFeatureSet e);

        /// <summary>
        /// Show dynamic visibility dialog
        /// </summary>
        /// <param name="e">Dynamic visibility</param>
        void LabelExtents(IDynamicVisibility e);

        /// <summary>
        /// Show label setup dialog
        /// </summary>
        /// <param name="e">Label layer</param>
        void LabelSetup(ILabelLayer e);

        /// <summary>
        /// Show attributes dialog
        /// </summary>
        /// <param name="e">Feature layer</param>
        void ShowAttributes(IFeatureLayer e);

        /// <summary>
        /// Show export dialog
        /// </summary>
        /// <param name="e">Feature layer</param>
        void ExportData(IFeatureLayer e);
    }
}