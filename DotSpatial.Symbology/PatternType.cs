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
        Simple,
    }
}