// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// A simple action which is represented by a button or clickable surface.
    /// </summary>
    public class SimpleActionItem : ActionItem
    {
        #region Fields

        private Image _largeImage;
        private string _menuContainerKey;
        private bool _showInQuickAccessToolbar;
        private Image _smallImage;
        private short _sortOrder;
        private string _toggleGroupKey;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleActionItem"/> class.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="clickEventHandler">The click event handler.</param>
        public SimpleActionItem(string caption, EventHandler clickEventHandler)
        {
            Caption = caption;
            Click = clickEventHandler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleActionItem"/> class.
        /// </summary>
        /// <param name="rootKey">The root key.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="clickEventHandler">The click event handler.</param>
        public SimpleActionItem(string rootKey, string caption, EventHandler clickEventHandler)
            : this(caption, clickEventHandler)
        {
            RootKey = rootKey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleActionItem"/> class.
        /// </summary>
        /// <param name="rootKey">The root key.</param>
        /// <param name="menuContainerKey">The menu container key.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="clickEventHandler">The click event handler.</param>
        public SimpleActionItem(string rootKey, string menuContainerKey, string caption, EventHandler clickEventHandler)
            : this(rootKey, caption, clickEventHandler)
        {
            MenuContainerKey = menuContainerKey;
        }

        #endregion

        #region Events

        /// <summary>
        /// Gets or sets the click event handler.
        /// </summary>
        /// <value>The click event handler.</value>
        public event EventHandler Click;

        /// <summary>
        /// Thrown when the Toggle method is executed.
        /// </summary>
        public event EventHandler Toggling;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the large image.
        /// </summary>
        /// <value>The large image.</value>
        public Image LargeImage
        {
            get
            {
                return _largeImage;
            }

            set
            {
                if (_largeImage == value) return;
                _largeImage = value;
                OnPropertyChanged(nameof(LargeImage));
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
                return _menuContainerKey;
            }

            set
            {
                if (_menuContainerKey == value) return;
                _menuContainerKey = value;
                OnPropertyChanged(nameof(MenuContainerKey));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether an associated quick-access button is shown.
        /// </summary>
        public bool ShowInQuickAccessToolbar
        {
            get
            {
                return _showInQuickAccessToolbar;
            }

            set
            {
                if (_showInQuickAccessToolbar == value) return;
                _showInQuickAccessToolbar = value;
                OnPropertyChanged(nameof(ShowInQuickAccessToolbar));
            }
        }

        /// <summary>
        /// Gets or sets the small image.
        /// </summary>
        /// <value>The small image.</value>
        public Image SmallImage
        {
            get
            {
                return _smallImage;
            }

            set
            {
                if (_smallImage == value) return;
                _smallImage = value;
                OnPropertyChanged(nameof(SmallImage));
            }
        }

        /// <summary>
        /// Gets or sets the sort order. Lower values will suggest that an item should appear further left in a LeftToRight environment. Or higher up in a top to bottom environment.
        /// </summary>
        /// <remarks>Use a multiple of 100 or so to allow other developers some 'space' to place their groups.</remarks>
        /// <value>The sort order.</value>
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
                OnPropertyChanged(nameof(SortOrder));
            }
        }

        /// <summary>
        /// Gets or sets the toggle button group key used to select what buttons should toggle each other.
        /// </summary>
        /// <value>The toggle button key.</value>
        public string ToggleGroupKey
        {
            get
            {
                return _toggleGroupKey;
            }

            set
            {
                if (_toggleGroupKey == value) return;
                _toggleGroupKey = value;
                OnPropertyChanged(nameof(ToggleGroupKey));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Triggers the Click event.
        /// </summary>
        /// <param name="e">The event args.</param>
        public virtual void OnClick(EventArgs e)
        {
            Click?.Invoke(this, e);
        }

        /// <summary>
        /// Triggers the Toggling event.
        /// </summary>
        /// <param name="e">The event args.</param>
        public virtual void OnToggle(EventArgs e)
        {
            Toggling?.Invoke(this, e);
        }

        /// <summary>
        /// Checks the button it it is unchecked and unchecks the button it if is checked.
        /// </summary>
        /// <remarks>This method has no effect if ToggleGroupKey is null.</remarks>
        public virtual void Toggle()
        {
            if (ToggleGroupKey != null)
                OnToggle(EventArgs.Empty);
        }

        #endregion
    }
}