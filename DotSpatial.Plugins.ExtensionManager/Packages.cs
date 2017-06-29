// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Packages.cs" company="">
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Net;
using DotSpatial.Controls;
using NuGet;
using System;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class Packages
    {
        #region Constants and Fields

        private const string PackageSourceUrl = "http://www.myget.org/F/cuahsi/";
        private const string coreRepoUrl = "https://nuget.org/api/v2/";
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

        /// <summary>
        /// Gets the package manger.
        /// </summary>
        public IPackageManager Manager
        {
            get
            {
                return packageManager;
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
                    packageManager.InstallPackage(package, true, false);
                    return package;
                }
            }
            catch (WebException ex)
            {
                // Timed out.
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }

            return null;
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
            packageManager.InstallPackage(package, true, false);
        }

        public void SetNewSource(string source)
        {
            repo = PackageRepositoryFactory.Default.CreateRepository(source);
            packageManager = new PackageManager(Repo, new DefaultPackagePathResolver(source), new PhysicalFileSystem(repositoryLocation));
        }
    }

        #endregion
}