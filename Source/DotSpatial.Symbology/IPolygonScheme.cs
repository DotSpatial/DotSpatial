// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/21/2009 9:57:24 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.ComponentModel;

namespace DotSpatial.Symbology
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public interface IPolygonScheme : IFeatureScheme
    {
        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of scheme categories belonging to this scheme.
        /// </summary>
        PolygonCategoryCollection Categories { get; set; }

        #endregion
    }
}