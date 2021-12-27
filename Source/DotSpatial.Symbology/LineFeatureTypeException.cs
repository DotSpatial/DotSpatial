// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// LineFeatureTypeException.
    /// </summary>
    public class LineFeatureTypeException : ArgumentException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LineFeatureTypeException"/> class.
        /// </summary>
        public LineFeatureTypeException()
            : base(SymbologyMessageStrings.LineFeatureTypeException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineFeatureTypeException"/> class.
        /// </summary>
        /// <param name="message">The custom error message for this exception.</param>
        public LineFeatureTypeException(string message)
            : base(message)
        {
        }

        #endregion
    }
}