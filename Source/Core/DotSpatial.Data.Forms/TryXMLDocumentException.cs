// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// Exception that is caused when trying to get something from xml.
    /// </summary>
    public class TryXmlDocumentException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TryXmlDocumentException"/> class.
        /// </summary>
        /// <param name="exceptionText">Error Message.</param>
        public TryXmlDocumentException(string exceptionText)
            : base(exceptionText)
        {
            Log();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Logs this exception.
        /// </summary>
        public void Log()
        {
            LogManager.DefaultLogManager.Exception(this);
        }

        #endregion
    }
}