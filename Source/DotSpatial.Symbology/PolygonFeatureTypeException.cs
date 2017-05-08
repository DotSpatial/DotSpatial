// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/26/2009 3:35:16 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// PolygonFeatureTypeException
    /// </summary>
    public class PolygonFeatureTypeException : ArgumentException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonFeatureTypeException"/> class.
        /// </summary>
        public PolygonFeatureTypeException()
            : base(SymbologyMessageStrings.PointFeatureTypeException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonFeatureTypeException"/> class.
        /// </summary>
        /// <param name="message">The custom error message to use for this exception</param>
        public PolygonFeatureTypeException(string message)
            : base(message)
        {
        }

        #endregion
    }
}