// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/29/2009 11:11:45 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// LineSymbolizerEditor
    /// </summary>
    public class StrokesEditor : UITypeEditor
    {
        #region Private Variables

        private ILineSymbolizer _editCopy;
        private ILineSymbolizer _original;

        #endregion

        #region Methods

        /// <summary>
        /// Launches a form for editing the line symbolizer
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _original = context.Instance as ILineSymbolizer;
            IWindowsFormsEditorService dialogProvider = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            _editCopy = _original.Copy();
            DetailedLineSymbolDialog dialog = new DetailedLineSymbolDialog(_editCopy);
            dialog.ChangesApplied += DialogChangesApplied;
            if (_original != null)
                return dialogProvider.ShowDialog(dialog) != DialogResult.OK ? _original.Strokes : _editCopy.Strokes;
            return null;
        }

        private void DialogChangesApplied(object sender, EventArgs e)
        {
            _original.CopyProperties(_editCopy);
        }

        /// <summary>
        /// Specifies that a form will be used for editing (a Modal form behaves like a dialog)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        #endregion

        #region Properties

        #endregion
    }
}