// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/5/2009 12:14:19 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// SnapShotEventArgs
    /// </summary>
    public class SnapShotEventArgs : EventArgs
    {
        #region Private Variables

        private Bitmap _picture;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SnapShotEventArgs
        /// </summary>
        public SnapShotEventArgs(Bitmap inPicture)
        {
            _picture = inPicture;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the picture that was taken by the snapshot
        /// </summary>
        public Bitmap Picture
        {
            get { return _picture; }
            protected set { _picture = value; }
        }

        #endregion
    }
}