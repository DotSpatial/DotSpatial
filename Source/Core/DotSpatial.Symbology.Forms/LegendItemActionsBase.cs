// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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
        /// Gets or sets owner for any dialogs that need to be launched.
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
            if (form == null) throw new ArgumentNullException(nameof(form));
            return form.ShowDialog(Owner);
        }

        /// <summary>
        /// Runs a common dialog box with the owner defined in this class.
        /// </summary>
        /// <param name="dlg">Dialog to show.</param>
        /// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult"/> values.</returns>
        /// <exception cref="ArgumentNullException">dlg is null.</exception>
        protected DialogResult ShowDialog(CommonDialog dlg)
        {
            if (dlg == null) throw new ArgumentNullException(nameof(dlg));
            return dlg.ShowDialog(Owner);
        }
    }
}