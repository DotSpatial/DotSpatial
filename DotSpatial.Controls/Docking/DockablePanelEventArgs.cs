// -----------------------------------------------------------------------
// <copyright company="DotSpatial Team">
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace DotSpatial.Controls.Docking
{
    /// <summary>
    /// The active panel changed event args.
    /// </summary>
    public class DockablePanelEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the ActivePanelChangedEventArgs class.
        /// </summary>
        /// <param name="activePanelKey">
        /// The active Panel Key.
        /// </param>
        public DockablePanelEventArgs(string activePanelKey)
        {
            ActivePanelKey = activePanelKey;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the active panel key.
        /// </summary>
        /// <value>
        ///   The active panel key.
        /// </value>
        public string ActivePanelKey { get; set; }

        #endregion
    }
}