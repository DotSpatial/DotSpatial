// ********************************************************************************************************
// Product Name: DotSpatial.Forms.LayoutScaleBarMapEditor
// Description:  Used to select which map layout elements link to
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll version 6.0
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Jul, 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// ------------------|------------|---------------------------------------------------------------
// Ted Dunsford      | 8/28/2009  | Cleaned up some code formatting using resharper
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Layout Map Editor is a UIType Editor that allows selecting a new map.
    /// </summary>
    public class LayoutMapEditor : UITypeEditor
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
        /// <param name="context">Contains the scalebar and legend.</param>
        /// <param name="provider">The service provider.</param>
        /// <param name="value">The selected item.</param>
        /// <returns>Returns the selected item.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _dialogProvider = provider?.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (context == null) return null;

            LayoutScaleBar scaleBar = context.Instance as LayoutScaleBar;
            LayoutLegend legend = context.Instance as LayoutLegend;
            LayoutControl lc = null;
            if (scaleBar?.LayoutControl != null)
                lc = scaleBar.LayoutControl;
            else if (legend?.LayoutControl != null)
                lc = legend.LayoutControl;

            ListBox lb = new ListBox();
            if (lc == null) return null;

            foreach (LayoutElement le in lc.LayoutElements.FindAll(o => o is LayoutMap))
            {
                lb.Items.Add(le);
            }

            lb.SelectedItem = value;
            lb.SelectedValueChanged += LbSelectedValueChanged;
            _dialogProvider?.DropDownControl(lb);
            return lb.SelectedItem;
        }

        /// <summary>
        /// Gets the UITypeEditorEditStyle, which in this case is drop down.
        /// </summary>
        /// <param name="context">not used</param>
        /// <returns>The UITypeEditorEditStyle.</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        private void LbSelectedValueChanged(object sender, EventArgs e)
        {
            _dialogProvider.CloseDropDown();
        }

        #endregion
    }
}