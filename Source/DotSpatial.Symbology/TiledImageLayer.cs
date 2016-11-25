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
    public class TiledImageLayer : Layer
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of TiledImageLayer
        /// </summary>
        public TiledImageLayer(ITiledImage dataset)
        {
            DataSet = dataset;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The bounding envelope of the image layer
        /// </summary>
        public override Extent Extent
        {
            get { return DataSet.Extent; }
        }

        /// <summary>
        /// The tiled image dataset
        /// </summary>
        public new ITiledImage DataSet { get; set; }

        #endregion
    }
}