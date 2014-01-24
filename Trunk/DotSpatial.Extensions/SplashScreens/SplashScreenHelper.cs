// -----------------------------------------------------------------------
// <copyright file="SplashScreenHelper.cs" company="DotSpatial Team">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;

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
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Application Extensions");
            if (!Directory.Exists(path))
                return null;

            string file = Directory.EnumerateFiles(path, "*SplashScreen*.dll").FirstOrDefault();
            if (file == null) return null;

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
    }
}