// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// Unlike forms that have a disposable life cycle with a clear chain of ownership, or purely managed
    /// libraries that have in a built in reference counting system, disposable members in DotSpatial
    /// require the ability to behave in some cases like a layer that should be removed from memory
    /// as soon as it is removed from the map, and other times like a persistent object. The DisposeLock
    /// concept works like an industrial lock-out system. Each additional lock increases the lock count.
    /// When all users have released the lock, the IsDisposedLocked property will be false, and the next
    /// action like a removal from the parent layers will properly dispose the item. Users may feel free
    /// to add the lock in order to handle disposal on the layers themselves. These methods will not
    /// actually prevent Dispose from functioning, but understand that calling dispose when there is an
    /// IsDisposeLocked is true means that there will likely be a problem.
    /// </summary>
    public interface IDisposeLock
    {
        /// <summary>
        /// Gets a value indicating whether an existing reference is requesting that the object is not disposed of.
        /// </summary>
        bool IsDisposeLocked { get; }

        /// <summary>
        /// Locks dispose. This typically adds one instance of an internal reference counter.
        /// </summary>
        void LockDispose();

        /// <summary>
        /// Unlocks dispose. This typically removes one instance of an internal reference counter.
        /// </summary>
        void UnlockDispose();
    }
}