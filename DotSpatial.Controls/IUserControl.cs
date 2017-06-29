// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Provides an empty control that can be used to create other controls.
    /// </summary>
    [Obsolete("Do not use it. This interface is not used in DotSpatial anymore.")] // Marked in 1.7
    public interface IUserControl : IContainerControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets how the control will resize itself.
        /// </summary>
        /// <returns>
        /// A value from the AutoSizeMode enumeration. The default is AutoSizeMode.GrowOnly.
        /// </returns>
        AutoSizeMode AutoSizeMode { get; set; }

        /// <summary>
        /// Gets or sets the border style of the tree view control.
        /// </summary>
        /// <returns>
        /// One of the BorderStyle values. The default is BorderStyle.Fixed3D.
        /// </returns>
        /// <exception cref="InvalidEnumArgumentException">
        /// InvalidEnumArgumentException: The assigned value is not one of the BorderStyle values.
        /// </exception>
        BorderStyle BorderStyle { get; set; }

        #endregion

        /// <summary>
        /// Occurs when the AutoSize changes
        /// </summary>
        event EventHandler AutoSizeChanged;

        /// <summary>
        /// Occurs when the UserControl.AutoValidate property changes.
        /// </summary>
        event EventHandler AutoValidateChanged;

        /// <summary>
        /// Occurs before the control becomes visible for the first time.
        /// </summary>
        event EventHandler Load;
    }
}