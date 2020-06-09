// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using DotSpatial.Data;
using NetTopologySuite.Geometries;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for selection.
    /// </summary>
    public interface ISelection : IAttributeSource
    {
        #region Events

        /// <summary>
        /// Occurs when members are added to or removed from this collection. If SuspendChanges
        /// is called, this will temporarilly prevent this event from firing, until ResumeEvents
        /// has been called.
        /// </summary>
        event EventHandler Changed;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether changes are suspended. To suspend events, call SuspendChanges. Then to resume events, call ResumeEvents. If the
        /// suspension is greater than 0, then events are suspended.
        /// </summary>
        bool ChangesSuspended { get; }

        /// <summary>
        /// Gets the integer count of the members in the collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the envelope of this collection.
        /// </summary>
        Envelope Envelope { get; }

        /// <summary>
        /// Gets or sets the handler to use for progress messages during selection.
        /// </summary>
        IProgressHandler ProgressHandler { get; set; }

        /// <summary>
        /// Gets or sets the region category. Setting this to a specific category will only allow selection by
        /// region to affect the features that are within the specified category.
        /// </summary>
        IFeatureCategory RegionCategory { get; set; }

        /// <summary>
        /// Gets or sets the selection mode to use when Adding or Removing features
        /// from a specified envelope region.
        /// </summary>
        SelectionMode SelectionMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether members of this collection are considered selected.
        /// </summary>
        bool SelectionState { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This uses extent checking (rather than full polygon intersection checking). It will add
        /// any members that are either contained by or intersect with the specified region
        /// depending on the SelectionMode property. The order of operation is the region
        /// acting on the feature, so Contains, for instance, would work with points.
        /// </summary>
        /// <param name="region">The region that contains the features that get added.</param>
        /// <param name="affectedArea">The affected area of this addition.</param>
        /// <returns>True if any item was actually added to the collection.</returns>
        bool AddRegion(Envelope region, out Envelope affectedArea);

        /// <summary>
        /// Clears the selection.
        /// </summary>
        void Clear();

        /// <summary>
        /// Inverts the selection based on the current SelectionMode.
        /// </summary>
        /// <param name="region">The geographic region to reverse the selected state.</param>
        /// <param name="affectedArea">The affected area to invert.</param>
        /// <returns>True, if the selection was changed.</returns>
        bool InvertSelection(Envelope region, out Envelope affectedArea);

        /// <summary>
        /// Tests each member currently in the selected features based on
        /// the SelectionMode. If it passes, it will remove the feature from
        /// the selection.
        /// </summary>
        /// <param name="region">The geographic region to remove.</param>
        /// <param name="affectedArea">A geographic area that was affected by this change.</param>
        /// <returns>Boolean, true if the collection was changed.</returns>
        bool RemoveRegion(Envelope region, out Envelope affectedArea);

        /// <summary>
        /// Resumes the events. If any changes occured during the period of time when
        /// the events were suspended, this will automatically fire the chnaged event.
        /// </summary>
        void ResumeChanges();

        /// <summary>
        /// Causes this filter collection to suspend the Changed event, so that
        /// it will only be fired once after a series of updates.
        /// </summary>
        void SuspendChanges();

        /// <summary>
        /// Exports the members of this collection as a list of IFeature.
        /// </summary>
        /// <returns>A List of IFeature.</returns>
        List<IFeature> ToFeatureList();

        /// <summary>
        /// Returns a new featureset based on the features in this collection.
        /// </summary>
        /// <returns>An in memory featureset that has not yet been saved to a file in any way.</returns>
        FeatureSet ToFeatureSet();

        #endregion
    }
}