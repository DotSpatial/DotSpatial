// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for LineScheme.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public interface ILineScheme : IFeatureScheme
    {
        #region Properties

        /// <summary>
        /// Gets or sets the list of scheme categories belonging to this scheme.
        /// </summary>
        LineCategoryCollection Categories { get; set; }

        #endregion
    }
}