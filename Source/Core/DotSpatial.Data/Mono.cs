// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// This class contains methods for Mono.
    /// </summary>
    public static class Mono
    {
        /// <summary>
        /// Checks whether DotSpatial is running on Mono.
        /// </summary>
        /// <returns>True, if DotSpatial is running on Mono.</returns>
        public static bool IsRunningOnMono()
        {
            Type t = Type.GetType("Mono.Runtime");
            return t != null;
        }
    }
}