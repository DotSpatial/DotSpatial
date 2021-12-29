// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// UnspecifiedFeaturetypeException.
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