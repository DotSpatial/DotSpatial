// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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
        /// <param name="e">The dynamic visibility object that gets manipulated.</param>
        /// <param name="mapFrame">The map frame.</param>
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