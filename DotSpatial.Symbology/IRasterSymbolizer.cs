// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/20/2008 3:44:36 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IRasterSymbolizer
    /// </summary>
    public interface IRasterSymbolizer : ILegendItem
    {
        #region Events

        /// <summary>
        /// This event occurs after a new bitmap has been created to act as a texture.
        /// </summary>
        event EventHandler ColorSchemeUpdated;

        #endregion

        #region Methods

        /// <summary>
        /// Creates a bmp from the in-memory portion of the raster.
        /// </summary>
        /// <returns></returns>
        Bitmap CreateBitmap();

        /// <summary>
        /// Causes the raster to calculate a hillshade based on this symbolizer
        /// </summary>
        void CreateHillShade();

        /// <summary>
        /// Causes the raster to calculate a hillshade using the specified progress handler
        /// </summary>
        /// <param name="progressHandler">The progress handler to use</param>
        void CreateHillShade(IProgressHandler progressHandler);

        /// <summary>
        /// Gets the color for a specific row and column.  This does not include any hillshade information.
        /// </summary>
        /// <param name="value">The double value to find in the colorbreaks.</param>
        /// <returns>A Color</returns>
        Color GetColor(double value);

        /// <summary>
        /// Creates a bitmap based on the specified RasterSymbolizer
        /// </summary>
        /// <param name="bitmap"> the bitmap to paint to</param>
        /// <param name="progressHandler">The progress handler</param>
        void PaintShadingToBitmap(Bitmap bitmap, IProgressHandler progressHandler);

        /// <summary>
        /// Sends a symbology updated event, which should cause the layer to be refreshed.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Indicates that the bitmap has been updated and that the colorscheme is currently
        /// synchronized with the characteristics of this symbolizer.  This also fires the
        /// ColorSchemeChanged event.
        /// </summary>
        void Validate();

        #endregion

        #region Properties

        /// <summary>
        /// If this value is true, whenever a texture is created, the vector layers are drawn onto the texture.
        /// </summary>
        bool DrapeVectorLayers
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the editor settings class to help setup up the symbology controls appropriately.
        /// </summary>
        RasterEditorSettings EditorSettings
        {
            get;
            set;
        }

        /// <summary>
        /// This is kept separate from extrusion to reduce confusion.  This is a conversion factor that will
        /// convert the units of elevation into the same units that the latitude and longitude are stored in.
        /// To convert feet to decimal degrees is around a factor of .00000274.  This is used only in the
        /// 3D-context and does not affect ShadedRelief.
        /// </summary>
        float ElevationFactor
        {
            get;
            set;
        }

        /// <summary>
        /// A float value expression that modifies the "height" of the apparent shaded relief.  A value
        /// of 1 should show the mountains at their true elevations, presuming the ElevationFactor is
        /// correct.  A value of 0 would be totally flat, while 2 would be twice the value.  This controls
        /// the 3D effects and has nothing to do with the creation of shaded releif on the texture.
        /// </summary>
        float Extrusion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the calculated hillshade map, or re-calculates it if something has changed
        /// </summary>
        float[][] HillShade
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the symbol characteristics for the border of this raster
        /// </summary>
        IFeatureSymbolizerOld ImageOutline
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean that determines whether to treat the values as if they are elevation
        /// in the 3-D context.  If this is true, then it will automatically use this grid for
        /// calculating elevation values.
        /// </summary>
        bool IsElevation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether or not this raster should render itself
        /// </summary>
        bool IsVisible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether or not htis raster should be anti-alliased
        /// </summary>
        bool IsSmoothed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the color to use if the value of the cell corresponds to a No-Data value
        /// </summary>
        Color NoDataColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a float value from 0 to 1, where 1 is fully opaque while 0 is fully transparent
        /// </summary>
        float Opacity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the raster that should provide elevation values, but only if "IsElevation" is false.
        /// </summary>
        IRaster Raster
        {
            get;
            set;
        }

        /// <summary>
        /// This should be set to true if the elevation values have changed
        /// </summary>
        bool MeshHasChanged
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the parent layer.  This is not always used, but can be useful for symbolic editing
        /// that may require a bitmap to be drawn with draped vector layers.
        /// </summary>
        IRasterLayer ParentLayer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the coloring scheme for the raster.
        /// </summary>
        IColorScheme Scheme
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the characteristics of the shaded relief.  This only affects the coloring,
        /// and will not control any 3-D properties.
        /// </summary>
        IShadedRelief ShadedRelief
        {
            get;
            set;
        }

        /// <summary>
        /// This should be set to true if the texture needs to be reloaded from a file
        /// </summary>
        bool ColorSchemeHasUpdated
        {
            get;
            set;
        }

        #endregion
    }
}