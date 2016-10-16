// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/6/2010 11:49:32 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ITiledImageLayer
    /// </summary>
    public interface ITiledImageLayer : ILayer
    {
        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the TiledImage that supports the tile data for this image.
        /// </summary>
        new ITiledImage DataSet
        {
            get;
            set;
        }

        #endregion
    }
}