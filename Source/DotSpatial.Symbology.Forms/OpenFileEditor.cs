// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// UIOpenFileEditor.
    /// </summary>
    public class OpenFileEditor : UITypeEditor
    {
        /// <summary>
        /// This should launch an open file dialog instead of the usual thing.
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="provider">The service provider.</param>
        /// <param name="value">Not used.</param>
        /// <returns>Returns the selected file name.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() != DialogResult.OK) return null;
                return ofd.FileName;
            }
        }

        /// <summary>
        /// Either allows the editor to work or else nips it in the butt.
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <returns>The UITypeEditorEditStyle.</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}