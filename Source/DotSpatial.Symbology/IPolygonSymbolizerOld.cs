// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// An interface for a symbolizer specific to polygons.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public interface IPolygonSymbolizerOld : IFeatureSymbolizerOld
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether or not the polygon border should be drawn.
        /// </summary>
        bool BorderIsVisible { get; set; }

        /// <summary>
        /// Gets or sets the border symbolizer.
        /// </summary>
        ILineSymbolizer BorderSymbolizer { get; set; }

        #endregion
    }
}