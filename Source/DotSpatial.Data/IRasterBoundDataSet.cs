// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// This interface specifically insists on an IRasterBounds as the Bounds property.
    /// </summary>
    public interface IRasterBoundDataSet : IDataSet, IContainRasterBounds
    {
    }
}