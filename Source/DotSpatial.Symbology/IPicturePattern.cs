// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
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
        double Angle { get; set; }

        /// <summary>
        /// Gets the string dialog filter that represents the supported picture file formats.
        /// </summary>
        string DialogFilter { get; }

        /// <summary>
        /// Gets or sets the image to use as a repeating texture
        /// </summary>
        Image Picture { get; set; }

        /// <summary>
        /// Gets or sets the picture fileName.  Setting this will load the picture.
        /// </summary>
        string PictureFilename { get; set; }

        /// <summary>
        /// Gets or sets a multiplier that should be multiplied against the width and height of the
        /// picture before it is used as a texture in pixel coordinates.
        /// </summary>
        Position2D Scale { get; set; }

        /// <summary>
        /// Gets or sets the wrap mode.
        /// </summary>
        WrapMode WrapMode { get; set; }

        #endregion
    }
}