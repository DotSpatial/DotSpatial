// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
   /// <summary>
   /// Indicates which kind of clear operation should be used.
   /// </summary>
   public enum ClearStates
    {
        /// <summary>
        /// The selected features won't be cleared.
        /// </summary>
        False = 0,

        /// <summary>
        /// The selected features will only be cleared if SelectionEnabled is true.
        /// </summary>
        True = 1,

        /// <summary>
        /// The selected features will be cleared even if SelectionEnabled is false.
        /// </summary>
        Force = 2
    }
}
