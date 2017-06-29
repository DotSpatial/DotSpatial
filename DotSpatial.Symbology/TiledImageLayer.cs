// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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