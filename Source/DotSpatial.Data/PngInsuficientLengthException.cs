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
    public class PngInsuficientLengthException : ArgumentException
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of PngInsuficientLengthException
        /// </summary>
        public PngInsuficientLengthException(int length, int totalLength, int offset)
            : base(DataStrings.PngInsuficientLengthException.Replace("%S1", length.ToString()).Replace("%S2", totalLength.ToString()).Replace("%S3", offset.ToString()))
        {
        }

        #endregion
    }
}