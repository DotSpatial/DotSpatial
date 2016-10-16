// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Projections library.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in September, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// Legend actions on a raster layer.
    /// </summary>
    public class RasterLayerActions: LegendItemActionsBase, IRasterLayerActions
    {
        /// <summary>
        /// Shows the properties of the current raster legend item.
        /// </summary>
        /// <param name="e"></param>
        public void ShowProperties(IRasterLayer e)
        {
            using (var dlg = new LayerDialog(e, new RasterCategoryControl()))
            {
                ShowDialog(dlg);
            }
        }

        /// <summary>
        /// Export data from a raster layer.
        /// </summary>
        /// <param name="e"></param>
        public void ExportData(IRaster e)
        {
            using (var sfd = new SaveFileDialog
                {
                    Filter = DataManager.DefaultDataManager.RasterWriteFilter
                })
            {
                if (ShowDialog(sfd) == DialogResult.OK)
                {
                    e.SaveAs(sfd.FileName);        
                }
            }
        }
    }
}