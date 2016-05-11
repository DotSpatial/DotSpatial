// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Packages.cs" company="">
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using NuGet;

namespace DotSpatial.Plugins.ExtensionManager.Updater
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
        public Packages(String ExtensionFolder)
        {
            repo = PackageRepositoryFactory.Default.CreateRepository(PackageSourceUrl);
            repositoryLocation = Path.Combine(ExtensionFolder);
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

        public IPackage GetLocalPackage(string id)
        {
            var pack = packageManager.LocalRepository.FindPackage(id);
            String[] files = Directory.GetDirectories(repositoryLocation, id + '*');
            if (files.Length > 0 && files[0] != Path.Combine(repositoryLocation, packageManager.PathResolver.GetPackageDirectory(pack)))
            {
                Directory.Move(files[0], Path.Combine(repositoryLocation, packageManager.PathResolver.GetPackageDirectory(pack)));
                pack = packageManager.LocalRepository.FindPackage(id);
            }
            return pack;
        }

        /// <summary>
        /// Updates the specified package and dependencies.
        /// </summary>
        /// <param name="package">The package.</param>
        public void Update(string packageId, SemanticVersion version)
        {
            var pack = GetLocalPackage(packageId);
            packageManager.UninstallPackage(pack, true, false);
            packageManager.InstallPackage(packageId, version, false, false);
        }

        public void SetNewSource(string source)
        {
            repo = PackageRepositoryFactory.Default.CreateRepository(source);
            packageManager = new PackageManager(Repo, new DefaultPackagePathResolver(source), new PhysicalFileSystem(repositoryLocation));
        }
    }

        #endregion
}