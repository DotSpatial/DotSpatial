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
            using (var dlg = new PropertyDialog())
            {
                dlg.PropertyGrid.SelectedObject = e.Copy();
                dlg.OriginalObject = e;
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