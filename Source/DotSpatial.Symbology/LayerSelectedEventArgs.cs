// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/10/2008 3:05:28 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// LayerSelectedEventArgs
    /// </summary>
    public class LayerSelectedEventArgs : LayerEventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerSelectedEventArgs"/> class.
        /// </summary>
        /// <param name="layer">The layer of the event.</param>
        /// <param name="selected">Indicates whether the layer is selected.</param>
        public LayerSelectedEventArgs(ILayer layer, bool selected)
            : base(layer)
        {
            IsSelected = selected;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether or not the layer is selected.
        /// </summary>
        public bool IsSelected { get; protected set; }

        #endregion
    }
}