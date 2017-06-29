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
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/29/2009 4:08:15 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IntervalSnapMethods
    /// </summary>
    public enum IntervalSnapMethod
    {
        /// <summary>
        /// Snap the chosen values to the nearest data value.
        /// </summary>
        DataValue,
        /// <summary>
        /// No snapping at all is used
        /// </summary>
        None,
        /// <summary>
        /// Snaps to the nearest integer value.
        /// </summary>
        Rounding,
        /// <summary>
        /// Disregards scale, and preserves a fixed number of figures.
        /// </summary>
        SignificantFigures,
    }
}