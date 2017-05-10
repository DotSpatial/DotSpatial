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

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Event args for events that need a layer.
    /// </summary>
    public class LayerEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerEventArgs"/> class.
        /// </summary>
        /// <param name="layer">The layer of the event.</param>
        public LayerEventArgs(ILayer layer)
        {
            Layer = layer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a layer.
        /// </summary>
        public ILayer Layer { get; protected set; }

        #endregion
    }
}