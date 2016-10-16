// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
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