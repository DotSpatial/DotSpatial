// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// A set of MessageCancelEventArgs that can be used in order to cancel an event.
    /// </summary>
    public class MessageCancelEventArgs : EventArgs
    {
        #region Fields

        private bool _cancel; // decides to cancel something
        private string _message; // a message of what is happening.

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageCancelEventArgs"/> class.
        /// </summary>
        /// <param name="message">A string message to convey with this event.</param>
        public MessageCancelEventArgs(string message)
        {
            _cancel = false;
            _message = message;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the action that caused this event should be canceled.
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
        /// Gets or sets the message allowing someone to decide whether or not the process should be canceled. For instance,
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

        #endregion
    }
}