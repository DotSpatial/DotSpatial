// CSharp Editor Example with Code Completion
// Copyright (c) 2006, Daniel Grunwald
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, are
// permitted provided that the following conditions are met:
//
// - Redistributions of source code must retain the above copyright notice, this list
//   of conditions and the following disclaimer.
//
// - Redistributions in binary form must reproduce the above copyright notice, this list
//   of conditions and the following disclaimer in the documentation and/or other materials
//   provided with the distribution.
//
// - Neither the name of the ICSharpCode team nor the names of its contributors may be used to
//   endorse or promote products derived from this software without specific prior written
//   permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS &AS IS& AND ANY EXPRESS
// OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY
// AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER
// IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT
// OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

namespace CSharpEditor
{
    /// <summary>
    /// CodeCompletionKeyHandler.
    /// </summary>
    internal class CodeCompletionKeyHandler
    {
        #region Fields

        private readonly TextEditorControl _editor;
        private readonly MainForm _mainForm;
        private CodeCompletionWindow _codeCompletionWindow;

        #endregion

        #region Constructors

        private CodeCompletionKeyHandler(MainForm mainForm, TextEditorControl editor)
        {
            _mainForm = mainForm;
            _editor = editor;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Attaches the events to the editor.
        /// </summary>
        /// <param name="mainForm">MainForm needed for creating the CodeCompletionKeyHandler.</param>
        /// <param name="editor">The editor the events get attached to.</param>
        /// <returns>The created CodeCompletionKeyHandler.</returns>
        public static CodeCompletionKeyHandler Attach(MainForm mainForm, TextEditorControl editor)
        {
            CodeCompletionKeyHandler h = new(mainForm, editor);

            editor.ActiveTextAreaControl.TextArea.KeyEventHandler += h.TextAreaKeyEventHandler;

            // When the editor is disposed, close the code completion window
            editor.Disposed += h.CloseCodeCompletionWindow;

            return h;
        }

        private void CloseCodeCompletionWindow(object sender, EventArgs e)
        {
            if (_codeCompletionWindow != null)
            {
                _codeCompletionWindow.Closed -= CloseCodeCompletionWindow;
                _codeCompletionWindow.Dispose();
                _codeCompletionWindow = null;
            }
        }

        /// <summary>
        /// Return true to handle the keypress, return false to let the text area handle the keypress.
        /// </summary>
        /// <param name="key">Key that gets checked.</param>
        /// <returns>True, if the keypress was handled.</returns>
        private bool TextAreaKeyEventHandler(char key)
        {
            if (_codeCompletionWindow != null)
            {
                // If completion window is open and wants to handle the key, don't let the text area
                // handle it
                if (_codeCompletionWindow.ProcessKeyEvent(key)) return true;
            }

            if (key == '.')
            {
                ICompletionDataProvider completionDataProvider = new CodeCompletionProvider(_mainForm);

                _codeCompletionWindow = CodeCompletionWindow.ShowCompletionWindow(
                    (Form)_mainForm.Parent.Parent.Parent, // The parent window for the completion window
                    _editor, // The text editor to show the window for
                    MainForm.DummyFileName, // Filename - will be passed back to the provider
                    completionDataProvider, // Provider to get the list of possible completions
                    key); // Key pressed - will be passed to the provider

                if (_codeCompletionWindow != null)
                {
                    // ShowCompletionWindow can return null when the provider returns an empty list
                    _codeCompletionWindow.Closed += CloseCodeCompletionWindow;
                }
            }

            return false;
        }

        #endregion
    }
}