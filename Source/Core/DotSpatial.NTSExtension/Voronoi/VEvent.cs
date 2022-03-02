// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.NTSExtension.Voronoi
{
    /// <summary>
    /// The VEvent.
    /// </summary>
    internal abstract class VEvent : IComparable
    {
        #region Properties

        /// <summary>
        /// Gets the Y coordinate.
        /// </summary>
        public abstract double Y { get; }

        /// <summary>
        /// Gets the X coordinate.
        /// </summary>
        protected abstract double X { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Compares this event to the other event.
        /// </summary>
        /// <param name="obj">Second VEvent to compare.</param>
        /// <returns>True, if the events are equal.</returns>
        /// <exception cref="ArgumentException">Thrown if obj is not a VEvent.</exception>
        public int CompareTo(object obj)
        {
            if (!(obj is VEvent))
                throw new ArgumentException("obj is not a VEvent!");
            int i = Y.CompareTo(((VEvent)obj).Y);
            return i != 0 ? i : X.CompareTo(((VEvent)obj).X);
        }

        #endregion
    }
}