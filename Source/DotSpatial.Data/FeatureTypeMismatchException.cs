// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/2/2010 12:48:42 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// FeatureTypeMismatchException
    /// </summary>
    public class FeatureTypeMismatchException : ArgumentException
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of FeatureTypeMismatchException
        /// </summary>
        public FeatureTypeMismatchException()
            : base(DataStrings.FeatureTypeMismatch)
        {
        }

        #endregion
    }
}