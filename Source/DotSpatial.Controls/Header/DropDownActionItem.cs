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
        private readonly List<object> _Items = new List<object>();
        private bool allowEditingText;
        private string nullValuePrompt;
        private object selectedItem;
        private int width;
        private Color fontColor;
        private bool multiSelect;
        private string displayText;
        public System.Windows.Forms.ToolStripComboBox combobox {get;set;}

        /// <summary>
        /// Initializes a new instance of the DropDownActionItem class.
        /// </summary>
        public DropDownActionItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the DropDownActionItem class.
        /// </summary>
        public DropDownActionItem(string key, string caption)
            : base(key, caption)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user may enter their own value into the dropdown.
        /// </summary>
        /// <value>
        ///<c>true</c> if [allow editing text]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowEditingText
        {
            get
            {
                return allowEditingText;
            }
            set
            {
                if (allowEditingText == value) return;
                allowEditingText = value;
                OnPropertyChanged("AllowEditingText");
            }
        }

        /// <summary>
        /// Gets the items contained in the dropdown. Changes are not supported after the item is added to the header control.
        /// </summary>
        public List<object> Items
        {
            get
            {
                return _Items;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user is selecting multiple elements from the dropdownlist or not.
        /// </summary>
        public bool MultiSelect
        {
            get
            {
                return multiSelect;
            }
            set
            {
                if (multiSelect == value) return;
                multiSelect = value;
                OnPropertyChanged("MultiSelect");
            }
        }


        /// <summary>
        /// Gets or sets a value indicating the color of the text in the dropdownbox
        /// </summary>
        public Color FontColor
        {
            get
            {
                return fontColor;
            }
            set
            {
                if (fontColor == value) return;
                fontColor = value;
                OnPropertyChanged("FontColor");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the display text in the dropdownbox
        /// </summary>
        public string DisplayText
        {
            get
            {
                return displayText;
            }
            set
            {
                if (displayText == value) return;
                displayText = value;
                OnPropertyChanged("DisplayText");
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
        /// Gets or sets the text displayed grayed out (as a watermark) when the editor doesn't have focus, and its edit value is null.
        /// </summary>
        /// <value>
        /// The prompt.
        /// </value>
        public string NullValuePrompt
        {
            get
            {
                return nullValuePrompt;
            }
            set
            {
                if (nullValuePrompt == value) return;
                nullValuePrompt = value;
                OnPropertyChanged("NullValuePrompt");
            }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        public object SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                if (selectedItem == value) return;
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
                OnSelectedValueChanged(new SelectedValueChangedEventArgs(selectedItem));
            }
        }

        /// <summary>
        /// Gets or sets an event handler fired on selected value changed.
        /// </summary>
        /// <value>
        /// The on selected value changed.
        /// </value>
        public event EventHandler<SelectedValueChangedEventArgs> SelectedValueChanged;

        #region OnSelectedValueChanged

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