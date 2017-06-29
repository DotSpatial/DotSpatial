using System;
using System.ComponentModel;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// An interactive element.
    /// </summary>
    public abstract class ActionItem : GroupedItem
    {
        private string caption;
        private bool enabled = true;
        private string toolTipText;
        private bool visible = true;

        /// <summary>
        /// Initializes a new instance of the ActionItem class.
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
        /// Initializes a new instance of the ActionItem class.
        /// </summary>
        protected ActionItem(string key, string caption)
        {
            this.Key = key;
            this.Caption = caption;
        }

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption
        {
            get
            {
                return caption;
            }
            set
            {
                if (caption == value) return;
                caption = value;
                OnPropertyChanged("Caption");
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
                return toolTipText;
            }
            set
            {
                if (toolTipText == value) return;
                toolTipText = value;
                OnPropertyChanged("ToolTipText");
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
            get { return enabled; }
            set
            {
                if (enabled == value) return;
                enabled = value;
                OnPropertyChanged("Enabled");
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
            get { return visible; }
            set
            {
                if (visible == value) return;
                visible = value;
                OnPropertyChanged("Visible");
            }
        }
    }
}