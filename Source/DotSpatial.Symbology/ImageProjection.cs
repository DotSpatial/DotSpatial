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
    public class ImageProjection : IProj
    {
        #region Private Variables

        private readonly Rectangle _destRectangle;
        private readonly Extent _extents;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the ImageGraphics class for assisting with drawing.
        /// </summary>
        /// <param name="inExtent"></param>
        /// <param name="inDestRectangle"></param>
        public ImageProjection(Extent inExtent, Rectangle inDestRectangle)
        {
            _extents = inExtent;
            _destRectangle = inDestRectangle;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The destination rectangle on the bitmap where drawing should occur.
        /// </summary>
        public Rectangle ImageRectangle
        {
            get { return _destRectangle; }
        }

        /// <summary>
        /// The geographic extents where drawing will take place.
        /// </summary>
        public Extent GeographicExtents
        {
            get { return _extents; }
        }

        #endregion
    }
}