// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  A library module for the DotSpatial geospatial framework for .Net.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/16/2009 4:42:29 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// TryXmlDocumentException
    /// </summary>
    public class TryXmlDocumentException : ApplicationException, ILog
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of TryXmlDocumentException
        /// </summary>
        public TryXmlDocumentException(string exceptionText)
            : base(exceptionText)
        {
            Log();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Logs this exception
        /// </summary>
        public void Log()
        {
            LogManager.DefaultLogManager.Exception(this);
        }

        #endregion
    }
}