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
    /// Indicates whether the control is now highlighted.
    /// </summary>
    public class HighlightEventArgs : EventArgs
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightEventArgs"/> class.
        /// </summary>
        /// <param name="isHighlighted">Indicates whether the control is now highlighted.</param>
        public HighlightEventArgs(bool isHighlighted)
        {
            IsHighlighted = isHighlighted;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether or not the control is now highlighted.
        /// </summary>
        public bool IsHighlighted { get; protected set; }

        #endregion
    }
}