// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/21/2009 9:24:14 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// PicturePattern
    /// </summary>
    public class PicturePattern : Pattern, IPicturePattern
    {
        #region Fields

        private string _pictureFilename;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PicturePattern"/> class.
        /// </summary>
        public PicturePattern()
        {
            Scale = new Position2D(1, 1);
            WrapMode = WrapMode.Tile;
            DialogFilter = "Image Files|*.bmp;*.gif;*.jpg;*.png;*.tif;*.ico";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PicturePattern"/> class with the specified image.
        /// </summary>
        /// <param name="picture">The picture to draw</param>
        /// <param name="wrap">The way to wrap the picture</param>
        /// <param name="angle">The angle to rotate the image</param>
        public PicturePattern(Image picture, WrapMode wrap, double angle)
        {
            Picture = picture;
            WrapMode = wrap;
            Angle = angle;
            DialogFilter = "Image Files|*.bmp;*.gif;*.jpg;*.png;*.tif;*.ico";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the angle for the texture in degrees.
        /// </summary>
        [Serialize("Angle")]
        public double Angle { get; set; }

        /// <summary>
        /// Gets or sets the dialog filter that represents the supported picture file formats.
        /// </summary>
        public string DialogFilter { get; protected set; }

        /// <summary>
        /// Gets or sets the image to use as a repeating texture.
        /// </summary>
        public Image Picture { get; set; }

        /// <summary>
        /// Gets or sets the picture fileName. Setting this will load the picture.
        /// </summary>
        [Serialize("PictureFilename")]
        public string PictureFilename
        {
            get
            {
                return _pictureFilename;
            }

            set
            {
                _pictureFilename = value;
                Open(value);
            }
        }

        /// <summary>
        /// Gets or sets a multiplier that should be multiplied against the width and height of the
        /// picture before it is used as a texture in pixel coordinates.
        /// </summary>
        [Serialize("Scale")]
        public Position2D Scale { get; set; }

        /// <summary>
        /// Gets or sets the wrap mode.
        /// </summary>
        [Serialize("WrapMode")]
        public WrapMode WrapMode { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Disposes the image picture for this PicturePattern.
        /// </summary>
        public virtual void Dispose()
        {
            Picture.Dispose();
        }

        /// <summary>
        /// Instructs the drawing code to fill the specified path with the specified image.
        /// </summary>
        /// <param name="g">The Graphics device to draw to</param>
        /// <param name="gp">The GraphicsPath to fill</param>
        public override void FillPath(Graphics g, GraphicsPath gp)
        {
            if (Picture == null) return;
            if (Scale.X == 0 || Scale.Y == 0) return;
            if (Scale.X * Picture.Width * Scale.Y * Picture.Height > 8000 * 8000) return; // The scaled image is too large, will cause memory exceptions.

            Bitmap scaledBitmap = new Bitmap((int)(Picture.Width * Scale.X), (int)(Picture.Height * Scale.Y));
            Graphics scb = Graphics.FromImage(scaledBitmap);
            scb.DrawImage(Picture, new Rectangle(0, 0, scaledBitmap.Width, scaledBitmap.Height), new Rectangle(0, 0, Picture.Width, Picture.Height), GraphicsUnit.Pixel);

            TextureBrush tb = new TextureBrush(scaledBitmap, WrapMode);
            tb.RotateTransform(-(float)Angle);
            g.FillPath(tb, gp);
            tb.Dispose();
            scb.Dispose();
            base.FillPath(g, gp);
        }

        /// <summary>
        /// Opens the specified image or icon file to a local copy. Icons are converted into bitmaps.
        /// </summary>
        /// <param name="fileName">The string fileName to open.</param>
        public void Open(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            if (extension != null)
            {
                if (extension.ToLower() == ".ico")
                {
                    Icon ico = new Icon(fileName);
                    Picture = ico.ToBitmap();
                }
                else
                {
                    Picture = Image.FromFile(fileName);
                }
            }

            _pictureFilename = fileName;
        }

        #endregion
    }
}