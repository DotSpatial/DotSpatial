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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/11/2009 4:54:13 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Allows the selection of several pre-defined shapes that have built
    /// in drawing code.
    /// </summary>
    public enum PointShape
    {
        /// <summary>
        /// Like a rectangle, but oriented with the points vertically
        /// </summary>
        Diamond,

        /// <summary>
        /// An rounded elipse.  The Size parameter determines the size of the ellipse before rotation.
        /// </summary>
        Ellipse,

        /// <summary>
        /// A hexagon drawn to fit the size specified.  Only the smaller dimension is used.
        /// </summary>
        Hexagon,

        /// <summary>
        /// A rectangle fit to the Size before any rotation occurs.
        /// </summary>
        Rectangle,

        /// <summary>
        /// A pentagon drawn to fit the size specified.  Only the smaller dimension is used.
        /// </summary>
        Pentagon,

        /// <summary>
        /// A star drawn to fit the size.  Only the smaller size dimension is used.
        /// </summary>
        Star,

        /// <summary>
        /// Triangle with the point facing upwards initially.
        /// </summary>
        Triangle,

        /// <summary>
        /// The default value.
        /// </summary>
        Undefined,
    }
}