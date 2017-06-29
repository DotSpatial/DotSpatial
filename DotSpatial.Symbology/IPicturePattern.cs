// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/21/2009 9:29:15 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IPicturePattern
    /// </summary>
    public interface IPicturePattern : IPattern, IDisposable
    {
        #region Methods

        /// <summary>
        /// Opens the specified image or icon file to a local copy.  Icons are converted into bitmaps.
        /// </summary>
        /// <param name="fileName">The string fileName to open.</param>
        void Open(string fileName);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the angle for the texture in degrees.
        /// </summary>
        double Angle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the string dialog filter that represents the supported picture file formats.
        /// </summary>
        string DialogFilter
        {
            get;
        }

        /// <summary>
        /// Gets or sets the image to use as a repeating texture
        /// </summary>
        Image Picture
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the picture fileName.  Setting this will load the picture.
        /// </summary>
        string PictureFilename
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a multiplier that should be multiplied against the width and height of the
        /// picture before it is used as a texture in pixel coordinates.
        /// </summary>
        Position2D Scale
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the wrap mode.
        /// </summary>
        WrapMode WrapMode
        {
            get;
            set;
        }

        #endregion
    }
}