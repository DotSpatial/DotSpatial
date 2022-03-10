// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DotSpatial.Extensions.SplashScreens
{
    /// <summary>
    /// Contains helper methods for splash screens.
    /// </summary>
    public sealed class SplashScreenHelper
    {
        /// <summary>
        /// Searches "Application Extensions" for and activates "*SplashScreen*.dll".
        /// </summary>
        /// <returns>Null if no ISplashScreenManager was found otherwise the first ISplashScreenManager that was found.</returns>
        public static ISplashScreenManager GetSplashScreenManager()
        {
            // This is a specific directory where a splash screen may be located.
            Assembly asm = Assembly.GetEntryAssembly();
            var directories = new List<string>
            {
                AppDomain.CurrentDomain.BaseDirectory + "Application Extensions",
                AppDomain.CurrentDomain.BaseDirectory + "Plugins",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), asm.ManifestModule.Name, "Extensions"),
                AppDomain.CurrentDomain.BaseDirectory + "Extensions"
            };

            foreach (string directory in directories)
            {
                if (!Directory.Exists(directory))
                    continue;

                string file = Directory.EnumerateFiles(directory, "*SplashScreen*.dll", SearchOption.AllDirectories).FirstOrDefault();
                if (file == null)
                    continue;

                using AggregateCatalog splashCatalog = new();
                splashCatalog.Catalogs.Add(new AssemblyCatalog(file));

                using CompositionContainer splashBatch = new(splashCatalog);
                var splash = splashBatch.GetExportedValueOrDefault<ISplashScreenManager>();
                splash?.Activate();
                return splash;
            }

            return null;
        }
    }
}