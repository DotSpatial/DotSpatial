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
//
// ********************************************************************************************************

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