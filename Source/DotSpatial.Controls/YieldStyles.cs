// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created during refactoring 2010.
// ********************************************************************************************************

using System;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Describe different behaviors that map functions can have when working with
    /// respect to other map-functions.
    /// </summary>
    [Flags]
    public enum YieldStyles
    {
        /// <summary>
        /// This is a null state for testing, and should not be used directly.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// This function will deactivate if another LeftButton function activates.
        /// </summary>
        LeftButton = 0x1,

        /// <summary>
        /// This function will deactivate if another RightButton function activates.
        /// </summary>
        RightButton = 0x2,

        /// <summary>
        /// This function will deactivate if another scroll function activates.
        /// </summary>
        Scroll = 0x4,

        /// <summary>
        /// This function will deactivate if another keyboard function activates.
        /// </summary>
        Keyboard = 0x8,

        /// <summary>
        /// This function is like a glyph and never yields to other functions.
        /// </summary>
        AlwaysOn = 16,
    }
}