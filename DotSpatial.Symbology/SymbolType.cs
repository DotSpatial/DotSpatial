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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/19/2009 10:55:14 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// SymbolTypes
    /// </summary>
    public enum SymbolType
    {
        /// <summary>
        /// A symbol based on a character, including special purpose symbolic character sets.
        /// </summary>
        Character,
        /// <summary>
        /// An extended, custom symbol that is not part of the current design.
        /// </summary>
        Custom,
        /// <summary>
        /// A symbol based on an image or icon.
        /// </summary>
        Picture,
        /// <summary>
        /// A symbol described by a simple geometry, outline and color.
        /// </summary>
        Simple
    }
}