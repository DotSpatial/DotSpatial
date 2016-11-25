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

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// Actions that occur on a layer legend item.
    /// </summary>
    public class LayerActions : LegendItemActionsBase, ILayerActions
    {
        /// <summary>
        /// Determines whether a layer has dynamic visibility and hence is only shown at certain scales.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="mapFrame"></param>
        public void DynamicVisibility(IDynamicVisibility e, IFrame mapFrame)
        {
            using (var dvg = new DynamicVisibilityModeDialog())
            {
                switch (ShowDialog(dvg))
                {
                    case DialogResult.OK:
                        e.DynamicVisibilityMode = dvg.DynamicVisibilityMode;
                        e.UseDynamicVisibility = true;
                        e.DynamicVisibilityWidth = mapFrame.ViewExtents.Width;
                        break;
                    case DialogResult.No:
                        e.UseDynamicVisibility = false;
                        break;
                }
            }
        }
    }
}