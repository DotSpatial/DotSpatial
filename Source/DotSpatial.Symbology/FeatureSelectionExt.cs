// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using DotSpatial.Data;
using GeoAPI.Geometries;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Contains extension methods for IFeatureSelection.
    /// </summary>
    public static class FeatureSelectionExt
    {
        #region Methods

        /// <summary>
        /// Adds all the features in the specified range. This will throw an exception if the
        /// the features are not already in the feature list, since this is simply trying
        /// to select those features.
        /// </summary>
        /// <param name="self">The IFeatureSelection to add the range to</param>
        /// <param name="features">The features being selected.</param>
        public static void AddRange(this IFeatureSelection self, IEnumerable<IFeature> features)
        {
            self.SuspendChanges();
            foreach (IFeature f in features)
            {
                self.Add(f);
            }

            self.ResumeChanges();
        }

        /// <summary>
        /// This uses extent checking (rather than full polygon intersection checking). It will add
        /// any members that are either contained by or intersect with the specified region
        /// depending on the SelectionMode property. The order of operation is the region
        /// acting on the feature, so Contains, for instance, would work with points.
        /// </summary>
        /// <param name="self">The IFeatureSelection that this should be applied to.</param>
        /// <param name="region">The geographic region to add.</param>
        /// <returns>True if any item was actually added to the collection</returns>
        public static bool AddRegion(this IFeatureSelection self, Envelope region)
        {
            Envelope ignoreMe;
            return self.AddRegion(region, out ignoreMe);
        }

        /// <summary>
        /// Removes the entire list of features.
        /// </summary>
        /// <param name="self">The IFeatureSelection to remove the range from</param>
        /// <param name="features">The enumerable collection of IFeatures.</param>
        public static void RemoveRange(this IFeatureSelection self, IEnumerable<IFeature> features)
        {
            self.SuspendChanges();
            foreach (IFeature f in features)
            {
                self.Remove(f);
            }

            self.ResumeChanges();
        }

        /// <summary>
        /// Tests each member currently in the selected features based on the SelectionMode.
        /// If it passes, it will remove the feature from the selection.
        /// </summary>
        /// <param name="self">The IFeatureSelection that this should be applied to.</param>
        /// <param name="region">The geographic region to remove.</param>
        /// <returns>Boolean, true if the collection was changed</returns>
        public static bool RemoveRegion(this IFeatureSelection self, Envelope region)
        {
            Envelope ignoreMe;
            return self.RemoveRegion(region, out ignoreMe);
        }

        #endregion
    }
}