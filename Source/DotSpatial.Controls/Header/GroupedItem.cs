// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// A visually distinguished container of <see cref="ActionItem"/> instances that are Grouped inside of <see cref="RootItem"/>s.
    /// </summary>
    public abstract class GroupedItem : HeaderItem
    {
        #region Fields

        private string _groupCaption;
        private string _rootKey;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupedItem"/> class.
        /// </summary>
        protected GroupedItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupedItem"/> class.
        /// </summary>
        /// <param name="rootKey">The root key.</param>
        /// <param name="groupCaption">The groups caption.</param>
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

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the group. This is a logical unit.
        /// </summary>
        /// <value>The group.</value>
        public string GroupCaption
        {
            get
            {
                return _groupCaption;
            }

            set
            {
                if (_groupCaption == value) return;
                _groupCaption = value;
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
                return _rootKey;
            }

            set
            {
                if (_rootKey == value) return;
                _rootKey = value;
                OnPropertyChanged("RootKey");
            }
        }

        #endregion
    }
}