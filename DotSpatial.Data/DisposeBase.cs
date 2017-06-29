// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
// The Original Code is from MapWindow.dll version 6.0
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/21/2010 11:25:19 AM
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// ********************************************************************************************************

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
        private bool _isDisposed;

        /// <summary>
        /// Gets a value indicating whether this instance has already had the Dispose method called on it.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDisposed
        {
            get { return _isDisposed; }
            set { _isDisposed = value; }
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes
        /// </summary>
        public void Dispose()
        {
            // During debugging look for instances that are disposing when they shouldn't be.
            Debug.Assert(IsDisposeLocked == false);
            if (!_isDisposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            else
            {
                Debug.WriteLine(GetType().Name + " was disposed more than once!");
            }
            _isDisposed = true;
        }

        #endregion

        #region IDisposeLock Members

        /// <summary>
        /// Adds one request or "reference count" for this item not to be disposed.  When an owner is finished,
        /// if this is 0, then dispose should be called.  For now this does not prevent Dispose from being
        /// called, it is simply for tracking purposes.
        /// </summary>
        public void LockDispose()
        {
            _disposeCount++;
        }

        /// <summary>
        /// Gets a value indicating whether there are outstanding references that may be using the item
        /// that would prefer it if you did not dispose of the item while they are still using it.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDisposeLocked
        {
            get { return (_disposeCount > 0); }
        }

        /// <summary>
        /// Removes one reference or request to prevent an object from being automatically disposed.
        /// </summary>
        public void UnlockDispose()
        {
            _disposeCount--;
        }

        #endregion

        /// <summary>
        /// Finalizes an instance of the <see cref="DisposeBase"/> class.
        /// </summary>
        ~DisposeBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// This is where the meat of the dispose work is done.  Subclasses should call dispose on any disposable
        /// members or internal members (presuming they are not dispose locked).
        /// </summary>
        /// <param name="isDisposing">True if the "Dispose" method was called instead of the destructor.</param>
        protected virtual void Dispose(bool isDisposing)
        {
        }
    }
}