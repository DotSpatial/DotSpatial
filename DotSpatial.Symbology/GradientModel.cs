// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
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
// The Initial Developer of this Original Code is Ted Dunsford. 2/17/2008 12:28:12 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// An enumeration specifying the way that a gradient of color is attributed to the values in the specified range.
    /// </summary>
    public enum GradientModel
    {
        /// <summary>
        /// The values are colored in even steps in each of the Red, Green and Blue bands.
        /// </summary>
        Linear,
        /// <summary>
        /// The even steps between values are used as powers of two, greatly increasing the impact of higher values.
        /// </summary>
        Exponential,

        /// <summary>
        /// The log of the values is used, reducing the relative impact of the higher values in the range.
        /// </summary>
        Logarithmic
    }
}