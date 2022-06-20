// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DotSpatial.Symbology;
using Timer = System.Windows.Forms.Timer;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// DirectoryItems can be either Files or Folders.
    /// </summary>
    public class DirectoryItem
    {
        #region Fields

        private Rectangle _box;
        private Timer _highlightTimer;
        private bool _isHighlighted;
        private ItemType _itemType;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryItem"/> class.
        /// </summary>
        public DirectoryItem()
        {
            Configure(null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryItem"/> class based on the specified path.
        /// </summary>
        /// <param name="path">Path of the directoryItem.</param>
        public DirectoryItem(string path)
        {
            Configure(path);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the background color for this item.
        /// </summary>
        public Color BackColor { get; set; }

        /// <summary>
        /// Gets or sets the bounds rectangle.
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                return _box;
            }

            set
            {
                _box = value;
            }
        }

        /// <summary>
        /// Gets a rectangle in control coordinates showing the size of this control.
        /// </summary>
        public Rectangle ClientRectangle => new(0, 0, Width, Height);

        /// <summary>
        /// Gets or sets the custom icon that is used if the ItemType is set to custom.
        /// </summary>
        public Image CustomImage { get; set; }

        /// <summary>
        /// Gets or sets the font for this directory item.
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// Gets or sets the color that should be used for drawing the fonts.
        /// </summary>
        public Color FontColor { get; set; }

        /// <summary>
        /// Gets or sets the height of this control.
        /// </summary>
        public int Height
        {
            get
            {
                return _box.Height;
            }

            set
            {
                _box.Height = value;
            }
        }

        /// <summary>
        /// Gets the icon that should be used.
        /// </summary>
        public virtual Image Image
        {
            get
            {
                return _itemType switch
                {
                    ItemType.Custom => CustomImage,
                    ItemType.Folder => DialogImages.FolderOpen,
                    ItemType.Image => DialogImages.FileImage,
                    ItemType.Line => DialogImages.FileLine,
                    ItemType.Point => DialogImages.FilePoint,
                    ItemType.Polygon => DialogImages.FilePolygon,
                    ItemType.Raster => DialogImages.FileGrid,
                    _ => CustomImage,
                };
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this specific item should be drawn highlighted.
        /// </summary>
        public bool IsHighlighted
        {
            get
            {
                return _isHighlighted;
            }

            set
            {
                if (_isHighlighted != value)
                {
                    _isHighlighted = value;
                    if (_isHighlighted) _highlightTimer.Start();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not a black dotted rectangle will surround this item.
        /// </summary>
        public bool IsOutlined { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this specific item should be drawn highlighted.
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets the ItemType for this particular directory item.
        /// </summary>
        public ItemType ItemType
        {
            get
            {
                return _itemType;
            }

            set
            {
                _itemType = value;
                if (_itemType != ItemType.Custom) ShowImage = true;
            }
        }

        /// <summary>
        /// Gets or sets the complete path for this directory item.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not an icon should be drawn.
        /// </summary>
        public bool ShowImage { get; set; }

        /// <summary>
        /// Gets or sets the string text for this directory item.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the integer top of this item.
        /// </summary>
        public int Top
        {
            get
            {
                return _box.Y;
            }

            set
            {
                _box.Y = value;
            }
        }

        /// <summary>
        /// Gets or sets the width of this control.
        /// </summary>
        public int Width
        {
            get
            {
                return _box.Width;
            }

            set
            {
                _box.Width = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method instructs this item to draw itself onto the specified graphics surface.
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the Graphics object needed for drawing.</param>
        public virtual void Draw(PaintEventArgs e)
        {
            Matrix oldMatrix = e.Graphics.Transform;
            Matrix mat = new();
            mat.Translate(Bounds.Left, Bounds.Top);
            e.Graphics.Transform = mat;
            OnDraw(e);
            e.Graphics.Transform = oldMatrix;
        }

        /// <summary>
        /// This supplies the basic drawing for this one element where the graphics object has been transformed
        /// based on the position of this item.
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the Graphics object needed for drawing.</param>
        protected virtual void OnDraw(PaintEventArgs e)
        {
            int left = 1;
            Pen border = Pens.White;
            Pen innerBorder = Pens.White;
            Brush fill = Brushes.White;
            Pen dots = new(Color.Black);
            bool specialDrawing = false;
            dots.DashStyle = DashStyle.Dot;
            if (_isHighlighted && !IsSelected)
            {
                border = new Pen(Color.FromArgb(216, 240, 250));
                innerBorder = new Pen(Color.FromArgb(248, 252, 254));
                fill = new LinearGradientBrush(ClientRectangle, Color.FromArgb(245, 250, 253), Color.FromArgb(232, 245, 253), LinearGradientMode.Vertical);
                specialDrawing = true;
            }

            if (IsSelected && !_isHighlighted)
            {
                border = new Pen(Color.FromArgb(153, 222, 253));
                innerBorder = new Pen(Color.FromArgb(231, 245, 253));
                fill = new LinearGradientBrush(ClientRectangle, Color.FromArgb(241, 248, 253), Color.FromArgb(213, 239, 252), LinearGradientMode.Vertical);
                specialDrawing = true;
            }

            if (IsSelected && _isHighlighted)
            {
                border = new Pen(Color.FromArgb(182, 230, 251));
                innerBorder = new Pen(Color.FromArgb(242, 249, 253));
                fill = new LinearGradientBrush(ClientRectangle, Color.FromArgb(232, 246, 253), Color.FromArgb(196, 232, 250), LinearGradientMode.Vertical);
                specialDrawing = true;
            }

            e.Graphics.FillRectangle(fill, new Rectangle(1, 1, Width - 2, Height - 2));
            SymbologyGlobal.DrawRoundedRectangle(e.Graphics, innerBorder, new Rectangle(2, 2, Width - 4, Height - 4));
            SymbologyGlobal.DrawRoundedRectangle(e.Graphics, border, new Rectangle(1, 1, Width - 2, Height - 2));

            if (IsOutlined)
            {
                e.Graphics.DrawRectangle(dots, new Rectangle(2, 2, Width - 4, Height - 4));
            }

            if (ShowImage)
            {
                if (Height > 20)
                {
                    e.Graphics.DrawImage(Image, new Rectangle(1, 1, Height - 2, Height - 2));
                }
                else
                {
                    e.Graphics.DrawImage(Image, new Rectangle(1, 1, Image.Width, Image.Height));
                }

                left = Height + 2;
            }

            Brush b = new SolidBrush(FontColor);
            e.Graphics.DrawString(Text, Font, b, new PointF(left, 1f));
            b.Dispose();

            if (specialDrawing)
            {
                border.Dispose();
                innerBorder.Dispose();
                fill.Dispose();
                dots.Dispose();
            }
        }

        private void Configure(string path)
        {
            Width = 500;
            Height = 20;
            BackColor = Color.White;
            FontColor = Color.Black;
            _highlightTimer = new Timer { Interval = 10 };
            if (path == null) return;
            Path = path;
            string[] directoryParts = path.Split(System.IO.Path.DirectorySeparatorChar);
            Text = directoryParts[directoryParts.GetUpperBound(0)];
        }

        #endregion
    }
}