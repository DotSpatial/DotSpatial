// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// RasterColorSchemeEditor.
    /// </summary>
    public class RasterColorSchemeEditor : UITypeEditor
    {
        /// <summary>
        /// This should launch a frmRasterSymbolizer.
        /// </summary>
        /// <param name="context">ITypeDescriptorContext context.</param>
        /// <param name="provider">IServiceProvider provider.</param>
        /// <param name="value">object value.</param>
        /// <returns>A new RasterSymbolizer.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IRasterSymbolizer rs = value as IRasterSymbolizer ?? new RasterSymbolizer();

            IWindowsFormsEditorService dialogProvider = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            IRasterLayer parent = rs.ParentLayer;
            LayerDialog frm = new LayerDialog(parent, new RasterCategoryControl());
            dialogProvider.ShowDialog(frm);
            return parent.Symbolizer;
        }

        /// <summary>
        /// This controls the editor style and sets up a backup copy of the symbolizer.
        /// </summary>
        /// <param name="context">ITypeDescriptorContext.</param>
        /// <returns>UITypeEditorEditStyle.</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}