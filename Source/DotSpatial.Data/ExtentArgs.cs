// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/11/2009 2:34:48 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// An EventArgs class for passing around an extent.
    /// </summary>
    public class ExtentArgs : EventArgs
    {
        private Extent _extent;

        /// <summary>
        /// Initializes a new instance of the ExtentArgs class.
        /// </summary>
        /// <param name="value">The value for this event.</param>
        public ExtentArgs(Extent value)
        {
            _extent = value;
        }

        /// <summary>
        /// Gets or sets the Extents for this event.
        /// </summary>
        public Extent Extent
        {
            get { return _extent; }
            protected set { _extent = value; }
        }
    }
}