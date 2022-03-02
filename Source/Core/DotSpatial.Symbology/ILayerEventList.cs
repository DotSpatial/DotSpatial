// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for LayerEventList.
    /// </summary>
    /// <typeparam name="T">Type of the items in the list.</typeparam>
    public interface ILayerEventList<T> : ILayerEvents, IChangeEventList<T>
        where T : ILayer
    {
    }
}