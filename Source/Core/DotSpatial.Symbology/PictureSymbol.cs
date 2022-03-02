// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Xml.Serialization;
using DotSpatial.Data;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// PictureSymbol.
    /// </summary>
    public class PictureSymbol : OutlinedSymbol, IPictureSymbol
    {
        #region Fields

        private Image _image;
        private string _imageFilename;
        private float _opacity;
        private Image _original; // Non-transparent version

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PictureSymbol"/> class.
        /// </summary>
        public PictureSymbol()
        {
            SymbolType = SymbolType.Picture;
            _opacity = 1F;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PictureSymbol"/> class from the specified image.
        /// </summary>
        /// <param name="image">The image to use when creating the symbol.</param>
        public PictureSymbol(Image image)
        {
            SymbolType = SymbolType.Picture;
            _opacity = 1F;
            Image = image;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PictureSymbol"/> class from the specified image.
        /// The larger dimension from the image will be adjusted to fit the size,
        /// while the smaller dimension will be kept proportional.
        /// </summary>
        /// <param name="image">The image to use for this symbol.</param>
        /// <param name="size">The double size to use for the larger of the two dimensions of the image.</param>
        public PictureSymbol(Image image, double size)
        {
            SymbolType = SymbolType.Picture;
            _opacity = 1F;
            Image = image;
            if (image == null) return;

            double scale;
            if (image.Width > image.Height)
            {
                scale = size / image.Width;
            }
            else
            {
                scale = size / image.Height;
            }

            Size = new Size2D(scale * image.Width, scale * image.Height);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PictureSymbol"/> class from the specified icon.
        /// </summary>
        /// <param name="icon">The icon to use when creating this symbol.</param>
        public PictureSymbol(Icon icon)
        {
            SymbolType = SymbolType.Picture;
            _opacity = 1F;
            _original = icon.ToBitmap();
            _image = MakeTransparent(_original, _opacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PictureSymbol"/> class given an existing imageData object.
        /// </summary>
        /// <param name="imageData">The imageData object to use.</param>
        public PictureSymbol(IImageData imageData)
        {
            SymbolType = SymbolType.Picture;
            _opacity = 1F;
            _image = imageData.GetBitmap();
            _original = _image;
            _imageFilename = imageData.Filename;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the image to use when the PictureMode is set to Image.
        /// </summary>
        [XmlIgnore]
        public Image Image
        {
            get
            {
                return _image;
            }

            set
            {
                if (_original != null && _original != value)
                {
                    _original.Dispose();
                    _original = null;
                }

                if (_image != null && _image != _original && _image != value)
                {
                    _image.Dispose();
                    _image = null;
                }

                _original = value;
                _image = MakeTransparent(value, _opacity);
                if (_original != null) IsDisposed = false;
            }
        }

        /// <summary>
        /// Gets or sets the image in 'base64' string format.
        /// This can be used if the image file name is not set.
        /// </summary>
        [Serialize("ImageBase64String")]
        public string ImageBase64String
        {
            get
            {
                if (string.IsNullOrEmpty(ImageFilename))
                {
                    return ConvertImageToBase64(Image);
                }

                return string.Empty;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _original?.Dispose();
                    _original = null;
                    _image?.Dispose();
                    _image = null;

                    Image = ConvertBase64ToImage(value);
                    if (_opacity < 1)
                    {
                        _image = MakeTransparent(_original, _opacity);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the string image fileName to use.
        /// </summary>
        [Serialize("ImageFilename")]
        public string ImageFilename
        {
            get
            {
                return _imageFilename;
            }

            set
            {
                _imageFilename = value;
                _original?.Dispose();
                _original = null;
                _image?.Dispose();
                _image = null;
                if (_imageFilename == null || !File.Exists(_imageFilename)) return;

                _original = Path.GetExtension(_imageFilename) == ".ico" ? new Icon(_imageFilename).ToBitmap() : Image.FromFile(_imageFilename);
                _image = MakeTransparent(_original, _opacity);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the symbol is disposed. This is set to true when the dispose method is called.
        /// This will be set to false again if the image is set after that.
        /// </summary>
        public bool IsDisposed { get; set; }

        /// <summary>
        /// Gets or sets the opacity for this image. Setting this will automatically change the image in memory.
        /// </summary>
        [Serialize("Opacity")]
        public float Opacity
        {
            get
            {
                return _opacity;
            }

            set
            {
                _opacity = value;
                if (_image != null && _image != _original) _image.Dispose();
                _image = MakeTransparent(_original, _opacity);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Disposes the current images.
        /// </summary>
        public void Dispose()
        {
            OnDisposing();
            IsDisposed = true;
        }

        /// <inheritdoc />
        public override void SetColor(Color color)
        {
            Bitmap bm = new Bitmap(_image.Width, _image.Height);
            Graphics g = Graphics.FromImage(bm);

            float r = (color.R / 255f) / 2;
            float gr = (color.G / 255f) / 2;
            float b = (color.B / 255f) / 2;

            ColorMatrix cm = new ColorMatrix(new[] { new[] { r, gr, b, 0, 0 }, new[] { r, gr, b, 0, 0 }, new[] { r, gr, b, 0, 0 }, new float[] { 0, 0, 0, 1, 0, 0 }, new float[] { 0, 0, 0, 0, 1, 0 }, new float[] { 0, 0, 0, 0, 0, 1 } });

            ImageAttributes ia = new ImageAttributes();
            ia.SetColorMatrix(cm);
            g.DrawImage(_image, new Rectangle(0, 0, _image.Width, _image.Height), 0, 0, _image.Width, _image.Height, GraphicsUnit.Pixel, ia);
            g.Dispose();
            _image = bm;
        }

        /// <summary>
        /// This helps the copy process by preparing the internal variables after memberwiseclone has created this.
        /// </summary>
        /// <param name="copy">The copy.</param>
        protected override void OnCopy(Descriptor copy)
        {
            // Setting the image property has a built in check to dispose the older reference.
            // After a MemberwiseClone, however, this older reference is still in use in the original PictureSymbol.
            // We must, therefore, set the private variables to null before doing the cloning.
            PictureSymbol duplicate = copy as PictureSymbol;
            if (duplicate != null)
            {
                duplicate._image = null;
                duplicate._original = null;
            }

            base.OnCopy(copy);
            if (duplicate != null)
            {
                duplicate._image = _image.Copy();
                duplicate._original = _original.Copy();
            }
        }

        /// <summary>
        /// Overrideable functions for handling the basic disposal of image classes in this object.
        /// </summary>
        protected virtual void OnDisposing()
        {
            _image.Dispose();
            _original.Dispose();
        }

        /// <summary>
        /// OnDraw.
        /// </summary>
        /// <param name="g">Graphics object.</param>
        /// <param name="scaleSize">The double scale Size.</param>
        protected override void OnDraw(Graphics g, double scaleSize)
        {
            float dx = (float)(scaleSize * Size.Width / 2);
            float dy = (float)(scaleSize * Size.Height / 2);
            GraphicsPath gp = new GraphicsPath();
            gp.AddRectangle(new RectangleF(-dx, -dy, dx * 2, dy * 2));
            if (_image != null)
            {
                g.DrawImage(_image, new RectangleF(-dx, -dy, dx * 2, dy * 2));
            }

            OnDrawOutline(g, scaleSize, gp);
            gp.Dispose();
        }

        /// <summary>
        /// We can randomize the opacity.
        /// </summary>
        /// <param name="generator">The generator used for randomization.</param>
        protected override void OnRandomize(Random generator)
        {
            base.OnRandomize(generator);
            Opacity = generator.NextFloat();
        }

        private static Image MakeTransparent(Image image, float opacity)
        {
            if (image == null) return null;
            if (opacity == 1F) return image.Clone() as Image;

            Bitmap bmp = new Bitmap(image.Width, image.Height);
            Graphics g = Graphics.FromImage(bmp);
            float[][] ptsArray =
            {
                new float[] { 1, 0, 0, 0, 0 }, // R
                new float[] { 0, 1, 0, 0, 0 }, // G
                new float[] { 0, 0, 1, 0, 0 }, // B
                new[] { 0, 0, 0, opacity, 0 }, // A
                new float[] { 0, 0, 0, 0, 1 }
            };
            ColorMatrix clrMatrix = new ColorMatrix(ptsArray);
            ImageAttributes att = new ImageAttributes();
            att.SetColorMatrix(clrMatrix);
            g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, att);
            g.Dispose();
            return bmp;
        }

        private Image ConvertBase64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        private string ConvertImageToBase64(Image image)
        {
            if (image == null) return string.Empty;

            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, ImageFormat.Png);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        #endregion
    }
}