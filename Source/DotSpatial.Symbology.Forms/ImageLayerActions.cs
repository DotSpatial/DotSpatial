﻿// ********************************************************************************************************
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
    /// Actions that occur on an image layer in the legend.
    /// </summary>
    public class ImageLayerActions : LegendItemActionsBase, IImageLayerActions
    {
        /// <summary>
        /// Show the properties of an image layer in the legend. 
        /// </summary>
        /// <param name="e"></param>
        public void ShowProperties(IImageLayer e)
        {
            using (var dlg = new LayerDialog(e,new ImageCategoryControl()))
            {
                ShowDialog(dlg);
            }
        }

        /// <summary>
        /// Export data from an image layer.
        /// </summary>
        /// <param name="e"></param>
        public void ExportData(IImageData e)
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