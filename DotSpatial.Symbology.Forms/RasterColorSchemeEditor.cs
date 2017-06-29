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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/1/2008 11:02:31 AM
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
    /// RasterColorSchemeEditor
    /// </summary>
    public class RasterColorSchemeEditor : UITypeEditor
    {
        /// <summary>
        /// This should launch a frmRasterSymbolizer
        /// </summary>
        /// <param name="context">ITypeDescriptorContext context</param>
        /// <param name="provider">IServiceProvider provider</param>
        /// <param name="value">object value</param>
        /// <returns>A new RasterSymbolizer</returns>
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
        /// This controls the editor style and sets up a backup copy of the symbolizer
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <returns>UITypeEditorEditStyle</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}