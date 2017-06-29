// *******************************************************************************************************
// Product:  DotSpatial.Symbology.Forms.LegendItemActionsBase
// Description: Base class for SymbologyActions childs
// Copyright & License: See www.DotSpatial.org.
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// Max Miroshnikov    |  3/2013            |  Initial commit
// *******************************************************************************************************

using System;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// Contains common methods for "Actions" classes (e.g. LayerActions, etc...)
    /// </summary>
    public abstract class LegendItemActionsBase : IIWin32WindowOwner
    {
        /// <summary>
        /// Allows setting the owner for any dialogs that need to be launched.
        /// </summary>
        public IWin32Window Owner { get; set; }  
 
        /// <summary>
        /// Shows the form as a modal dialog box with the owner defined in this class. 
        /// </summary>
        /// <param name="form">Form to show.</param>
        /// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult"/> values.</returns>
        /// <exception cref="ArgumentNullException">form is null.</exception>
        protected DialogResult ShowDialog(Form form)
        {
            if (form == null) throw new ArgumentNullException("form");
            return form.ShowDialog(Owner);
        }

        /// <summary>
        ///  Runs a common dialog box with the owner defined in this class. 
        /// </summary>
        /// <param name="dlg">Dialog to show.</param>
        /// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult"/> values.</returns>
        /// <exception cref="ArgumentNullException">dlg is null.</exception>
        protected DialogResult ShowDialog(CommonDialog dlg)
        {
            if (dlg == null) throw new ArgumentNullException("dlg");
            return dlg.ShowDialog(Owner);
        }
    }
}