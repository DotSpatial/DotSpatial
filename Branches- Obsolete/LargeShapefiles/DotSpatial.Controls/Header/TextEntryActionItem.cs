using System.Drawing;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// A textbox item
    /// </summary>
    public class TextEntryActionItem : ActionItem
    {
        private string text;
        private int width;
        private Color fontColor;

        /// <summary>
        /// Gets or sets the width of the item displayed in the header control.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                if (width == value) return;
                width = value;
                OnPropertyChanged("Width");
            }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text
        {
            get { return text; }
            set
            {
                if (text == value) return;
                text = value;
                OnPropertyChanged("Text");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the color of the text in the dropdownbox
        /// </summary>
        public Color FontColor
        {
            get { return fontColor; }
            set
            {
                if (fontColor == value) return;
                fontColor = value;
                OnPropertyChanged("FontColor");
            }
        }
    }
}