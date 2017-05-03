using System;
using System.ComponentModel;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// The StatusPanel class allows adding user-defined status panels to the
    /// status bar
    /// </summary>
    public class StatusPanel : INotifyPropertyChanged
    {
        #region Fields

        private string _caption;
        private string _key;
        private int _width;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusPanel"/> class.
        /// </summary>
        public StatusPanel()
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
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key
        {
            get
            {
                return _key;
            }

            private set
            {
                if (_key == value) return;
                _key = value;
                OnPropertyChanged("Key");
            }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width
        {
            get
            {
                return _width;
            }

            set
            {
                if (_width == value) return;
                _width = value;
                OnPropertyChanged("Width");
            }
        }

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