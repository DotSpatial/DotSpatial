// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for FeatureSelection.
    /// </summary>
    public interface IFeatureSelection : ICollection<IFeature>, ISelection
    {
        /// <summary>
        /// Gets the integer count of the members in the collection.
        /// </summary>
        new int Count { get; }

        /// <summary>
        /// Gets the drawing filter used by this collection.
        /// </summary>
        IDrawingFilter Filter { get; }

        /// <summary>
        /// Clears the selection.
        /// </summary>
        new void Clear();
    }
}