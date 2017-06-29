namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// An item that appears as a root-level container of other <see cref="ActionItem"/> instances.
    /// </summary>
    public class RootItem : HeaderItem
    {
        private string caption;
        private short sortOrder;
        private bool visible;

        /// <summary>
        /// Initializes a new instance of RootItem MenuContainerItem class.
        /// </summary>
        public RootItem(string key, string caption)
            : this()
        {
            Key = key;
            Caption = caption;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public RootItem()
        {
            visible = true;
        }

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption
        {
            get { return caption; }
            set
            {
                if (caption == value) return;
                caption = value;
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
            get { return sortOrder; }
            set
            {
                if (sortOrder == value) return;
                sortOrder = value;
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