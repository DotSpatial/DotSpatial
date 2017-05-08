// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/6/2010 11:44:38 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// TiledImageLayer
    /// </summary>
    public class TiledImageLayer : Layer
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TiledImageLayer"/> class.
        /// </summary>
        /// <param name="dataset">TiledImage the layer is based on.</param>
        public TiledImageLayer(ITiledImage dataset)
        {
            DataSet = dataset;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the tiled image dataset.
        /// </summary>
        public new ITiledImage DataSet { get; set; }

        /// <summary>
        /// Gets the bounding envelope of the image layer.
        /// </summary>
        public override Extent Extent => DataSet.Extent;

        #endregion
    }
}