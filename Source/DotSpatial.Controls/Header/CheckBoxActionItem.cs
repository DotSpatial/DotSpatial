// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// A simple action which is represented by a checkbox.
    /// </summary>
    public class CheckBoxActionItem : ActionItem
    {
        #region Fields

        private bool _checked;
        private string _menuContainerKey;
        private string _toggleGroupKey;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBoxActionItem"/> class.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="toggleEventHandler">The toggle event handler.</param>
        public CheckBoxActionItem(string caption, EventHandler toggleEventHandler)
        {
            Caption = caption;
            Toggling = toggleEventHandler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBoxActionItem"/> class.
        /// </summary>
        /// <param name="rootKey">The root key.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="toggleEventHandler">The toggle event handler.</param>
        public CheckBoxActionItem(string rootKey, string caption, EventHandler toggleEventHandler)
            : this(caption, toggleEventHandler)
        {
            RootKey = rootKey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBoxActionItem"/> class.
        /// </summary>
        /// <param name="rootKey">The root key.</param>
        /// <param name="menuContainerKey">The menu container key.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="toggleEventHandler">The toggle event handler.</param>
        public CheckBoxActionItem(string rootKey, string menuContainerKey, string caption, EventHandler toggleEventHandler)
            : this(rootKey, caption, toggleEventHandler)
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

        /// <summary>
        /// Thrown when the Toggle method is executed.
        /// </summary>
        public event EventHandler CheckedChanged;

        #endregion

        #region Properties

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
                OnPropertyChanged("MenuContainerKey");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether an associated quick-access button is shown.
        /// </summary>
        public bool Checked
        {
            get
            {
                return _checked;
            }

            set
            {
                if (_checked == value) return;
                _checked = value;
                OnPropertyChanged("Checked");
                CheckedChanged?.Invoke(this, new EventArgs());
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
                OnPropertyChanged("ToggleGroupKey");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Triggers the CheckedChanged event.
        /// </summary>
        /// <param name="e">The event args.</param>
        public virtual void OnCheckedChanged(EventArgs e)
        {
            CheckedChanged?.Invoke(this, e);
        }

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