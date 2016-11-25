// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/15/2009 3:24:43 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology
{
    public class ChangedObjectEventArgs : EventArgs
    {
        #region Private Variables

        private readonly object _item;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ChangedObjectEvent
        /// </summary>
        public ChangedObjectEventArgs(object changedItem)
        {
            _item = changedItem;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the item that has been changed
        /// </summary>
        public object Item
        {
            get { return _item; }
        }

        #endregion
    }
}