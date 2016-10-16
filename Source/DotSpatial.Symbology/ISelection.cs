// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/24/2009 9:36:49 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using DotSpatial.Data;
using GeoAPI.Geometries;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ISelection
    /// </summary>
    public interface ISelection : IChangeable, IAttributeSource
    {
        /// <summary>
        /// Gets the integer count of the members in the collection
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// Calculates the envelope of this collection
        /// </summary>
        Envelope Envelope
        {
            get;
        }

        /// <summary>
        /// Gets or sets the handler to use for progress messages during selection.
        /// </summary>
        IProgressHandler ProgressHandler { get; set; }

        /// <summary>
        /// Selection Mode controls how envelopes are treated when working with geometries.
        /// </summary>
        SelectionMode SelectionMode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether this should work as "Selected" indices (true) or
        /// "UnSelected" indices (false).
        /// </summary>
        bool SelectionState
        {
            get;
            set;
        }

        /// <summary>
        /// Setting this to a specific category will only allow selection by
        /// region to affect the features that are within the specified category.
        /// </summary>
        IFeatureCategory RegionCategory
        {
            get;
            set;
        }

        /// <summary>
        /// Clears the selection
        /// </summary>
        void Clear();

        /// <summary>
        /// Add REgion
        /// </summary>
        /// <param name="region"></param>
        /// <param name="affectedArea"></param>
        /// <returns></returns>
        bool AddRegion(Envelope region, out Envelope affectedArea);

        /// <summary>
        ///
        /// </summary>
        /// <param name="region"></param>
        /// <param name="affectedArea"></param>
        /// <returns></returns>
        bool InvertSelection(Envelope region, out Envelope affectedArea);

        /// <summary>
        ///
        /// </summary>
        /// <param name="region"></param>
        /// <param name="affectedArea"></param>
        /// <returns></returns>
        bool RemoveRegion(Envelope region, out Envelope affectedArea);

        /// <summary>
        /// Returns a new featureset based on the features in this collection
        /// </summary>
        /// <returns></returns>
        FeatureSet ToFeatureSet();

        /// <summary>
        /// Generates a list of the features that match this collection
        /// </summary>
        /// <returns></returns>
        List<IFeature> ToFeatureList();
    }
}