// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// An interactive element.
    /// </summary>
    public abstract class ActionItem : GroupedItem
    {
        #region Fields

        private string _caption;
        private bool _enabled = true;
        private string _toolTipText;
        private bool _visible = true;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionItem"/> class.
        /// </summary>
        protected ActionItem()
        {
            Key = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionItem"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        protected ActionItem(string key)
            : base(key)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionItem"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="caption">The caption.</param>
        protected ActionItem(string key, string caption)
        {
            Key = key;
            Caption = caption;
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
        /// Gets or sets a value indicating whether this <see cref="ActionItem"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                if (_enabled == value) return;
                _enabled = value;
                OnPropertyChanged("Enabled");
            }
        }

        /// <summary>
        /// Gets or sets the simple tool tip.
        /// </summary>
        /// <value>The simple tool tip.</value>
        public string ToolTipText
        {
            get
            {
                return _toolTipText;
            }

            set
            {
                if (_toolTipText == value) return;
                _toolTipText = value;
                OnPropertyChanged("ToolTipText");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ActionItem"/> is visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if visible; otherwise, <c>false</c>.
        /// </value>
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