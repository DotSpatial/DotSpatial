using System;
using System.ComponentModel;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// An item that has a visible presence in the <see cref="HeaderControl"/>
    /// </summary>
    public abstract class HeaderItem : INotifyPropertyChanged
    {
        private string key;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderItem"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        protected HeaderItem(string key)
        {
            Key = key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderItem"/> class.
        /// </summary>
        protected HeaderItem()
        {
            Key = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The unique identifier.</value>
        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                if (key == value)
                    return;
                key = value;
                OnPropertyChanged("Key");
            }
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Triggers the PropertyChanged event.
        /// </summary>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var h = PropertyChanged;
            if (h != null)
            {
                h(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}