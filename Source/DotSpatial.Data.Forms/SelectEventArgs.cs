// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  A library module for the DotSpatial geospatial framework for .Net.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/13/2008 2:01:06 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Windows.Forms;

namespace DotSpatial.Data.Forms
{
    public class SelectEventArgs : EventArgs
    {
        #region Private Variables

        private bool _isSelected;
        private Keys _modifiers;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SelectEventArgs
        /// </summary>
        public SelectEventArgs(bool isSelected)
        {
            _isSelected = isSelected;
            _modifiers = Control.ModifierKeys;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether or not the new state of the item is selected
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            protected set { _isSelected = value; }
        }

        /// <summary>
        /// Gets the modifiers that existed when this event is fired.
        /// use like if (Modifiers and Keys.Shift){}
        /// </summary>
        public Keys Modifiers
        {
            get { return _modifiers; }
            protected set { _modifiers = value; }
        }

        #endregion
    }
}