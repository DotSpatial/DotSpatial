// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/20/2009 3:18:22 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// FeatureSymbolizerEventArgs
    /// </summary>
    public class FeatureSymbolizerEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSymbolizerEventArgs"/> class.
        /// </summary>
        /// <param name="symbolizer">The feature symbolizer of the event.</param>
        public FeatureSymbolizerEventArgs(IFeatureSymbolizer symbolizer)
        {
            Symbolizer = symbolizer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the feature symbolizer for this event.
        /// </summary>
        public IFeatureSymbolizer Symbolizer { get; protected set; }

        #endregion
    }
}