// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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