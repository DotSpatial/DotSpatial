// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Handles drawing by adding an icon to the mix.
    /// Owner draw must be set to true for this to work.
    /// </summary>
    [ToolboxItem(false)]
    public class IconMenuItem : MenuItem
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IconMenuItem"/> class with the specified name.
        /// </summary>
        /// <param name="name">Name of the menu item.</param>
        /// <param name="icon">Icon of the menu item.</param>
        /// <param name="onClick">The click event handler.</param>
        public IconMenuItem(string name, Icon icon, EventHandler onClick)
            : base(name, onClick)
        {
            Icon = icon;
            OwnerDraw = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IconMenuItem"/> class with the specified name.
        /// </summary>
        /// <param name="name">Name of the menu item.</param>
        /// <param name="image">Image of the menu item.</param>
        /// <param name="onClick">The click event handler.</param>
        public IconMenuItem(string name, Image image, EventHandler onClick)
            : base(name, onClick)
        {
            Image = image;
            OwnerDraw = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IconMenuItem"/> class with the specified name.
        /// </summary>
        /// <param name="name">Name of the menu item.</param>
        /// <param name="onClick">The click event handler.</param>
        public IconMenuItem(string name, EventHandler onClick)
            : base(name, onClick)
        {
            OwnerDraw = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the icon to be drawn to the left of this menu item.
        /// </summary>
        public Icon Icon { get; set; }

        /// <summary>
        /// Gets or sets the image to be drawn to the left of this menu item
        /// </summary>
        public Image Image { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc/>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            Rectangle dRect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                SolidBrush back = new SolidBrush(Color.FromArgb(236, 240, 246));
                e.Graphics.FillRectangle(back, e.Bounds);
                back.Dispose();

                Brush iconBack = new SolidBrush(Color.FromArgb(210, 214, 220));
                e.Graphics.FillRectangle(iconBack, e.Bounds.X, e.Bounds.Y, 22, e.Bounds.Height);
                iconBack.Dispose();

                Pen border = new Pen(Color.FromArgb(174, 207, 247));
                e.Graphics.DrawRectangle(border, dRect);
                border.Dispose();
            }
            else
            {
                SolidBrush b = new SolidBrush(Color.FromArgb(240, 240, 240));
                e.Graphics.FillRectangle(b, e.Bounds);
                b.Dispose();
                Brush iconBack = new SolidBrush(Color.FromArgb(220, 220, 220));
                e.Graphics.FillRectangle(iconBack, e.Bounds.X, e.Bounds.Y, 22, e.Bounds.Height);
                iconBack.Dispose();
            }

            Rectangle tight = new Rectangle(e.Bounds.Left + 2, e.Bounds.Top + 2, 16, 16);
            if (Icon != null) e.Graphics.DrawIcon(Icon, e.Bounds.Left + 2, e.Bounds.Top + 2);
            if (Image != null) e.Graphics.DrawImage(Image, tight);
            Font menuFont = SystemInformation.MenuFont;
            if (Text != null) e.Graphics.DrawString(Text, menuFont, Brushes.Black, e.Bounds.X + 25, e.Bounds.Top + 2);
        }

        /// <inheritdoc/>
        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            Font menuFont = SystemInformation.MenuFont;
            StringFormat strfmt = new StringFormat();
            SizeF sizef = e.Graphics.MeasureString(Text, menuFont, 1000, strfmt);
            e.ItemWidth = (int)Math.Ceiling(sizef.Width) + 20;
            e.ItemHeight = (int)Math.Max(Math.Ceiling(sizef.Height), 20);
        }

        #endregion
    }
}