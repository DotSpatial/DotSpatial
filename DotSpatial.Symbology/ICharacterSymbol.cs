// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/2/2009 9:46:34 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ICharacterSymbol
    /// </summary>
    public interface ICharacterSymbol : ISymbol, IColorable
    {
        #region Methods

        /// <summary>
        /// Gets the string equivalent of the specified character code.
        /// </summary>
        /// <returns>A string version of the character</returns>
        string ToString();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unicode category for this character.
        /// </summary>
        [Description("Gets the unicode category for this character.")]
        UnicodeCategory Category
        {
            get;
        }

        /// <summary>
        /// Gets or sets the character that this represents.
        /// </summary>
        [Description("Gets or sets the character for this symbol.")]
        char Character
        {
            get;
            set;
        }

        /// <summary>
        /// Unicode characters consist of 2 bytes.  This represents the first byte,
        /// which can be thought of as specifying a typeset.
        /// </summary>
        [Description("Gets or sets the upper unicode byte, or character set.")]
        byte CharacterSet
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the byte code for the lower 256 values.  This represents the
        /// specific character in a given "typeset" range.
        /// </summary>
        /// <remarks>
        /// [Editor(typeof(CharacterCodeEditor), typeof(UITypeEditor))]
        /// </remarks>
        [Description("Gets or sets the lower unicode byte or character ASCII code")]
        byte Code
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the string font family name to use for this character set.
        /// </summary>
        /// <remarks>[Editor(typeof(FontFamilyNameEditor), typeof(UITypeEditor))]</remarks>
        [Description("Gets or sets the font family name to use when building the font.")]
        string FontFamilyName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the font style to use for this character layer.
        /// </summary>
        [Description("Gets or sets the font style to use for this character layer.")]
        FontStyle Style
        {
            get;
            set;
        }

        #endregion
    }
}