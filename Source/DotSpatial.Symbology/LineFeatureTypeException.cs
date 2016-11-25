// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/26/2009 3:33:45 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// LineFeatureTypeException
    /// </summary>
    public class LineFeatureTypeException : ArgumentException
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of LineFeatureTypeException
        /// </summary>
        public LineFeatureTypeException()
            : base(SymbologyMessageStrings.LineFeatureTypeException)
        {
        }

        /// <summary>
        /// Creates a new instance of LineFeatureTypeException
        /// </summary>
        /// <param name="message">The custom error message for this exception</param>
        public LineFeatureTypeException(string message)
            : base(message)
        {
        }

        #endregion
    }
}