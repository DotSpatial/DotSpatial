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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/21/2009 2:25:05 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IGradientPath
    /// </summary>
    public interface IGradientPattern : IPattern
    {
        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the angle for the gradient pattern.
        /// </summary>
        double Angle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an array of colors that match the corresponding positions.  The length of
        /// colors and positions should be the same length.
        /// </summary>
        Color[] Colors
        {
            get;
            set;
        }

        /// <summary>
        /// The positions as floating point values from 0 to 1 that represent the corresponding location
        /// in the gradient brush pattern.
        /// </summary>
        float[] Positions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the gradient type
        /// </summary>
        GradientType GradientType
        {
            get;
            set;
        }

        #endregion
    }
}