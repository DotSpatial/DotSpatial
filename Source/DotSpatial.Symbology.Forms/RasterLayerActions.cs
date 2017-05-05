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
    public class RasterLayerActions : LegendItemActionsBase, IRasterLayerActions
    {
        #region Methods

        /// <summary>
        /// Export data from a raster layer.
        /// </summary>
        /// <param name="raster">The raster that contains the data that gets exported.</param>
        public void ExportData(IRaster raster)
        {
            using (var sfd = new SaveFileDialog { Filter = DataManager.DefaultDataManager.RasterWriteFilter })
            {
                if (ShowDialog(sfd) == DialogResult.OK)
                {
                    raster.SaveAs(sfd.FileName);
                }
            }
        }

        /// <summary>
        /// Shows the properties of the current raster legend item.
        /// </summary>
        /// <param name="layer">The raster layer</param>
        public void ShowProperties(IRasterLayer layer)
        {
            using (var dlg = new LayerDialog(layer, new RasterCategoryControl()))
            {
                ShowDialog(dlg);
            }
        }

        #endregion
    }
}