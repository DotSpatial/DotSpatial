// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/20/2009 3:49:30 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// PolygonSymbolizerEventArgs
    /// </summary>
    public class PolygonSymbolizerEventArgs : FeatureSymbolizerEventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonSymbolizerEventArgs"/> class.
        /// </summary>
        /// <param name="symbolizer">Symbolizer of the event.</param>
        public PolygonSymbolizerEventArgs(IPolygonSymbolizer symbolizer)
            : base(symbolizer)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Symbolizer, casting it to an IPolygonSymbolizer
        /// </summary>
        public new IPolygonSymbolizer Symbolizer
        {
            get
            {
                return base.Symbolizer as IPolygonSymbolizer;
            }

            set
            {
                base.Symbolizer = value;
            }
        }

        #endregion
    }
}