// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/14/2009 4:24:12 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for ImageSymbolizer.
    /// </summary>
    public interface IImageSymbolizer : ILegendItem
    {
        /// <summary>
        /// Gets or sets a float value from 0 to 1, where 1 is fully opaque while 0 is fully transparent.
        /// </summary>
        float Opacity { get; set; }
    }
}