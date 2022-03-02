// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Projections.Forms
{
    /// <summary>
    /// The Undefined Projection Action enumeration.
    /// </summary>
    public enum UndefinedProjectionAction
    {
        /// <summary>
        /// No action should be taken.
        /// </summary>
        Nothing,

        /// <summary>
        /// Always assume an undefined projection is Latitude Longitude
        /// </summary>
        Wgs84,

        /// <summary>
        /// Always rely on the existing Map projection
        /// </summary>
        Map,

        /// <summary>
        /// Use a projection that was specified from the list
        /// </summary>
        Chosen
    }
}