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

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This member is virtual to allow custom event handlers to be used instead.
    /// </summary>
    public class LayerEventReceiver
    {
        private readonly LayerEventSender _layerEventSender = LayerEventSender.Instance;

        /// <summary>
        /// Creates a new instance of the LayerEventHandler
        /// </summary>
        public LayerEventReceiver()
        {
            _layerEventSender.EditColorBreakClicked += ColorCategory_EditClicked;
            _layerEventSender.EditDynamicVisibilityClicked += DynamicVisibility;
        }

        /// <summary>
        /// Allows setting the owner for any dialogs that need to be launched.
        /// </summary>
        public IWin32Window Owner { get; set; }

        private void ColorCategory_EditClicked(object sender, ColorCategoryEventArgs e)
        {
            ColorPicker frm = new ColorPicker(e.ColorCategory);
            frm.ShowDialog(Owner);
        }

        private void DynamicVisibility(object sender, DynamicVisibilityEventArgs e)
        {
            DynamicVisibilityModeDialog dvg = new DynamicVisibilityModeDialog();
            dvg.ShowDialog(Owner);
            e.Item.DynamicVisibilityMode = dvg.DynamicVisibilityMode;
        }
    }
}