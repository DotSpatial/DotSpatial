// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Data;

namespace DotSpatial.Analysis
{
    /// <summary>
    /// A class for supporting methods to generate random points.
    /// </summary>
    public static class Overlay
    {
        #region Methods

        /// <summary>
        /// Add the features from SourceFeatures to the TargetFeatures feature set.
        /// </summary>
        /// <param name="targetFeatures">Feature set to which features will be added.</param>
        /// <param name="sourceFeatures">Source of features to add to the target feature set. </param>
        /// <returns>A point feature set with the randomly created features.</returns>
        public static FeatureSet AppendFeatures(FeatureSet targetFeatures, FeatureSet sourceFeatures)
        {
            // Add the features from SourceFeatures to the TargetFeatures feature set
            // Note: we use the ShapeIndices here rather than for each feature in featureset.features as a memory management technique.
            // Dan Ames 2/27/2013
            for (short j = 0; j <= sourceFeatures.ShapeIndices.Count - 1; j++)
            {
                var sf = sourceFeatures.GetFeature(j);
                targetFeatures.AddFeature(sf.Geometry).CopyAttributes(sf); // by default this will try to copy attributes over that have the same name.
            }

            return targetFeatures;
        }

        /// <summary>
        /// Erase features from one feature set where they are intersected by another feature set.
        /// </summary>
        /// <param name="targetFeatures">Features which will be erased in part or whole.</param>
        /// <param name="sourceFeatures">Features which represent areas to erase.</param>
        /// <param name="cancelProgressHandler">Optional parameter to report progress and cancel entire process if needed.</param>
        /// <returns>A point feature set with the randomly created features.</returns>
        public static FeatureSet EraseFeatures(IFeatureSet targetFeatures, IFeatureSet sourceFeatures, ICancelProgressHandler cancelProgressHandler = null)
        {
            if (targetFeatures == null || sourceFeatures == null)
            {
                return null;
            }

            // Erase features from one feature set where they are intersected by another feature set
            // Note: we use the ShapeIndices here rather than for each feature in featureset.features as a memory management technique.
            // The current version does not preserve any attribute info.
            // Dan Ames 2/27/2013
            FeatureSet resultFeatures = new(); // the resulting featureset
            resultFeatures.CopyTableSchema(targetFeatures); // set up the data table in the new feature set

            for (short i = 0; i <= targetFeatures.ShapeIndices.Count - 1; i++)
            {
                var tf = targetFeatures.GetFeature(i); // get the full undifferenced feature
                for (short j = 0; j <= sourceFeatures.ShapeIndices.Count - 1; j++)
                {
                    var sf = sourceFeatures.GetFeature(j);
                    if (sf.Geometry.Envelope.Intersects(tf.Geometry.Envelope))
                    {
                        tf = tf.Difference(sf.Geometry); // clip off any pieces of SF that overlap FR
                    }

                    if (tf == null)
                    {
                        // sometimes difference leaves nothing left of a feature
                        break;
                    }
                }

                if (tf != null)
                {
                    resultFeatures.AddFeature(tf.Geometry).CopyAttributes(targetFeatures.GetFeature(i)); // add the fully clipped feature to the results
                }

                if (cancelProgressHandler != null)
                {
                    if (cancelProgressHandler.Cancel)
                    {
                        return null;
                    }

                    int progress = Convert.ToInt32(i * 100 / targetFeatures.ShapeIndices.Count);
                    cancelProgressHandler.Progress(progress, string.Empty);
                }
            }

            return resultFeatures;
        }

        #endregion
    }
}