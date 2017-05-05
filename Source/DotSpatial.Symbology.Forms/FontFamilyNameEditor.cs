// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/31/2009 9:59:36 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// FontFamilyNameEditor
    /// </summary>
    public class FontFamilyNameEditor : UITypeEditor
    {
        #region Fields

        private IWindowsFormsEditorService _dialogProvider;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the drop down is resizeable.
        /// </summary>
        public override bool IsDropDownResizable => true;

        #endregion

        #region Methods

        /// <summary>
        /// Edits a value based on some user input which is collected from a character control.
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="provider">The service provider.</param>
        /// <param name="value">Not used.</param>
        /// <returns>The selected font family name.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _dialogProvider = provider?.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            ListBox cmb = new ListBox();
            FontFamily[] fams = FontFamily.Families;
            cmb.SuspendLayout();
            foreach (FontFamily fam in fams)
            {
                cmb.Items.Add(fam.Name);
            }

            cmb.SelectedValueChanged += CmbSelectedValueChanged;
            cmb.ResumeLayout();
            _dialogProvider?.DropDownControl(cmb);
            string test = (string)cmb.SelectedItem;
            return test;
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

        private void CmbSelectedValueChanged(object sender, EventArgs e)
        {
            _dialogProvider.CloseDropDown();
        }

        #endregion
    }
}