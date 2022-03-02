// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.IO;

namespace DotSpatial.Controls
{
    /// <summary>
    /// The layout bitmap provides the ability to add any custom image to the layout.
    /// </summary>
    public class LayoutBitmap : LayoutElement
    {
        #region Fields

        private Bitmap _bitmap;
        private int _brightness;
        private int _contrast;
        private string _fileName;
        private bool _preserveAspectRatio;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBitmap"/> class.
        /// </summary>
        public LayoutBitmap()
        {
            ResizeStyle = ResizeStyle.HandledInternally;
            _preserveAspectRatio = true;
            Name = "Bitmap";
            ResizeStyle = ResizeStyle.StretchToFit;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets bitmap to use.
        /// </summary>
        [Browsable(false)]
        public Bitmap Bitmap
        {
            get
            {
                return _bitmap;
            }

            set
            {
                if (_bitmap == value) return;
                _bitmap?.Dispose();
                _bitmap = value;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the brightness of the bitmap relative to its original brightness +/- 255. Doesn't modify original bitmap.
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        public int Brightness
        {
            get
            {
                return _brightness;
            }

            set
            {
                if (_brightness == value) return;
                if (value < -255) _brightness = -255;
                else if (value > 255) _brightness = 255;
                else _brightness = value;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the contrast of the bitmap relative to its original contrast +/- 255. Doesn't modify original bitmap.
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        public int Contrast
        {
            get
            {
                return _contrast;
            }

            set
            {
                if (_contrast == value) return;
                if (value < -255) _contrast = -255;
                else if (value > 255) _contrast = 255;
                else _contrast = value;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the string fileName of the bitmap to use.
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        [Editor("System.Windows.Forms.Design.FileNameEditor, System.Design", typeof(UITypeEditor))]
        public string Filename
        {
            get
            {
                return _fileName;
            }

            set
            {
                if (_fileName == value) return;
                _fileName = value;
                Bitmap = File.Exists(_fileName) ? new Bitmap(_fileName) : null;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the aspect ratio is preserved. If false stretching of
        /// the bitmap is allowed to occur.
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        public bool PreserveAspectRatio
        {
            get
            {
                return _preserveAspectRatio;
            }

            set
            {
                if (_preserveAspectRatio == value) return;
                _preserveAspectRatio = value;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This gets called to instruct the element to draw itself in the appropriate spot of the graphics object.
        /// </summary>
        /// <param name="g">The graphics object to draw to.</param>
        /// <param name="printing">Boolean, true if this is being drawn to a print document.</param>
        public override void Draw(Graphics g, bool printing)
        {
            if (_bitmap == null) return;

            // This color matrix is used to adjust how the image is drawn to the graphics object
            var bright = _brightness / 255.0F;
            var cont = (_contrast + 255.0F) / 255.0F;
            float[][] colorArray = { new[] { cont, 0, 0, 0, 0 }, new[] { 0, cont, 0, 0, 0 }, new[] { 0, 0, cont, 0, 0 }, new float[] { 0, 0, 0, 1, 0 }, new[] { bright, bright, bright, 0, 1 } };
            var cm = new ColorMatrix(colorArray);
            var imgAttrib = new ImageAttributes();
            imgAttrib.SetColorMatrix(cm);

            // Defines a parallelgram where the image is to be drawn
            var destPoints = new[] { LocationF, new PointF(LocationF.X + Size.Width, LocationF.Y), new PointF(LocationF.X, LocationF.Y + Size.Height) };

            var destSize = _bitmap.Size;
            if (_preserveAspectRatio)
            {
                if ((Size.Width / destSize.Width) < (Size.Height / destSize.Height))
                    destPoints[2] = new PointF(LocationF.X, LocationF.Y + (Size.Width * destSize.Height / destSize.Width));
                else
                    destPoints[1] = new PointF(LocationF.X + (Size.Height * destSize.Width / destSize.Height), LocationF.Y);
            }

            var srcRect = new Rectangle(0, 0, _bitmap.Width, _bitmap.Height);
            g.DrawImage(_bitmap, destPoints, srcRect, GraphicsUnit.Pixel, imgAttrib);
            imgAttrib.Dispose();
        }

        #endregion
    }
}