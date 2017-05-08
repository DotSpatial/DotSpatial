// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/18/2009 1:30:48 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Event args for events that need a feature layer selection.
    /// </summary>
    public class FeatureLayerSelectionEventArgs : FeatureLayerEventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureLayerSelectionEventArgs"/> class.
        /// </summary>
        /// <param name="fl">The feature layer.</param>
        /// <param name="selection">The selection.</param>
        public FeatureLayerSelectionEventArgs(IFeatureLayer fl, ISelection selection)
            : base(fl)
        {
            Selection = selection;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list of changed features.
        /// </summary>
        public ISelection Selection { get; protected set; }

        #endregion
    }
}