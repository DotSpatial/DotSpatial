// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using DotSpatial.Data.Forms;

namespace DotSpatial.Analysis
{
    /// <summary>
    /// Initializes a new instance of the FileLogger class which is an implementation of ILogger designed to save content to a file.
    /// This is a default file logger in DotSpatial illustrating how to implement the interface for logging.
    /// </summary>
    public class FileLogger : object, ILogger
    {
        #region Fields

        private string _debugFile;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the string file to append debug messages to.
        /// </summary>
        public virtual string DebugFile
        {
            get
            {
                return _debugFile;
            }

            set
            {
                _debugFile = value;
            }
        }

        /// <summary>
        /// Gets a description of this logger.
        /// </summary>
        public string Description => "This is a default file logger in DotSpatial illustrating how to implement the interface for logging.";

        /// <summary>
        /// Gets or sets the integer key that allows us to retrieve this logger from the Manager when we wish to remove it.
        /// </summary>
        public int Key { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// An exception was thrown, so this will post the stack trace and message to debug.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        public virtual void Exception(Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            if (_debugFile != null)
            {
                TextWriter tw = new StreamWriter(_debugFile, true);
                tw.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// This method allows the logger to receive information about input boxes that were shown
        /// as well as the values entered into them and the result.
        /// </summary>
        /// <param name="messageText">The string message that appeared on the input box.</param>
        /// <param name="result">The System.Windows.Forms.DialogResult describing if the value was canceled.</param>
        /// <param name="value">The string containing the value entered.</param>
        public virtual void InputBoxShown(string messageText, DialogResult result, string value)
        {
            // This can be fleshed out later
            string message = "InputBox Shown: " + messageText + " and the user entered: " + value + " and the result was " + result;
            Debug.WriteLine(message);
            if (_debugFile == null)
            {
                return;
            }

            TextWriter tw = new StreamWriter(_debugFile, true);
            tw.WriteLine(message);
        }

        /// <summary>
        /// Handles the situation where a simple message box where only a message was specified
        /// was shown to the user.  It also shows the result that the user pressed.
        /// </summary>
        /// <param name="messageText">The message text.</param>
        /// <param name="result">The boolean result.</param>
        public virtual void MessageBoxShown(string messageText, DialogResult result)
        {
            // This can be fleshed out later
            string message = "Message box shown: " + messageText + " and the user chose: " + result;
            Debug.WriteLine(message);
            if (_debugFile == null)
            {
                return;
            }

            TextWriter tw = new StreamWriter(_debugFile, true);
            tw.WriteLine(message);
        }

        /// <summary>
        /// This is not really used because this saves data to a file.
        /// </summary>
        /// <param name="percent">The string percent to appear in a progress message.</param>
        /// <param name="message">The string message including the percent information if wanted.</param>
        public virtual void Progress(int percent, string message)
        {
            // We don't actually want to save progress messages to the file
        }

        /// <summary>
        /// This is not really used because progress isn't used either.
        /// </summary>
        public virtual void Reset()
        {
        }

        /// <summary>
        /// This handles the situation where a public method has been entered.
        /// </summary>
        /// <param name="methodName">The method name.</param>
        /// <param name="parameters">The list of parameters.</param>
        public virtual void PublicMethodEntered(string methodName, IEnumerable<string> parameters)
        {
            string message = "Entering: " + methodName + "(\n";
            foreach (string parameter in parameters)
            {
                message += "     " + parameter + "\n";
            }

            message += ") at " + DateTime.Now.ToShortDateString() + ": " + DateTime.Now.ToShortTimeString();
            Debug.WriteLine(message);
            if (_debugFile != null)
            {
                TextWriter tw = new StreamWriter(_debugFile, true);
                tw.WriteLine(message);
            }
        }

        /// <summary>
        /// This handles the situation where a public method has been left.
        /// </summary>
        /// <param name="methodName">The method name of the function being left.</param>
        public virtual void PublicMethodLeft(string methodName)
        {
            string message = "Leaving: " + methodName;
            Debug.WriteLine(message);
            if (_debugFile != null)
            {
                TextWriter tw = new StreamWriter(_debugFile, true);
                tw.WriteLine(message);
            }
        }
        
        /// <summary>
        /// Handles the situation where a status message has been posted.
        /// </summary>
        /// <param name="message">The status message text.</param>
        public virtual void Status(string message)
        {
            string msg = "Status: " + message;
            Debug.WriteLine(msg);
            if (_debugFile == null)
            {
                return;
            }

            TextWriter tw = new StreamWriter(_debugFile, true);
            tw.WriteLine(msg);
        }

        #endregion
    }
}