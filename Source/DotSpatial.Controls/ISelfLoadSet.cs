// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using DotSpatial.Data;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This interface is meant to be used as base for classes that can handle files that contain data for more than one layer.
    /// </summary>
    public interface ISelfLoadSet : IDataSet
    {
        /// <summary>
        /// Gets a list of the DataSets contained in this ISelfLoadSet.
        /// </summary>
        IList<IDataSet> DataSets { get; }

        /// <summary>
        /// Gets or sets the name that should be shown as the LegendText of the MapSelfLoadGroup returned by GetLayer.
        /// </summary>
        string GroupName { get; set; }

        /// <summary>
        /// Gets the IMapSelfLoadLayer that contains all the data of the datasets.
        /// </summary>
        /// <returns>Null, if there a no DataSets otherwise the IMapSelfLoadLayer that contains the data of all the datasets.</returns>
        IMapSelfLoadLayer GetLayer();
    }
}
