using System.Collections.Generic;
using System.Linq;
using DotSpatial.Extensions;
using NuGet;

namespace DotSpatial.Plugins.ExtensionManager
{
    internal class GetPackage
    {
        #region Fields

        private readonly Packages packages;

        #endregion

        #region  Constructors

        public GetPackage(Packages packageHelper)
        {
            packages = packageHelper;
        }

        #endregion

        #region Methods

        public IPackage GetPackageFromExtension(IExtension extension)
        {
            string id = extension.AssemblyQualifiedName.Substring(0, extension.AssemblyQualifiedName.IndexOf(',')); // Grab the part prior to the first comma
            if (id.Contains("."))
                id = id.Substring(0, id.LastIndexOf('.')); // Grab the part prior to the last period, only if id contains period (Changed by JLeiss)
            var pack = packages.GetLocalPackage(id);
            return pack;
        }

        public IEnumerable<IPackage> GetPackagesFromExtensions(IEnumerable<IExtension> extensions)
        {
            return extensions.Select(GetPackageFromExtension).Where(package => package != null);
        }

        #endregion
    }
}