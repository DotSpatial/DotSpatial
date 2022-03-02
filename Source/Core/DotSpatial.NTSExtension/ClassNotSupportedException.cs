// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.NTSExtension
{
    /// <summary>
    /// A ClassNotSupportedException Class.
    /// </summary>
    public class ClassNotSupportedException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassNotSupportedException"/> class.
        /// </summary>
        /// <param name="name">Name of the class that is not supported.</param>
        public ClassNotSupportedException(string name)
            : base(TopologyText.ClassNotSupportedException_S.Replace("%S", name))
        {
        }

        #endregion
    }
}