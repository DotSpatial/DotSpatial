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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/24/2009 9:36:49 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using DotSpatial.Data;
using DotSpatial.Topology;

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
        IEnvelope Envelope
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
        bool AddRegion(IEnvelope region, out IEnvelope affectedArea);

        /// <summary>
        ///
        /// </summary>
        /// <param name="region"></param>
        /// <param name="affectedArea"></param>
        /// <returns></returns>
        bool InvertSelection(IEnvelope region, out IEnvelope affectedArea);

        /// <summary>
        ///
        /// </summary>
        /// <param name="region"></param>
        /// <param name="affectedArea"></param>
        /// <returns></returns>
        bool RemoveRegion(IEnvelope region, out IEnvelope affectedArea);

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