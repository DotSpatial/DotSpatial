// ********************************************************************************************************
// Product Name: DotSpatial.Forms.LayoutLayerEditor
// Description:  Used to pick which layers show up in the layout legend
//
// ********************************************************************************************************
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
    /// Layout Layer Editor allows the user to select which layers appear in the legend.
    /// </summary>
    internal class LayoutLayerEditor : UITypeEditor
    {
        #region Fields

        private IWindowsFormsEditorService _dialogProvider;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether we can widen the drop-down without having to close the drop down,
        /// widen the control, and re-open it again.
        /// </summary>
        public override bool IsDropDownResizable => false;

        #endregion

        #region Methods

        /// <summary>
        /// Edits a value based on some user input which is collected from a character control.
        /// </summary>
        /// <param name="context">Contains the layout legend.</param>
        /// <param name="provider">The service provider.</param>
        /// <param name="value">List with the items that should be added to the CheckedListBox.</param>
        /// <returns>List with the items.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _dialogProvider = provider?.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            List<int> layerList = new List<int>();
            LayoutLegend legend = context?.Instance as LayoutLegend;
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

            _dialogProvider?.DropDownControl(lb);

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
        /// <param name="context">not used</param>
        /// <returns>The UITypeEditorEditStyle</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        #endregion
    }
}