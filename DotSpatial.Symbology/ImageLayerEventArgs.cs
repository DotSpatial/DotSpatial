// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
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

namespace DotSpatial.Symbology
{
    /// <summary>
    /// An EventArgs specifically tailored to ImageLayerEventArgs.
    /// </summary>
    public class ImageLayerEventArgs : EventArgs
    {
        private IImageLayer _imageLayer;

        /// <summary>
        /// Initializes a new instance of the ImageLayerEventArgs class.
        /// </summary>
        /// <param name="imageLayer">The IImageLayer that is involved in this event.</param>
        public ImageLayerEventArgs(IImageLayer imageLayer)
        {
            _imageLayer = imageLayer;
        }

        /// <summary>
        /// Gets the ImageLayer associated with this event.
        /// </summary>
        public IImageLayer ImageLayer
        {
            get { return _imageLayer; }
            protected set { _imageLayer = value; }
        }
    }
}