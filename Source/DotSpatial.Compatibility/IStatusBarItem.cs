// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// The object given back when a panel is added to the status bar.
    /// </summary>
    public interface IStatusBarItem
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Alignment of the text.
        /// </summary>
        HJustification Alignment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this StatusBarItem should auto size itself.
        /// </summary>
        bool AutoSize { get; set; }

        /// <summary>
        /// Gets or sets the minimum allowed width for this StatusBarItem.
        /// </summary>
        int MinWidth { get; set; }

        /// <summary>
        /// Gets or sets the Text within the StatusBarItem.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the width of the StatusBarItem.
        /// </summary>
        int Width { get; set; }

        #endregion
    }
}