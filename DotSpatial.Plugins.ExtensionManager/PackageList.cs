using System;
using System.Collections.Generic;
using System.Linq;
using NuGet;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class PackageList
    {
        public IPackage[] packages { get; set; }

        public int TotalPackageCount { get; set; }
    }
}