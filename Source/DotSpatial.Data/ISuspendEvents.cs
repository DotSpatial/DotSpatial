// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/24/2009 12:34:51 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    public interface ISuspendEvents : ICloneable
    {
        /// <summary>
        /// Gets whether or not the list is currently suspended
        /// </summary>
        bool EventsSuspended
        {
            get;
        }

        /// <summary>
        /// Resumes event sending and fires a ListChanged event if any changes have taken place.
        /// This will not track all the individual changes that may have fired in the meantime.
        /// </summary>
        void ResumeEvents();

        /// <summary>
        /// Temporarilly suspends notice events, allowing a large number of changes.
        /// </summary>
        void SuspendEvents();
    }
}