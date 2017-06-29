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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/9/2009 3:18:04 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// StrokeTypes
    /// </summary>
    public enum StrokeStyle
    {
        /// <summary>
        /// The most complex form, containing a linear pattern that can have a hash as well as decorations
        /// </summary>
        Catographic,
        /// <summary>
        /// This is not directly supported by DotSpatial, but is in fact, some new type that
        /// will have to be returned.
        /// </summary>
        Custom,
        /// <summary>
        /// Draws only the marker symbols where the line occurs, and uses the dash pattern to control placement.
        /// </summary>
        Marker,
        /// <summary>
        /// The simplest line, offering the easiest interface to use
        /// </summary>
        Simple,
        /// <summary>
        /// A hash line
        /// </summary>
        Hash,
        /// <summary>
        /// Uses a picture to generate a texture
        /// </summary>
        Picture,
    }
}