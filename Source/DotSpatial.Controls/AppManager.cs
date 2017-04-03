// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/26/2009 6:17:22 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------
// Jiri Kadlec        | 03/08/2010         |  Added the PanelManager to work with tabs and panels
// Yang Cao           | 05/16/2011         |  Added the IHeaderControl to work with standard toolbar and ribbon control
// Eric Hullinger     | 01/02/2014         |  Made changes so that the progress bar on the splash screen will update properly
// Jacob Gillespie    | 03/31/2014         |  Plugins can now be stored in and loaded from one extensions/plugins directory.
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using DotSpatial.Controls.DefaultRequiredImports;
using DotSpatial.Controls.Docking;
using DotSpatial.Controls.Header;
using DotSpatial.Data;
using DotSpatial.Extensions;
using DotSpatial.Extensions.SplashScreens;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A component that manages the loading of extensions (including data providers), and helps with file serialization.
    /// </summary>
    /// <remarks>Will soon be responsible for tools and layer providers, too.</remarks>
    [Export]
    public class AppManager : Component
    {
        #region Constants and Fields

        /// <summary>
        /// Name of the folder where packages reside.
        /// Found within the Extensions Directory.
        /// </summary>
        public const string PackageDirectory = "Packages";

        private const string ExtensionsDirectory = "Extensions";
        private const int SplashDirectoryMessageLimit = 50;

        private AggregateCatalog _catalog;
        private IContainer _components;
        private string _message = "";
        private ISplashScreenManager _splashScreen;
        private IHeaderControl _headerControl;
        private IMap _map;

        #endregion

        #region Constructors and Destructors

        static AppManager()
        {
            BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppManager"/> class.
        /// </summary>
        public AppManager()
        {
            InitializeComponent();
            Directories = new List<string> { "Application Extensions", "Plugins" };
            SerializationManager = new SerializationManager(this);
            Extensions = new List<IExtension>();
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Occurs when all plugins are loaded.
        /// </summary>
        public event EventHandler ExtensionsActivated;

        /// <summary>
        /// Occurs just before starting to activate extensions.
        /// Use this event to run a custom task before extensions
        /// are activated.
        /// </summary>
        public event EventHandler ExtensionsActivating;

        /// <summary>
        /// Occurs after the extensions that are of type
        /// SatisfyImportsExtensions have been activated. At this stage
        /// the DockManager, ProgressHandler and HeaderControl are available.
        /// Use this event to add custom items to the DockManager, ProgressHandler
        /// or HeaderControl before other extensions are activated.
        /// </summary>
        public event EventHandler SatisfyImportsExtensionsActivated;

        /// <summary>
        /// Occurs when HeaderControl changed.
        /// </summary>
        public event EventHandler HeaderControlChanged;

        /// <summary>
        /// Occurs when Map Changed
        /// </summary>
        public event EventHandler<MapChangedEventArgs> MapChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Used in conjuction with <see cref="UseBaseDirectoryForExtensionsDirectory"/>. Default is AppDomain.CurrentDomain.BaseDirectory.
        /// </summary>
        public static string BaseDirectory { get; set; }

        /// <summary>
        /// A known directory from where extensions will be loaded, in addition to the configurable Directories list.
        /// Assemblies placed directly in this directory will not be loaded, but rather those nested inside of a folder
        /// more than one level deep.
        /// </summary>
        public static string AbsolutePathToExtensions
        {
            get
            {
                string absolutePathToExtensions;
                if (UseBaseDirectoryForExtensionsDirectory)
                {
                    absolutePathToExtensions = Path.Combine(BaseDirectory, ExtensionsDirectory);
                }
                else
                {
                    // by placing data in the AppData location, ClickOnce appications won't be subject to limits on size.
                    Assembly asm = Assembly.GetEntryAssembly();
                    absolutePathToExtensions = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                            asm.ManifestModule.Name, ExtensionsDirectory);
                }
                return absolutePathToExtensions;
            }
        }

        /// <summary>
        /// Gets the catalog containing all off the know extensions. Add any additional extensions to Catalog.Catalogs.
        /// </summary>
        [Browsable(false)]
        public AggregateCatalog Catalog
        {
            get
            {
                return _catalog;
            }
        }

        /// <summary>
        /// Gets or sets the composition container.
        /// </summary>
        /// <value>
        /// The composition container.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CompositionContainer CompositionContainer { get; set; }

        /// <summary>
        /// Gets or sets the list of string paths (relative to this one) to search for plugins.
        /// </summary>
        [Description("Gets or sets the list of string paths (relative to this one) to search for plugins.")]
        public List<string> Directories { get; set; }

        /// <summary>
        /// Gets or sets the dock manager that is being used to storing dock panels. You can leave this empty to use default dock manager.
        /// </summary>
        /// <value>
        /// The dock manager.
        /// </value>
        [Description("Gets or sets the dock manager that is being used to storing dock panels. You can leave this empty to use default dock manager.")]
        public IDockManager DockManager { get; set; }

        /// <summary>
        /// Gets the extensions.
        /// </summary>
        [Browsable(false)]
        [ImportMany(AllowRecomposition = true)]
        public IEnumerable<IExtension> Extensions { get; private set; }

        /// <summary>
        /// Gets or sets the header control
        /// </summary>
        [Description("Gets or sets the header control. You can leave this empty to use default header control.")]
        public IHeaderControl HeaderControl
        {
            get { return _headerControl; }
            set
            {
                if (value == _headerControl) return;
                _headerControl = value;

                var h = HeaderControlChanged;
                if (h != null) h(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the Legend (Table of Contents) associated with the plugin manager
        /// </summary>
        [Description("Gets or sets the Legend (Table of Contents) associated with the plugin manager")]
        public ILegend Legend { get; set; }

        /// <summary>
        /// Gets or sets the Map associated with the plugin manager
        /// </summary>
        [Description("Gets or sets the Map associated with the plugin manager")]
        public IMap Map
        {
            get { return _map; }
            set
            {
                if (value == _map) return;
                var oldMap = _map;
                _map = value;

                var h = MapChanged;
                if (h != null) h(this, new MapChangedEventArgs(oldMap, value));
            }
        }

        /// <summary>
        /// Gets or sets the progress handler that is being used to display status messages.
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the progress handler that is being used to display status messages. You can leave this empty to use default status bar.")]
        public IStatusControl ProgressHandler { get; set; }

        [Browsable(false)]
        [ImportMany]
        private IEnumerable<ISatisfyImportsExtension> SatisfyImportsExtensions { get; set; }

        /// <summary>
        /// Gets or sets the serialization manager.
        /// </summary>
        [Browsable(false)]
        public SerializationManager SerializationManager { get; set; }

        /// <summary>
        /// Gets or sets the method for enabling extension Apps.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets or sets the method for enabling extension Apps.")]
        public ShowExtensionsDialogMode ShowExtensionsDialogMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether extensions should be placed in <see cref="BaseDirectory"/>
        /// </summary>
        /// <value>
        /// <c>true</c> if extensions should be placed in <see cref="BaseDirectory"/>; otherwise, extensions will be placed in a user profile folder based on the entry assembly name.
        /// This must be set before calling LoadExtensions();
        /// </value>
        public static bool UseBaseDirectoryForExtensionsDirectory { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Activates the extensions passed in and deactivates the rest.
        /// If null is passed in, all extensions are deactivated.
        /// Only affects extensions where DeactivationAllowed is true.
        /// </summary>
        /// <param name="names">The names.</param>
        public void ActivateExtensionsExclusively(string[] names)
        {
            // Consider only extensions that can be deactivated.
            var extensions = Extensions.Where(t => t.DeactivationAllowed).ToList();

            var extensionCount = extensions.Count();
            int progress = 0;

            foreach (var extension in extensions.OrderBy(_ => _.Priority))
            {
                //report progress
                if (ProgressHandler != null)
                {
                    int percent = (progress * 100) / extensionCount;
                    var msg = string.Format(MessageStrings.LoadingPluginsPercentComplete, extension.Name, percent);
                    ProgressHandler.Progress(MessageStrings.LoadingPlugins, percent, msg);
                    progress++;
                }

                if (names != null && names.Contains(extension.Name))
                {
                    // activate
                    if (extension.IsActive == false)
                        extension.TryActivate();
                }
                else
                {
                    //deactivate
                    if (extension.IsActive)
                        extension.Deactivate();
                }
            }

            //report progress
            if (ProgressHandler != null)
            {
                ProgressHandler.Progress(MessageStrings.LoadingPlugins, 0, string.Empty);
            }
        }

        /// <summary>
        /// Ensures the required imports are available for IExtension implementors. We guarantee DockManager, HeaderControl and ProgressHandler
        /// are available when an IExtension loads, so that the developer of an IExtension doesn't need to check to see whether they are null.
        /// We make sure these are available before activating an IExtension.
        /// </summary>
        /// <returns></returns>
        public bool EnsureRequiredImportsAreAvailable()
        {
            if (DockManager == null)
            {
            DockManager = GetRequiredImport<IDockManager>();
            }

            if (HeaderControl == null)
            {
            HeaderControl = GetRequiredImport<IHeaderControl>();
            }

            if (ProgressHandler == null)
            {
            ProgressHandler = GetRequiredImport<IStatusControl>();
            }

            if (DockManager == null || HeaderControl == null || ProgressHandler == null)
                return false;

            return true;
        }

        /// <summary>
        /// Gets the extension.
        /// </summary>
        /// <param name="assemblyTitle">The assembly title.</param>
        /// <returns>
        /// Null if the extension is not present.
        /// </returns>
        public IExtension GetExtension(string assemblyTitle)
        {
            return Extensions.FirstOrDefault(t => t.AssemblyQualifiedName.Contains(assemblyTitle + ","));
        }

        /// <summary>
        /// Gets the extension.
        /// </summary>
        /// <param name="assemblyTitle">The assembly title.</param>
        /// <param name="version">The version.</param>
        /// <returns>
        /// Null if the extension is not present.
        /// </returns>
        public IExtension GetExtension(string assemblyTitle, string version)
        {
            return Extensions.FirstOrDefault(t => t.AssemblyQualifiedName.Contains(assemblyTitle + ",") && t.Version == version);
        }

        /// <summary>
        /// Loads Extensions using MEF and then activates them.
        /// Should only be called once on startup.
        /// </summary>
        public virtual void LoadExtensions()
        {
            if (DesignMode)
                return;

            if (Extensions.Any())
            {
                throw new InvalidOperationException("LoadExtensions() should only be called once. Subsequent calls should be made to RefreshExtensions(). ");
            }

            PackageManager.RemovePendingPackagesAndExtensions();
            _splashScreen = SplashScreenHelper.GetSplashScreenManager();

            Thread updateThread = new Thread(AppLoadExtensions);
            updateThread.Start();

            //Update splash screen's progress bar while thread is active.
            while (updateThread.IsAlive)
            {
                UpdateSplashScreen(_message);
            }
            updateThread.Join();

            ActivateAllExtensions();
            OnExtensionsActivated(EventArgs.Empty);

            if (_splashScreen != null)
            {
                _splashScreen.Deactivate();
                _splashScreen = null;
            }

            // Set the DefaultDataManager progress handler.
            // It doesn’t seem like the solution is as simple as adding the
            //        [Import(typeof(IProgressHandler), AllowDefault = true)]
            // Attribute to the ProgressHandler property on DataManager, because the ProgressHandler we are typically
            // using only export IStatusControl and we would require each IStatusControl to
            //    [Export(typeof(DotSpatial.Data.IProgressHandler))]
            // To get that working.
            DataManager.DefaultDataManager.ProgressHandler = this.ProgressHandler;
        }

        /// <summary>
        /// Triggers the ExtensionsActivated event.
        /// </summary>
        public virtual void OnExtensionsActivated(EventArgs ea)
        {
            if (ExtensionsActivated != null)
            {
                ExtensionsActivated(this, ea);
            }
        }

        /// <summary>
        /// Refreshes the extensions - activating any newly discovered ones.
        /// </summary>
        public void RefreshExtensions()
        {
            RefreshExtensions(Catalog);
        }

        /// <summary>
        /// Updates the ProgressHandler.
        /// </summary>
        /// <param name="msg">The message.</param>
        public void UpdateProgress(string msg)
        {
            if (_splashScreen != null)
                UpdateSplashScreen(msg);
            else if (ProgressHandler != null)
                ProgressHandler.Progress(string.Empty, 0, msg);
            else
            {
                MessageBox.Show(msg);
            }
        }

        #endregion

        #region Methods

        private static void Activate(IExtension extension)
        {
            if (!extension.IsActive)
            {
                if (!extension.TryActivate())
                    MessageBox.Show(string.Format(MessageStrings.ErrorWhileWhileActivating, extension.AssemblyQualifiedName));
            }
        }

        private void ActivateAllExtensions()
        {
            foreach (var extension in SatisfyImportsExtensions.OrderBy(_ => _.Priority))
            {
                extension.Activate();
            }

            if (!EnsureRequiredImportsAreAvailable()) return;
            OnSatisfyImportsExtensionsActivated(EventArgs.Empty);

            // Load "Application Extensions" first. We do this to temporarily deal with the situation where specific root menu items
            // need to be created before other plugins are loaded.
            foreach (var extension in Extensions.Where(_ => !_.DeactivationAllowed).OrderBy(_ => _.Priority))
            {
                    Activate(extension);
            }

            // Activate remaining extensions
            foreach (var extension in Extensions.Where(_ => _.DeactivationAllowed).OrderBy(_ => _.Priority))
            {
                Activate(extension);
            }
        }

        /// <summary>
        /// Catalogs all extensions and then composes their parts.
        /// </summary>
        private void AppLoadExtensions()
        {
            _message = "Discovering Extensions...";
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            _catalog = GetCatalog();

            CompositionContainer = new CompositionContainer(_catalog);

            try
            {
                CompositionContainer.ComposeParts(this, DataManager.DefaultDataManager, SerializationManager);
            }
            catch (CompositionException compositionException)
            {
                Trace.WriteLine(compositionException.Message);
                throw;
            }

            _message = "Loading Extensions...";
            OnExtensionsActivating(EventArgs.Empty);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">
        /// true if managed resources should be disposed; otherwise, false.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (_components != null))
            {
                _components.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Looks for the assembly in a path like Extensions\Packages\
        /// </summary>
        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var knownExtensions = new[] { "dll", "exe" };
            string assemblyName = new AssemblyName(args.Name).Name;
            string packagesFolder = Path.Combine(AbsolutePathToExtensions, PackageDirectory);

            //check the ProgramData directory
            if (Directory.Exists(packagesFolder))
            {
                foreach (string extension in knownExtensions)
                {
                    var potentialFiles = Directory.GetFiles(packagesFolder, assemblyName + "." + extension, SearchOption.AllDirectories);
                    if (potentialFiles.Length > 0)
                        return Assembly.LoadFrom(potentialFiles[0]);
                }
            }

            //check the installation directory
            foreach (string directory in Directories)
            {
                string path = Path.Combine(BaseDirectory, directory);

                if (Directory.Exists(path))
                {
                    foreach (string extension in knownExtensions)
                    {
                        var potentialFiles = Directory.GetFiles(path, assemblyName + "." + extension, SearchOption.AllDirectories);
                        if (potentialFiles.Length > 0)
                            return Assembly.LoadFrom(potentialFiles[0]);
                    }
                }
            }

            //assembly not found
            return null;
        }

        private static bool DirectoryCatalogExists(AggregateCatalog catalog, string dir)
        {
            return catalog.Catalogs.OfType<DirectoryCatalog>()
                .Any(directoryCatalog => directoryCatalog.FullPath.Equals(dir, StringComparison.OrdinalIgnoreCase));
        }

        private AggregateCatalog GetCatalog()
        {
            var catalog = new AggregateCatalog();

            // Add main exe
            Assembly mainExe = Assembly.GetEntryAssembly();
            if (mainExe != null)
            {
                // if there is a managed entry assembly running, add it.
                catalog.Catalogs.Add(new AssemblyCatalog(mainExe));
                Trace.WriteLine("Cataloging: " + mainExe.FullName);
            }

            // Add DotSpatial Controls (includes defaut required extensions)
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(AppManager).Assembly));

            // Add DotSpatial Data (Which includes data providers)
            Assembly dataDll = typeof(BinaryRasterProvider).Assembly;
            catalog.Catalogs.Add(new AssemblyCatalog(dataDll));
            Trace.WriteLine("Cataloging: " + dataDll.FullName);

            // Add all of the directories in ExtensionsDirectory, nested any number of levels.
            RefreshExtensions(catalog);

            // Visit each directory in Directories Property (usually set by application)
            foreach (string dir in GetDirectoriesNestedOneLevel())
            {
                // Add files in the current directory as well.
                Trace.WriteLine("Cataloging: " + dir);
                // UpdateSplashScreen("Cataloging: " + PrefixWithEllipsis(dir, SplashDirectoryMessageLimit));
                _message = "Cataloging: " + PrefixWithEllipsis(dir, SplashDirectoryMessageLimit);
                if (!DirectoryCatalogExists(catalog, dir))
                    TryLoadingCatalog(catalog, new DirectoryCatalog(dir));
            }

            return catalog;
        }

        /// <summary>
        /// Gets the directories in Directories and those nested one level deep.
        /// </summary>
        private IEnumerable<string> GetDirectoriesNestedOneLevel()
        {
            // Visit each directory in Directories Property (usually set by application)
            Directories.Add(Mono.IsRunningOnMono() ? "Mono Extensions" : "Windows Extensions");
            foreach (string directory in Directories.Union(new[] { "Data Extensions", "Tools" }))
            {
                string path = Path.Combine(BaseDirectory, directory);

                if (Directory.Exists(path))
                {
                    yield return path;

                    // Add all of the directories in here, nested one level deep.
                    var dirs = Directory.EnumerateDirectories(path, "*", SearchOption.TopDirectoryOnly);

                    foreach (var dir in dirs)
                    {
                        yield return dir;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the paths of dlls for extensions that were downloaded as packages.
        /// </summary>
        private static IEnumerable<string> GetPackageExtensionPaths(string absolutePathToExtensions)
        {
            if (!Directory.Exists(absolutePathToExtensions))
                yield break;

            var packagesFolder = Path.Combine(absolutePathToExtensions, PackageDirectory);
            if (!Directory.Exists(packagesFolder))
                yield break;

            var packageFolders = Directory.EnumerateDirectories(packagesFolder, "*", SearchOption.AllDirectories);
            foreach (var packageFolder in packageFolders)
                yield return packageFolder;
        }

        private T GetRequiredImport<T>() where T : class
        {
            var imports = CompositionContainer.GetExportedValues<T>().ToList();

            if (imports.Count > 1)
            {
                // If more then one required import, skip default imports
                imports = imports
                    .Where(_ => !_.GetType().GetCustomAttributes(typeof (DefaultRequiredImportAttribute), false).Any())
                    .ToList();
            }

            if (imports.Count != 1)
            {
                var importCount = imports.Count;
                string extensionTypeName = typeof(T).Name;
                MessageBox.Show(importCount > 1
                    ? string.Format("You may only include one {0} Extension. {1} were found.", extensionTypeName, importCount)
                    : string.Format("A {0} extension must be included because a UI plugin was found.", extensionTypeName));
                return null;
            }

            return imports[0];
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _components = new Container();
        }

        private void LocateExtensions(AggregateCatalog catalog)
        {
            var directories = GetPackageExtensionPaths(AbsolutePathToExtensions);
            foreach (var dir in directories)
            {
                Trace.WriteLine("Cataloging: " + dir);
                //UpdateSplashScreen("Cataloging: " + PrefixWithEllipsis(dir, SplashDirectoryMessageLimit));
                _message = "Cataloging: " + PrefixWithEllipsis(dir, SplashDirectoryMessageLimit);
                // todo: consider using a file system watcher if it would provider better performance.
                if (!DirectoryCatalogExists(catalog, dir))
                    TryLoadingCatalog(catalog, new DirectoryCatalog(dir));
            }
        }

        private void OnExtensionsActivating(EventArgs ea)
        {
            if (ExtensionsActivating != null)
            {
                ExtensionsActivating(this, ea);
            }
        }

        private void OnSatisfyImportsExtensionsActivated(EventArgs ea)
        {
            var h = SatisfyImportsExtensionsActivated;
            if (h != null)
                h(this, ea);
        }

        private string PrefixWithEllipsis(string text, int length)
        {
            if (text.Length <= length) return text;

            return "..." + text.Substring(Math.Max(2, text.Length - length - 3));
        }

        private void RefreshExtensions(AggregateCatalog catalog)
        {
            LocateExtensions(catalog);
        }

        private static void TryLoadingCatalog(AggregateCatalog catalog, ComposablePartCatalog cat)
        {
            try
            {
                // We call Parts.Count simply to load the dlls in this directory, so that we can determine whether they will load properly.
                if (cat.Parts.Any())
                    catalog.Catalogs.Add(cat);
            }
            catch (ReflectionTypeLoadException ex)
            {
                Type type = ex.Types[0];
                string typeAssembly;
                if (type != null)
                    typeAssembly = type.Assembly.ToString();
                else
                    typeAssembly = string.Empty;

                string message = string.Format("Skipping extension {0}. {1}", typeAssembly, ex.LoaderExceptions.First().Message);
                Trace.WriteLine(message);
                MessageBox.Show(message);
            }
        }

        /// <summary>
        /// Update splash screen with given text.
        /// </summary>
        /// <param name="text">Splash screen text.</param>
        public void UpdateSplashScreen(string text)
        {
            if (_splashScreen != null && text != null)
                _splashScreen.ProcessCommand(SplashScreenCommand.SetDisplayText, text);
        }

        #endregion
    }


    /// <summary>
    /// Provides data for the <see langword='MapChanged'/> event.
    /// </summary>
    public class MapChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets old map.
        /// </summary>
        public IMap OldValue { get; private set; }

        /// <summary>
        /// Gets new map.
        /// </summary>
        public IMap NewValue { get; private set; }

        /// <summary>
        /// Create new instance of <see cref="MapChangedEventArgs"/>.
        /// </summary>
        public MapChangedEventArgs(IMap oldValue, IMap newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}