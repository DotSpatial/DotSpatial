// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for IndexSelection.
    /// </summary>
    public interface IIndexSelection : ISelection, ICollection<int>
    {
        #region Properties

        /// <summary>
        /// Gets the integer count of the members in the collection
        /// </summary>
        new int Count { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a range of indices all at once.
        /// </summary>
        /// <param name="indices">The indices to add</param>
        void AddRange(IEnumerable<int> indices);

        /// <summary>
        /// Clears the selection
        /// </summary>
        new void Clear();

        /// <summary>
        /// Removes a set of indices all at once.
        /// </summary>
        /// <param name="indices">Indices that get removed.</param>
        /// <returns>True, if the selection was changed.</returns>
        bool RemoveRange(IEnumerable<int> indices);

        #endregion
    }
}