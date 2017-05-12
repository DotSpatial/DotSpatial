// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/27/2010 11:06:36 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// HfaEnumerationNotFoundException
    /// </summary>
    public class HfaEnumerationNotFoundException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HfaEnumerationNotFoundException"/> class.
        /// </summary>
        /// <param name="name">The unknown value.</param>
        public HfaEnumerationNotFoundException(string name)
            : base(string.Format(DataStrings.HfaEnumerationNotFound, name))
        {
        }

        #endregion
    }
}