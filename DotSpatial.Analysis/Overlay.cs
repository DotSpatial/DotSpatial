// *************************************************
// DotSpatial.Analysis class for overlay functions
// This class can hold other overlay functions such as 
// intersect, union, etc.
// Dan Ames, 2/27/2013
// **************************************************

using System;
using DotSpatial.Data;
using DotSpatial.Topology;

namespace DotSpatial.Analysis
{
    /// <summary>
    /// A class for supporting methods to generate random points.
    /// </summary>
    public static class Overlay
    {
        /// <summary>
        /// Erase features from one feature set where they are intersected by another feature set. 
        /// </summary>
        /// <param name="TargetFeatures">Features which will be erased in part or whole.</param>
        /// <param name="SourceFeatures">Features which represent areas to erase.</param>
        /// <returns>A point feature set with the randomly created features.</returns>
        public static FeatureSet EraseFeatures(FeatureSet TargetFeatures, FeatureSet SourceFeatures)
        {
            //Erase features from one feature set where they are intersected by another feature set
            //Note: we use the ShapeIndices here rather than for each feature in featureset.features as a memory management technique.
            //Dan Ames 2/27/2013
            FeatureSet ResultFeatures = new FeatureSet();                  //the resulting featureset
            IFeature FR, SF;                                                //a single output feature

            for (Int16 i = 0; i <= TargetFeatures.ShapeIndices.Count - 1; i++)
            {
                FR = TargetFeatures.GetFeature(i);               //get the full undifferenced feature
                for (Int16 j = 0; j <= SourceFeatures.ShapeIndices.Count - 1; j++)
                {
                    SF = SourceFeatures.GetFeature(j);
                    if (SF.Envelope.Intersects(FR.Envelope))
                    {
                        FR = FR.Difference(SF);                  //clip off any pieces of SF that overlap FR
                    }
                    if (FR == null)
                    {                       //sometimes difference leaves nothing left of a feature
                        break;
                    }
                }
                if (FR != null)
                {
                    ResultFeatures.Features.Add(FR);             //add the fully clipped feature to the results
                }
            }
            return ResultFeatures;
        }
        /// <summary>
        /// Add the features from SourceFeatures to the TargetFeatures feature set. 
        /// </summary>
        /// <param name="TargetFeatures">Feature set to which features will be added.</param>
        /// <param name="SourceFeatures">Source of features to add to the target feature set. </param>
        /// <returns>A point feature set with the randomly created features.</returns>
        public static FeatureSet AppendFeatures(FeatureSet TargetFeatures, FeatureSet SourceFeatures)
        {
            //Add the features from SourceFeatures to the TargetFeatures feature set
            //Note: we use the ShapeIndices here rather than for each feature in featureset.features as a memory management technique.
            //Dan Ames 2/27/2013
            IFeature SF;
            for (Int16 j = 0; j <= SourceFeatures.ShapeIndices.Count - 1; j++)
            {
                SF = SourceFeatures.GetFeature(j);
                TargetFeatures.Features.Add(SF);
            }
            return TargetFeatures;
        }
    }
}
