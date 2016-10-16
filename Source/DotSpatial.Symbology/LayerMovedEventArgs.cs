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
    public class LayerMovedEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of LayerEventArgs
        /// </summary>
        public LayerMovedEventArgs(ILayer layer, int newPosition)
        {
            Layer = layer;
            NewPosition = NewPosition;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Layer that was moved.
        /// </summary>
        public ILayer Layer { get; protected set; }

        /// <summary>
        /// Position the layer was moved to.
        /// </summary>
        public int NewPosition { get; protected set; }

        #endregion
    }
}