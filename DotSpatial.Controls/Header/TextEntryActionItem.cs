using System.ComponentModel;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// A textbox item
    /// </summary>
    public class TextEntryActionItem : ActionItem
    {
        private string text;
        private int width;

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
                if (width == value)
                    return;
                width = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Width"));
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
                if (text == value)
                    return;
                text = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Text"));
            }
        }
    }
}