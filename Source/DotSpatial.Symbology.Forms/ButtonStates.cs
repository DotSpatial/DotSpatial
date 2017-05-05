// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
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