using System;
using System.ComponentModel;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// An item that has a visible presence in the <see cref="HeaderControl"/>.
    /// </summary>
    public abstract class HeaderItem : INotifyPropertyChanged
    {
        #region Fields

        private string _key;

        #endregion

        #region  Constructors

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

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The unique identifier.</value>
        public string Key
        {
            get
            {
                return _key;
            }

            set
            {
                if (_key == value)
                    return;
                _key = value;
                OnPropertyChanged("Key");
            }
        }

        /// <summary>
        /// Gets or sets an <see cref="object"/> that contains data about the HeaderItem. The default is null.
        /// </summary>
        /// <remarks>
        /// Any type derived from the <see cref="object"/> class can be assigned to this property.
        /// A common use for the Tag property is to store data that is closely associated with the item.
        /// </remarks>
        public object Tag { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Triggers the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}