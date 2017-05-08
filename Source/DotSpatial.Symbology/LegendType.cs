// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/10/2009 12:51:18 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// LegendType
    /// </summary>
    public enum LegendType
    {
        /// <summary>
        /// Schemes can contain symbols and be contained by layers
        /// </summary>
        Scheme,

        /// <summary>
        /// The ability to contain another layer type is controlled by CanReceiveItem instead
        /// of being specified by these pre-defined criteria.
        /// </summary>
        Custom,

        /// <summary>
        /// Groups can be contained by groups, and contain groups or layers, but not categories or symbols
        /// </summary>
        Group,

        /// <summary>
        /// Layers can contain symbols or categories, but not other layers or groups
        /// </summary>
        Layer,

        /// <summary>
        /// Symbols can't contain anything, but can be contained by layers and categories
        /// </summary>
        Symbol
    }
}