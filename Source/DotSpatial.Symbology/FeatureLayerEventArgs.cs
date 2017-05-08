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
    /// <summary>
    /// Event args for events that need a feature layer.
    /// </summary>
    public class FeatureLayerEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureLayerEventArgs"/> class.
        /// </summary>
        /// <param name="featureLayer">FeatureLayer of the event.</param>
        public FeatureLayerEventArgs(IFeatureLayer featureLayer)
        {
            FeatureLayer = featureLayer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the feature layer for this event.
        /// </summary>
        public IFeatureLayer FeatureLayer { get; protected set; }

        #endregion
    }
}