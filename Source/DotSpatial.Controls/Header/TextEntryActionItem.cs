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
        private string _menuContainerKey;
        private System.Windows.Forms.HorizontalAlignment _textAlignment = System.Windows.Forms.HorizontalAlignment.Left;

        // CGX
        private string _menuContainerKey;
        private System.Windows.Forms.HorizontalAlignment _textAlignment = System.Windows.Forms.HorizontalAlignment.Left;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TextEntryActionItem"/> class.
        /// </summary>
        public TextEntryActionItem()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextEntryActionItem"/> class.
        /// </summary>
        /// <param name="rootKey">The root key.</param>
        /// <param name="caption">The caption.</param>
        public TextEntryActionItem(string rootKey, string caption)
            : base(caption)
        {
            Caption = caption;
            RootKey = rootKey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextEntryActionItem"/> class.
        /// </summary>
        /// <param name="rootKey">The root key.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="menuContainerKey">The menu container key.</param>
        public TextEntryActionItem(string rootKey, string menuContainerKey, string caption)
            : this(rootKey, caption)
        {
            MenuContainerKey = menuContainerKey;
        }// CGX END
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TextEntryActionItem"/> class.
        /// </summary>
        public TextEntryActionItem()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextEntryActionItem"/> class.
        /// </summary>
        /// <param name="rootKey">The root key.</param>
        /// <param name="caption">The caption.</param>
        public TextEntryActionItem(string rootKey, string caption)
            : base(caption)
        {
            Caption = caption;
            RootKey = rootKey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextEntryActionItem"/> class.
        /// </summary>
        /// <param name="rootKey">The root key.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="menuContainerKey">The menu container key.</param>
        public TextEntryActionItem(string rootKey, string menuContainerKey, string caption)
            : this(rootKey, caption)
        {
            MenuContainerKey = menuContainerKey;
        }// CGX END
        #endregion

        #region Properties

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

        // CGX

        /// <summary>
        /// Gets or sets the menu container key.
        /// </summary>
        /// <value>The menu container key.</value>
        public string MenuContainerKey
        {
            get
            {
                return _menuContainerKey;
            }

            set
            {
                _menuContainerKey = value;
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
                return _textAlignment;
            }

            set
            {
                _textAlignment = value;
                OnPropertyChanged("TextAlignment");
            }
        }// CGX END

        #endregion
    }
}