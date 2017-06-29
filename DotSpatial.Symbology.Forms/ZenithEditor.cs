// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/22/2009 3:10:14 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// OpacityEditor
    /// </summary>
    public class ZenithEditor : UITypeEditor
    {
        #region Private Variables

        IWindowsFormsEditorService _dialogProvider;

        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// Edits the value by showing a slider control in the drop down.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _dialogProvider = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            RampSlider rs = new RampSlider();
            rs.Maximum = 90;
            rs.Minimum = 0;
            rs.MaximumColor = Color.SteelBlue;
            rs.MinimumColor = Color.Transparent;
            rs.RampText = "Zenith";
            rs.RampTextBehindRamp = false;
            rs.Value = Convert.ToDouble(value);
            rs.ValueChanged += RsValueChanged;
            rs.ShowValue = false;
            rs.Width = 75;
            rs.Height = 50;
            if (_dialogProvider != null) _dialogProvider.DropDownControl(rs);
            return (float)rs.Value;
        }

        private void RsValueChanged(object sender, EventArgs e)
        {
            _dialogProvider.CloseDropDown();
        }

        /// <summary>
        /// Sets the behavior to drop-down
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
            get
            {
                return true;
            }
        }

        #endregion

        #region Protected Methods

        #endregion
    }
}