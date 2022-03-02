// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// This interface is used to access the list of shapes that were found during an Identify function call.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public interface IdentifiedShapes
    {
        #region Properties

        /// <summary>
        /// Gets the number of shapes that were identified.
        /// </summary>
        int Count { get; }

        #endregion

        /// <summary>
        /// Gets the shape index of an identified that is stored at the position
        /// specified by the Index parameter.
        /// </summary>
        /// <param name="index">Index of the element to get.</param>
        int this[int index] { get; }
    }
}