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
    public class ImageLayerEventReceiver
    {
        private readonly ImageLayerEventSender _imageEventSender = ImageLayerEventSender.Instance;

        /// <summary>
        /// Creates a new ImagelayerEventReceiver
        /// </summary>
        public ImageLayerEventReceiver()
        {
            _imageEventSender.ExportDataClicked += Image_ExportDataClicked;
            _imageEventSender.ShowPropertiesClicked += Image_PropertiesClicked;
        }

        /// <summary>
        /// Allows setting the owner for any dialogs that need to be launched.
        /// </summary>
        public IWin32Window Owner { get; set; }

        private void Image_PropertiesClicked(object sender, ImageLayerEventArgs e)
        {
            using (PropertyDialog dlg = new PropertyDialog())
            {
                dlg.PropertyGrid.SelectedObject = e.ImageLayer.Copy();
                dlg.OriginalObject = e.ImageLayer;
                dlg.ShowDialog(Owner);
            }
        }

        private static void Image_ExportDataClicked(object sender, ImageDataEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
                                 {
                                     Filter = DataManager.DefaultDataManager.RasterWriteFilter
                                 };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            e.ImageData.SaveAs(sfd.FileName);
        }
    }
}