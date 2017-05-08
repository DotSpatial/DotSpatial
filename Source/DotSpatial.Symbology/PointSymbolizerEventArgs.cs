// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/20/2009 3:17:36 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// PointSymbolizerEventArgs
    /// </summary>
    public class PointSymbolizerEventArgs : FeatureSymbolizerEventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PointSymbolizerEventArgs"/> class.
        /// </summary>
        /// <param name="symbolizer">The symbolizer of the event.</param>
        public PointSymbolizerEventArgs(IPointSymbolizer symbolizer)
            : base(symbolizer)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbolizer cast as an IPointSymbolizer.
        /// </summary>
        public new IPointSymbolizer Symbolizer
        {
            get
            {
                return base.Symbolizer as IPointSymbolizer;
            }

            protected set
            {
                base.Symbolizer = value;
            }
        }

        #endregion
    }
}