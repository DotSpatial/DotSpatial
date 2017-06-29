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
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;

namespace CSharpEditor
{
    public partial class MainForm : UserControl
    {
        /// <summary>
        /// Many SharpDevelop.Dom methods take a file name, which is really just a unique identifier
        /// for a file - Dom methods don't try to access code files on disk, so the file does not have
        /// to exist.
        /// SharpDevelop itself uses internal names of the kind "[randomId]/Class1.cs" to support
        /// code-completion in unsaved files.
        /// </summary>
        public const string DummyFileName = "edited.cs";

        private static string gErrorMsg = "";

        /// <summary>
        ///
        /// </summary>
        public static bool IsVisualBasic;

        static readonly LanguageProperties CurrentLanguageProperties = IsVisualBasic ? LanguageProperties.VBNet : LanguageProperties.CSharp;

        /// <summary>
        ///
        /// </summary>
        public string Language = "VBNET";

        /// <summary>
        ///
        /// </summary>
        public ICompilationUnit lastCompilationUnit;

        /// <summary>
        ///
        /// </summary>
        public DefaultProjectContent myProjectContent;
        /// <summary>
        ///
        /// </summary>
        public ParseInformation parseInformation = new ParseInformation();

        Thread parserThread;

        /// <summary>
        ///
        /// </summary>
        public ProjectContentRegistry pcRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //            if (IsVisualBasic) {
            //                textEditorControl1.Text = @"
            //Class A
            // Sub B
            //  Dim xx As String
            //
            // End Sub
            //End Class
            //";
            //                textEditorControl1.SetHighlighting("VBNET");
            //            } else {
            //                textEditorControl1.Text = @"using System;
            //class A
            //{
            // void B()
            // {
            //  string x;
            //
            // }
            //}
            //";
            //                textEditorControl1.SetHighlighting("C#");
            //            }
            textEditorControl1.ShowEOLMarkers = false;
            textEditorControl1.ShowInvalidLines = false;

            HostCallbackImplementation.Register(this);
            CodeCompletionKeyHandler.Attach(this, textEditorControl1);
            ToolTipProvider.Attach(this, textEditorControl1);

            pcRegistry = new ProjectContentRegistry(); // Default .NET 2.0 registry

            // Persistence lets SharpDevelop.Dom create a cache file on disk so that
            // future starts are faster.
            // It also caches XML documentation files in an on-disk hash table, thus
            // reducing memory usage.
            // Paul Meems, 1 Sept. 2010: Create the file if it doesn't exist (Bug #1763):
            string cacheFolder = Path.Combine(Path.GetTempPath(), "CSharpCodeCompletion");
            if (!Directory.Exists(cacheFolder))
            {
                Directory.CreateDirectory(cacheFolder);
            }
            pcRegistry.ActivatePersistence(cacheFolder);

            myProjectContent = new DefaultProjectContent();
            myProjectContent.Language = CurrentLanguageProperties;
        }

        /// <summary>
        /// </summary>
        /// <returns>The text associated with this control.</returns>
        public override string Text
        {
            get
            {
                return textEditorControl1.Text;
            }
            set
            {
                textEditorControl1.Text = value;
            }
        }

        /// <summary>
        /// Inits this instance.
        /// </summary>
        public void Init()
        {
            parserThread = new Thread(ParserThread);
            parserThread.IsBackground = true;
            parserThread.Start();
        }

        /// <summary>
        /// Shutdowns this instance.
        /// </summary>
        public void Shutdown()
        {
            if (parserThread != null) parserThread.Abort();
        }

        /// <summary>
        /// Sets as VB.
        /// </summary>
        public void SetVB()
        {
            Language = "VBNET";
            textEditorControl1.SetHighlighting(Language);
            myProjectContent.Language = LanguageProperties.VBNet;
            IsVisualBasic = true;
        }

        /// <summary>
        /// Sets as CS.
        /// </summary>
        public void SetCS()
        {
            Language = "C#";
            textEditorControl1.SetHighlighting(Language);
            myProjectContent.Language = LanguageProperties.CSharp;
            IsVisualBasic = false;
        }

        private void ParserThread()
        {
            BeginInvoke(new MethodInvoker(delegate { parserThreadLabel.Text = "Loading mscorlib..."; }));
            myProjectContent.AddReferencedContent(pcRegistry.Mscorlib);

            // do one initial parser step to enable code-completion while other
            // references are loading
            ParseStep();
            try
            {
                string appPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\";
                //string[] referencedAssemblies = {
                //     "System", "System.Data", "System.Drawing", "System.Xml", "System.Windows.Forms", "Microsoft.VisualBasic", appPath + "Interop.MapWinGIS.dll", appPath + "MapWinInterfaces.dll", appPath + "MapWinGeoProc.dll"
                //};
                string[] referencedAssemblies = {
                     "System", "System.Data", "System.Drawing", "System.Xml", "System.Windows.Forms", "Microsoft.VisualBasic", appPath + "DotSpatial.Controls.dll", appPath + "DotSpatial.Data.dll", appPath + "DotSpatial.Topology.dll"
                };
                foreach (string assemblyName in referencedAssemblies)
                {
                    string assemblyNameCopy = assemblyName; // copy for anonymous method
                    BeginInvoke(new MethodInvoker(delegate { parserThreadLabel.Text = "Loading " + assemblyNameCopy + "..."; }));
                    IProjectContent referenceProjectContent = pcRegistry.GetProjectContentForReference(assemblyName, assemblyName);

                    //\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/
                    //if (assemblyNameCopy.Contains("Interop.MapWinGIS.dll"))
                    //{
                    //    Dom.DefaultCompilationUnit myDefaultCompilationUnit = new Dom.DefaultCompilationUnit(referenceProjectContent);
                    //    foreach (Dom.IClass iClass in referenceProjectContent.Classes)
                    //    {
                    //        //*********  ADD CLASSES TO HIDE HERE ******************
                    //        if (iClass.Name.StartsWith("_"))
                    //            myDefaultCompilationUnit.Classes.Add(iClass);
                    //    }
                    //    referenceProjectContent.RemoveCompilationUnit(myDefaultCompilationUnit);
                    //}
                    //\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/

                    myProjectContent.AddReferencedContent(referenceProjectContent);
                    if (referenceProjectContent is ReflectionProjectContent)
                    {
                        (referenceProjectContent as ReflectionProjectContent).InitializeReferences();
                    }
                }
            }
            catch (Exception e)
            {
                gErrorMsg = e.Message + e;
            }

            if (IsVisualBasic)
            {
                myProjectContent.DefaultImports = new DefaultUsing(myProjectContent);
                myProjectContent.DefaultImports.Usings.Add("System");
                myProjectContent.DefaultImports.Usings.Add("System.Text");
                myProjectContent.DefaultImports.Usings.Add("Microsoft.VisualBasic");
            }

            BeginInvoke(new MethodInvoker(delegate { parserThreadLabel.Text = "Ready"; }));

            // Parse the current file every 2 seconds
            while (!IsDisposed)
            {
                ParseStep();

                Thread.Sleep(2000);
            }
        }

        private void ParseStep()
        {
            string code = null;
            Invoke(new MethodInvoker(delegate
            {
                code = textEditorControl1.Text;
            }));
            TextReader textReader = new StringReader(code);
            ICompilationUnit newCompilationUnit;
            SupportedLanguage supportedLanguage;
            if (IsVisualBasic)
                supportedLanguage = SupportedLanguage.VBNet;
            else
                supportedLanguage = SupportedLanguage.CSharp;
            using (IParser p = ParserFactory.CreateParser(supportedLanguage, textReader))
            {
                // we only need to parse types and method definitions, no method bodies
                // so speed up the parser and make it more resistent to syntax
                // errors in methods
                p.ParseMethodBodies = false;
                p.Parse();
                newCompilationUnit = ConvertCompilationUnit(p.CompilationUnit);
            }
            // Remove information from lastCompilationUnit and add information from newCompilationUnit.
            myProjectContent.UpdateCompilationUnit(lastCompilationUnit, newCompilationUnit, DummyFileName);
            lastCompilationUnit = newCompilationUnit;
            parseInformation.SetCompilationUnit(newCompilationUnit);
        }

        private ICompilationUnit ConvertCompilationUnit(CompilationUnit cu)
        {
            NRefactoryASTConvertVisitor converter;
            converter = new NRefactoryASTConvertVisitor(myProjectContent);
            cu.AcceptVisitor(converter, null);
            return converter.Cu;
        }
    }
}