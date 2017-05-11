// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
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
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageLayerEventArgs"/> class.
        /// </summary>
        /// <param name="imageLayer">The IImageLayer that is involved in this event.</param>
        public ImageLayerEventArgs(IImageLayer imageLayer)
        {
            ImageLayer = imageLayer;
        }

        /// <summary>
        /// Gets or sets the ImageLayer associated with this event.
        /// </summary>
        public IImageLayer ImageLayer { get; protected set; }
    }
}