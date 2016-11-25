// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/18/2009 1:28:27 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology
{
    public class FeatureLayerEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of FeatureLayerEventArgs
        /// </summary>
        public FeatureLayerEventArgs(IFeatureLayer featureLayer)
        {
            FeatureLayer = featureLayer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the feature layer for this event
        /// </summary>
        public IFeatureLayer FeatureLayer { get; protected set; }

        #endregion
    }
}