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
    /// <summary>
    /// Interface for PointScheme.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public interface IPointScheme : IFeatureScheme
    {
        #region Properties

        /// <summary>
        /// Gets or sets the list of scheme categories belonging to this scheme.
        /// </summary>
        PointCategoryCollection Categories { get; set; }

        #endregion
    }
}