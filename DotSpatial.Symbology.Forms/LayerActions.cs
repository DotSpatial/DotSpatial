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
    /// Actions that occur on a layer legend item.
    /// </summary>
    public class LayerActions : LegendItemActionsBase, ILayerActions
    {
        /// <summary>
        /// Determines whether a layer has dynamic visibility and hence is only shown at certain scales.
        /// </summary>
        /// <param name="e"></param>
        public void DynamicVisibility(IDynamicVisibility e, IFrame MapFrame)
        {
            using (var dvg = new DynamicVisibilityModeDialog())
            {
                var result = ShowDialog(dvg);

                if (result == DialogResult.OK)
                {
                    e.DynamicVisibilityMode = dvg.DynamicVisibilityMode;
                    e.UseDynamicVisibility = true;
                    e.DynamicVisibilityWidth = MapFrame.ViewExtents.Width;
                }

                if (result == DialogResult.No)
                {
                    e.UseDynamicVisibility = false;
                }
            }
        }
    }
}