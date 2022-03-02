// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// An item that appears as a root-level container of other <see cref="ActionItem"/> instances.
    /// </summary>
    public class RootItem : HeaderItem
    {
        #region Fields

        private string _caption;
        private short _sortOrder;
        private bool _visible;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RootItem"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="caption">The caption.</param>
        public RootItem(string key, string caption)
            : this()
        {
            Key = key;
            Caption = caption;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RootItem"/> class.
        /// </summary>
        public RootItem()
        {
            _visible = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption
        {
            get
            {
                return _caption;
            }

            set
            {
                if (_caption == value) return;
                _caption = value;
                OnPropertyChanged("Caption");
            }
        }

        /// <summary>
        /// Gets or sets the sort order. Lower values will suggest that an item should appear further left in a LeftToRight environment.
        /// </summary>
        /// <remarks>Use a multiple of 10 or so to allow other developers some 'space' to place their menus.</remarks>
        /// <value>
        /// The sort order.
        /// </value>
        public short SortOrder
        {
            get
            {
                return _sortOrder;
            }

            set
            {
                if (_sortOrder == value) return;
                _sortOrder = value;
                OnPropertyChanged("SortOrder");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RootItem"/> is visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if visible; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>Will generally bring this <see cref="RootItem"/> into focus when Visible is set to true.</remarks>
        public bool Visible
        {
            get
            {
                return _visible;
            }

            set
            {
                if (_visible == value) return;
                _visible = value;
                OnPropertyChanged("Visible");
            }
        }

        #endregion
    }
}