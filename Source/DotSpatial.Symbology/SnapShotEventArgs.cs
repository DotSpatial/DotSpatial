// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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