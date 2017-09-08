// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Data;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This interface is meant to be used as base for classes that can handle files that contain data for more than one layer.
    /// </summary>
    public interface ISelfLoadSet : IDataSet
    {
        /// <summary>
        /// Gets the IMapSelfLoadLayer that contains all the data of the underlying file.
        /// </summary>
        /// <returns>Null, if there is no data otherwise the IMapSelfLoadLayer that contains the data underlying file.</returns>
        IMapSelfLoadLayer GetLayer();

        /// <summary>
        /// Gets the IMapSelfLoadLayer that contains all the data of the underlying file.
        /// </summary>
        /// <param name="layerNames">Names of the layers that should be returned. This can be used for files that contain more than one layer but should not return all of them.</param>
        /// <returns>Null, if there is no data otherwise the IMapSelfLoadLayer that contains the data underlying file.</returns>
        IMapSelfLoadLayer GetLayer(string[] layerNames);
    }
}
