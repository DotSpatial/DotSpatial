// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Globalization;

namespace DotSpatial.Data
{
    /// <summary>
    /// CulturePreferences.
    /// </summary>
    public static class CulturePreferences
    {
        /// <summary>
        /// Gets or sets the CultureInformation. This culture information is useful for things like Number Formatting.
        /// This defaults to CurrentCulture, but can be specified through preferences or whatever.
        /// </summary>
        public static CultureInfo CultureInformation { get; set; } = CultureInfo.CurrentCulture;
    }
}