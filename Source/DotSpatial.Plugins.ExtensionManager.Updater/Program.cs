// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Reflection;
using System.Windows.Forms;

namespace DotSpatial.Plugins.ExtensionManager.Updater
{
    /// <summary>
    /// Start point for the Updater.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">The arguments</param>
        [STAThread]
        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += Resolver;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Updater(args));
        }

        /// <summary>
        /// Handles the resolve event.
        /// </summary>
        /// <param name="sender">The sender that raised the event.</param>
        /// <param name="args">The event args.</param>
        /// <returns>The loaded assembly.</returns>
        private static Assembly Resolver(object sender, ResolveEventArgs args)
        {
            var a1 = Assembly.GetExecutingAssembly();
            var s = a1.GetManifestResourceStream(string.Format("{0}.Resources.{1}.dll", typeof(Updater).Namespace, new AssemblyName(args.Name).Name));
            var block = new byte[s.Length];
            s.Read(block, 0, block.Length);
            s.Dispose();
            var a2 = Assembly.Load(block);
            return a2;
        }
    }
}
