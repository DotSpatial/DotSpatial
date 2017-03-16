// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/23/2009 10:26:39 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    public interface IDrawnState
    {
        #region Properties

        /// <summary>
        /// Gets or sets the integer chunk that this item belongs to.
        /// </summary>
        int Chunk { get; set; }

        /// <summary>
        /// Gets or sets the scheme category
        /// </summary>
        IFeatureCategory SchemeCategory { get; set; }

        /// <summary>
        /// Gets or sets a boolean, true if this feature is currently selected
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets whether this feature is currently being drawn.
        /// </summary>
        bool IsVisible { get; set; }

        #endregion
    }
}