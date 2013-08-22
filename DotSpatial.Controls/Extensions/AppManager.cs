// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/26/2009 6:17:22 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------
// Jiri Kadlec        | 03/08/2010         |  Added the PanelManager to work with tabs and panels
// Yang Cao           | 05/16/2011         |  Added the IHeaderControl to work with standard toolbar and ribbon control
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using DotSpatial.Controls.Docking;
using DotSpatial.Controls.Header;
using DotSpatial.Data;
using DotSpatial.Extensions;
using DotSpatial.Extensions.SplashScreens;
using DotSpatial.Controls.Extensions;
using System.Threading;

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

        private const int SplashDirectoryMessageLimit = 50;
        private const string ExtensionsDirectory = "Extensions";

        /// <summary>
        /// Name of the folder where packages reside.
        /// </summary>
        public const string PackageDirectory = "Packages";

        private static ResourceManager resources;

        private AggregateCatalog _catalog;
        private IContainer _components;
        private ISplashScreenManager splashScreen;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AppManager"/> class.
        /// </summary>
        public AppManager()
        {
            InitializeComponent();
            Directories = new List<string> { "Application Extensions", "Plugins" };
            SerializationManager = new SerializationManager(this);
            Extensions = new List<IExtension>();
            resources = new ResourceManager("DotSpatial.Controls.MessageStrings", Assembly.GetExecutingAssembly());
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

        #endregion

        #region Public Properties

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
                    absolutePathToExtensions = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ExtensionsDirectory);
                else
                {
                    // by placing data in the AppData location, ClickOnce appications won't be subject to limits on size.
                    Assembly asm = Assembly.GetEntryAssembly();
                    absolutePathToExtensions = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), asm.ManifestModule.Name, ExtensionsDirectory);
                }
                return absolutePathToExtensions;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether extensions should be placed in AppDomain.CurrentDomain.BaseDirectory.
        /// </summary>
        /// <value>
        /// <c>true</c> if extensions should be placed in AppDomain.CurrentDomain.BaseDirectory; otherwise, extensions will be placed in a user profile folder based on the entry assembly name.
        /// This must be set before calling LoadExtensions();
        /// </value>
        public static bool UseBaseDirectoryForExtensionsDirectory { get; set; }

        /// <summary>
        /// Gets the catalog containing all off the know extensions. Add any additional extensions to Catalog.Catalogs.
        /// </summary>
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
        public CompositionContainer CompositionContainer { get; set; }

        /// <summary>
        /// Gets or sets the list of string paths (relative to this one) to search for plugins.
        /// </summary>
        public List<string> Directories { get; set; }

        /// <summary>
        /// Gets or sets the dock manager.
        /// </summary>
        /// <value>
        /// The dock manager.
        /// </value>
        [Browsable(false)]
        public IDockManager DockManager { get; set; }

        /// <summary>
        /// Gets the extensions.
        /// </summary>
        [Browsable(false)]
        [ImportMany(AllowRecomposition = true)]
        public IEnumerable<IExtension> Extensions { get; private set; }

        [Browsable(false)]
        [ImportMany]
        private IEnumerable<ISatisfyImportsExtension> SatisfyImportsExtensions { get; set; }

        /// <summary>
        /// Gets or sets the header control
        /// </summary>
        [Browsable(false)]
        public IHeaderControl HeaderControl { get; set; }

        /// <summary>
        /// Gets the Legend (Table of Contents) associated with the plugin manager
        /// </summary>
        public ILegend Legend { get; set; }

        /// <summary>
        /// Gets the Map associated with the plugin manager
        /// </summary>
        public IMap Map { get; set; }

        /// <summary>
        /// Gets the progress handler that is being used to display status messages.
        /// </summary>
        [Browsable(false)]
        public IStatusControl ProgressHandler { get; set; }

        /// <summary>
        /// Gets or sets the serialization manager.
        /// </summary>
        /// <value>
        /// The serialization manager.
        /// </value>
        public SerializationManager SerializationManager { get; set; }

        /// <summary>
        /// Gets or sets the method for enabling extension Apps.
        /// </summary>
        public ShowExtensionsDialog ShowExtensionsDialog { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the extension.
        /// </summary>
        /// <param name="assemblyTitle">The assembly title.</param>
        /// <returns>
        /// Null if the extension is not present.
        /// </returns>
        public IExtension GetExtension(string assemblyTitle)
        {
            return Extensions.Where(t => t.AssemblyQualifiedName.Contains(assemblyTitle + ",")).FirstOrDefault();
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
            return Extensions.Where(t => t.AssemblyQualifiedName.Contains(assemblyTitle + ",") && t.Version == version).FirstOrDefault();
        }

        /// <summary>
        /// Activates the extensions passed in and deactivates the rest.
        /// If null is passed in, all extensions are deactivated.
        /// Only affects extensions where DeactivationAllowed is true.
        /// </summary>
        /// <param name="names">The names.</param>
        public void ActivateExtensionsExclusively(string[] names)
        {
            // Consider only extensions that can be deactivated.
            IEnumerable<IExtension> extensions = Extensions.Where(t => t.DeactivationAllowed);

            var extensionCount = extensions.Count();
            int progress = 0;

            foreach (var extension in extensions)
            {
                //report progress
                if (ProgressHandler != null)
                {
                    int percent = (progress * 100) / extensionCount;
                    string message = String.Format(resources.GetString("LoadingPluginsPercentComplete"), extension.Name, percent);
                    ProgressHandler.Progress(resources.GetString("LoadingPlugins"), percent, message);
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
                ProgressHandler.Progress(resources.GetString("LoadingPlugins"), 0, String.Empty);
            }
        }

        private string PrefixWithEllipsis(string text, int length)
        {
            if (text.Length <= length) return text;

            return "..." + text.Substring(Math.Max(2, text.Length - length - 3));
        }

        private void UpdateSplashScreen(string text)
        {
            if (splashScreen != null)
                splashScreen.ProcessCommand(SplashScreenCommand.SetDisplayText, text);
        }

        /// <summary>
        /// Updates the ProgressHandler.
        /// </summary>
        /// <param name="message">The message.</param>
        public void UpdateProgress(string message)
        {
            if (splashScreen != null)
                UpdateSplashScreen(message);
            else if (ProgressHandler != null)
                ProgressHandler.Progress(String.Empty, int.MinValue, message);
            else
            {
                MessageBox.Show(message);
            }
        }

        /// <summary>
        /// Activates the extensions. This should be called only once.
        /// </summary>
        public virtual void LoadExtensions()
        {
            if (DesignMode)
                return;

            if (Extensions.Any())
            {
                throw new InvalidOperationException("LoadExtensions() should only be called once. Subsequent calls should be made to RefreshExtensions(). ");
            }

            // We need to uninstall any outstanding extensions before loading ...
            PackageManager.RemovePendingPackagesAndExtensions();

            splashScreen = SplashScreenHelper.GetSplashScreenManager();

            UpdateSplashScreen("Discovering Extensions...");

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            _catalog = GetCatalog();

            CompositionContainer = new CompositionContainer(_catalog);

            try
            {
                IDataManager dataManager = DataManager.DefaultDataManager;
                CompositionContainer.ComposeParts(this, dataManager, this.SerializationManager);
            }
            catch (CompositionException compositionException)
            {
                Trace.WriteLine(compositionException.Message);
                throw;
            }

            UpdateSplashScreen("Loading Extensions...");

            OnExtensionsActivating(EventArgs.Empty);

            ActivateAllExtensions();

            OnExtensionsActivated(EventArgs.Empty);

            if (splashScreen != null)
            {
                splashScreen.Deactivate();
                splashScreen = null;
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

        // Looks for the assembly in a path like Extensions\Packages\PackageName.Here-1.3.190\lib\net40
        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var knownExtensions = new[] { "dll", "exe" };
            string assemblyName = args.Name.Split(',').First();
            var tempAssemblyName = assemblyName.Remove(assemblyName.LastIndexOf("."));

            var packagesFolder = Path.Combine(AbsolutePathToExtensions, PackageDirectory);
            if (!Directory.Exists(packagesFolder))
                return null;

            var potentialPackage = Directory.EnumerateDirectories(packagesFolder, tempAssemblyName + "*").FirstOrDefault();
            if (potentialPackage == null)
                return null;

            var potentialFolder = Path.Combine(potentialPackage, "lib", "net40");
            if (!Directory.Exists(potentialFolder))
            {
                // see if the client profile was targeted.
                potentialFolder = Path.Combine(potentialPackage, "lib", "net40-Client");
              
                if (!Directory.Exists(potentialFolder))
                {
                    // see if the net35 framework was targeted.
                    potentialFolder = Path.Combine(potentialPackage, "lib", "net35");
                    
                    if (!Directory.Exists(potentialFolder))
                    {
                        // see if the net20 framework was targeted.
                        potentialFolder = Path.Combine(potentialPackage, "lib", "net20");
                        if (!Directory.Exists(potentialFolder))
                            return null;
                    }
                }
            }

            foreach (string extension in knownExtensions)
            {
                string potentialFile = Path.Combine(potentialFolder, string.Format(CultureInfo.CurrentCulture, "{0}.{1}", assemblyName, extension));
                if (File.Exists(potentialFile))
                {
                    return Assembly.LoadFrom(potentialFile);
                }
            }

            return null;
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
            if (SatisfyImportsExtensionsActivated != null)
            {
                SatisfyImportsExtensionsActivated(this, ea);
            }
        }

        /// <summary>
        /// Triggers the ExtensionsActivated event.
        /// </summary>
        /// <param name="ea">
        /// The ea.
        /// </param>
        public virtual void OnExtensionsActivated(EventArgs ea)
        {
            if (ExtensionsActivated != null)
            {
                ExtensionsActivated(this, ea);
            }
        }

        #endregion

        #region Methods

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

        private static void TryLoadingCatalog(AggregateCatalog catalog, ComposablePartCatalog cat)
        {
            try
            {
                // We call Parts.Count simply to load the dlls in this directory, so that we can determine whether they will load properly.
                if (cat.Parts.Count() > 0)
                    catalog.Catalogs.Add(cat);
            }
            catch (ReflectionTypeLoadException ex)
            {
                Type type = ex.Types[0];
                string typeAssembly;
                if (type != null)
                    typeAssembly = type.Assembly.ToString();
                else
                    typeAssembly = String.Empty;

                string message = String.Format("Skipping extension {0}. {1}", typeAssembly, ex.LoaderExceptions.First().Message);
                Trace.WriteLine(message);
                MessageBox.Show(message);
            }
        }

        private void ActivateAllExtensions()
        {
            if (Extensions != null && Extensions.Any())
            {
                foreach (ISatisfyImportsExtension extension in SatisfyImportsExtensions.OrderBy(t => t.Priority))
                {
                    extension.Activate();
                }
                if (!EnsureRequiredImportsAreAvailable())
                    return;
                else
                    OnSatisfyImportsExtensionsActivated(EventArgs.Empty);             

                // Load "Application Extensions" first. We do this to temporarily deal with the situation where specific root menu items
                // need to be created before other plugins are loaded.
                foreach (IExtension extension in Extensions.Where(t => t.DeactivationAllowed == false))
                {
                    if (extension.Name.Equals("DotSpatial.Plugins.ExtensionManager"))
                    {
                        Thread updateThread = new Thread(()=>Activate(extension));
                        updateThread.Start();
                        DateTime timeStarted = DateTime.UtcNow;

                        //Update splash screen's progress bar while thread is active or 10 seconds have past.
                        TimeSpan span = TimeSpan.FromMilliseconds(0);
                        while (updateThread.IsAlive && span.TotalMilliseconds < 10000)
                        {
                            UpdateSplashScreen("Looking for updates");
                            span = DateTime.UtcNow - timeStarted;
                        }

                        //Join the threads. If the thread is still active, wait a full second before giving up.
                        updateThread.Join(1000);
                        UpdateSplashScreen("Finished.");
                    }
                    else
                    {
                        Activate(extension);
                    }
                }

                foreach (IExtension extension in Extensions.Where(t => t.DeactivationAllowed))
                {
                    Activate(extension);
                }
            }
        }

        private static void Activate(IExtension extension)
        {
            if (!extension.IsActive)
            {
                if (!extension.TryActivate())
                    MessageBox.Show(String.Format(resources.GetString("ErrorWhileWhileActivating"), extension.AssemblyQualifiedName));
            }
        }

        /// <summary>
        /// Ensures the required imports are available for IExtension implementors. We guarantee DockManager, HeaderControl, and ProgressHandler
        /// are available when an IExtension loads, so that the developer of an IExtension doesn't need to check to see whether they are null.
        /// We make sure these are available before activating an IExtension.
        /// </summary>
        /// <returns></returns>
        public bool EnsureRequiredImportsAreAvailable()
        {
            DockManager = GetRequiredImport<IDockManager>();
            HeaderControl = GetRequiredImport<IHeaderControl>();
            ProgressHandler = GetRequiredImport<IStatusControl>();

            if (DockManager == null || HeaderControl == null || ProgressHandler == null)
                return false;

            return true;
        }

        private T GetRequiredImport<T>() where T : class
        {
            T import = CompositionContainer.GetExportedValueOrDefault<T>();

            if (import == default(T))
            {
                int importCount = CompositionContainer.GetExportedValues<T>().Count();
                string extensionTypeName = typeof(T).Name;
                if (importCount > 1)
                    MessageBox.Show(String.Format("You may only include one {0} Extension. {1} were found.", extensionTypeName, importCount));
                else
                {
                    MessageBox.Show(String.Format("A {0} extension must be included because a UI plugin was found. See http://wp.me/pvy5A-2v", extensionTypeName));
                }
            }
            return import;
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
                UpdateSplashScreen("Cataloging: " + PrefixWithEllipsis(dir, SplashDirectoryMessageLimit));
                if (!DirectoryCatalogExists(catalog, dir))
                    TryLoadingCatalog(catalog, new DirectoryCatalog(dir));
            }

            return catalog;
        }

        /// <summary>
        /// Gets the directories in Directories and those nested one level deep.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetDirectoriesNestedOneLevel()
        {
            // Visit each directory in Directories Property (usually set by application)
            if (DotSpatial.Mono.Mono.IsRunningOnMono())
            {
                Directories.Add("Mono Extensions");
            }
            else
            {
                Directories.Add("Windows Extensions");
            }
            foreach (string directory in Directories.Union(new[] { "Data Extensions", "Tools" }))
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directory);

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
        /// Refreshes the extensions - activating any newly discovered ones.
        /// </summary>
        public void RefreshExtensions()
        {
            RefreshExtensions(this.Catalog);
        }

        private void RefreshExtensions(AggregateCatalog catalog)
        {
            LocateExtensions(catalog, AbsolutePathToExtensions);
        }

        private void LocateExtensions(AggregateCatalog catalog, string absolutePathToExtensions)
        {
            var directories = GetPackageExtensionPaths(AbsolutePathToExtensions);
            foreach (var dir in directories)
            {
                Trace.WriteLine("Cataloging: " + dir);
                UpdateSplashScreen("Cataloging: " + PrefixWithEllipsis(dir, SplashDirectoryMessageLimit));

                // todo: consider using a file system watcher if it would provider better performance.
                if (!DirectoryCatalogExists(catalog, dir))
                    TryLoadingCatalog(catalog, new DirectoryCatalog(dir));
            }
        }

        /// <summary>
        /// Gets the paths of dlls for extensions that were downloaded as packages.
        /// </summary>
        /// <param name="absolutePathToExtensions">The absolute path to extensions.</param>
        /// <returns></returns>
        private static IEnumerable<string> GetPackageExtensionPaths(string absolutePathToExtensions)
        {
            if (!Directory.Exists(absolutePathToExtensions))
                yield break;

            var packagesFolder = Path.Combine(absolutePathToExtensions, PackageDirectory);
            if (!Directory.Exists(packagesFolder))
                yield break;

            var packageFolders = Directory.EnumerateDirectories(packagesFolder, "*", SearchOption.TopDirectoryOnly);
            foreach (var packageFolder in packageFolders)
            {
                var potentialFolder = Path.Combine(packageFolder, "lib");
                if (Directory.Exists(potentialFolder))
                {
                    yield return potentialFolder;

                    potentialFolder = Path.Combine(packageFolder, "lib", "net40");
                    if (Directory.Exists(potentialFolder))
                    {
                        yield return potentialFolder;
                    }

                    // see if the client profile was targeted.
                    potentialFolder = Path.Combine(packageFolder, "lib", "net40-Client");
                    if (Directory.Exists(potentialFolder))
                    {
                        yield return potentialFolder;
                    }
                }
            }
        }

        private static bool DirectoryCatalogExists(AggregateCatalog catalog, string dir)
        {
            foreach (var cat in catalog.Catalogs)
            {
                var directoryCatalog = cat as DirectoryCatalog;
                if (directoryCatalog != null)
                {
                    if (directoryCatalog.FullPath.Equals(dir, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _components = new Container();
        }

        #endregion
    }
}