// ********************************************************************************************************
// Product Name: DotSpatial.Forms.LayoutLayerEditor
// Description:  Used to pick which layers show up in the layout legend
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
// The Original Code is DotSpatial.dll version 6.0
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Sept, 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Layout Layer Editor allows the user to select which layers appear in the legend
    /// </summary>
    internal class LayoutLayerEditor : UITypeEditor
    {
        IWindowsFormsEditorService _dialogProvider;

        #region Methods

        /// <summary>
        /// Edits a value based on some user input which is collected from a character control.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _dialogProvider = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            List<int> layerList = new List<int>();
            LayoutLegend legend = context.Instance as LayoutLegend;
            LayoutMap map = null;
            if (legend != null)
                map = legend.Map;
            if (map == null)
                return layerList;

            CheckedListBox lb = new CheckedListBox();

            List<int> originalList = value as List<int>;
            if (originalList != null)
            {
                for (int i = map.MapControl.Layers.Count - 1; i >= 0; i--)
                    lb.Items.Add(map.MapControl.Layers[i].LegendText, originalList.Contains(i));
            }

            if (_dialogProvider != null) _dialogProvider.DropDownControl(lb);

            for (int i = 0; i < lb.Items.Count; i++)
            {
                if (lb.GetItemChecked(i))
                    layerList.Add(lb.Items.Count - 1 - i);
            }

            return layerList;
        }

        /// <summary>
        /// Gets the UITypeEditorEditStyle, which in this case is drop down.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Ensures that we can widen the drop-down without having to close the drop down,
        /// widen the control, and re-open it again.
        /// </summary>
        public override bool IsDropDownResizable
        {
            get { return false; }
        }

        #endregion
    }
}