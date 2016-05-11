using System;
using System.Reflection;
using System.Windows.Forms;

namespace DotSpatial.Plugins.ExtensionManager.Updater
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(Resolver);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Updater(args));
        }

        static Assembly Resolver(object sender, ResolveEventArgs args)
        {
            var a1 = Assembly.GetExecutingAssembly();
            var s = a1.GetManifestResourceStream(string.Format("{0}.Resources.{1}.dll", 
                typeof(Updater).Namespace, new AssemblyName(args.Name).Name));
            var block = new byte[s.Length];
            s.Read(block, 0, block.Length);
            s.Dispose();
            var a2 = Assembly.Load(block);
            return a2;
        }
    }
}
