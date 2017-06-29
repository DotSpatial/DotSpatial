// -----------------------------------------------------------------------
// <copyright file="SplashScreenHelper.cs" company="DotSpatial Team">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace DotSpatial.Extensions.SplashScreens
{
    public sealed class SplashScreenHelper
    {
        /// <summary>
        /// Searches "Application Extensions" for and activates "*SplashScreen*.dll"
        /// </summary>
        /// <returns></returns>
        public static ISplashScreenManager GetSplashScreenManager()
        {
            // This is a specific directory where a splash screen may be located.
            Assembly asm = Assembly.GetEntryAssembly();
            var directories = new List<string> { AppDomain.CurrentDomain.BaseDirectory + "Application Extensions", 
                                                 AppDomain.CurrentDomain.BaseDirectory + "Plugins",
                                                 Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), asm.ManifestModule.Name, "Extensions"),
                                                 AppDomain.CurrentDomain.BaseDirectory + "Extensions"};

            foreach (string directory in directories)
            {
                if (!Directory.Exists(directory))
                    continue;

                string file = Directory.EnumerateFiles(directory, "*SplashScreen*.dll", SearchOption.AllDirectories).FirstOrDefault();
                if (file == null)
                    continue;

                using (AggregateCatalog splashCatalog = new AggregateCatalog())
                {
                    splashCatalog.Catalogs.Add(new AssemblyCatalog(file));

                    using (CompositionContainer splashBatch = new CompositionContainer(splashCatalog))
                    {
                        var splash = splashBatch.GetExportedValueOrDefault<ISplashScreenManager>();

                        if (splash != null)
                            splash.Activate();

                        return splash;
                    }
                }

            }

            return null;
        }
    }
}