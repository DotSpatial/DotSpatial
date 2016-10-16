// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/20/2009 2:49:20 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IPointSchemeCategory
    /// </summary>
    public interface IPolygonCategory : IFeatureCategory
    {
        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbolizer for this category
        /// </summary>
        new IPolygonSymbolizer Symbolizer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the symbolizer to use to draw selected features from this category.
        /// </summary>
        new IPolygonSymbolizer SelectionSymbolizer
        {
            get;
            set;
        }

        #endregion
    }
}