// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/11/2009 4:28:17 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IPictureSymbol
    /// </summary>
    public interface IPictureSymbol : IOutlinedSymbol, IDisposable
    {
        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the image to use when the PictureMode is set to Image
        /// </summary>
        Image Image
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the string fileName, if any, associated with the image file.
        /// Setting this will automatically also load the Image.
        /// </summary>
        string ImageFilename
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the opacity for this image.  Setting this will automatically change the image in memory.
        /// </summary>
        float Opacity
        {
            get;
            set;
        }

        #endregion
    }
}