// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// IdentifiedLayers is used to access the list of layers that contained any
    /// information gathered during an Identify function call.
    /// </summary>
    public interface IIdentifiedLayers
    {
        #region Properties

        /// <summary>
        /// Gets the number of layers that had information from the Identify function call.
        /// </summary>
        int Count { get; }

        #endregion

        /// <summary>
        /// Returns an <c>IdentifiedShapes</c> object containing inforamtion about shapes that were
        /// identified during the Identify function call.
        /// </summary>
        /// <param name="layerHandle">Handle of the layer.</param>
        IdentifiedShapes this[int layerHandle] { get; }
    }
}