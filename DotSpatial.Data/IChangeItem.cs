// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/8/2008 8:09:59 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// IChangeItem
    /// </summary>
    public interface IChangeItem
    {
        /// <summary>
        /// Occurs when internal properties or characteristics of this member change.
        /// The member should send itself as the sender of the event.
        /// </summary>
        event EventHandler ItemChanged;

        /// <summary>
        /// An instruction has been sent to remove the specified item from its container.
        /// </summary>
        event EventHandler RemoveItem;
    }
}