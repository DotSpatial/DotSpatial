// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/14/2009 9:24:40 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;

namespace DotSpatial.Symbology
{
    public interface IColorable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Color
        /// </summary>
        Color Color
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the opacity
        /// </summary>
        float Opacity
        {
            get;
            set;
        }

        #endregion
    }
}