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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2009 4:57:39 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IDrawingFilter
    /// </summary>
    public interface IDrawingFilter : IEnumerable<IFeature>, ICloneable
    {
        /// <summary>
        /// Occurs after this filter has built its internal list of items.
        /// </summary>
        event EventHandler Initialized;

        #region Methods

        /// <summary>
        /// This will set all values to the default (0) category.  Then, it will use the filter
        /// expressions on the remaining categories to change the categories for those members.
        /// This means that an item will be classified as the last filter that it qualifies for.
        /// </summary>
        /// <param name="scheme">The scheme of categories to apply to the drawing states</param>
        void ApplyScheme(IFeatureScheme scheme);

        /// <summary>
        /// Invalidates this drawing filter, forcing a re-creation
        /// of the entire dictionary from the source featureset.
        /// This should only be done if changes are made to the
        /// feature list while SuspendChanges on the list is true.
        /// </summary>
        void Invalidate();

        /// <summary>
        /// If UseChunks is true, this uses the index value combined with the chunk size
        /// to calculate the chunk, and also sets the category to the [0] category and the
        /// selection state to unselected.  This can be overridden in sub-classes to come up
        /// with a different default state.
        /// </summary>
        /// <param name="index">The integer index to get the default state of</param>
        /// <returns>An IDrawnState</returns>
        IDrawnState GetDefaultState(int index);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the scheme category to use
        /// </summary>
        IFeatureCategory Category
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the integer chunk that the filter should use
        /// </summary>
        int Chunk
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the integer size of each chunk.  Setting this to
        /// a new value will cycle through and update the chunk on all
        /// the features.
        /// </summary>
        int ChunkSize
        {
            get;
            set;
        }

        /// <summary>
        /// If the drawing state for any features has changed, or else if
        /// the state of any members has changed, this will cycle through
        /// the filter members and cache a new count.  If nothing has
        /// changed, then this will simply return the cached value.
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// Gets the default category for the scheme.
        /// </summary>
        IFeatureCategory DefaultCategory
        {
            get;
        }

        /// <summary>
        /// Gets the dictionary of drawn states that this drawing filter uses.
        /// </summary>
        IDictionary<IFeature, IDrawnState> DrawnStates
        {
            get;
        }

        /// <summary>
        /// Gets the underlying list of features that this drawing filter
        /// is ultimately based upon.
        /// </summary>
        IFeatureList FeatureList
        {
            get;
        }

        /// <summary>
        /// If chunks are being used, then this indicates the total count of chunks.
        /// Otherwise, this returns 1 as everything is effectively in one chunk.
        /// </summary>
        int NumChunks
        {
            get;
        }

        /// <summary>
        /// If UseSelection is true, this will get or set the boolean selection state
        /// that will be used to select values.
        /// </summary>
        bool Selected
        {
            get;
            set;
        }

        /// <summary>
        /// This uses the feature as the key and attempts to find the specified drawn state
        /// that describes selection, chunk and category.
        /// </summary>
        /// <param name="key">The feature</param>
        /// <remarks>The strength is that if someone inserts a new member or re-orders
        /// the features in the featureset, we don't forget which ones are selected.
        /// The disadvantage is that duplicate features in the same featureset
        /// will cause an exception.</remarks>
        /// <returns></returns>
        IDrawnState this[IFeature key]
        {
            get;
            set;
        }

        /// <summary>
        /// This is less direct as it requires searching two indices rather than one, but
        /// allows access to the drawn state based on the feature ID.
        /// </summary>
        /// <param name="index">The integer index in the underlying featureSet.</param>
        /// <returns>The current IDrawnState for the current feature.</returns>
        IDrawnState this[int index]
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean that indicates whether we should use the chunk
        /// </summary>
        bool UseChunks
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean that indicates whether this filter should use the Selected
        /// </summary>
        bool UseSelection
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean that indicates whether the filter should subdivide based on category.
        /// </summary>
        bool UseCategory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean that indicates whether the filter should consider the IsVisible property
        /// </summary>
        bool UseVisibility
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean that specifies whether to return visible, or hidden features if UseVisibility is true.
        /// </summary>
        bool Visible
        {
            get;
            set;
        }

        #endregion
    }
}