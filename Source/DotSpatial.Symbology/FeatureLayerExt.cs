// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/23/2009 4:23:30 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using GeoAPI.Geometries;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Extension methods for DotSpatial.Symbology.IFeatureLayer.
    /// </summary>
    public static class FeatureLayerExt
    {
        #region Methods

        /// <summary>
        /// Inverts the selection.
        /// </summary>
        /// <param name="featureLayer">IFeatureLayer whose selection is inverted.</param>
        public static void InvertSelection(this IFeatureLayer featureLayer)
        {
            Envelope ignoreMe;
            Envelope env = featureLayer.Extent.ToEnvelope();
            featureLayer.InvertSelection(env, env, SelectionMode.IntersectsExtent, out ignoreMe);
        }

        #endregion

    }
}