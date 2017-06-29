// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/8/2010 10:28:33 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// ITiledImage
    /// </summary>
    public interface ITiledImage : IImageSet
    {
        #region Properties

        /// <summary>
        /// Gets or sets the bounds for this image
        /// </summary>
        IRasterBounds Bounds
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the fileName for this tiled image.
        /// </summary>
        string Filename
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the integer height in pixels for the combined image at its maximum resolution
        /// </summary>
        int Height
        {
            get;
        }

        /// <summary>
        /// Gets the stride, or total width in pixels of the byte data, which might not match exactly with the visible width.
        /// </summary>
        int Stride
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the tile width
        /// </summary>
        int TileWidth
        {
            get;
        }

        /// <summary>
        /// Gets the tile height
        /// </summary>
        int TileHeight
        {
            get;
        }

        /// <summary>
        /// Gets or sets the integer pixel width for the combined image at its maximum resolution.
        /// </summary>
        int Width
        {
            get;
        }

        /// <summary>
        /// Gets or sets the WorldFile for this set of tiles.
        /// </summary>
        WorldFile WorldFile
        {
            get;
            set;
        }

        #endregion
    }
}