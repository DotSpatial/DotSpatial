// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/26/2009 3:20:43 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// PointFeatureException
    /// </summary>
    public class PointFeatureTypeException : ArgumentException
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of PointFeatureTypeException
        /// </summary>
        public PointFeatureTypeException()
            : base(SymbologyMessageStrings.PointFeatureTypeException)
        {
        }

        /// <summary>
        /// Creates a new instance of PointFeatureTypeException, but with a custom error message
        /// </summary>
        /// <param name="message">The string error message to include in the exception</param>
        public PointFeatureTypeException(string message)
            : base(message)
        {
        }

        #endregion
    }
}