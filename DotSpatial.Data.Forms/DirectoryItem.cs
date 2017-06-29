// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  A library module for the DotSpatial geospatial framework for .Net.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/12/2008 2:06:42 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DotSpatial.Symbology;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// DirectoryItems can be either Files or Folders
    /// </summary>
    public class DirectoryItem
    {
        #region Events

        #endregion

        #region Private Variables

        private Color _backColor;
        private Rectangle _box;
        private Image _customImage;
        private Font _font;
        private Color _fontColor;
        private Timer _highlightTimer;
        private bool _isHighlighted;
        private bool _isOutlined;
        private bool _isSelected;
        private ItemType _itemType;
        private string _path;
        private bool _showImage;
        private string _text;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DirectoryItem
        /// </summary>
        public DirectoryItem()
        {
            Configure(null);
        }

        /// <summary>
        /// Creates a new instance of a directory item based on the specified path.
        /// </summary>
        /// <param name="path"></param>
        public DirectoryItem(string path)
        {
            Configure(path);
        }

        private void Configure(string path)
        {
            Width = 500;
            Height = 20;
            BackColor = Color.White;
            _fontColor = Color.Black;
            _highlightTimer = new Timer();
            _highlightTimer.Interval = 10;
            if (path == null) return;
            _path = path;
            string[] directoryParts = path.Split(System.IO.Path.DirectorySeparatorChar);
            _text = directoryParts[directoryParts.GetUpperBound(0)];
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the background color for this item.
        /// </summary>
        public Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        /// <summary>
        /// Gets or sets the rectangle in
        /// </summary>
        public Rectangle Bounds
        {
            get { return _box; }
            set { _box = value; }
        }

        /// <summary>
        /// Gets a rectangle in control coordinates showing the size of this control
        /// </summary>
        public Rectangle ClientRectangle
        {
            get { return new Rectangle(0, 0, Width, Height); }
        }

        /// <summary>
        /// Gets or sets the custom icon that is used if the ItemType is set to custom
        /// </summary>
        public Image CustomImage
        {
            get { return _customImage; }
            set { _customImage = value; }
        }

        /// <summary>
        /// Gets or sets the font for this directory item
        /// </summary>
        public Font Font
        {
            get { return _font; }
            set { _font = value; }
        }

        /// <summary>
        /// Gets or sets the color that should be used for drawing the fonts.
        /// </summary>
        public Color FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        /// <summary>
        /// Gets the icon that should be used
        /// </summary>
        public virtual Image Image
        {
            get
            {
                switch (_itemType)
                {
                    case ItemType.Custom: return _customImage;
                    case ItemType.Folder: return DialogImages.FolderOpen;
                    case ItemType.Image: return DialogImages.FileImage;
                    case ItemType.Line: return DialogImages.FileLine;
                    case ItemType.Point: return DialogImages.FilePoint;
                    case ItemType.Polygon: return DialogImages.FilePolygon;
                    case ItemType.Raster: return DialogImages.FileGrid;
                }
                return _customImage;
            }
        }

        /// <summary>
        /// Gets or sets whether this specific item should be drawn highlighted
        /// </summary>
        public bool IsHighlighted
        {
            get { return _isHighlighted; }
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
        /// Gets or sets a boolean that controls whether or not a black dotted rectangle
        /// will surround this item.
        /// </summary>
        public bool IsOutlined
        {
            get { return _isOutlined; }
            set
            {
                _isOutlined = value;
            }
        }

        /// <summary>
        /// Gets or sets whether this specific item should be drawn highlighted
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                }
            }
        }

        /// <summary>
        /// Gets or set the ItemType for this particular directory item.
        /// </summary>
        public ItemType ItemType
        {
            get { return _itemType; }
            set
            {
                _itemType = value;
                if (_itemType != ItemType.Custom) _showImage = true;
            }
        }

        /// <summary>
        /// Gets or sets the complete path for this directory item.
        /// </summary>
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        /// <summary>
        /// Gets or sets a boolean governing whether or not an icon should be drawn.
        /// </summary>
        public bool ShowImage
        {
            get { return _showImage; }
            set { _showImage = value; }
        }

        /// <summary>
        /// Gets or sets the string text for this directory item.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// Gets or set the integer top of this item
        /// </summary>
        public int Top
        {
            get { return _box.Y; }
            set { _box.Y = value; }
        }

        /// <summary>
        /// Gets or sets the width of this control
        /// </summary>
        public int Width
        {
            get { return _box.Width; }
            set { _box.Width = value; }
        }

        /// <summary>
        /// Gets or sets the height of this control
        /// </summary>
        public int Height
        {
            get { return _box.Height; }
            set { _box.Height = value; }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// This method instructs this item to draw itself onto the specified graphics surface.
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the Graphics object needed for drawing.</param>
        public virtual void Draw(PaintEventArgs e)
        {
            Matrix oldMatrix = e.Graphics.Transform;
            Matrix mat = new Matrix();
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
            //System.Diagnostics.Debug.WriteLine("top: " + this.Top.ToString());
            int left = 1;
            Pen border = Pens.White;
            Pen innerBorder = Pens.White;
            Brush fill = Brushes.White;
            Pen dots = new Pen(Color.Black);
            bool specialDrawing = false;
            dots.DashStyle = DashStyle.Dot;
            if (_isHighlighted && _isSelected == false)
            {
                border = new Pen(Color.FromArgb(216, 240, 250));
                innerBorder = new Pen(Color.FromArgb(248, 252, 254));
                fill = new LinearGradientBrush(ClientRectangle, Color.FromArgb(245, 250, 253), Color.FromArgb(232, 245, 253), LinearGradientMode.Vertical);
                specialDrawing = true;
            }
            if (_isSelected && _isHighlighted == false)
            {
                border = new Pen(Color.FromArgb(153, 222, 253));
                innerBorder = new Pen(Color.FromArgb(231, 245, 253));
                fill = new LinearGradientBrush(ClientRectangle, Color.FromArgb(241, 248, 253), Color.FromArgb(213, 239, 252), LinearGradientMode.Vertical);
                specialDrawing = true;
            }
            if (_isSelected && _isHighlighted)
            {
                border = new Pen(Color.FromArgb(182, 230, 251));
                innerBorder = new Pen(Color.FromArgb(242, 249, 253));
                fill = new LinearGradientBrush(ClientRectangle, Color.FromArgb(232, 246, 253), Color.FromArgb(196, 232, 250), LinearGradientMode.Vertical);
                specialDrawing = true;
            }

            e.Graphics.FillRectangle(fill, new Rectangle(1, 1, Width - 2, Height - 2));
            SymbologyGlobal.DrawRoundedRectangle(e.Graphics, innerBorder, new Rectangle(2, 2, Width - 4, Height - 4));
            SymbologyGlobal.DrawRoundedRectangle(e.Graphics, border, new Rectangle(1, 1, Width - 2, Height - 2));

            if (_isOutlined)
            {
                e.Graphics.DrawRectangle(dots, new Rectangle(2, 2, Width - 4, Height - 4));
            }

            if (_showImage)
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
            Brush b = new SolidBrush(_fontColor);
            e.Graphics.DrawString(Text, Font, b, new PointF(left, 1f));
            b.Dispose();

            if (specialDrawing)
            {
                if (border != null) border.Dispose();
                if (innerBorder != null) innerBorder.Dispose();
                if (fill != null) fill.Dispose();
                if (dots != null) dots.Dispose();
            }

            //base.OnPaint(e);
        }

        #endregion
    }
}