// *******************************************************************************************************
// Product: DotSpatial.Analysis.Overlay.cs
// Description: Class for overlay functions. Put other overlay functions here such as intersect, union.
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// Dan Ames           |  2/27/2013         |  Initially written.  
// *******************************************************************************************************

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
        /// <param name="cancelProgressHandler">Optional parameter to report progress and cancel entire process if needed.</param>
        /// <returns>A point feature set with the randomly created features.</returns>
        public static FeatureSet EraseFeatures(IFeatureSet TargetFeatures, IFeatureSet SourceFeatures, ICancelProgressHandler cancelProgressHandler = null)
        {
            if (TargetFeatures == null || SourceFeatures == null)
            {
                return null;
            }
            //Erase features from one feature set where they are intersected by another feature set
            //Note: we use the ShapeIndices here rather than for each feature in featureset.features as a memory management technique.
            //The current version does not preserve any attribute info. 
            //Dan Ames 2/27/2013
            FeatureSet ResultFeatures = new FeatureSet();                   //the resulting featureset
            IFeature TF, SF;                                                //a single output feature
            ResultFeatures.CopyTableSchema(TargetFeatures);                 //set up the data table in the new feature set

            for (Int16 i = 0; i <= TargetFeatures.ShapeIndices.Count - 1; i++)
            {
                TF = TargetFeatures.GetFeature(i);                          //get the full undifferenced feature
                for (Int16 j = 0; j <= SourceFeatures.ShapeIndices.Count - 1; j++)
                {
                    SF = SourceFeatures.GetFeature(j);
                    if (SF.Envelope.Intersects(TF.Envelope))
                    {
                        TF = TF.Difference(SF);                             //clip off any pieces of SF that overlap FR
                    }
                    if (TF == null)
                    {                                                       //sometimes difference leaves nothing left of a feature
                        break;
                    }
                }
                if (TF != null)
                {
                    ResultFeatures.AddFeature(TF).CopyAttributes(TargetFeatures.GetFeature(i));  //add the fully clipped feature to the results
                }
                if (cancelProgressHandler != null)
                {
                    if (cancelProgressHandler.Cancel) { return null; }
                    int progress = Convert.ToInt32(i * 100 / TargetFeatures.ShapeIndices.Count);
                    cancelProgressHandler.Progress(String.Empty, progress, String.Empty);
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
                TargetFeatures.AddFeature(SF).CopyAttributes(SourceFeatures.GetFeature(j));   //by default this will try to copy attributes over that have the same name.
            }
            return TargetFeatures;
        }
    }
}
