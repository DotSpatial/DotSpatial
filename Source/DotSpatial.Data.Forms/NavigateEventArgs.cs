// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  A library module for the DotSpatial geospatial framework for .Net.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/13/2008 4:00:47 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data.Forms
{
    public class NavigateEventArgs : EventArgs
    {
        #region Private Variables

        private string _path;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NavigateEventArgs
        /// </summary>
        public NavigateEventArgs(string path)
        {
            _path = path;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the string path that is being navigated to
        /// </summary>
        public string Path
        {
            get { return _path; }
            protected set { _path = value; }
        }

        #endregion
    }
}