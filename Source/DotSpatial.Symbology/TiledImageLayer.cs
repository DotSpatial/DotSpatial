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
    public class TiledImageLayer : Layer, ITiledImageLayer
    {
        #region Private Variables

        private ITiledImage _dataSet;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of TiledImageLayer
        /// </summary>
        public TiledImageLayer(ITiledImage dataset)
        {
            _dataSet = dataset;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// The bounding envelope of the image layer
        /// </summary>
        public override Extent Extent
        {
            get
            {
                return _dataSet.Extent;
            }
        }

        /// <summary>
        /// The tiled image dataset
        /// </summary>
        public new ITiledImage DataSet
        {
            get { return _dataSet; }
            set { _dataSet = value; }
        }

        #endregion
    }
}