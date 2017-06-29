// *******************************************************************************************************
// Product: DotSpatial.Analysis.FileLogger.cs
// Description: This is a default file logger in DotSpatial illustrating how to implement the interface for logging.
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// Ted Dunsford       |  8/2007            |  Initially written.  
//--------------------|--------------------|--------------------------------------------------------------
// Ted Dunsford       |  6/30/2010         |  Moved from MapWindow6.dll to DotSpatial.  
//--------------------|--------------------|--------------------------------------------------------------
// Dan Ames           |  3/2013            |  Adding this header.  
// *******************************************************************************************************

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
    /// </summary>
    public class FileLogger : Object, ILogger
    {
        private string _debugFile;
        private int _key;

        /// <summary>
        /// Gets or sets the string file to append debug messages to.
        /// </summary>
        public virtual string DebugFile
        {
            get { return _debugFile; }
            set { _debugFile = value; }
        }

        #region ILogger Members

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

        /// <summary>
        /// This is not really used because this saves data to a file.
        /// </summary>
        /// <param name="key">A basic string to help categorize the message, usually just the message with no percentage information.</param>
        /// <param name="percent">The string percent to appear in a progress message.</param>
        /// <param name="message">The string message combining both the key and the percent information.</param>
        public virtual void Progress(string key, int percent, string message)
        {
            // We don't actually want to save progress messages to the file
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
        /// This method allows the logger to receive information about input boxes that were shown
        /// as well as the values entered into them and the result.
        /// </summary>
        /// <param name="messageText">The string message that appeared on the input box.</param>
        /// <param name="result">The System.Windows.Forms.DialogResult describing if the value was cancelled.</param>
        /// <param name="value">The string containing the value entered.</param>
        public virtual void InputBoxShown(string messageText, DialogResult result, string value)
        {
            // This can be fleshed out later
            string message = "InputBox Shown: " + messageText + " and the user entered: " + value +
                             " and the result was " + result;
            Debug.WriteLine(message);
            if (_debugFile == null)
            {
                return;
            }
            TextWriter tw = new StreamWriter(_debugFile, true);
            tw.WriteLine(message);
        }

        /// <summary>
        /// Gets a description of this logger.
        /// </summary>
        public string Description
        {
            get
            {
                return
                    "This is a default file logger in DotSpatial illustrating how to implement the interface for logging.";
            }
        }

        /// <summary>
        /// Gets or sets the integer key that allows us to retrieve this logger from the Manager when we wish to remove it.
        /// </summary>
        public int Key
        {
            get { return _key; }
            set { _key = value; }
        }

        #endregion
    }
}