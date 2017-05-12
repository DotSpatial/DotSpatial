// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/2/2010 12:41:52 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// UnspecifiedFeaturetypeException
    /// </summary>
    public class UnspecifiedFeaturetypeException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnspecifiedFeaturetypeException"/> class.
        /// </summary>
        public UnspecifiedFeaturetypeException()
            : base(DataStrings.FeaturetypeUnspecified)
        {
        }

        #endregion
    }
}