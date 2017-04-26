// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  A library module for the DotSpatial geospatial framework for .Net.
// ********************************************************************************************************
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