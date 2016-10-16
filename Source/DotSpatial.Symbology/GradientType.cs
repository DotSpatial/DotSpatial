// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/22/2009 9:08:29 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// GradientTypes
    /// </summary>
    public enum GradientType
    {
        /// <summary>
        /// Draws the gradient with the start color for the center of the circle
        /// and the end color for the surround color
        /// </summary>
        Circular,
        /// <summary>
        /// Draws the gradient with the start color for the center of the circle
        /// and the end color for the color at the contour.
        /// </summary>
        Contour,
        /// <summary>
        /// Draws the gradient in a line with a specified direction
        /// </summary>
        Linear,
        /// <summary>
        /// Draws the gradient in a rectangular path with the start color
        /// at the center and the end color for the surround color
        /// </summary>
        Rectangular
    }
}