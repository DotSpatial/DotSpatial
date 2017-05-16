// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using DotSpatial.Extensions;
using NuGet;

namespace DotSpatial.Plugins.ExtensionManager
{
    /// <summary>
    /// Class used for getting packages.
    /// </summary>
    internal class GetPackage
    {
        #region Fields

        private readonly Packages _packages;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPackage"/> class.
        /// </summary>
        /// <param name="packageHelper">The packages.</param>
        public GetPackage(Packages packageHelper)
        {
            _packages = packageHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the packages from the extensions.
        /// </summary>
        /// <param name="extension">Extensions to get packages from.</param>
        /// <returns>The packages.</returns>
        public IPackage GetPackageFromExtension(IExtension extension)
        {
            string id = extension.AssemblyQualifiedName.Substring(0, extension.AssemblyQualifiedName.IndexOf(',')); // Grab the part prior to the first comma
            if (id.Contains("."))
                id = id.Substring(0, id.LastIndexOf('.')); // Grab the part prior to the last period, only if id contains period (Changed by JLeiss)
            var pack = _packages.GetLocalPackage(id);
            return pack;
        }

        /// <summary>
        /// Gets the packages from the extensions.
        /// </summary>
        /// <param name="extensions">Extensions to get packages from.</param>
        /// <returns>The packages.</returns>
        public IEnumerable<IPackage> GetPackagesFromExtensions(IEnumerable<IExtension> extensions)
        {
            return extensions.Select(GetPackageFromExtension).Where(package => package != null);
        }

        #endregion
    }
}