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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/2/2009 9:51:15 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// ButtonStatesEM
    /// </summary>
    public static class ButtonStatesEM
    {
        /// <summary>
        /// Returns true if the button state is depressed
        /// </summary>
        /// <param name="self">the button state</param>
        /// <returns>Boolean, true if the button is currently pressed</returns>
        public static bool IsPressed(this ButtonStates self)
        {
            ButtonStates temp = (self & ButtonStates.Depressed);
            return (temp == ButtonStates.Depressed);
        }

        /// <summary>
        /// Returns true if the button state is illuminated
        /// </summary>
        /// <param name="self">The button state</param>
        /// <returns>Boolean, true if the button is currently illuminated</returns>
        public static bool IsLit(this ButtonStates self)
        {
            return ((self & ButtonStates.Illuminated) == ButtonStates.Illuminated);
        }

        /// <summary>
        /// Sets the state to being lit
        /// </summary>
        /// <param name="self">This button state</param>
        public static ButtonStates Lit(this ButtonStates self)
        {
            if (IsLit(self))
            {
                return self;
            }
            else
            {
                return (self | ButtonStates.Illuminated);
            }
        }

        /// <summary>
        /// Sets the state to being Depressed
        /// </summary>
        /// <param name="self">This button state</param>
        public static ButtonStates Pressed(this ButtonStates self)
        {
            if (IsPressed(self))
            {
                return self;
            }
            else
            {
                return (self | ButtonStates.Depressed);
            }
        }

        /// <summary>
        /// Removes the pressed condition from the button
        /// </summary>
        /// <param name="self">This button state</param>
        public static ButtonStates Raised(this ButtonStates self)
        {
            if (IsPressed(self))
            {
                return (self & ~ButtonStates.Depressed);
            }
            else
            {
                return self;
            }
        }

        /// <summary>
        /// Removes the lit condition from the button
        /// </summary>
        /// <param name="self">This button state</param>
        public static ButtonStates Darkened(this ButtonStates self)
        {
            if (IsLit(self))
            {
                return (self & ~ButtonStates.Illuminated);
            }
            else
            {
                return self;
            }
        }

        /// <summary>
        /// Changes pressed to unpressed or unpressed to pressed
        /// </summary>
        /// <param name="self"></param>
        public static ButtonStates InverseDepression(this ButtonStates self)
        {
            if (IsPressed(self))
            {
                return Raised(self);
            }
            else
            {
                return Pressed(self);
            }
        }
    }
}