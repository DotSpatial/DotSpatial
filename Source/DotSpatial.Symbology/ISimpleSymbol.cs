// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/11/2009 3:17:59 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ISimpleSymbol
    /// </summary>
    public interface ISimpleSymbol : IOutlinedSymbol, IColorable
    {
        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the PointTypes enumeration that describes how to draw the simple symbol.
        /// </summary>
        PointShape PointShape
        {
            get;
            set;
        }

        #endregion
    }
}