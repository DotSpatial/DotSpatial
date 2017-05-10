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
    /// Event args for the LayerMoved event.
    /// </summary>
    public class LayerMovedEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerMovedEventArgs"/> class.
        /// </summary>
        /// <param name="layer">Layer that was moved.</param>
        /// <param name="newPosition">Position the layer was moved to.</param>
        public LayerMovedEventArgs(ILayer layer, int newPosition)
        {
            Layer = layer;
            NewPosition = newPosition;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the layer that was moved.
        /// </summary>
        public ILayer Layer { get; protected set; }

        /// <summary>
        /// Gets or sets the position the layer was moved to.
        /// </summary>
        public int NewPosition { get; protected set; }

        #endregion
    }
}