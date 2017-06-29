// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  The core libraries for the DotSpatial project.
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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/26/2008 10:04:01 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// Implementing this interface will allow the plugin developer to
    /// intercept the different log levels.
    /// </summary>
    public interface ILogger : IProgressHandler
    {
        /// <summary>
        /// Gets a string description for this logger.
        /// </summary>
        string Description
        {
            get;
        }

        /// <summary>
        /// Gets or Sets an integer key to keep track of this logger.
        /// When you add this logger to a LogManager, it will attempt to keep track of the
        /// logger by using the key it was given.  If that key is already in use, this
        /// will be set to the next available integer.
        /// </summary>
        int Key
        {
            get;
            set;
        }

        /// <summary>
        /// The Complete exception is passed here.  To get the stack
        /// trace, be sure to call ex.ToString().
        /// </summary>
        /// <param name="ex">The exception that was thrown by DotSpatial.</param>
        void Exception(Exception ex);

        /// <summary>
        /// This event will allow the registering of an entrance into a public Method of a "tools" related
        /// action to register its entrance into a function as well as logging the parameter names
        /// and a type specific indicator of their value.
        /// </summary>
        /// <param name="methodName">The string name of the method</param>
        /// <param name="parameters">The list of calling parameters</param>
        void PublicMethodEntered(string methodName, IEnumerable<string> parameters);

        /// <summary>
        /// This event will allow the registering of the exit from each public method
        /// </summary>
        /// <param name="methodName">The Method name of the method being left</param>
        void PublicMethodLeft(string methodName);

        /// <summary>
        /// A status message was sent.  Complex methods that have a few major steps will
        /// call a status message to show which step the process is in.  Loops will call
        /// the progress method instead.
        /// </summary>
        /// <param name="message">The string message that was posted.</param>
        void Status(string message);

        /// <summary>
        /// This method allows a user to recieve messages that were shown to the user, as well as
        /// their choice on those message boxes.
        /// </summary>
        /// <param name="messageText">A MessageBox after it has resolved.</param>
        /// <param name="result">A DialogResult from showing a message</param>
        void MessageBoxShown(string messageText, DialogResult result);

        /// <summary>
        /// This method allows the logger to recieve information about input boxes that were shown
        /// as well as the values enterred into them and the result.
        /// </summary>
        /// <param name="messageText">The string message that appeared on the InputBox</param>
        /// <param name="result">The ystem.Windows.Forms.DialogResult describing if the value was cancelled </param>
        /// <param name="value">The string containing the value entered.</param>
        void InputBoxShown(string messageText, DialogResult result, string value);
    }
}