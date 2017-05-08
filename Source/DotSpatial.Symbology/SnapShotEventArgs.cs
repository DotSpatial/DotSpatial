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
    /// Event args of a SnapShot event.
    /// </summary>
    public class SnapShotEventArgs : EventArgs
    {
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapShotEventArgs"/> class.
        /// </summary>
        /// <param name="inPicture">The bitmap of the event.</param>
        public SnapShotEventArgs(Bitmap inPicture)
        {
            Picture = inPicture;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the picture that was taken by the snapshot.
        /// </summary>
        public Bitmap Picture { get; protected set; }

        #endregion
    }
}