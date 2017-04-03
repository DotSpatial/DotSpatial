// ********************************************************************************************************
// Product Name: DotSpatial.Interfaces Alpha
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
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
        /// The message allowing someone to decide whether or not the process should be canceled.  For instance,
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