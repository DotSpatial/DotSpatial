// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// A textbox item
    /// </summary>
    public class TextEntryActionItem : ActionItem
    {
        #region Fields

        private Color _fontColor;
        private string _text;
        private int _width;
		// CGX
		private string menuContainerKey;
        private System.Windows.Forms.HorizontalAlignment textAlignment = System.Windows.Forms.HorizontalAlignment.Left;

        #endregion

        #region Properties

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
        /// Gets or sets a value indicating the color of the text in the dropdownbox
        /// </summary>
        public Color FontColor
        {
            get
            {
                return _fontColor;
            }

            set
            {
                if (_fontColor == value) return;
                _fontColor = value;
                OnPropertyChanged("FontColor");
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
            get
            {
                return _text;
            }

            set
            {
                if (_text == value) return;
                _text = value;
                OnPropertyChanged("Text");
            }
        }

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
                return _width;
            }

            set
            {
                if (_width == value) return;
                _width = value;
                OnPropertyChanged("Width");
            }
        }

        #endregion
    }
}