// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/19/2009 11:49:19 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// PatternTypes
    /// </summary>
    public enum PatternType
    {
        /// <summary>
        /// A pattern that gradually changes from one color to another
        /// </summary>
        Gradient,

        /// <summary>
        /// A pattern comprised of evenly spaced lines
        /// </summary>
        Line,

        /// <summary>
        /// A pattern comprised of point symbolizers
        /// </summary>
        Marker,

        /// <summary>
        /// A pattern comprised of a tiled texture
        /// </summary>
        Picture,

        /// <summary>
        /// A pattern comprised strictly of a fill color.
        /// </summary>
        Simple
    }
}