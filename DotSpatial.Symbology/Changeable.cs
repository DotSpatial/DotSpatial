// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/24/2009 9:10:58 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology
{
    public class Changeable
    {
        #region Events

        /// <summary>
        /// Occurs when members are added to or removed from this collection.  If SuspendChanges
        /// is called, this will temporarilly prevent this event from firing, until ResumeEvents
        /// has been called.
        /// </summary>
        public event EventHandler Changed;

        #endregion

        #region Private Variables

        private bool _changed;
        private bool _ignoreChanges;
        private int _suspendLevel;

        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// Forces the Changed event to fire.  If events are suspended,
        /// then this simply will mark the changes so that when
        /// the ResumeChanges is called it will automatically fire
        /// the Changed events.
        /// </summary>
        public virtual void ForceChange()
        {
        }

        /// <summary>
        /// Resumes the events.  If any changes occured during the period of time when
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

        #region Properties

        /// <summary>
        /// To suspend events, call SuspendChanges.  Then to resume events, call ResumeEvents.  If the
        /// suspension is greater than 0, then events are suspended.
        /// </summary>
        public bool ChangesSuspended
        {
            get { return (_suspendLevel > 0); }
        }

        #endregion

        #region Protected Methods

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
            if (Changed != null) Changed(this, EventArgs.Empty);
            _ignoreChanges = false;
        }

        #endregion
    }
}