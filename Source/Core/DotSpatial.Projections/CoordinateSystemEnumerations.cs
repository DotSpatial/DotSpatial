// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotSpatial.Projections
{
    /// <summary>
    /// An emum that describes the coordinate system type. A spatial reference system, also referred to as a coordinate system,
    /// is a geographic (latitude-longitude), a projected (X,Y), or a geocentric (X,Y,Z) coordinate system.
    /// </summary>
    /// <remarks>Based on article: https://github.com/NetTopologySuite/ProjNet4GeoAPI/wiki/Well-Known-Text</remarks>
    public enum CoordinateSystemType
    {
        /// <summary>
        /// Unknown coordinate system.
        /// </summary>
        Unknown,

        /// <summary>
        /// Geocentric coordinate system.
        /// </summary>
        Geocentric,

        /// <summary>
        /// Geographic coordinate system.
        /// </summary>
        Geographic,

        /// <summary>
        /// Projected coordinate system.
        /// </summary>
        Projected,
    }
}
