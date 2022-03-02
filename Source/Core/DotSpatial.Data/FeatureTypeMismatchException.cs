// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// FeatureTypeMismatchException.
    /// </summary>
    public class FeatureTypeMismatchException : ArgumentException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureTypeMismatchException"/> class.
        /// </summary>
        public FeatureTypeMismatchException()
            : base(DataStrings.FeatureTypeMismatch)
        {
        }

        #endregion
    }
}