// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Projections library.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
    /// This member is virtual to allow custom event handlers to be used instead.
    /// </summary>
    public class RasterLayerEventReceiver
    {
        private readonly RasterLayerEventSender _rasterEventSender = RasterLayerEventSender.Instance;

        /// <summary>
        /// Creates a new RasterLayerEventReceiver
        /// </summary>
        public RasterLayerEventReceiver()
        {
            _rasterEventSender.ExportDataClicked += Raster_ExportDataClicked;
            _rasterEventSender.ShowPropertiesClicked += Raster_PropertiesClicked;
        }

        /// <summary>
        /// Allows setting the owner for any dialogs that need to be launched.
        /// </summary>
        public IWin32Window Owner { get; set; }

        private void Raster_PropertiesClicked(object sender, RasterLayerEventArgs e)
        {
            LayerDialog dlg = new LayerDialog(e.RasterLayer, new RasterCategoryControl());
            dlg.ShowDialog(Owner);
        }

        private static void Raster_ExportDataClicked(object sender, RasterEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
                                 {
                                     Filter = DataManager.DefaultDataManager.RasterWriteFilter
                                 };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            e.Raster.SaveAs(sfd.FileName);
        }
    }
}