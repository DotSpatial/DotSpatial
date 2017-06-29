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

        private string caption;
        private string key;
        private int width;

        #endregion

        /// <summary>
        /// Initializes a new instance of the StatusPanel class.
        /// </summary>
        public StatusPanel()
        {
            Key = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>
        /// The caption.
        /// </value>
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
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                if (width == value) return;
                width = value;
                OnPropertyChanged("Width");
            }
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key
        {
            get { return key; }
            private set
            {
                if (key == value) return;
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