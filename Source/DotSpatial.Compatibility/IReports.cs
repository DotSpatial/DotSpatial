// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;

using GeoAPI.Geometries;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Reports
    /// </summary>
    public interface IReports
    {
        #region Methods

        /// <summary>
        /// Similar to <c>GetLegendSnapshot</c> except that only one layer is considered.
        /// </summary>
        /// <param name="layerHandle">Handle of the layer to take a snapshot of.</param>
        /// <param name="imgWidth">Maximum width of the image. The height of the image depends on the coloring scheme of the layer.</param>
        /// <returns>A snapshow of one legend layer.</returns>
        Image GetLegendLayerSnapshot(int layerHandle, int imgWidth);

        /// <summary>
        /// Returns an image of the legend.
        /// </summary>
        /// <param name="visibleLayersOnly">Specifies that only the visible layers are part of the snapshot.</param>
        /// <param name="imgWidth">Maximum width of the image. The height of the image depends on the number of layers loaded.</param>
        /// <returns>A snapshow of the legend.</returns>
        Image GetLegendSnapshot(bool visibleLayersOnly, int imgWidth);

        /// <summary>
        /// Returns an image of a north arrow.
        /// </summary>
        /// <returns>An image of a north arrow.</returns>
        Image GetNorthArrow();

        /// <summary>
        /// Returns an image that represents an accurate scale bar.
        /// </summary>
        /// <param name="mapUnits">You must specify what the map units are.</param>
        /// <param name="scalebarUnits">The unit of measurement to display on the scale bar. This function can convert the map units to any other unit.</param>
        /// <param name="maxWidth">Maximum width of the scale bar image.</param>
        /// <returns>An image that represents an accurate scale bar.</returns>
        Image GetScaleBar(UnitsOfMeasure mapUnits, UnitsOfMeasure scalebarUnits, int maxWidth);

        /// <summary>
        /// Returns an image that represents an accurate scale bar.
        /// </summary>
        /// <param name="mapUnits">You must specify what the map units are.</param>
        /// <param name="scalebarUnits">The unit of measurement to display on the scale bar. This function can convert the map units to any other unit.</param>
        /// <param name="maxWidth">Maximum width of the scale bar image.</param>
        /// <returns>An image that represents an accurate scale bar.</returns>
        Image GetScaleBar(string mapUnits, string scalebarUnits, int maxWidth);

        /// <summary>
        /// Returns a <c>MapWinGIS.Image</c> of the view at the specified extents.
        /// </summary>
        /// <param name="boundBox">The area that you wish to take the picture of. Uses projected map units.</param>
        /// <returns>a <c>MapWinGIS.Image</c> of the view at the specified extents.</returns>
        Image GetScreenPicture(Envelope boundBox);

        #endregion
    }
}