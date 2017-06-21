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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/24/2009 9:12:08 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IChangeable
    /// </summary>
    public interface IChangeable
    {
        #region Events

        /// <summary>
        /// Occurs when members are added to or removed from this collection.  If SuspendChanges
        /// is called, this will temporarilly prevent this event from firing, until ResumeEvents
        /// has been called.
        /// </summary>
        event EventHandler Changed;

        #endregion

        #region Methods

        /// <summary>
        /// Resumes the events.  If any changes occured during the period of time when
        /// the events were suspended, this will automatically fire the chnaged event.
        /// </summary>
        void ResumeChanges();

        /// <summary>
        /// Causes this filter collection to suspend the Changed event, so that
        /// it will only be fired once after a series of updates.
        /// </summary>
        void SuspendChanges();

        #endregion

        #region Properties

        /// <summary>
        /// To suspend events, call SuspendChanges.  Then to resume events, call ResumeEvents.  If the
        /// suspension is greater than 0, then events are suspended.
        /// </summary>
        bool ChangesSuspended
        {
            get;
        }

        #endregion
    }
}