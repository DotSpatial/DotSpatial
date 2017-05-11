// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/21/2009 9:29:15 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for HatchPattern.
    /// </summary>
    public interface IHatchPattern : IPattern
    {
        #region Properties

        /// <summary>
        /// Gets or sets the hatch style.
        /// </summary>
        HatchStyle HatchStyle { get; set; }

        /// <summary>
        /// Gets or sets the fore color of the hatch pattern
        /// </summary>
        Color ForeColor { get; set; }

        /// <summary>
        /// Gets or sets the opacity of this simple pattern by modifying the alpha channel of the fill color.
        /// </summary>
        float ForeColorOpacity { get; set; }

        /// <summary>
        /// Gets or sets the background color of the hatch pattern.
        /// </summary>
        Color BackColor { get; set; }

        /// <summary>
        /// Gets or sets the opacity of this simple pattern by modifying the alpha channel of the fill color.
        /// </summary>
        float BackColorOpacity { get; set; }

        #endregion
    }
}