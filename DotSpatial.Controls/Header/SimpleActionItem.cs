// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleActionItem.cs" company="">
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Drawing;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// A simple action which is represented by a button or clickable surface.
    /// </summary>
    public class SimpleActionItem : ActionItem
    {
        #region Constants and Fields

        private Image largeImage;
        private string menuContainerKey;
        private bool showInQuickAccessToolbar;
        private Image smallImage;
        private short sortOrder;
        private string toggleGroupKey;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the SimpleActionItem class.
        /// </summary>
        public SimpleActionItem(string caption, EventHandler clickEventHandler)
        {
            Caption = caption;
            Click = clickEventHandler;
        }

        /// <summary>
        /// Initializes a new instance of the SimpleActionItem class.
        /// </summary>
        public SimpleActionItem(string rootKey, string caption, EventHandler clickEventHandler)
            : this(caption, clickEventHandler)
        {
            RootKey = rootKey;
        }

        /// <summary>
        /// Initializes a new instance of the SimpleActionItem class.
        /// </summary>
        public SimpleActionItem(string rootKey, string menuContainerKey, string caption, EventHandler clickEventHandler)
            : this(rootKey, caption, clickEventHandler)
        {
            MenuContainerKey = menuContainerKey;
        }

        #endregion

        #region Public Events

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

        #region Public Properties

        /// <summary>
        /// True if an associated quick-access button is shown.
        /// False if the associated quick-access button is not shown.
        /// </summary>
        public bool ShowInQuickAccessToolbar
        {
            get
            {
                return showInQuickAccessToolbar;
            }

            set
            {
                if (showInQuickAccessToolbar == value) return;
                showInQuickAccessToolbar = value;
                OnPropertyChanged("ShowInQuickAccessToolbar");
            }
        }

        /// <summary>
        /// Gets or sets the large image.
        /// </summary>
        /// <value>The large image.</value>
        public Image LargeImage
        {
            get
            {
                return largeImage;
            }

            set
            {
                if (largeImage == value) return;
                largeImage = value;
                OnPropertyChanged("LargeImage");
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
                if (menuContainerKey == value) return;
                menuContainerKey = value;
                OnPropertyChanged("MenuContainerKey");
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
                return smallImage;
            }

            set
            {
                if (smallImage == value) return;
                smallImage = value;
                OnPropertyChanged("SmallImage");
            }
        }

        /// <summary>
        /// Gets or sets the sort order. Lower values will suggest that an item should appear further left in a LeftToRight environment. Or higher up in a top to bottom environment.
        /// </summary>
        /// <remarks>Use a multiple of 100 or so to allow other developers some 'space' to place their groups.</remarks>
        /// <value>
        /// The sort order.
        /// </value>
        public short SortOrder
        {
            get
            {
                return sortOrder;
            }
            set
            {
                if (sortOrder == value) return;
                sortOrder = value;
                OnPropertyChanged("SortOrder");
            }
        }

        /// <summary>
        /// Gets or sets the toggle button group key used to select what buttons should toggle each other..
        /// </summary>
        /// <value>The toggle button key.</value>
        public string ToggleGroupKey
        {
            get
            {
                return toggleGroupKey;
            }
            set
            {
                if (toggleGroupKey == value) return;
                toggleGroupKey = value;
                OnPropertyChanged("ToggleGroupKey");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Triggers the Click event.
        /// </summary>
        public virtual void OnClick(EventArgs ea)
        {
            if (Click != null)
            {
                Click(this, ea);
            }
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

        #region OnToggling

        /// <summary>
        /// Triggers the Toggling event.
        /// </summary>
        public virtual void OnToggle(EventArgs ea)
        {
            if (Toggling != null)
                Toggling(this, ea);
        }

        #endregion

        #endregion
    }
}