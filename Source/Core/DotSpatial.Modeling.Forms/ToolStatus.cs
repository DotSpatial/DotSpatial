// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// Indicates the allowed values for the status of the element, illustrated by the light.
    /// </summary>
    public enum ToolStatus
    {
        /// <summary>
        /// Indicates that no value has been set for this yet.
        /// </summary>
        Empty,

        /// <summary>
        /// Indicates that the element parameter is ok and won't halt.
        /// </summary>
        Ok,

        /// <summary>
        /// Indicates that the element value will cause an error.
        /// </summary>
        Error,
    }
}