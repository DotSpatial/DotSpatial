// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in September, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A layer with drawing characteristics for LineStrings
    /// </summary>
    public interface IPolygonLayer : IFeatureLayer
    {
        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbolizer describing the selection on the default category.
        /// </summary>
        new IPolygonSymbolizer SelectionSymbolizer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the symbolizer describing the symbolizer on the default category.
        /// </summary>
        new IPolygonSymbolizer Symbolizer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the polygon scheme that symbolically breaks down the drawing into symbol categories.
        /// </summary>
        new IPolygonScheme Symbology
        {
            get;
            set;
        }

        #endregion
    }
}