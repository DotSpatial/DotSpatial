using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using DotSpatial.Controls;
using NuGet;
using PackageManager = NuGet.PackageManager;

namespace DotSpatial.Plugins.ExtensionManager
{
    /// <summary>
    /// Packages
    /// </summary>
    internal class Packages
    {
        #region Fields
        private const string PackageSourceUrl = "http://www.myget.org/F/cuahsi/";
        private PackageManager _packageManager;
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Packages"/> class.
        /// </summary>
        public Packages()
        {
            Repo = PackageRepositoryFactory.Default.CreateRepository(PackageSourceUrl);
            RepositoryLocation = Path.Combine(AppManager.AbsolutePathToExtensions, AppManager.PackageDirectory);
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
        /// Finds the package belonging to the given id.
        /// </summary>
        /// <param name="id">Id of the package that should be returned.</param>
        /// <returns>The package belonging to the id.</returns>
        public IPackage GetLocalPackage(string id)
        {
            return _packageManager.LocalRepository.FindPackage(id);
        }

        /// <summary>
        /// Installs the specified package.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Null on error, otherwise the package belonging to the name.</returns>
        public IPackage Install(string name)
        {
            try
            {
                IPackage package = Repo.FindPackage(name);

                // important: the following line will throw an exception when debugging
                // if using the official Nuget.Core dll.
                // Run without debugging to avoid the exception and install the package
                // more at http://nuget.codeplex.com/discussions/259099
                // We include a custom nuget.core without SecurityTransparent to avoid the error.
                if (package != null)
                {
                    _packageManager.InstallPackage(package, true, false);
                    return package;
                }
            }
            catch (WebException ex)
            {
                // Timed out.
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }

            return null;
        }

        /// <summary>
        /// Binds the repository and the packet manager to a new source.
        /// </summary>
        /// <param name="source">Source that gets set.</param>
        public void SetNewSource(string source)
        {
            Repo = PackageRepositoryFactory.Default.CreateRepository(source);
            _packageManager = new PackageManager(Repo, new DefaultPackagePathResolver(source), new PhysicalFileSystem(RepositoryLocation));
        }

        /// <summary>
        /// Updates the specified package and dependencies.
        /// </summary>
        /// <param name="package">The package.</param>
        public void Update(IPackage package)
        {
            _packageManager.InstallPackage(package, true, false);
        }

        #endregion
    }

}