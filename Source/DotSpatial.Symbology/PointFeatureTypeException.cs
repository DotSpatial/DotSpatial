// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// PointFeatureTypeException.
    /// </summary>
    public class PointFeatureTypeException : ArgumentException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PointFeatureTypeException"/> class.
        /// </summary>
        public PointFeatureTypeException()
            : base(SymbologyMessageStrings.PointFeatureTypeException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointFeatureTypeException"/> class.
        /// </summary>
        /// <param name="message">The string error message to include in the exception.</param>
        public PointFeatureTypeException(string message)
            : base(message)
        {
        }

        #endregion
    }
}