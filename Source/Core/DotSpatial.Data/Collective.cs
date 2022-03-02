// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// Carries event arguments for the generic IEventList.
    /// </summary>
    /// <typeparam name="T">Type of the contained items.</typeparam>
    public class Collective<T> : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Collective{T}"/> class.
        /// </summary>
        /// <param name="inCollection">The IEnumerable&lt;T&gt; specified during the event"/>.</param>
        public Collective(IEnumerable<T> inCollection)
        {
            Collection = inCollection;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list item being referenced by this event.
        /// </summary>
        public IEnumerable<T> Collection { get; protected set; }

        #endregion
    }
}