// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Controls
{
    /// <summary>
    /// This interface is meant as base for layers that load themselves.
    /// </summary>
    public interface IMapSelfLoadLayer : IMapLayer
    {
        /// <summary>
        /// Gets or sets the file path of the layers underlying file.
        /// </summary>
        string FilePath { get; set; }
    }
}
