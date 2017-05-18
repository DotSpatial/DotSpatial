// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ModifySelectionModes
    /// </summary>
    public enum ModifySelectionMode
    {
        /// <summary>
        /// Appends the newly selected features to the existing selection
        /// </summary>
        Append,

        /// <summary>
        /// Subtracts the newly selected features from the existing features.
        /// </summary>
        Subtract,

        /// <summary>
        /// Clears the current selection and selects the new features
        /// </summary>
        Replace,

        /// <summary>
        /// Selects the new features only from the existing selection
        /// </summary>
        SelectFrom
    }
}