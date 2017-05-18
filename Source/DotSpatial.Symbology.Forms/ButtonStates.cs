// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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