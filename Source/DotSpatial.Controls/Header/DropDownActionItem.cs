// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// A dropdown (combo box) style item.
    /// </summary>
    public class DropDownActionItem : ActionItem
    {
        #region Fields

        private bool _allowEditingText;
        private string _displayText;
        private Color _fontColor;
        private bool _multiSelect;
        private string _nullValuePrompt;
        private object _selectedItem;
        private int _width;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DropDownActionItem"/> class.
        /// </summary>
        public DropDownActionItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DropDownActionItem"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="caption">The caption.</param>
        public DropDownActionItem(string key, string caption)
            : base(key, caption)
        {
        }

        #endregion

        #region Events

        /// <summary>
        /// Gets or sets an event handler fired on selected value changed.
        /// </summary>
        /// <value>The on selected value changed.</value>
        public event EventHandler<SelectedValueChangedEventArgs> SelectedValueChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the user may enter their own value into the dropdown.
        /// </summary>
        /// <value><c>true</c> if [allow editing text]; otherwise, <c>false</c>.</value>
        public bool AllowEditingText
        {
            get
            {
                return _allowEditingText;
            }

            set
            {
                if (_allowEditingText == value) return;
                _allowEditingText = value;
                OnPropertyChanged("AllowEditingText");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the display text in the dropdownbox.
        /// </summary>
        public string DisplayText
        {
            get
            {
                return _displayText;
            }

            set
            {
                if (_displayText == value) return;
                _displayText = value;
                OnPropertyChanged("DisplayText");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the color of the text in the dropdownbox.
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
        /// Gets the items contained in the dropdown. Changes are not supported after the item is added to the header control.
        /// </summary>
        public List<object> Items { get; } = new List<object>();

        /// <summary>
        /// Gets or sets a value indicating whether the user is selecting multiple elements from the dropdownlist or not.
        /// </summary>
        public bool MultiSelect
        {
            get
            {
                return _multiSelect;
            }

            set
            {
                if (_multiSelect == value) return;
                _multiSelect = value;
                OnPropertyChanged("MultiSelect");
            }
        }

        /// <summary>
        /// Gets or sets the text displayed grayed out (as a watermark) when the editor doesn't have focus, and its edit value is null.
        /// </summary>
        /// <value>The prompt.</value>
        public string NullValuePrompt
        {
            get
            {
                return _nullValuePrompt;
            }

            set
            {
                if (_nullValuePrompt == value) return;
                _nullValuePrompt = value;
                OnPropertyChanged("NullValuePrompt");
            }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public object SelectedItem
        {
            get
            {
                return _selectedItem;
            }

            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
                OnSelectedValueChanged(new SelectedValueChangedEventArgs(_selectedItem));
            }
        }

        /// <summary>
        /// Gets or sets the width of the item displayed in the header control.
        /// </summary>
        /// <value>The width.</value>
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

        #region Methods

        /// <summary>
        /// Triggers the SelectedValueChanged event.
        /// </summary>
        /// <param name="e">The eventargs.</param>
        public virtual void OnSelectedValueChanged(SelectedValueChangedEventArgs e)
        {
            SelectedValueChanged?.Invoke(this, e);
        }

        #endregion
    }
}