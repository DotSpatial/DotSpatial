// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  A library module for the DotSpatial geospatial framework for .Net.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/13/2008 1:12:13 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// HighlightEventArgs
    /// </summary>
    public class HighlightEventArgs : EventArgs
    {
        #region Private Variables

        private bool _isHighlighted;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of HighlightEventArgs
        /// </summary>
        public HighlightEventArgs(bool isHighlighted)
        {
            _isHighlighted = isHighlighted;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether or not the control is now highlighted
        /// </summary>
        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            protected set { _isHighlighted = value; }
        }

        #endregion
    }
}