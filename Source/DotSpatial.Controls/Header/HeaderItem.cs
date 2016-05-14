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
            get { return key; }
            set
            {
                if (key == value)
                    return;
                key = value;
                OnPropertyChanged("Key");
            }
        }

        /// <summary>
        /// An <see cref="Object"/> that contains data about the HeaderItem. The default is null.
        /// </summary>
        /// <remarks>
        /// Any type derived from the <see cref="Object"/> class can be assigned to this property. 
        /// A common use for the Tag property is to store data that is closely associated with the item. 
        /// </remarks>
        public object Tag { get; set; }

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