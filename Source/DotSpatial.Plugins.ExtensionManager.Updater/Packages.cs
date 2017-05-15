using System.IO;
using NuGet;

namespace DotSpatial.Plugins.ExtensionManager.Updater
{
    /// <summary>
    /// Packages
    /// </summary>
    public class Packages
    {
        #region Fields

        private const string PackageSourceUrl = "http://www.myget.org/F/cuahsi/";
        private PackageManager _packageManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Packages"/> class.
        /// </summary>
        /// <param name="extensionFolder">Folder that contains the extensions.</param>
        public Packages(string extensionFolder)
        {
            Repo = PackageRepositoryFactory.Default.CreateRepository(PackageSourceUrl);
            RepositoryLocation = Path.Combine(extensionFolder);
            _packageManager = new PackageManager(Repo, new DefaultPackagePathResolver(PackageSourceUrl), new PhysicalFileSystem(RepositoryLocation));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the package manger.
        /// </summary>
        public IPackageManager Manager => _packageManager;

        /// <summary>
        /// Gets the PackageRepository.
        /// </summary>
        public IPackageRepository Repo { get; private set; }

        /// <summary>
        /// Gets the repository location.
        /// </summary>
        public string RepositoryLocation { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the package with the given id from the local repository of the package manager.
        /// </summary>
        /// <param name="id">Id of the package, that should be returned.</param>
        /// <returns>The package belonging to the given id.</returns>
        public IPackage GetLocalPackage(string id)
        {
            var pack = _packageManager.LocalRepository.FindPackage(id);
            string[] files = Directory.GetDirectories(RepositoryLocation, id + '*');
            if (files.Length > 0 && files[0] != Path.Combine(RepositoryLocation, _packageManager.PathResolver.GetPackageDirectory(pack)))
            {
                Directory.Move(files[0], Path.Combine(RepositoryLocation, _packageManager.PathResolver.GetPackageDirectory(pack)));
                pack = _packageManager.LocalRepository.FindPackage(id);
            }

            return pack;
        }

        /// <summary>
        /// Changes the source for the repository and package manager.
        /// </summary>
        /// <param name="source">The new source.</param>
        public void SetNewSource(string source)
        {
            Repo = PackageRepositoryFactory.Default.CreateRepository(source);
            _packageManager = new PackageManager(Repo, new DefaultPackagePathResolver(source), new PhysicalFileSystem(RepositoryLocation));
        }

        /// <summary>
        /// Updates the specified package and dependencies.
        /// </summary>
        /// <param name="packageId">The package id.</param>
        /// <param name="version">The version.</param>
        public void Update(string packageId, SemanticVersion version)
        {
            var pack = GetLocalPackage(packageId);
            _packageManager.UninstallPackage(pack, true, false);
            _packageManager.InstallPackage(packageId, version, false, false);
        }

        #endregion
    }
}