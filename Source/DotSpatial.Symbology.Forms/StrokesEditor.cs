// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// StrokesEditor
    /// </summary>
    public class StrokesEditor : UITypeEditor
    {
        #region Fields

        private ILineSymbolizer _editCopy;
        private ILineSymbolizer _original;

        #endregion

        #region Methods

        /// <summary>
        /// Launches a form for editing the line symbolizer.
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="provider">The service provider.</param>
        /// <param name="value">Not used.</param>
        /// <returns>The strokes.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _original = context?.Instance as ILineSymbolizer;
            IWindowsFormsEditorService dialogProvider = (IWindowsFormsEditorService)provider?.GetService(typeof(IWindowsFormsEditorService));
            _editCopy = _original.Copy();
            DetailedLineSymbolDialog dialog = new DetailedLineSymbolDialog(_editCopy);
            dialog.ChangesApplied += DialogChangesApplied;
            if (_original != null && dialogProvider != null)
                return dialogProvider.ShowDialog(dialog) != DialogResult.OK ? _original.Strokes : _editCopy.Strokes;
            return null;
        }

        /// <summary>
        /// Specifies that a form will be used for editing (a Modal form behaves like a dialog)
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <returns>The UITypeEditorEditStyle</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        private void DialogChangesApplied(object sender, EventArgs e)
        {
            _original.CopyProperties(_editCopy);
        }

        #endregion
    }
}