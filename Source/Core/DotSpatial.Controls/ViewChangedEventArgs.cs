// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Arguments of the ViewChangedEvent.
    /// </summary>
    public class ViewChangedEventArgs : EventArgs
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewChangedEventArgs"/> class.
        /// </summary>
        /// <param name="oldView">The view that existed before the change.</param>
        /// <param name="newView">The view that exists after the change.</param>
        public ViewChangedEventArgs(Rectangle oldView, Rectangle newView)
        {
            OldView = oldView;
            NewView = newView;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the view after the change.
        /// </summary>
        public Rectangle NewView { get; }

        /// <summary>
        /// Gets the view before the change.
        /// </summary>
        public Rectangle OldView { get; }

        #endregion
    }
}