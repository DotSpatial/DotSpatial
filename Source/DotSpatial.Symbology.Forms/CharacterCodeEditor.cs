// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/7/2009 2:14:56 PM
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
    /// CharacterCodeEditor
    /// </summary>
    public class CharacterCodeEditor : UITypeEditor
    {
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
        /// <param name="value">Not used</param>
        /// <returns>Returns the selected character.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            ICharacterSymbol symbol = context?.Instance as ICharacterSymbol;
            IWindowsFormsEditorService dialogProvider = provider?.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            CharacterControl cc = new CharacterControl(dialogProvider, symbol);
            dialogProvider?.DropDownControl(cc);
            return cc.SelectedChar;
        }

        /// <summary>
        /// Gets the UITypeEditorEditStyle, which in this case is drop down.
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