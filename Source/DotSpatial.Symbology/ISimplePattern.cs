// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/19/2009 1:16:23 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for SimplePattern.
    /// </summary>
    public interface ISimplePattern : IPattern
    {
        #region Properties

        /// <summary>
        /// Gets or sets solid Color used for filling this pattern.
        /// </summary>
        Color FillColor { get; set; }

        /// <summary>
        /// Gets or sets the opacity of this simple pattern by modifying the alpha channel of the fill color.
        /// </summary>
        float Opacity { get; set; }

        #endregion
    }
}