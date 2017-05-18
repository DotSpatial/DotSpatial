// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Diagnostics;

namespace DotSpatial.Data
{
    /// <summary>
    /// This base class tries to correctly implement disposable, and should help make the behaviors
    /// more consistent across classes that inherit from this class.
    /// </summary>
    public class DisposeBase : IDisposeLock, IDisposable
    {
        private int _disposeCount;

        /// <summary>
        /// Finalizes an instance of the <see cref="DisposeBase"/> class.
        /// </summary>
        ~DisposeBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has already had the Dispose method called on it.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDisposed { get; set; }

        /// <summary>
        /// Gets a value indicating whether there are outstanding references that may be using the item
        /// that would prefer it if you did not dispose of the item while they are still using it.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDisposeLocked => _disposeCount > 0;

        /// <summary>
        /// Disposes
        /// </summary>
        public void Dispose()
        {
            // During debugging look for instances that are disposing when they shouldn't be.
            Debug.Assert(!IsDisposeLocked, "Disposing is currently forbitten.");
            if (!IsDisposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            else
            {
                Debug.WriteLine(GetType().Name + " was disposed more than once!");
            }

            IsDisposed = true;
        }

        /// <summary>
        /// Adds one request or "reference count" for this item not to be disposed. When an owner is finished,
        /// if this is 0, then dispose should be called. For now this does not prevent Dispose from being
        /// called, it is simply for tracking purposes.
        /// </summary>
        public void LockDispose()
        {
            _disposeCount++;
        }

        /// <summary>
        /// Removes one reference or request to prevent an object from being automatically disposed.
        /// </summary>
        public void UnlockDispose()
        {
            _disposeCount--;
        }

        /// <summary>
        /// This is where the meat of the dispose work is done. Subclasses should call dispose on any disposable
        /// members or internal members (presuming they are not dispose locked).
        /// </summary>
        /// <param name="isDisposing">True if the "Dispose" method was called instead of the destructor.</param>
        protected virtual void Dispose(bool isDisposing)
        {
        }
    }
}