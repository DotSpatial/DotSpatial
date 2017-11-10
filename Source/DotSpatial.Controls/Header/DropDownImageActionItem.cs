// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleActionItem.cs" company="">
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Collections.Generic;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// A simple action which is represented by a button or clickable surface.
    /// </summary>
    public class DropDownImageActionItem : ActionItem
    {
        #region Constants and Fields

        private Image selectedImage = null;
        private readonly List<Image> _Items;
        private readonly List<string> _hints;
        private string menuContainerKey;
        private bool showInQuickAccessToolbar;
        private short sortOrder;
        private string toggleGroupKey;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the SimpleActionItem class.
        /// </summary>
        public DropDownImageActionItem(string caption, EventHandler<SelectedValueChangedEventArgs> selectionEventHandler)
        {
            _Items = new List<Image>();
            _hints = new List<string>();
            selectedImage = null;
            Caption = caption;
            SelectedValueChanged = selectionEventHandler;
        }

        /// <summary>
        /// Initializes a new instance of the SimpleActionItem class.
        /// </summary>
        public DropDownImageActionItem(string rootKey, string caption, EventHandler<SelectedValueChangedEventArgs> selectionEventHandler)
            : this(caption, selectionEventHandler)
        {
            RootKey = rootKey;
        }

        /// <summary>
        /// Initializes a new instance of the SimpleActionItem class.
        /// </summary>
        public DropDownImageActionItem(string rootKey, string menuContainerKey, string caption, EventHandler<SelectedValueChangedEventArgs> selectionEventHandler)
            : this(rootKey, caption, selectionEventHandler)
        {
            MenuContainerKey = menuContainerKey;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Gets or sets an event handler fired on selected value changed.
        /// </summary>
        /// <value>
        /// The on selected value changed.
        /// </value>
        public event EventHandler<SelectedValueChangedEventArgs> SelectedValueChanged;

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

        public Image SelectedItem
        {
            get
            {
                return selectedImage;
            }
            set
            {
                if (selectedImage == value) return;
                selectedImage = value;
                OnPropertyChanged("SelectedItem");
                OnSelectedValueChanged(new SelectedValueChangedEventArgs(selectedImage));
            }
        }

        public Image[] Items
        {
            get { return _Items.ToArray(); }
        }

        public string[] Hints
        {
            get { return _hints.ToArray(); }
        }

        #endregion

        #region Public Methods

        public void AddItem(Image image, string sHint)
        {
            _Items.Add(image);
            _hints.Add(sHint);
            OnPropertyChanged("Items");
        }

        public void RemoveItem(Image image)
        {
            int iPos = _Items.FindIndex(image.Equals);
            if (iPos >= 0)
            {
                _Items.RemoveAt(iPos);
                _hints.RemoveAt(iPos);
                OnPropertyChanged("Items");
            }
        }

        /// <summary>
        /// Triggers the SelectedValueChanged event.
        /// </summary>
        public virtual void OnSelectedValueChanged(SelectedValueChangedEventArgs ea)
        {
            if (SelectedValueChanged != null)
                SelectedValueChanged(this, ea);
        }

        #endregion
    }
}