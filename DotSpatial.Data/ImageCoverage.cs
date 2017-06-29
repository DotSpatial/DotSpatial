// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/6/2010 11:21:10 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.Drawing;
using DotSpatial.Serialization;

namespace DotSpatial.Data
{
    /// <summary>
    /// TiledImage is a class for actually controlling the data in several tiles.  This does not supply
    /// direct accessors for
    /// modifying the bytes directly, and instead expects the user to edit the image on a tile-by-tile basis.  However,
    /// the GetBitmap method will produce a representation of the envelope scaled to the specified window.
    /// </summary>
    public class ImageCoverage : DataSet, IImageCoverage
    {
        #region Private Variables

        //private RasterBounds _bounds;
        [Serialize("FileName", ConstructorArgumentIndex = 0)]
        private string _fileName;

        private List<IImageData> _images;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of TiledImage
        /// </summary>
        public ImageCoverage()
        {
            _images = new List<IImageData>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the bitmap for the specified geographic envelope scaled to fit on a bitmap of the specified size in pixels.
        /// </summary>
        /// <param name="envelope"></param>
        /// <param name="pixelSize"></param>
        /// <returns></returns>
        public virtual Bitmap GetBitmap(Extent envelope, Size pixelSize)
        {
            Bitmap result = new Bitmap(pixelSize.Width, pixelSize.Height);
            Graphics g = Graphics.FromImage(result);
            foreach (var image in GetImages())
            {
                Extent bounds = envelope.Intersection(image.Extent);

                Size ps = new Size((int)(pixelSize.Width * bounds.Width / envelope.Width),
                                   (int)(pixelSize.Height * bounds.Height / envelope.Height));
                int x = pixelSize.Width * (int)((bounds.X - envelope.X) / envelope.Width);
                int y = pixelSize.Height * (int)((envelope.Y - bounds.Y) / envelope.Height);
                if (ps.Width > 0 && ps.Height > 0)
                {
                    Bitmap tile = image.GetBitmap(bounds, ps);
                    g.DrawImageUnscaled(tile, x, y);
                }
            }
            return result;
        }

        /// <inheritdoc />
        public IEnumerable<IImageData> GetImages()
        {
            return _images;
        }

        /// <summary>
        /// Cycles through each of the images and calls the open method on each one.
        /// </summary>
        public virtual void Open()
        {
            // it seems that the images would already be open if they were instantiated.
            //foreach (IImageData id in _images)
            //{
            //    id.Open();
            //}
        }

        /// <summary>
        /// Cycles through each of the images and calls the save method on each one
        /// </summary>
        public void Save()
        {
            foreach (IImageData id in _images)
            {
                id.Save();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Filename
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <inheritdoc />
        public int Count
        {
            get { return _images.Count; }
        }

        /// <inheritdoc />
        public override Extent Extent
        {
            get
            {
                Extent ext = new Extent();
                foreach (IImageData tile in _images)
                {
                    ext.ExpandToInclude(tile.Extent);
                }
                return ext;
            }
        }

        /// <inheritdoc />
        public virtual List<IImageData> Images
        {
            get { return _images; }
            set { _images = value; }
        }

        #endregion
    }
}