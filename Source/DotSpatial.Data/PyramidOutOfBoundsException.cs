// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/11/2010 11:51:17 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// The exception that is thrown when range specified is outside the bounds for the specified image scale.
    /// </summary>
    public class PyramidOutOfBoundsException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PyramidOutOfBoundsException"/> class.
        /// </summary>
        public PyramidOutOfBoundsException()
            : base(DataStrings.PyramidOutOfBoundsException)
        {
        }

        #endregion
    }
}