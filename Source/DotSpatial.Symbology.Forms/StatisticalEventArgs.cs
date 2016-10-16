// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/27/2009 9:32:38 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// StatisticalEventArgs
    /// </summary>
    public class StatisticalEventArgs : EventArgs
    {
        #region Private Variables

        private Statistics _stats;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of StatisticalEventArgs
        /// </summary>
        public StatisticalEventArgs(Statistics statistics)
        {
            _stats = statistics;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets the set of statistics related to this event.
        /// </summary>
        public Statistics Statistics
        {
            get { return _stats; }
            protected set { _stats = value; }
        }

        #endregion
    }
}