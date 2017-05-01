using System;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    ///
    /// </summary>
    public class SelectedValueChangedEventArgs : EventArgs
    {
        #region Fields

        // Fields...
        private object _SelectedItem;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedValueChangedEventArgs"/> class.
        /// </summary>
        public SelectedValueChangedEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the SelectedValueChangedEventArgs class.
        /// </summary>
        /// <param name="selectedItem"></param>
        public SelectedValueChangedEventArgs(object selectedItem)
        {
            _SelectedItem = selectedItem;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        public object SelectedItem
        {
            get
            {
                return _SelectedItem;
            }

            set
            {
                _SelectedItem = value;
            }
        }

        #endregion
    }
}