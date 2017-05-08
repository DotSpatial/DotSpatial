// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/25/2009 3:48:20 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Selection is used to remember all the selected features of the corresponding IFeatureSet.
    /// </summary>
    public class Selection : FeatureSelection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Selection"/> class.
        /// </summary>
        /// <param name="fs">The feature set.</param>
        /// <param name="inFilter">The drawing filter.</param>
        public Selection(IFeatureSet fs, IDrawingFilter inFilter)
            : base(fs, inFilter, FilterType.Selection)
        {
            Selected = true;
            UseSelection = true;
            UseCategory = false;
            UseVisibility = false;
            UseChunks = false;
            SelectionMode = SelectionMode.IntersectsExtent;
        }

        #endregion
    }
}