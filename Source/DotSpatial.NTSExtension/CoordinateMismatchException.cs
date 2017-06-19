// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.NTSExtension
{
    /// <summary>
    /// CoordinateMismatchException
    /// </summary>
    public class CoordinateMismatchException : ArgumentException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateMismatchException"/> class.
        /// </summary>
        public CoordinateMismatchException()
            : base(TopologyText.CoordinateMismatchException)
        {
        }

        #endregion
    }
}