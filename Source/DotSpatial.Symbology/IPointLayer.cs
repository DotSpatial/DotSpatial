// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This is a specialized FeatureLayer that specifically handles point drawing.
    /// </summary>
    public interface IPointLayer : IFeatureLayer
    {
        #region Properties

        /// <summary>
        /// Gets or sets the FeatureSymbolizerOld determining the shared properties. This is actually still the PointSymbolizerOld
        /// and should not be used directly on Polygons or Lines.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Unable to assign a non-point symbolizer to a PointLayer.</exception>
        new IPointSymbolizer Symbolizer { get; set; }

        /// <summary>
        /// Gets or sets the pointSymbolizer characteristics to use for the selected features.
        /// </summary>
        new IPointSymbolizer SelectionSymbolizer { get; set; }

        /// <summary>
        /// Gets or sets the currently applied scheme. Because setting the scheme requires a processor intensive
        /// method, we use the ApplyScheme method for assigning a new scheme. This allows access
        /// to editing the members of an existing scheme directly, however.
        /// </summary>
        new IPointScheme Symbology { get; set; }

        #endregion
    }
}