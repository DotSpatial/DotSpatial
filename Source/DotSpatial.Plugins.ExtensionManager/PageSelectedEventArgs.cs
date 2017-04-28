using System;

namespace DotSpatial.Plugins.ExtensionManager
{
    internal class PageSelectedEventArgs : EventArgs
    {
        #region Properties

        public int SelectedPage { get; set; }

        #endregion
    }
}