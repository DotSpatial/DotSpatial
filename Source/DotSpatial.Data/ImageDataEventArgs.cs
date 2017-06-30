// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from DotSpatial.Symbology.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/29/2010 9:05:01 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// An EventArgs specifically tailored to ImageData.
    /// </summary>
    [Obsolete("Do not use it. This class is not used in DotSpatial anymore.")] // Marked in 1.7
    public class ImageDataEventArgs : EventArgs
    {
        private IImageData _imageData;

        /// <summary>
        /// Initializes a new instance of the ImageDataEventArgs class.
        /// </summary>
        /// <param name="imageData">The IImageData that is involved in this event.</param>
        public ImageDataEventArgs(IImageData imageData)
        {
            _imageData = imageData;
        }

        /// <summary>
        /// Gets the ImageData associated with this event.
        /// </summary>
        public IImageData ImageData
        {
            get { return _imageData; }
            protected set { _imageData = value; }
        }
    }
}