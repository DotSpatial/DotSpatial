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
// The Initial Developer of this Original Code is Ted Dunsford. Created 6/27/2008 3:39:13 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Specifies whether non-coordinate drawing properties like width or size
    /// use pixels or map coordinates.  If pixels are used, a "back transform"
    /// to approximate pixel sizes.
    /// </summary>
    public enum ScaleMode
    {
        /// <summary>
        /// Uses the simplest symbology possible, but can draw quickly
        /// </summary>
        Simple = 0,

        /// <summary>
        /// Symbol sizing parameters are based in world coordinates and will get smaller when zooming out like a real object.
        /// </summary>
        Geographic = 1,

        /// <summary>
        /// The symbols approximately preserve their size as you zoom
        /// </summary>
        Symbolic = 2
    }
}