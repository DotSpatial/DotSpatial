// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/8/2010 10:28:33 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// Interface for TiledImage.
    /// </summary>
    public interface ITiledImage : IImageSet
    {
        #region Properties

        /// <summary>
        /// Gets or sets the bounds for this image
        /// </summary>
        IRasterBounds Bounds { get; set; }

        /// <summary>
        /// Gets the integer height in pixels for the combined image at its maximum resolution
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Gets or sets the stride, or total width in pixels of the byte data, which might not match exactly with the visible width.
        /// </summary>
        int Stride { get; set; }

        /// <summary>
        /// Gets the tile width
        /// </summary>
        int TileWidth { get; }

        /// <summary>
        /// Gets the tile height
        /// </summary>
        int TileHeight { get; }

        /// <summary>
        /// Gets the integer pixel width for the combined image at its maximum resolution.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets or sets the WorldFile for this set of tiles.
        /// </summary>
        WorldFile WorldFile { get; set; }

        #endregion
    }
}