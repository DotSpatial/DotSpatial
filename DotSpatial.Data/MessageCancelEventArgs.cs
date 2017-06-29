// ********************************************************************************************************
// Product Name: DotSpatial.Interfaces Alpha
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in February 2008
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// A set of PaintEventArgs that can be used before a drawing function in order to cancel an event.
    /// </summary>
    public class MessageCancelEventArgs : EventArgs
    {
        private bool _cancel; // decides to cancel something
        private string _message; // a message of what is happening.

        /// <summary>
        /// Creates a new instance of the MessageCancel Event Arguments
        /// </summary>
        /// <param name="message">A string message to convey with this event.</param>
        public MessageCancelEventArgs(string message)
        {
            _cancel = false;
            _message = message;
        }

        /// <summary>
        /// Returns a boolean specifying whether the action that caused this event should be canceled.
        /// </summary>
        public virtual bool Cancel
        {
            get
            {
                return _cancel;
            }
            set
            {
                _cancel = value;
            }
        }

        /// <summary>
        /// The message allowing someone to decide whether or not the process should be cancelled.  For instance,
        /// when writing a new file, a message might show "The file C:\bob.txt already exists, overwrite it?"
        /// </summary>
        public virtual string Message
        {
            get
            {
                return _message;
            }
            protected set
            {
                _message = value;
            }
        }
    }
}