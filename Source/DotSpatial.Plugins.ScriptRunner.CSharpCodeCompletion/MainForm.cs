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

using System.IO;
using System.Threading;
using System.Windows.Forms;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.NRefactoryResolver;

namespace CSharpEditor
{
    /// <summary>
    /// The main form of the script runner.
    /// </summary>
    public partial class MainForm : UserControl
    {
        #region Fields

        /// <summary>
        /// Many SharpDevelop.Dom methods take a file name, which is really just a unique identifier
        /// for a file - Dom methods don't try to access code files on disk, so the file does not have
        /// to exist.
        /// SharpDevelop itself uses internal names of the kind "[randomId]/Class1.cs" to support
        /// code-completion in unsaved files.
        /// </summary>
        public const string DummyFileName = "edited.cs";

        private Thread _parserThread;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            // The InitializeComponent() call is required for Windows Forms designer support.
            InitializeComponent();

            textEditorControl1.ShowEOLMarkers = false;
            textEditorControl1.ShowInvalidLines = false;

            HostCallbackImplementation.Register(this);
            CodeCompletionKeyHandler.Attach(this, textEditorControl1);
            ToolTipProvider.Attach(this, textEditorControl1);

            PcRegistry = new ProjectContentRegistry(); // Default .NET 2.0 registry

            // Persistence lets SharpDevelop.Dom create a cache file on disk so that future starts are faster.
            // It also caches XML documentation files in an on-disk hash table, thus reducing memory usage.
            // Paul Meems, 1 Sept. 2010: Create the file if it doesn't exist (Bug #1763):
            string cacheFolder = Path.Combine(Path.GetTempPath(), "CSharpCodeCompletion");
            if (!Directory.Exists(cacheFolder))
            {
                Directory.CreateDirectory(cacheFolder);
            }

            PcRegistry.ActivatePersistence(cacheFolder);

            MyProjectContent = new DefaultProjectContent
            {
                Language = IsVisualBasic ? LanguageProperties.VBNet : LanguageProperties.CSharp
            };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether to use VB or C#.
        /// </summary>
        public static bool IsVisualBasic { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        public string Language { get; set; } = "VBNET";

        /// <summary>
        /// Gets or sets the last compilation unit.
        /// </summary>
        public ICompilationUnit LastCompilationUnit { get; set; }

        /// <summary>
        /// Gets or sets the DefaultProjectContent.
        /// </summary>
        public DefaultProjectContent MyProjectContent { get; set; }

        /// <summary>
        /// Gets or sets the ParseInformation.
        /// </summary>
        public ParseInformation ParseInformation { get; set; } = new ParseInformation();

        /// <summary>
        /// Gets or sets the ProjectContentRegistry.
        /// </summary>
        public ProjectContentRegistry PcRegistry { get; set; }

        /// <summary>
        /// Gets or sets the text.
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

        #endregion

        #region Methods

        /// <summary>
        /// Inits this instance.
        /// </summary>
        public void Init()
        {
            _parserThread = new Thread(ParserThread)
            {
                IsBackground = true
            };
            _parserThread.Start();
        }

        /// <summary>
        /// Sets as CS.
        /// </summary>
        public void SetCs()
        {
            Language = "C#";
            textEditorControl1.SetHighlighting(Language);
            MyProjectContent.Language = LanguageProperties.CSharp;
            IsVisualBasic = false;
        }

        /// <summary>
        /// Sets as VB.
        /// </summary>
        public void SetVb()
        {
            Language = "VBNET";
            textEditorControl1.SetHighlighting(Language);
            MyProjectContent.Language = LanguageProperties.VBNet;
            IsVisualBasic = true;
        }

        /// <summary>
        /// Shutdowns this instance.
        /// </summary>
        public void Shutdown()
        {
            _parserThread?.Abort();
        }

        private ICompilationUnit ConvertCompilationUnit(CompilationUnit cu)
        {
            var converter = new NRefactoryASTConvertVisitor(MyProjectContent);
            cu.AcceptVisitor(converter, null);
            return converter.Cu;
        }

        private void ParserThread()
        {
            BeginInvoke(new MethodInvoker(() => parserThreadLabel.Text = "Loading mscorlib..."));
            MyProjectContent.AddReferencedContent(PcRegistry.Mscorlib);

            // do one initial parser step to enable code-completion while other
            // references are loading
            ParseStep();
            try
            {
                string appPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\";

                string[] referencedAssemblies = { "System", "System.Data", "System.Drawing", "System.Xml", "System.Windows.Forms", "Microsoft.VisualBasic", appPath + "DotSpatial.Controls.dll", appPath + "DotSpatial.Data.dll", appPath + "DotSpatial.Topology.dll" };
                foreach (string assemblyName in referencedAssemblies)
                {
                    string assemblyNameCopy = assemblyName; // copy for anonymous method
                    BeginInvoke(new MethodInvoker(() => parserThreadLabel.Text = "Loading " + assemblyNameCopy + "..."));
                    IProjectContent referenceProjectContent = PcRegistry.GetProjectContentForReference(assemblyName, assemblyName);

                    MyProjectContent.AddReferencedContent(referenceProjectContent);
                    (referenceProjectContent as ReflectionProjectContent)?.InitializeReferences();
                }
            }
            catch
            {
            }

            if (IsVisualBasic)
            {
                MyProjectContent.DefaultImports = new DefaultUsing(MyProjectContent);
                MyProjectContent.DefaultImports.Usings.Add("System");
                MyProjectContent.DefaultImports.Usings.Add("System.Text");
                MyProjectContent.DefaultImports.Usings.Add("Microsoft.VisualBasic");
            }

            BeginInvoke(new MethodInvoker(() => parserThreadLabel.Text = "Ready"));

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
            Invoke(new MethodInvoker(() => code = textEditorControl1.Text));
            TextReader textReader = new StringReader(code);
            ICompilationUnit newCompilationUnit;
            var supportedLanguage = IsVisualBasic ? SupportedLanguage.VBNet : SupportedLanguage.CSharp;
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
            MyProjectContent.UpdateCompilationUnit(LastCompilationUnit, newCompilationUnit, DummyFileName);
            LastCompilationUnit = newCompilationUnit;
            ParseInformation.SetCompilationUnit(newCompilationUnit);
        }

        #endregion
    }
}