// ********************************************************************************************************
// Product Name: DotSpatial.Ribbon.dll
// Description:  Original new code creatd by Ted Dunsford to help add items to panels
// ********************************************************************************************************
// The license is the Microsoft Public License (Ms-PL).  A copy of the license can be found
// here: http://www.opensource.org/licenses/ms-pl.html.  This is to keep it consistent with the
// rest of the ribbon code content.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Initial Developer was Ted Dunsford, August 22, 2010 11:34:00 AM.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// ********************************************************************************************************

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
        /// <summary>
        /// Creates a new instance of the Icon Menu Item with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="icon"></param>
        /// <param name="onClick"></param>
        public IconMenuItem(string name, Icon icon, EventHandler onClick)
            : base(name, onClick)
        {
            Icon = icon;
            OwnerDraw = true;
        }

        /// <summary>
        /// Creates a new instance of the Icon Menu Item with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="image"></param>
        /// <param name="onClick"></param>
        public IconMenuItem(string name, Image image, EventHandler onClick)
            : base(name, onClick)
        {
            Image = image;
            OwnerDraw = true;
        }

        /// <summary>
        /// Creates a new instance of the Icon Menu Item with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="onClick"></param>
        public IconMenuItem(string name, EventHandler onClick)
            : base(name, onClick)
        {
            OwnerDraw = true;
        }

        /// <summary>
        /// Gets or sets the icon to be drawn to the left of this menu item.
        /// </summary>
        public Icon Icon { get; set; }

        /// <summary>
        /// Gets or sets the image to be drawn to the left of this menu item
        /// </summary>
        public Image Image { get; set; }

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
    }
}