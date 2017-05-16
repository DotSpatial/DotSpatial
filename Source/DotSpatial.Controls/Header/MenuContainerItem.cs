// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// A container of other <see cref="ActionItem"/> instances.
    /// </summary>
    public class MenuContainerItem : ActionItem
    {
        #region Fields

        private Image _largeImage;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuContainerItem"/> class.
        /// </summary>
        public MenuContainerItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuContainerItem"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="caption">The caption.</param>
        public MenuContainerItem(string key, string caption)
            : base(key, caption)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuContainerItem"/> class.
        /// </summary>
        /// <param name="rootKey">The root key.</param>
        /// <param name="key">The key.</param>
        /// <param name="caption">The caption.</param>
        public MenuContainerItem(string rootKey, string key, string caption)
            : this(key, caption)
        {
            RootKey = rootKey;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the large image.
        /// </summary>
        /// <value>The large image.</value>
        public Image LargeImage
        {
            get
            {
                return _largeImage;
            }

            set
            {
                if (_largeImage == value) return;
                _largeImage = value;
                OnPropertyChanged("LargeImage");
            }
        }

        #endregion
    }
}