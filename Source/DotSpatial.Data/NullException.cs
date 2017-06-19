// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// An exception that gets thrown for elements that may not be null.
    /// </summary>
    public class NullException : ArgumentNullException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NullException"/> class.
        /// </summary>
        public NullException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullException"/> class.
        /// </summary>
        /// <param name="parameterName">Name of the parameter that is null.</param>
        public NullException(string parameterName)
            : base(string.Format(DataStrings.Argument_Null_S, parameterName))
        {
        }

        #endregion
    }
}