// ********************************************************************************************************
// Product Name: DotSpatial.Forms.LayoutScaleBarMapEditor
// Description:  Used to select which map layout elements link to
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
    /// Layout Map Editor is a UIType Editor that allows selecting a new map
    /// </summary>
    public class LayoutMapEditor : UITypeEditor
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
            LayoutScaleBar scaleBar = context.Instance as LayoutScaleBar;
            LayoutLegend legend = context.Instance as LayoutLegend;
            LayoutControl lc = null;
            if (scaleBar != null && scaleBar.LayoutControl != null)
                lc = scaleBar.LayoutControl;
            else if (legend != null && legend.LayoutControl != null)
                lc = legend.LayoutControl;

            ListBox lb = new ListBox();
            if (lc != null)
            {
                foreach (LayoutElement le in lc.LayoutElements.FindAll(o => (o is LayoutMap)))
                    lb.Items.Add(le);
                lb.SelectedItem = value;
            }
            else return null;
            lb.SelectedValueChanged += LbSelectedValueChanged;
            if (_dialogProvider != null) _dialogProvider.DropDownControl(lb);
            return lb.SelectedItem;
        }

        private void LbSelectedValueChanged(object sender, EventArgs e)
        {
            _dialogProvider.CloseDropDown();
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