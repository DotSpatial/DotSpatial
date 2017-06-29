using System;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// Information about the selected root item
    /// </summary>
    public class RootItemEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RootItemEventArgs"/> class.
        /// </summary>
        public RootItemEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the RootItemEventArgs class.
        /// </summary>
        /// <param name="selectedKey">The key of the selected root item</param>
        public RootItemEventArgs(string selectedKey)
        {
            SelectedRootKey = selectedKey;
        }

        /// <summary>
        /// Gets or sets the selected root item key.
        /// </summary>
        /// <value>
        /// The selected root item key
        /// </value>
        public string SelectedRootKey { get; set; }
    }
}