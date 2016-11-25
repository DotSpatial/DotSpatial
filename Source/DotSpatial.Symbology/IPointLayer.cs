// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Modified to do 3D in January 2008 by Ted Dunsford
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This is a specialized FeatureLayer that specifically handles point drawing
    /// </summary>
    public interface IPointLayer : IFeatureLayer
    {
        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the FeatureSymbolizerOld determining the shared properties.  This is actually still the PointSymbolizerOld
        /// and should not be used directly on Polygons or Lines.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Unable to assign a non-point symbolizer to a PointLayer</exception>
        new IPointSymbolizer Symbolizer { get; set; }

        /// <summary>
        /// Gets or sets the pointSymbolizer characteristics to use for the selected features.
        /// </summary>
        new IPointSymbolizer SelectionSymbolizer { get; set; }

        /// <summary>
        /// Gets the currently applied scheme.  Because setting the scheme requires a processor intensive
        /// method, we use the ApplyScheme method for assigning a new scheme.  This allows access
        /// to editing the members of an existing scheme directly, however.
        /// </summary>
        new IPointScheme Symbology { get; set; }

        #endregion
    }
}