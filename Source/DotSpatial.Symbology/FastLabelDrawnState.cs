// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 12/3/2009 12:26:14 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Fast drawn state for labels.
    /// </summary>
    public class FastLabelDrawnState
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FastLabelDrawnState"/> class.
        /// </summary>
        /// <param name="category">The category</param>
        public FastLabelDrawnState(ILabelCategory category)
        {
            Category = category;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        public ILabelCategory Category { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the label is selected.
        /// </summary>
        public bool Selected { get; set; }

        #endregion
    }
}