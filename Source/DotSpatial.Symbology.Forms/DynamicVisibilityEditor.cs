// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/27/2009 5:08:52 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// DynamicVisibilityEditor
    /// </summary>
    public class DynamicVisibilityEditor : UITypeEditor
    {
        #region Fields

        private ILayer _layer;

        #endregion

        #region Methods

        /// <summary>
        /// Display a drop down when editing instead of the normal control, and allow the user to "grab" a
        /// new dynamic visibility extent.
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="provider">The service provider.</param>
        /// <param name="value">Not used.</param>
        /// <returns>Returns whether or not to use the dynamic visibility.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _layer = context?.Instance as ILayer;
            IWindowsFormsEditorService dialogProvider = provider?.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            DynamicVisibilityControl dvc = new DynamicVisibilityControl(dialogProvider, _layer);
            dialogProvider?.DropDownControl(dvc);
            _layer?.Invalidate();
            return dvc.UseDynamicVisibility;
        }

        /// <summary>
        /// Indicate that we should use a drop-down for controlling dynamic visibility.
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <returns>The UITypeEditorEditStyle</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        #endregion
    }
}