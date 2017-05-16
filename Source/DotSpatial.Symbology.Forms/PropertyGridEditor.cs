// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// PropertyGridEditor
    /// </summary>
    public class PropertyGridEditor : UITypeEditor
    {
        /// <summary>
        /// This should launch an open file dialog instead of the usual thing.
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="provider">IServiceProvider</param>
        /// <param name="value">The object being displayed</param>
        /// <returns>A new version of the object if the dialog was ok.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            ICloneable parent = value as ICloneable;
            object backup = parent != null ? parent.Clone() : value;

            IWindowsFormsEditorService dialogProvider = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            PropertyDialog frm = new PropertyDialog();
            frm.PropertyGrid.SelectedObject = value;
            return dialogProvider.ShowDialog(frm) == DialogResult.OK ? value : backup;
        }

        /// <summary>
        /// Either allows the editor to work or else nips it in the butt
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <returns>UITypeEditorEditStyle</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}