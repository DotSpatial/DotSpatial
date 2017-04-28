using System;

namespace DotSpatial.Plugins.ExtensionManager
{
    /// <summary>
    /// Event args for page selected event.
    /// </summary>
    internal class PageSelectedEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets or sets the selected page.
        /// </summary>
        public int SelectedPage { get; set; }

        #endregion
    }
}