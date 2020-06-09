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
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.CSharp;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;
using ICSharpCode.SharpDevelop.Dom.VBNet;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

namespace CSharpEditor
{
    /// <summary>
    /// CodeCompletionProvider.
    /// </summary>
    internal class CodeCompletionProvider : ICompletionDataProvider
    {
        #region Fields

        private readonly MainForm _mainForm;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCompletionProvider"/> class.
        /// </summary>
        /// <param name="mainForm">The main form.</param>
        public CodeCompletionProvider(MainForm mainForm)
        {
            _mainForm = mainForm;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the default index.
        /// </summary>
        public int DefaultIndex => -1;

        /// <summary>
        /// Gets the image list.
        /// </summary>
        public ImageList ImageList => _mainForm.imageList1;

        /// <summary>
        /// Gets the preselection.
        /// </summary>
        public string PreSelection => null;

        #endregion

        #region Methods

        /// <summary>
        /// Generators completion data.
        /// </summary>
        /// <param name="fileName">not used.</param>
        /// <param name="textArea">The text area.</param>
        /// <param name="charTyped">The character type.</param>
        /// <returns>The completion data.</returns>
        public ICompletionData[] GenerateCompletionData(string fileName, TextArea textArea, char charTyped)
        {
            // We can return code-completion items like this:
            // return new ICompletionData[] {new DefaultCompletionData("Text", "Description", 1)};
            NRefactoryResolver resolver = new NRefactoryResolver(_mainForm.MyProjectContent.Language);
            ResolveResult rr = resolver.Resolve(FindExpression(textArea), _mainForm.ParseInformation, textArea.MotherTextEditorControl.Text);
            List<ICompletionData> resultList = new List<ICompletionData>();

            ArrayList completionData = rr?.GetCompletionData(_mainForm.MyProjectContent);
            if (completionData != null)
            {
                AddCompletionData(resultList, completionData);
            }

            return resultList.ToArray();
        }

        /// <summary>
        /// Called when entry should be inserted. Forward to the insertion action of the completion data.
        /// </summary>
        /// <param name="data">The completion data.</param>
        /// <param name="textArea">The text area.</param>
        /// <param name="insertionOffset">The insertion offset.</param>
        /// <param name="key">The key.</param>
        /// <returns>Boolean.</returns>
        public bool InsertAction(ICompletionData data, TextArea textArea, int insertionOffset, char key)
        {
            textArea.Caret.Position = textArea.Document.OffsetToPosition(insertionOffset);
            return data.InsertAction(textArea, key);
        }

        /// <summary>
        /// Processes the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The CompletionDataProviderKeyResult.</returns>
        public CompletionDataProviderKeyResult ProcessKey(char key)
        {
            if (char.IsLetterOrDigit(key) || key == '_')
            {
                return CompletionDataProviderKeyResult.NormalKey;
            }

            // key triggers insertion of selected items
            return CompletionDataProviderKeyResult.InsertionKey;
        }

        private void AddCompletionData(List<ICompletionData> resultList, ArrayList completionData)
        {
            // used to store the method names for grouping overloads
            Dictionary<string, CodeCompletionData> nameDictionary = new Dictionary<string, CodeCompletionData>();

            // Add the completion data as returned by SharpDevelop.Dom to the
            // list for the text editor
            foreach (object obj in completionData)
            {
                var s = obj as string;
                if (s != null)
                {
                    // namespace names are returned as string
                    resultList.Add(new DefaultCompletionData(s, "namespace " + obj, 5));
                }
                else if (obj is IClass)
                {
                    IClass c = (IClass)obj;
                    resultList.Add(new CodeCompletionData(c));
                }
                else if (obj is IMember)
                {
                    IMember m = (IMember)obj;
                    if (m is IMethod && (m as IMethod).IsConstructor)
                    {
                        // Skip constructors
                        continue;
                    }

                    // Group results by name and add "(x Overloads)" to the
                    // description if there are multiple results with the same name.
                    CodeCompletionData data;
                    if (nameDictionary.TryGetValue(m.Name, out data))
                    {
                        data.AddOverload();
                    }
                    else
                    {
                        nameDictionary[m.Name] = data = new CodeCompletionData(m);
                        resultList.Add(data);
                    }
                }
                else
                {
                    // Current ICSharpCode.SharpDevelop.Dom should never return anything else
                    throw new NotSupportedException();
                }
            }
        }

        /// <summary>
        /// Find the expression the cursor is at.
        /// Also determines the context (using statement, "new"-expression etc.) the cursor is at.
        /// </summary>
        /// <param name="textArea">Textarea that gets searched.</param>
        /// <returns>The expression that was found.</returns>
        private ExpressionResult FindExpression(TextArea textArea)
        {
            IExpressionFinder finder;
            if (MainForm.IsVisualBasic)
            {
                finder = new VBExpressionFinder();
            }
            else
            {
                finder = new CSharpExpressionFinder(_mainForm.ParseInformation);
            }

            ExpressionResult expression = finder.FindExpression(textArea.Document.TextContent, textArea.Caret.Offset);
            if (expression.Region.IsEmpty)
            {
                expression.Region = new DomRegion(textArea.Caret.Line + 1, textArea.Caret.Column + 1);
            }

            return expression;
        }

        #endregion
    }
}