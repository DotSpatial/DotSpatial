// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/9/2008 2:23:31 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This describes the three permitted states for legend items.
    /// Items can have a checkbox or a symbol, but not both.
    /// </summary>
    public enum SymbolMode
    {
        /// <summary>
        /// Display a checkbox next to the legend item
        /// </summary>
        Checkbox,

        /// <summary>
        /// Draws a symbol, but also allows collapsing.
        /// </summary>
        GroupSymbol,

        /// <summary>
        /// Display a symbol next to the legend item
        /// </summary>
        Symbol,

        /// <summary>
        /// Display only legend text
        /// </summary>
        None
    }
}