using System.ComponentModel;
using System.Drawing;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// A container of other <see cref="ActionItem"/> instances.
    /// </summary>
    public class MenuContainerItem : ActionItem
    {
        private Image largeImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuContainerItem"/> class.
        /// </summary>
        public MenuContainerItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MenuContainerItem class.
        /// </summary>
        public MenuContainerItem(string key, string caption)
            : base(key, caption)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MenuContainerItem class.
        /// </summary>
        public MenuContainerItem(string rootKey, string key, string caption)
            : this(key, caption)
        {
            RootKey = rootKey;
        }

        /// <summary>
        /// Gets or sets the large image.
        /// </summary>
        /// <value>The large image.</value>
        public Image LargeImage
        {
            get { return largeImage; }
            set
            {
                if (largeImage == value) return;
                largeImage = value;
                OnPropertyChanged("LargeImage");
            }
        }
    }
}