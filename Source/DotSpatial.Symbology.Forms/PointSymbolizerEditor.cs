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
    public class PointSymbolizerEditor : UITypeEditor
    {
        #region Fields

        private IPointSymbolizer _copy;
        private IPointSymbolizer _original;

        #endregion

        #region Methods

        /// <summary>
        /// Launches a form for editing the line symbolizer
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="provider">The service provider.</param>
        /// <param name="value">The point symbolizer.</param>
        /// <returns>The resulting point symbolizer</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _original = value as IPointSymbolizer;
            if (_original == null) return value;
            _copy = _original.Copy();
            IWindowsFormsEditorService dialogProvider = (IWindowsFormsEditorService)provider?.GetService(typeof(IWindowsFormsEditorService));
            DetailedPointSymbolDialog dialog = new DetailedPointSymbolDialog(_copy);
            dialog.ChangesApplied += DialogChangesApplied;
            if (dialogProvider?.ShowDialog(dialog) != DialogResult.OK) return value;
            _original.CopyProperties(_copy);
            return value;
        }

        /// <summary>
        /// Specifies that this should open a form and work using a modal behavior.
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <returns>The UITypeEditorEditStyle</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        private void DialogChangesApplied(object sender, EventArgs e)
        {
            _original.CopyProperties(_copy);
        }

        #endregion
    }
}