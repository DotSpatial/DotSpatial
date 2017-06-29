using System.ComponentModel;
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

        // CGX
        private string menuContainerKey;
        private System.Windows.Forms.HorizontalAlignment textAlignment = System.Windows.Forms.HorizontalAlignment.Left;

        public TextEntryActionItem()
            : base()
        {
        }

        public TextEntryActionItem(string rootKey, string caption)
            : base(caption)
        {
            Caption = caption;
            RootKey = rootKey;
        }

        public TextEntryActionItem(string rootKey, string menuContainerKey, string caption)
            : this(rootKey, caption)
        {
            MenuContainerKey = menuContainerKey;
        }
        // CGX END

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

        /// <summary>
        /// Gets or sets the menu container key.
        /// </summary>
        /// <value>The menu container key.</value>
        public string MenuContainerKey
        {
            get
            {
                return menuContainerKey;
            }

            set
            {
                menuContainerKey = value;
                OnPropertyChanged("MenuContainerKey");
            }
        }

        /// <summary>
        /// Gets or sets the Text Alignment.
        /// </summary>
        /// <value>The menu container key.</value>
        public System.Windows.Forms.HorizontalAlignment TextAlignment
        {
            get
            {
                return textAlignment;
            }

            set
            {
                textAlignment = value;
                OnPropertyChanged("TextAlignment");
            }
        }
    }
}