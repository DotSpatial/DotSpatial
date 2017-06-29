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
        #region Private Variables

        double _angle;
        private string _dialogFilter;
        Image _picture;
        string _pictureFilename;
        Position2D _scale;
        WrapMode _wrapMode;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PicturePattern
        /// </summary>
        public PicturePattern()
        {
            _scale = new Position2D(1, 1);
            _wrapMode = WrapMode.Tile;
            _dialogFilter = "Image Files|*.bmp;*.gif;*.jpg;*.png;*.tif;*.ico";
        }

        /// <summary>
        /// Creates a new PicturePattern with the specified image
        /// </summary>
        /// <param name="picture">The picture to draw</param>
        /// <param name="wrap">The way to wrap the picture</param>
        /// <param name="angle">The angle to rotate the image</param>
        public PicturePattern(Image picture, WrapMode wrap, double angle)
        {
            _picture = picture;
            _wrapMode = wrap;
            _angle = angle;
            _dialogFilter = "Image Files|*.bmp;*.gif;*.jpg;*.png;*.tif;*.ico";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Opens the specified image or icon file to a local copy.  Icons are converted into bitmaps.
        /// </summary>
        /// <param name="fileName">The string fileName to open.</param>
        public void Open(string fileName)
        {
            if (Path.GetExtension(fileName).ToLower() == ".ico")
            {
                Icon ico = new Icon(fileName);
                _picture = ico.ToBitmap();
            }
            else
            {
                _picture = Image.FromFile(fileName);
            }
            _pictureFilename = fileName;
        }

        /// <summary>
        /// Disposes the image picture for this PicturePattern.
        /// </summary>
        public virtual void Dispose()
        {
            _picture.Dispose();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the angle for the texture in degrees.
        /// </summary>
        [Serialize("Angle")]
        public double Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }

        /// <summary>
        /// Gets the string dialog filter that represents the supported picture file formats.
        /// </summary>
        public string DialogFilter
        {
            get { return _dialogFilter; }
            protected set { _dialogFilter = value; }
        }

        /// <summary>
        /// Gets or sets the image to use as a repeating texture
        /// </summary>
        public Image Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        /// <summary>
        /// Gets or sets the picture fileName.  Setting this will load the picture.
        /// </summary>
        [Serialize("PictureFilename")]
        public string PictureFilename
        {
            get { return _pictureFilename; }
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
        public Position2D Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        /// <summary>
        /// Gets or sets the wrap mode.
        /// </summary>
        [Serialize("WrapMode")]
        public WrapMode WrapMode
        {
            get { return _wrapMode; }
            set { _wrapMode = value; }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Instructs the drawing code to fill the specified path with the specified image.
        /// </summary>
        /// <param name="g">The Graphics device to draw to</param>
        /// <param name="gp">The GraphicsPath to fill</param>
        public override void FillPath(Graphics g, GraphicsPath gp)
        {
            if (_picture == null) return;
            if (_scale.X == 0 || _scale.Y == 0) return;
            if (_scale.X * _picture.Width * _scale.Y * _picture.Height > 8000 * 8000) return; // The scaled image is too large, will cause memory exceptions.
            Bitmap scaledBitmap = new Bitmap((int)(_picture.Width * _scale.X), (int)(_picture.Height * _scale.Y));
            Graphics scb = Graphics.FromImage(scaledBitmap);
            scb.DrawImage(_picture, new Rectangle(0, 0, scaledBitmap.Width, scaledBitmap.Height), new Rectangle(0, 0, _picture.Width, _picture.Height), GraphicsUnit.Pixel);

            TextureBrush tb = new TextureBrush(scaledBitmap, _wrapMode);
            tb.RotateTransform(-(float)_angle);
            g.FillPath(tb, gp);
            tb.Dispose();
            scb.Dispose();
            base.FillPath(g, gp);
        }

        #endregion
    }
}