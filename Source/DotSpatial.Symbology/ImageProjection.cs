// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/5/2009 12:50:00 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ImageProjection
    /// </summary>
    public class ImageProjection : IProj
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageProjection"/> class.
        /// </summary>
        /// <param name="inExtent">The geographic extent.</param>
        /// <param name="inDestRectangle">The image rectangle.</param>
        public ImageProjection(Extent inExtent, Rectangle inDestRectangle)
        {
            GeographicExtents = inExtent;
            ImageRectangle = inDestRectangle;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the geographic extents where drawing will take place.
        /// </summary>
        public Extent GeographicExtents { get; }

        /// <summary>
        /// Gets the destination rectangle on the bitmap where drawing should occur.
        /// </summary>
        public Rectangle ImageRectangle { get; }

        #endregion
    }
}