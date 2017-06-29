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