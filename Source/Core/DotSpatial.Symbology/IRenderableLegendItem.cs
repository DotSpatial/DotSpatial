// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Items with this setup can both be organized as an item,
    /// and feature the elemental control methods and properties
    /// around drawing. Layers, MapFrames, groups etc can fall in this
    /// category.
    /// </summary>
    public interface IRenderableLegendItem : IRenderable, ILegendItem
    {
    }
}