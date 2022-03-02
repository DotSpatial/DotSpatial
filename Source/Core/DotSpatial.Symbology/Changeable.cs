// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Changeable.
    /// </summary>
    public class Changeable
    {
        #region Fields

        private bool _changed;
        private bool _ignoreChanges;
        private int _suspendLevel;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when members are added to or removed from this collection. If SuspendChanges
        /// is called, this will temporarilly prevent this event from firing, until ResumeEvents
        /// has been called.
        /// </summary>
        public event EventHandler Changed;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether changes are suspended. To suspend events, call SuspendChanges. Then to resume events, call ResumeEvents. If the
        /// suspension is greater than 0, then events are suspended.
        /// </summary>
        public bool ChangesSuspended => _suspendLevel > 0;

        #endregion

        #region Methods

        /// <summary>
        /// Forces the Changed event to fire. If events are suspended,
        /// then this simply will mark the changes so that when
        /// the ResumeChanges is called it will automatically fire
        /// the Changed events.
        /// </summary>
        public virtual void ForceChange()
        {
        }

        /// <summary>
        /// Fires the Changed event as long as ChangesSuspended is not true.
        /// </summary>
        public virtual void OnChanged()
        {
            if (_ignoreChanges) return; // this prevents infinite loops by ignoring changes that were initiated by the OnChanged event

            if (ChangesSuspended)
            {
                _changed = true;
                return;
            }

            _ignoreChanges = true;
            Changed?.Invoke(this, EventArgs.Empty);
            _ignoreChanges = false;
        }

        /// <summary>
        /// Resumes the events. If any changes occured during the period of time when
        /// the events were suspended, this will automatically fire the chnaged event.
        /// </summary>
        public virtual void ResumeChanges()
        {
            _suspendLevel -= 1;
            if (ChangesSuspended == false)
            {
                if (_changed)
                {
                    OnChanged();
                }
            }

            if (_suspendLevel < 0) _suspendLevel = 0;
        }

        /// <summary>
        /// Causes this filter collection to suspend the Changed event, so that
        /// it will only be fired once after a series of updates.
        /// </summary>
        public virtual void SuspendChanges()
        {
            if (_suspendLevel == 0)
            {
                _changed = false;
            }

            _suspendLevel += 1;

            // using an integer allows many nested levels of suspension to exist,
            // resuming events only once all the nested resumes are called.
        }

        #endregion
    }
}