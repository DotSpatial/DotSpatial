// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// DynamicVisibilityEditor.
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
        /// <returns>The UITypeEditorEditStyle.</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        #endregion
    }
}