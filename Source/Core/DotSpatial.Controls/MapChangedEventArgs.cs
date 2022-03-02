// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Provides data for the <see langword='MapChanged'/> event.
    /// </summary>
    public class MapChangedEventArgs : EventArgs
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MapChangedEventArgs"/> class.
        /// </summary>
        /// <param name="oldValue">The old map.</param>
        /// <param name="newValue">The new map.</param>
        public MapChangedEventArgs(IMap oldValue, IMap newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the new map.
        /// </summary>
        public IMap NewValue { get; private set; }

        /// <summary>
        /// Gets the old map.
        /// </summary>
        public IMap OldValue { get; private set; }

        #endregion
    }
}