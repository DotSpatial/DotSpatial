// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// Interface for ChangeEventList{T}.
    /// </summary>
    /// <typeparam name="T">Type of the contained items.</typeparam>
    public interface IChangeEventList<T> : IList<T>, IChangeItem
        where T : IChangeItem
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether or not the list is currently suspended.
        /// </summary>
        bool EventsSuspended { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the elements of the specified collection to the end of the System.Collections.Generic.List&lt;T&gt;.
        /// </summary>
        /// <param name="collection">collection: The collection whose elements should be added to the end of the
        /// System.Collections.Generic.List&lt;T&gt;. The collection itself cannot be null, but it can contain
        /// elements that are null,
        /// if type T is a reference type.</param>
        /// <exception cref="System.ApplicationException">Unable to add while the ReadOnly property is set to
        ///  true.</exception>
        void AddRange(IEnumerable<T> collection);

        /// <summary>
        /// Resumes event sending and fires a ListChanged event if any changes have taken place.
        /// This will not track all the individual changes that may have fired in the meantime.
        /// </summary>
        void ResumeEvents();

        /// <summary>
        /// Temporarilly suspends notice events, allowing a large number of changes.
        /// </summary>
        void SuspendEvents();

        #endregion
    }
}