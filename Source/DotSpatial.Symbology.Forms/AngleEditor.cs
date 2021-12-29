// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// CharacterCodeEditor.
    /// </summary>
    public class AngleEditor : UITypeEditor
    {
        #region Fields

        private IWindowsFormsEditorService _dialogProvider;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether we can widen the drop-down without having to close the drop down,
        /// widen the control, and re-open it again.
        /// </summary>
        public override bool IsDropDownResizable => true;

        #endregion

        #region Methods

        /// <summary>
        /// Edits a value based on some user input which is collected from a character control.
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="provider">The service provider.</param>
        /// <param name="value">The angle value.</param>
        /// <returns>The angle.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _dialogProvider = provider?.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            AngleControl ac = new AngleControl
            {
                Angle = Convert.ToInt32(value)
            };
            ac.AngleChosen += AcAngleChosen;
            _dialogProvider?.DropDownControl(ac);
            return (double)ac.Angle;
        }

        /// <summary>
        /// Gets the UITypeEditorEditStyle, which in this case is drop down.
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <returns>The UITypeEditorEditStyle.</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        private void AcAngleChosen(object sender, EventArgs e)
        {
            _dialogProvider.CloseDropDown();
        }

        #endregion
    }
}