// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// ButtonStatesEm.
    /// </summary>
    public static class ButtonStatesEm
    {
        #region Methods

        /// <summary>
        /// Removes the lit condition from the button.
        /// </summary>
        /// <param name="self">This button state.</param>
        /// <returns>The resulting button state.</returns>
        public static ButtonStates Darkened(this ButtonStates self)
        {
            return IsLit(self) ? self & ~ButtonStates.Illuminated : self;
        }

        /// <summary>
        /// Changes pressed to unpressed or unpressed to pressed.
        /// </summary>
        /// <param name="self">Tis button state.</param>
        /// <returns>The resulting button state.</returns>
        public static ButtonStates InverseDepression(this ButtonStates self)
        {
            return IsPressed(self) ? Raised(self) : Pressed(self);
        }

        /// <summary>
        /// Returns true if the button state is illuminated.
        /// </summary>
        /// <param name="self">The button state.</param>
        /// <returns>Boolean, true if the button is currently illuminated.</returns>
        public static bool IsLit(this ButtonStates self)
        {
            return (self & ButtonStates.Illuminated) == ButtonStates.Illuminated;
        }

        /// <summary>
        /// Returns true if the button state is depressed.
        /// </summary>
        /// <param name="self">the button state.</param>
        /// <returns>Boolean, true if the button is currently pressed.</returns>
        public static bool IsPressed(this ButtonStates self)
        {
            return (self & ButtonStates.Depressed) == ButtonStates.Depressed;
        }

        /// <summary>
        /// Sets the state to being lit.
        /// </summary>
        /// <param name="self">This button state.</param>
        /// <returns>The resulting button state.</returns>
        public static ButtonStates Lit(this ButtonStates self)
        {
            return IsLit(self) ? self : self | ButtonStates.Illuminated;
        }

        /// <summary>
        /// Sets the state to being Depressed.
        /// </summary>
        /// <param name="self">This button state.</param>
        /// <returns>The resulting button state.</returns>
        public static ButtonStates Pressed(this ButtonStates self)
        {
            return IsPressed(self) ? self : self | ButtonStates.Depressed;
        }

        /// <summary>
        /// Removes the pressed condition from the button.
        /// </summary>
        /// <param name="self">This button state.</param>
        /// <returns>The resulting button state.</returns>
        public static ButtonStates Raised(this ButtonStates self)
        {
            return IsPressed(self) ? self & ~ButtonStates.Depressed : self;
        }

        #endregion
    }
}