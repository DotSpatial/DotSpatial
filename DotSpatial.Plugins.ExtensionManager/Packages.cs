// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Packages.cs" company="">
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using DotSpatial.Controls;
using NuGet;
using System.Net;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class Packages
    {
        #region Constants and Fields

        private const string PackageSourceUrl = "http://www.myget.org/F/dotspatial/";
        private PackageManager packageManager;
        private IPackageRepository repo;
        private string repositoryLocation;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the Packages class.
        /// </summary>
        public Packages()
        {
            repo = PackageRepositoryFactory.Default.CreateRepository(PackageSourceUrl);
            repositoryLocation = Path.Combine(AppManager.AbsolutePathToExtensions, AppManager.PackageDirectory);
            packageManager = new PackageManager(Repo, new DefaultPackagePathResolver(PackageSourceUrl), new PhysicalFileSystem(repositoryLocation));
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the PackageRepository.
        /// </summary>
        public IPackageRepository Repo
        {
            get
            {
                return repo;
            }
        }

        /// <summary>
        /// Gets the repository location.
        /// </summary>
        public string RepositoryLocation
        {
            get
            {
                return repositoryLocation;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Installs the specified package.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IPackage Install(string name)
        {
            IPackage package = Repo.FindPackage(name);

            // important: the following line will throw an exception when debugging
            // if using the official Nuget.Core dll.
            // Run without debugging to avoid the exception and install the package
            // more at http://nuget.codeplex.com/discussions/259099
            // We include a custom nuget.core without SecurityTransparent to avoid the error.
            if (package != null)
            {
                try
                {
                    packageManager.InstallPackage(package, false, false);
                }
                catch (WebException)
                {
                    // Timed out.
                    return null;
                }
            }

            return package;
        }

        public IPackage GetLocalPackage(string id)
        {
            return packageManager.LocalRepository.FindPackage(id);
        }

        /// <summary>
        /// Updates the specified package and dependencies.
        /// </summary>
        /// <param name="package">The package.</param>
        public void Update(IPackage package)
        {
            packageManager.UpdatePackage(package, true, false);
        }

        #endregion
    }
}