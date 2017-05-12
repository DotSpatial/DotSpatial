// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/19/2010 9:44:32 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// PngInsuficientLengthException
    /// </summary>
    public class PngInsuficientLengthException : ArgumentException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PngInsuficientLengthException"/> class.
        /// </summary>
        /// <param name="length">The desired length.</param>
        /// <param name="totalLength">The total length.</param>
        /// <param name="offset">The offset.</param>
        public PngInsuficientLengthException(int length, int totalLength, int offset)
            : base(string.Format(DataStrings.PngInsuficientLengthException, length, totalLength, offset))
        {
        }

        #endregion
    }
}