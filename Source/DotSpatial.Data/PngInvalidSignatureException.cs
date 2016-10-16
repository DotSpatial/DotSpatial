// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/19/2010 1:56:45 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// PngInvalidSignatureException
    /// </summary>
    public class PngInvalidSignatureException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of PngInvalidSignatureException
        /// </summary>
        public PngInvalidSignatureException()
            : base(DataStrings.PngInvalidSignatureException)
        {
        }

        #endregion
    }
}