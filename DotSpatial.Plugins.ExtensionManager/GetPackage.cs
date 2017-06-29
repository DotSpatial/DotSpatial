using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuGet;
using DotSpatial.Extensions;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class GetPackage
    {
        public GetPackage(Packages packageHelper)
        {
            this.packages = packageHelper;
        }
        private readonly Packages packages;


        public IPackage GetPackageFromExtension(IExtension extension)
        {
            string id = extension.AssemblyQualifiedName.Substring(0, extension.AssemblyQualifiedName.IndexOf(',')); // Grab the part prior to the first comma
            id = id.Substring(0, id.LastIndexOf('.')); // Grab the part prior to the last period
            var pack = packages.GetLocalPackage(id);
            return pack;
        }

        public IEnumerable<IPackage> GetPackagesFromExtensions(IEnumerable<IExtension> extensions)
        {
            foreach (IExtension extension in extensions)
            {
                var package = GetPackageFromExtension(extension);
                if (package != null)
                {
                    yield return package;
                }
            }
        }
    }
}
