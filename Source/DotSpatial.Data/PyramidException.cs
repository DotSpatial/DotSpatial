// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/11/2010 11:33:02 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    public class PyramidException : Exception
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of PyramidException
        /// </summary>
        public PyramidException(string message)
            : base(message)
        {
        }

        #endregion
    }
}