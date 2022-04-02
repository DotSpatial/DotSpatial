// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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
        /// <param name="layer">The image layer.</param>
        public void ShowProperties(IImageLayer layer)
        {
            using var dlg = new LayerDialog(layer, new ImageCategoryControl());
            ShowDialog(dlg);
        }

        /// <summary>
        /// Export data from an image layer.
        /// </summary>
        /// <param name="data">The image data.</param>
        public void ExportData(IImageData data)
        {
            using var sfd = new SaveFileDialog
            {
                Filter = DataManager.DefaultDataManager.RasterWriteFilter
            };
            if (ShowDialog(sfd) == DialogResult.OK)
            {
                data.SaveAs(sfd.FileName);
            }
        }
    }
}