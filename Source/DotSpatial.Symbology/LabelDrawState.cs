// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/21/2009 2:13:13 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// DrawnState for labels.
    /// </summary>
    public class LabelDrawState
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelDrawState"/> class where selected is false but visible is true.
        /// </summary>
        /// <param name="category">The category</param>
        public LabelDrawState(ILabelCategory category)
        {
            Category = category;
            Visible = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelDrawState"/> class.
        /// </summary>
        /// <param name="category">The category</param>
        /// <param name="selected">Boolean, true if the label is selected</param>
        /// <param name="visible">Boolean, true if the label should be visible</param>
        public LabelDrawState(ILabelCategory category, bool selected, bool visible)
        {
            Category = category;
            Selected = selected;
            Visible = visible;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a LabelCategory interface representing the drawing information for this label.
        /// </summary>
        public ILabelCategory Category { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this is selected.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the associated feature should be drawn.
        /// </summary>
        public bool Visible { get; set; }

        #endregion
    }
}