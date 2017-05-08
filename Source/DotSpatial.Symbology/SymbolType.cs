// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
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
    /// The symbol type.
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