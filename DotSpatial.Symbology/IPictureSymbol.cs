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