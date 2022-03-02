// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using NetTopologySuite.Geometries;

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