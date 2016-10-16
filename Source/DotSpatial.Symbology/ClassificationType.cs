// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/30/2009 12:54:50 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ClassificationTypes
    /// </summary>
    public enum ClassificationType
    {
        /// <summary>
        /// Each category is designed with a custom expression
        /// </summary>
        Custom,

        /// <summary>
        /// Unique values are added
        /// </summary>
        UniqueValues,

        /// <summary>
        /// A Quantile scheme is applied, which forces the behavior of continuous categories.
        /// </summary>
        Quantities,
    }
}