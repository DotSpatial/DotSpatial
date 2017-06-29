// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 11:33:47 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using DotSpatial.Topology;

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
        /// <param name="imgWidth">Maximum width of the image.  The height of the image depends on the coloring scheme of the layer.</param>
        Image GetLegendLayerSnapshot(int layerHandle, int imgWidth);

        /// <summary>
        /// Returns an image of the legend.
        /// </summary>
        /// <param name="visibleLayersOnly">Specifies that only the visible layers are part of the snapshot.</param>
        /// <param name="imgWidth">Maximum width of the image.  The height of the image depends on the number of layers loaded.</param>
        Image GetLegendSnapshot(bool visibleLayersOnly, int imgWidth);

        /// <summary>
        /// Returns an image of a north arrow.
        /// </summary>
        Image GetNorthArrow();

        /// <summary>
        /// Returns an image that represents an accurate scale bar.
        /// </summary>
        /// <param name="mapUnits">You must specify what the map units are.</param>
        /// <param name="scalebarUnits">The unit of measurement to display on the scale bar.  This function can convert the map units to any other unit.</param>
        /// <param name="maxWidth">Maximum width of the scale bar image.</param>
        Image GetScaleBar(UnitsOfMeasure mapUnits, UnitsOfMeasure scalebarUnits, int maxWidth);

        /// <summary>
        /// Returns an image that represents an accurate scale bar.
        /// </summary>
        /// <param name="mapUnits">You must specify what the map units are.</param>
        /// <param name="scalebarUnits">The unit of measurement to display on the scale bar.  This function can convert the map units to any other unit.</param>
        /// <param name="maxWidth">Maximum width of the scale bar image.</param>
        Image GetScaleBar(string mapUnits, string scalebarUnits, int maxWidth);

        /// <summary>
        /// Returns a <c>MapWinGIS.Image</c> of the view at the specified extents.
        /// </summary>
        /// <param name="boundBox">The area that you wish to take the picture of.  Uses projected map units.</param>
        Image GetScreenPicture(Envelope boundBox);

        #endregion
    }
}