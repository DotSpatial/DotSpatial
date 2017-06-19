// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// This component allows customization of how log messages are sent.
    /// </summary>
    public class LogManager : ILogManager
    {
        #region Fields

        // the actual collection of ILoggers
        private readonly IDictionary<int, ILogger> _loggers;

        // increments so that each addition increments the active key.
        private int _currentKey;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogManager"/> class.
        /// </summary>
        public LogManager()
        {
            InitializeComponent();
            _loggers = new Dictionary<int, ILogger>();
            _currentKey = 0;
            DefaultLogManager = this;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the default log manager. This ensures that there will always be some kind of log manager.
        /// When a new LogManager is created, this static is set to be that instance.
        /// Controlling the DefaultLogManager will control which log manager
        /// is actively in use.
        /// </summary>
        public static ILogManager DefaultLogManager { get; private set; } = new LogManager();

        /// <summary>
        /// Gets a required designer variable.
        /// </summary>
        public IContainer Components { get; private set; }

        /// <summary>
        /// Gets or sets the list of string directories that may contain dlls with ILogManagers.
        /// </summary>
        public List<string> Directories { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// To begin logging, create an implementation of the ILogHandler interface, or use the DefaultLogger class that is already implemented in this project.
        /// Then, call this function to add that logger to the list of active loggers.
        /// This function will return an integer key that you can use to keep track of your specific logger.
        /// </summary>
        /// <param name="logger">The logger that should be added to the list of active loggers.</param>
        /// <returns>Integer key of this logger.</returns>
        public int AddLogger(ILogger logger)
        {
            // The current key will keep going up even if loggers are removed, just so we don't get into trouble with redundancies.
            _loggers.Add(_currentKey, logger);
            _currentKey++;
            return _currentKey - 1;
        }

        /// <summary>
        /// Adds all the loggers from directories.
        /// </summary>
        /// <returns>A list of added loggers.</returns>
        public List<ILogger> AddLoggersFromDirectories()
        {
            return null;

            // TO DO: treat this like DataManager does
        }

        /// <summary>
        /// The Complete exception is passed here. To get the stack
        /// trace, be sure to call ex.ToString().
        /// </summary>
        /// <param name="ex">The exception that was thrown by DotSpatial.</param>
        public void Exception(Exception ex)
        {
            foreach (KeyValuePair<int, ILogger> logger in _loggers)
            {
                logger.Value.Exception(ex);
            }
        }

        /// <summary>
        /// This method echoes information about input boxes to all the loggers.
        /// </summary>
        /// <param name="text">The string message that appeared on the InputBox</param>
        /// <param name="result">The ystem.Windows.Forms.DialogResult describing if the value was cancelled </param>
        /// <param name="value">The string containing the value entered.</param>
        public void LogInput(string text, DialogResult result, string value)
        {
            foreach (KeyValuePair<int, ILogger> logger in _loggers)
            {
                logger.Value.InputBoxShown(text, result, value);
            }
        }

        /// <summary>
        /// Displays an InputBox form given the specified text string. The result is returned byref.
        /// A DialogResult is returned to show whether the user cancelled the form without providing input.
        /// </summary>
        /// <param name="text">The string text to use as an input prompt.</param>
        /// <param name="result">The string result that was typed into the dialog.</param>
        /// <returns>A DialogResult showing the outcome.</returns>
        public DialogResult LogInputBox(string text, out string result)
        {
            InputBox frm = new InputBox(text);
            result = frm.ShowDialog() != DialogResult.OK ? string.Empty : frm.Result;
            LogInput(text, frm.DialogResult, result);
            return frm.DialogResult;
        }

        /// <summary>
        /// Displays an InputBox form given the specified text string. The result is returned byref.
        /// A DialogResult is returned to show whether the user cancelled the form without providing input.
        /// </summary>
        /// <param name="text">The string text to use as an input prompt.</param>
        /// <param name="caption">The string to use in the title bar of the InputBox.</param>
        /// <param name="result">The string result that was typed into the dialog.</param>
        /// <returns>A DialogResult showing the outcome.</returns>
        public DialogResult LogInputBox(string text, string caption, out string result)
        {
            InputBox frm = new InputBox(text, caption);
            result = frm.ShowDialog() != DialogResult.OK ? string.Empty : frm.Result;
            LogInput(text, frm.DialogResult, result);
            return frm.DialogResult;
        }

        /// <summary>
        /// Displays an InputBox form given the specified text string. The result is returned byref.
        /// A DialogResult is returned to show whether the user cancelled the form without providing input.
        /// </summary>
        /// <param name="text">The string text to use as an input prompt.</param>
        /// <param name="caption">The string to use in the title bar of the InputBox.</param>
        /// <param name="validation">A DotSpatial.Data.ValidationType enumeration specifying acceptable validation to return OK.</param>
        /// <param name="result">The string result that was typed into the dialog.</param>
        /// <returns>A DialogResult showing the outcome.</returns>
        public DialogResult LogInputBox(string text, string caption, ValidationType validation, out string result)
        {
            InputBox frm = new InputBox(text, caption, validation);
            result = frm.ShowDialog() != DialogResult.OK ? string.Empty : frm.Result;
            LogInput(text, frm.DialogResult, result);
            return frm.DialogResult;
        }

        /// <summary>
        /// Displays an InputBox form given the specified text string. The result is returned byref.
        /// A DialogResult is returned to show whether the user cancelled the form without providing input.
        /// </summary>
        /// <param name="text">The string text to use as an input prompt.</param>
        /// <param name="caption">The string to use in the title bar of the InputBox.</param>
        /// <param name="validation">A DotSpatial.Data.ValidationType enumeration specifying acceptable validation to return OK.</param>
        /// <param name="icon">Specifies an icon to display on this form.</param>
        /// <param name="result">The string result that was typed into the dialog.</param>
        /// <returns>A DialogResult showing the outcome.</returns>
        public DialogResult LogInputBox(string text, string caption, ValidationType validation, Icon icon, out string result)
        {
            InputBox frm = new InputBox(text, caption, validation, icon);
            result = frm.ShowDialog() != DialogResult.OK ? string.Empty : frm.Result;
            LogInput(text, frm.DialogResult, result);
            return frm.DialogResult;
        }

        /// <summary>
        /// Displays an InputBox form given the specified text string. The result is returned byref.
        /// A DialogResult is returned to show whether the user cancelled the form without providing input.
        /// </summary>
        /// <param name="owner">The window that owns this modal dialog.</param>
        /// <param name="text">The string text to use as an input prompt.</param>
        /// <param name="result">The string result that was typed into the dialog.</param>
        /// <returns>A DialogResult showing the outcome.</returns>
        public DialogResult LogInputBox(Form owner, string text, out string result)
        {
            InputBox frm = new InputBox(owner, text);
            result = frm.ShowDialog() != DialogResult.OK ? string.Empty : frm.Result;
            LogInput(text, frm.DialogResult, result);
            return frm.DialogResult;
        }

        /// <summary>
        /// Displays an InputBox form given the specified text string. The result is returned byref.
        /// A DialogResult is returned to show whether the user cancelled the form without providing input.
        /// </summary>
        /// <param name="owner">The window that owns this modal dialog.</param>
        /// <param name="text">The string text to use as an input prompt.</param>
        /// <param name="caption">The string to use in the title bar of the InputBox.</param>
        /// <param name="result">The string result that was typed into the dialog.</param>
        /// <returns>A DialogResult showing the outcome.</returns>
        public DialogResult LogInputBox(Form owner, string text, string caption, out string result)
        {
            InputBox frm = new InputBox(owner, text, caption);
            result = frm.ShowDialog() != DialogResult.OK ? string.Empty : frm.Result;
            LogInput(text, frm.DialogResult, result);
            return frm.DialogResult;
        }

        /// <summary>
        /// Displays an InputBox form given the specified text string. The result is returned byref.
        /// A DialogResult is returned to show whether the user cancelled the form without providing input.
        /// </summary>
        /// <param name="owner">The window that owns this modal dialog.</param>
        /// <param name="text">The string text to use as an input prompt.</param>
        /// <param name="caption">The string to use in the title bar of the InputBox.</param>
        /// <param name="validation">A DotSpatial.Data.ValidationType enumeration specifying acceptable validation to return OK.</param>
        /// <param name="result">The string result that was typed into the dialog.</param>
        /// <returns>A DialogResult showing the outcome.</returns>
        public DialogResult LogInputBox(Form owner, string text, string caption, ValidationType validation, out string result)
        {
            InputBox frm = new InputBox(owner, text, caption, validation);
            result = frm.ShowDialog() != DialogResult.OK ? string.Empty : frm.Result;
            LogInput(text, frm.DialogResult, result);
            return frm.DialogResult;
        }

        /// <summary>
        /// Displays an InputBox form given the specified text string. The result is returned byref.
        /// A DialogResult is returned to show whether the user cancelled the form without providing input.
        /// </summary>
        /// <param name="owner">The window that owns this modal dialog.</param>
        /// <param name="text">The string text to use as an input prompt.</param>
        /// <param name="caption">The string to use in the title bar of the InputBox.</param>
        /// <param name="validation">A DotSpatial.Data.ValidationType enumeration specifying acceptable validation to return OK.</param>
        /// <param name="icon">Specifies an icon to display on this form.</param>
        /// <param name="result">The string result that was typed into the dialog.</param>
        /// <returns>A DialogResult showing the outcome.</returns>
        public DialogResult LogInputBox(Form owner, string text, string caption, ValidationType validation, Icon icon, out string result)
        {
            InputBox frm = new InputBox(owner, text, caption, validation, icon);
            result = frm.ShowDialog() != DialogResult.OK ? string.Empty : frm.Result;
            LogInput(text, frm.DialogResult, result);
            return frm.DialogResult;
        }

        /// <summary>
        /// This is called by each of the LogMessageBox methods automatically, but if the user wants to use
        /// a custom messagebox and then log the message and result directly this is the technique.
        /// </summary>
        /// <param name="text">The string text of the message that needs to be logged.</param>
        /// <param name="result">The dialog result from the shown messagebox.</param>
        public void LogMessage(string text, DialogResult result)
        {
            foreach (KeyValuePair<int, ILogger> logger in _loggers)
            {
                logger.Value.MessageBoxShown(text, result);
            }
        }

        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="owner">An implementation of the IWin32Window that will own the modal form dialog box.</param>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <returns>A DialogResult showing the user input from this messagebox.</returns>
        public DialogResult LogMessageBox(IWin32Window owner, string text)
        {
            DialogResult res = MessageBox.Show(owner, text);
            LogMessage(text, res);

            return res;
        }

        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="owner">An implementation of the IWin32Window that will own the modal form dialog box.</param>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <returns>A DialogResult showing the user input from this messagebox.</returns>
        public DialogResult LogMessageBox(IWin32Window owner, string text, string caption)
        {
            DialogResult res = MessageBox.Show(owner, text, caption);
            LogMessage(text, res);

            return res;
        }

        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="owner">An implementation of the IWin32Window that will own the modal form dialog box.</param>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <param name="buttons">One of the MessageBoxButtons that describes which button to display in the MessageBox</param>
        /// <returns>A DialogResult showing the user input from this messagebox.</returns>
        public DialogResult LogMessageBox(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            DialogResult res = MessageBox.Show(owner, text, caption, buttons);
            LogMessage(text, res);

            return res;
        }

        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="owner">An implementation of the IWin32Window that will own the modal form dialog box.</param>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <param name="buttons">One of the MessageBoxButtons that describes which button to display in the MessageBox</param>
        /// <param name="icon">One of the MessageBoxIcons that describes which icon to display in the MessageBox</param>
        /// <returns>A DialogResult showing the user input from this messagebox.</returns>
        public DialogResult LogMessageBox(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            DialogResult res = MessageBox.Show(owner, text, caption, buttons, icon);
            LogMessage(text, res);

            return res;
        }

        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="owner">An implementation of the IWin32Window that will own the modal form dialog box.</param>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <param name="buttons">One of the MessageBoxButtons that describes which button to display in the MessageBox</param>
        /// <param name="icon">One of the MessageBoxIcons that describes which icon to display in the MessageBox</param>
        /// <param name="defaultButton">One of the MessageBoxDefaultButtons that describes the default button for the MessageBox</param>
        /// <returns>A DialogResult showing the user input from this messagebox.</returns>
        public DialogResult LogMessageBox(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            DialogResult res = MessageBox.Show(owner, text, caption, buttons, icon, defaultButton);
            LogMessage(text, res);
            return res;
        }

        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="owner">An implementation of the IWin32Window that will own the modal form dialog box.</param>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <param name="buttons">One of the MessageBoxButtons that describes which button to display in the MessageBox</param>
        /// <param name="icon">One of the MessageBoxIcons that describes which icon to display in the MessageBox</param>
        /// <param name="defaultButton">One of the MessageBoxDefaultButtons that describes the default button for the MessageBox</param>
        /// <param name="options">One of the MessageBoxOptions that describes which display and association options to use for the MessageBox. You may pass 0 if you wish to use the defaults.</param>
        /// <returns>A DialogResult showing the user input from this messagebox.</returns>
        public DialogResult LogMessageBox(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            DialogResult res = MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options);
            LogMessage(text, res);
            return res;
        }

        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <returns>A DialogResult showing the user input from this messagebox.</returns>
        public DialogResult LogMessageBox(string text)
        {
            DialogResult res = MessageBox.Show(text);
            LogMessage(text, res);

            return res;
        }

        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <returns>A DialogResult showing the user input from this messagebox.</returns>
        public DialogResult LogMessageBox(string text, string caption)
        {
            DialogResult res = MessageBox.Show(text, caption);
            LogMessage(text, res);

            return res;
        }

        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <param name="buttons">One of the MessageBoxButtons that describes which button to display in the MessageBox</param>
        /// <returns>A DialogResult showing the user input from this messagebox.</returns>
        public DialogResult LogMessageBox(string text, string caption, MessageBoxButtons buttons)
        {
            DialogResult res = MessageBox.Show(text, caption, buttons);
            LogMessage(text, res);

            return res;
        }

        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <param name="buttons">One of the MessageBoxButtons that describes which button to display in the MessageBox</param>
        /// <param name="icon">One of the MessageBoxIcons that describes which icon to display in the MessageBox</param>
        /// <returns>A DialogResult showing the user input from this messagebox.</returns>
        public DialogResult LogMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            DialogResult res = MessageBox.Show(text, caption, buttons, icon);
            LogMessage(text, res);

            return res;
        }

        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <param name="buttons">One of the MessageBoxButtons that describes which button to display in the MessageBox</param>
        /// <param name="icon">One of the MessageBoxIcons that describes which icon to display in the MessageBox</param>
        /// <param name="defaultButton">One of the MessageBoxDefaultButtons that describes the default button for the MessageBox</param>
        /// <returns>A DialogResult showing the user input from this messagebox.</returns>
        public DialogResult LogMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            DialogResult res = MessageBox.Show(text, caption, buttons, icon, defaultButton);
            LogMessage(text, res);
            return res;
        }

        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <param name="buttons">One of the MessageBoxButtons that describes which button to display in the MessageBox</param>
        /// <param name="icon">One of the MessageBoxIcons that describes which icon to display in the MessageBox</param>
        /// <param name="defaultButton">One of the MessageBoxDefaultButtons that describes the default button for the MessageBox</param>
        /// <param name="options">One of the MessageBoxOptions that describes which display and association options to use for the MessageBox. You may pass 0 if you wish to use the defaults.</param>
        /// <returns>A DialogResult showing the user input from this messagebox.</returns>
        public DialogResult LogMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            DialogResult res = MessageBox.Show(text, caption, buttons, icon, defaultButton, options);
            LogMessage(text, res);
            return res;
        }

        /// <summary>
        /// Shows a MessageBox, logs the text of the text and the result chosen by the user.
        /// </summary>
        /// <param name="text">The text to display in the MessageBox</param>
        /// <param name="caption">The text to display in the title bar of the MessageBox</param>
        /// <param name="buttons">One of the MessageBoxButtons that describes which button to display in the MessageBox</param>
        /// <param name="icon">One of the MessageBoxIcons that describes which icon to display in the MessageBox</param>
        /// <param name="defaultButton">One of the MessageBoxDefaultButtons that describes the default button for the MessageBox</param>
        /// <param name="options">One of the MessageBoxOptions that describes which display and association options to use for the MessageBox. You may pass 0 if you wish to use the defaults.</param>
        /// <param name="displayHelpButton">A boolean indicating whether or not to display a help button on the messagebox</param>
        /// <returns>A DialogResult showing the user input from this messagebox.</returns>
        public DialogResult LogMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, bool displayHelpButton)
        {
            DialogResult res = MessageBox.Show(text, caption, buttons, icon, defaultButton, options, displayHelpButton);
            LogMessage(text, res);
            return res;
        }

        /// <summary>
        /// A progress message, generally as part of a long loop was sent. It is a bad
        /// idea to log these to a file as there may be thousands of them.
        /// </summary>
        /// <param name="baseMessage">The status part of the progress message with no percent information</param>
        /// <param name="percent">The integer percent from 0 to 100</param>
        /// <param name="message">The complete message, showing both status and completion percent</param>
        public void Progress(string baseMessage, int percent, string message)
        {
            foreach (KeyValuePair<int, ILogger> logger in _loggers)
            {
                logger.Value.Progress(baseMessage, percent, message);
            }
        }

        /// <summary>
        /// This event will allow the registering of an entrance into a public Method of a "tools" related
        /// action to register its entrance into a function as well as logging the parameter names
        /// and a type specific indicator of their value.
        /// </summary>
        /// <param name="methodName">The string name of the method</param>
        /// <param name="parameters">The List&lt;string&gt; of Parameter names and string form values</param>
        public void PublicMethodEntered(string methodName, List<string> parameters)
        {
            foreach (KeyValuePair<int, ILogger> logger in _loggers)
            {
                logger.Value.PublicMethodEntered(methodName, parameters);
            }
        }

        /// <summary>
        /// This event will allow the registering of the exit from each public method
        /// </summary>
        /// <param name="methodName">The Method name of the method being left</param>
        public void PublicMethodLeft(string methodName)
        {
            foreach (KeyValuePair<int, ILogger> logger in _loggers)
            {
                logger.Value.PublicMethodLeft(methodName);
            }
        }

        /// <summary>
        /// The key specified here is the key that was returned by the AddLogger method.
        /// </summary>
        /// <param name="key">The integer key of the logger to remove.</param>
        /// <returns>True if the logger was successfully removed, or false if the key could not be found</returns>
        /// <exception cref="System.ArgumentNullException">key is null</exception>
        public bool RemoveLogger(int key)
        {
            return _loggers.Remove(key);
        }

        /// <summary>
        /// A status message was sent. Complex methods that have a few major steps will
        /// call a status message to show which step the process is in. Loops will call the progress method instead.
        /// </summary>
        /// <param name="message">The string message that was posted.</param>
        public void Status(string message)
        {
            foreach (KeyValuePair<int, ILogger> logger in _loggers)
            {
                logger.Value.Status(message);
            }
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Components = new Container();
        }

        #endregion
    }
}