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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/11/2009 12:57:07 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IColorCategory
    /// </summary>
    public interface IColorCategory : ICategory
    {
        #region Methods

        /// <summary>
        /// This is primarilly used in the BiValue situation where a color needs to be generated
        /// somewhere between the startvalue and the endvalue.
        /// </summary>
        /// <param name="value">The integer value to be converted into a color from the range on this colorbreak</param>
        /// <returns>A color that is selected from the range values.</returns>
        Color CalculateColor(double value);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets how the color changes are distributed across the
        /// BiValued range.  If IsBiValue is false, this does nothing.
        /// </summary>
        GradientModel GradientModel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the second of two colors to be used.
        /// This is only used for BiValued breaks.
        /// </summary>
        Color HighColor
        {
            get;
            set;
        }

        /// <summary>
        /// This not only indicates that there are two values,
        /// but that the values are also different from one another.
        /// </summary>
        bool IsBiValue
        {
            get;
        }

        /// <summary>
        /// Gets or sets the color to be used for this break.  For
        /// BiValued breaks, this only sets one of the colors.  If
        /// this is higher than the high value, both are set to this.
        /// If this equals the high value, IsBiValue will be false.
        /// </summary>
        Color LowColor
        {
            get;
            set;
        }

        #endregion
    }
}