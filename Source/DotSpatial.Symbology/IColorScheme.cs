// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/11/2009 11:56:59 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Represents scheme with colors support.
    /// </summary>
    public interface IColorScheme : IScheme
    {
        #region Methods

        /// <summary>
        /// Applies the specified color scheme from a list of predefined scheme options.
        /// </summary>
        /// <param name="schemeType">The predefined color scheme</param>
        /// <param name="raster">The raster that provides values to govern symbolizing</param>
        void ApplyScheme(ColorSchemeType schemeType, IRaster raster);

        /// <summary>
        /// Creates the categories for this scheme based on statistics and values
        /// sampled from the specified raster.
        /// </summary>
        /// <param name="raster">The raster to use when creating categories</param>
        void CreateCategories(IRaster raster);

        /// <summary>
        /// Gets the values from the raster.  If MaxSampleCount is less than the
        /// number of cells, then it randomly samples the raster with MaxSampleCount
        /// values.  Otherwise it gets all the values in the raster.
        /// </summary>
        /// <param name="raster">The raster to sample</param>
        void GetValues(IRaster raster);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the floating point value for the opacity
        /// </summary>
        float Opacity { get; set; }

        /// <summary>
        /// Gets or sets the collection of color scheme categories to use.
        /// </summary>
        ColorCategoryCollection Categories { get; set; }

        /// <summary>
        /// gets or sets the editor settings for controls that affect the color scheme.
        /// </summary>
        new RasterEditorSettings EditorSettings { get; set; }

        #endregion
    }
}