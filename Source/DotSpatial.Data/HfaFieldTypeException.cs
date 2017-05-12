// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/25/2010 2:59:55 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// HfaFieldTypeException
    /// </summary>
    public class HfaFieldTypeException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HfaFieldTypeException"/> class.
        /// </summary>
        /// <param name="code">The unknown character.</param>
        public HfaFieldTypeException(char code)
            : base(string.Format(DataStrings.HfaFieldTypeException, code))
        {
        }

        #endregion
    }
}