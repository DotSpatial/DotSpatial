// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/14/2009 4:29:13 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for ImageLayer.
    /// </summary>
    public interface IImageLayer : ILayer
    {
        /// <summary>
        /// Gets or sets a class that has some basic parameters that control how the image layer
        /// is drawn.
        /// </summary>
        IImageSymbolizer Symbolizer { get; set; }

        /// <summary>
        /// Gets or sets the dataset specifically as an IImageData object
        /// </summary>
        new IImageData DataSet { get; set; }

        /// <summary>
        /// Gets or sets the image being drawn by this layer
        /// </summary>
        IImageData Image { get; set; }
    }
}