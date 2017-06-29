// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/2/2009 9:35:43 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// ButtonState
    /// </summary>
    [Flags]
    public enum ButtonStates
    {
        /// <summary>
        /// This is the default case, wher the button is neither depressed nor illuminated
        /// </summary>
        None = 0,
        /// <summary>
        /// The Button is depressed or pressed down
        /// </summary>
        Depressed = 0x1,
        /// <summary>
        /// The Button is illuminated or lit up
        /// </summary>
        Illuminated = 0x2
    }
}