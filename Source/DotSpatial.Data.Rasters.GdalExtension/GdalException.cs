// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using OSGeo.GDAL;

namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    /// GDalException
    /// </summary>
    public class GdalException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GdalException"/> class using Gdal.GetLastErrorMsg.
        /// </summary>
        public GdalException()
            : base(Gdal.GetLastErrorMsg())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GdalException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public GdalException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GdalException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public GdalException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}