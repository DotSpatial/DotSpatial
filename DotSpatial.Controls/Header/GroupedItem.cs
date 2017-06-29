using System.ComponentModel;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// A visually distinguished container of <see cref="ActionItem"/> instances that are Grouped inside of <see cref="RootItem"/>s.
    /// </summary>
    public abstract class GroupedItem : HeaderItem
    {
        private string groupCaption;
        private string rootKey;

        /// <summary>
        /// Initializes a new instance of the GroupItem class.
        /// </summary>
        protected GroupedItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the HeaderItem class.
        /// </summary>
        protected GroupedItem(string rootKey, string groupCaption)
            : this()
        {
            GroupCaption = groupCaption;
            RootKey = rootKey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupedItem"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        protected GroupedItem(string key)
            : base(key)
        {
        }

        /// <summary>
        /// Gets or sets the group. This is a logical unit.
        /// </summary>
        /// <value>The group.</value>
        public string GroupCaption
        {
            get
            {
                return groupCaption;
            }
            set
            {
                if (groupCaption == value) return;
                groupCaption = value;
                OnPropertyChanged("GroupCaption");
            }
        }

        /// <summary>
        /// Gets or sets the root key.
        /// </summary>
        /// <value>The root key.</value>
        public string RootKey
        {
            get
            {
                return rootKey;
            }
            set
            {
                if (rootKey == value) return;
                rootKey = value;
                OnPropertyChanged("RootKey");
            }
        }
    }
}