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
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticalEventArgs"/> class.
        /// </summary>
        /// <param name="statistics">The statistics of this event.</param>
        public StatisticalEventArgs(Statistics statistics)
        {
            Statistics = statistics;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the set of statistics related to this event.
        /// </summary>
        public Statistics Statistics { get; protected set; }

        #endregion
    }
}