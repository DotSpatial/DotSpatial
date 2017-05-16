// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for DrawingFilter.
    /// </summary>
    public interface IDrawingFilter : IEnumerable<IFeature>, ICloneable
    {
        #region Events

        /// <summary>
        /// Occurs after this filter has built its internal list of items.
        /// </summary>
        event EventHandler Initialized;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the scheme category to use
        /// </summary>
        IFeatureCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the integer chunk that the filter should use.
        /// </summary>
        int Chunk { get; set; }

        /// <summary>
        /// Gets or sets the integer size of each chunk. Setting this to
        /// a new value will cycle through and update the chunk on all
        /// the features.
        /// </summary>
        int ChunkSize { get; set; }

        /// <summary>
        /// Gets the count. If the drawing state for any features has changed, or else if
        /// the state of any members has changed, this will cycle through
        /// the filter members and cache a new count. If nothing has
        /// changed, then this will simply return the cached value.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the default category for the scheme.
        /// </summary>
        IFeatureCategory DefaultCategory { get; }

        /// <summary>
        /// Gets the dictionary of drawn states that this drawing filter uses.
        /// </summary>
        IDictionary<IFeature, IDrawnState> DrawnStates { get; }

        /// <summary>
        /// Gets the underlying list of features that this drawing filter
        /// is ultimately based upon.
        /// </summary>
        IFeatureList FeatureList { get; }

        /// <summary>
        /// Gets the total count of chunks.
        /// If not chunks are used, this returns 1 as everything is effectively in one chunk.
        /// </summary>
        int NumChunks { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the items are selected. If UseSelection is true, this will get or set the boolean selection state
        /// that will be used to select values.
        /// </summary>
        bool Selected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the filter should subdivide based on category.
        /// </summary>
        bool UseCategory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we should use the chunk.
        /// </summary>
        bool UseChunks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this filter should use the Selected
        /// </summary>
        bool UseSelection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the filter should consider the IsVisible property
        /// </summary>
        bool UseVisibility { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to return visible, or hidden features if UseVisibility is true.
        /// </summary>
        bool Visible { get; set; }

        #endregion

        #region Indexers

        /// <summary>
        /// This uses the feature as the key and attempts to find the specified drawn state
        /// that describes selection, chunk and category.
        /// </summary>
        /// <param name="key">The feature</param>
        /// <remarks>The strength is that if someone inserts a new member or re-orders
        /// the features in the featureset, we don't forget which ones are selected.
        /// The disadvantage is that duplicate features in the same featureset
        /// will cause an exception.</remarks>
        /// <returns>The drawn state for the feature.</returns>
        IDrawnState this[IFeature key] { get; set; }

        /// <summary>
        /// This is less direct as it requires searching two indices rather than one, but
        /// allows access to the drawn state based on the feature ID.
        /// </summary>
        /// <param name="index">The integer index in the underlying featureSet.</param>
        /// <returns>The current IDrawnState for the current feature.</returns>
        IDrawnState this[int index] { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This will set all values to the default (0) category. Then, it will use the filter
        /// expressions on the remaining categories to change the categories for those members.
        /// This means that an item will be classified as the last filter that it qualifies for.
        /// </summary>
        /// <param name="scheme">The scheme of categories to apply to the drawing states</param>
        void ApplyScheme(IFeatureScheme scheme);

        /// <summary>
        /// If UseChunks is true, this uses the index value combined with the chunk size
        /// to calculate the chunk, and also sets the category to the [0] category and the
        /// selection state to unselected. This can be overridden in sub-classes to come up
        /// with a different default state.
        /// </summary>
        /// <param name="index">The integer index to get the default state of</param>
        /// <returns>An IDrawnState</returns>
        IDrawnState GetDefaultState(int index);

        /// <summary>
        /// Invalidates this drawing filter, forcing a re-creation
        /// of the entire dictionary from the source featureset.
        /// This should only be done if changes are made to the
        /// feature list while SuspendChanges on the list is true.
        /// </summary>
        void Invalidate();

        #endregion
    }
}