// ********************************************************************************************************
// Product Name: DotSpatial.Layout.Elements.LayoutBitmap
// Description:  The DotSpatial LayoutBitmap element, holds bitmaps loaded from disk for the layout
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
// The Original Code is DotSpatial.dll Version 6.0
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Jul, 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// ------------------|------------|---------------------------------------------------------------
// Ted Dunsford      | 8/28/2009  | Cleaned up some code formatting using resharper
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.IO;

namespace DotSpatial.Controls
{
    /// <summary>
    /// The layout bitmap provides the ability to add any custom image to the layout
    /// </summary>
    public class LayoutBitmap : LayoutElement
    {
        private Bitmap _bitmap;
        private int _brightness;
        private int _contrast;
        private bool _draft;
        private string _fileName;
        private bool _preserveAspectRatio;

        #region ------------------ Public Properties

        /// <summary>
        /// Preserves the aspect ratio if this boolean is true, otherwise it allows stretching of
        /// the bitmap to occur
        /// </summary>
        [Browsable(true), Category("Symbol")]
        public bool PreserveAspectRatio
        {
            get { return _preserveAspectRatio; }
            set { _preserveAspectRatio = value; UpdateThumbnail(); OnInvalidate(); }
        }

        /// <summary>
        /// Modifies the brightness of the bitmap relative to its original brightness +/- 255 Doesn't modify original bitmap
        /// </summary>
        [Browsable(true), Category("Symbol")]
        public int Brightness
        {
            get { return _brightness; }
            set
            {
                if (value < -255) _brightness = -255;
                else if (value > 255) _brightness = 255;
                else _brightness = value;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Modifies the contrast of the bitmap relative to its original contrast +/- 255 Doesn't modify original bitmap
        /// </summary>
        [Browsable(true), Category("Symbol")]
        public int Contrast
        {
            get { return _contrast; }
            set
            {
                if (value < -255) _contrast = -255;
                else if (value > 255) _contrast = 255;
                else _contrast = value;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the string fileName of the bitmap to use
        /// </summary>
        [Browsable(true), Category("Symbol")]
        [Editor("System.Windows.Forms.Design.FileNameEditor, System.Design", typeof(UITypeEditor))]
        public string Filename
        {
            get { return _fileName; }
            set
            {
                try
                {
                    new Bitmap(value);
                    _fileName = value;
                    if (_bitmap != null) _bitmap.Dispose();
                    _bitmap = new Bitmap(_fileName);
                }
                catch
                {
                    // GDI+ supports the following file formats: BMP, GIF, EXIF, JPG, PNG and TIFF.
                    _fileName = string.Empty;
                }
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Allows for a faster but lower quality bitmap to be rendered to the screen
        /// and a higher quality bitmap to be printed during actual printing.
        /// </summary>
        [Browsable(true), Category("Behavior")]
        public bool Draft
        {
            get { return _draft; }
            set
            {
                _draft = value;
                if (_draft)
                {
                    if (_bitmap != null) _bitmap.Dispose();
                    _bitmap = null;
                }
                else
                {
                    if (File.Exists(_fileName))
                        _bitmap = new Bitmap(_fileName);
                }
                OnInvalidate();
            }
        }

        #endregion

        #region ------------------- public methods

        /// <summary>
        /// Constructor
        /// </summary>
        public LayoutBitmap()
        {
            ResizeStyle = ResizeStyle.HandledInternally;
            _preserveAspectRatio = true;
            Name = "Bitmap";
            _draft = true;
            _fileName = string.Empty;
            ResizeStyle = ResizeStyle.StretchToFit;
            _brightness = 0;
            _contrast = 0;
        }

        /// <summary>
        /// This gets called to instruct the element to draw itself in the appropriate spot of the graphics object
        /// </summary>
        /// <param name="g">The graphics object to draw to</param>
        /// <param name="printing">Boolean, true if this is being drawn to a print document</param>
        public override void Draw(Graphics g, bool printing)
        {
            //This color matrix is used to adjust how the image is drawn to the graphics object
            float bright = _brightness / 255.0F;
            float cont = (_contrast + 255.0F) / 255.0F;
            float[][] colorArray = {new[] {cont,   0,      0,      0,    0},
                                    new[] {0,      cont,   0,      0,    0},
                                    new[] {0,      0,      cont,   0,    0},
                                    new float[] {0,      0,      0,      1,    0},
                                    new[] {bright, bright, bright, 0,    1}};
            ColorMatrix cm = new ColorMatrix(colorArray);
            ImageAttributes imgAttrib = new ImageAttributes();
            imgAttrib.SetColorMatrix(cm);

            //Defines a parallelgram where the image is to be drawn
            PointF[] destPoints = new[]{LocationF,
                                        new PointF(LocationF.X + Size.Width, LocationF.Y),
                                        new PointF(LocationF.X, LocationF.Y + Size.Height)};
            Rectangle srcRect;

            //When printing we use this code
            if (printing)
            {
                //Open the original and gets its rectangle
                Bitmap original = new Bitmap(_fileName);
                srcRect = new Rectangle(0, 0, _bitmap.Width, _bitmap.Height);

                //Modifies the parallelogram if we are preserving aspect ration
                if (_preserveAspectRatio)
                {
                    if ((Size.Width / original.Width) < (Size.Height / original.Height))
                        destPoints[2] = new PointF(LocationF.X, LocationF.Y + (Size.Width * original.Height / original.Width));
                    else
                        destPoints[1] = new PointF(LocationF.X + (Size.Height * original.Width / original.Height), LocationF.Y);
                }

                //Draws the bitmap
                g.DrawImage(_bitmap, destPoints, srcRect, GraphicsUnit.Pixel, imgAttrib);

                //Clean up and return
                imgAttrib.Dispose();
                original.Dispose();
                return;
            }

            //If we are not resizing and in Draft mode
            if (Resizing == false && Draft)
            {
                if (File.Exists(_fileName))
                {
                    if ((_bitmap == null) || (_bitmap != null && _bitmap.Width != Convert.ToInt32(Size.Width)))
                    {
                        Bitmap original = new Bitmap(_fileName);
                        if (_bitmap != null) _bitmap.Dispose();
                        _bitmap = new Bitmap(Convert.ToInt32(Size.Width), Convert.ToInt32(Size.Width * original.Height / original.Width), PixelFormat.Format32bppArgb);
                        Graphics graph = Graphics.FromImage(_bitmap);
                        graph.DrawImage(original, 0, 0, _bitmap.Width, _bitmap.Height);
                        original.Dispose();
                        graph.Dispose();
                    }
                }
            }
            if (_bitmap == null) return;

            //Modifies the parallelogram if we are preserving aspect ration
            if (_preserveAspectRatio)
            {
                if ((Size.Width / _bitmap.Width) < (Size.Height / _bitmap.Height))
                    destPoints[2] = new PointF(LocationF.X, LocationF.Y + (Size.Width * _bitmap.Height / _bitmap.Width));
                else
                    destPoints[1] = new PointF(LocationF.X + (Size.Height * _bitmap.Width / _bitmap.Height), LocationF.Y);
            }
            //Draws the bitmap to the screen
            srcRect = new Rectangle(0, 0, _bitmap.Width, _bitmap.Height);
            g.DrawImage(_bitmap, destPoints, srcRect, GraphicsUnit.Pixel, imgAttrib);
        }

        #endregion
    }
}