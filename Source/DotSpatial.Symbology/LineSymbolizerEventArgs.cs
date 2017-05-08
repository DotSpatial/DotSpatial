// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/20/2009 3:26:07 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// LineSymbolizerEventArgs
    /// </summary>
    public class LineSymbolizerEventArgs : FeatureSymbolizerEventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSymbolizerEventArgs"/> class.
        /// </summary>
        /// <param name="symbolizer">The symbolizer of the event.</param>
        public LineSymbolizerEventArgs(ILineSymbolizer symbolizer)
            : base(symbolizer)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Symbolizer.
        /// </summary>
        public new ILineSymbolizer Symbolizer
        {
            get
            {
                return base.Symbolizer as ILineSymbolizer;
            }

            set
            {
                base.Symbolizer = value;
            }
        }

        #endregion
    }
}