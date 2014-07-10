using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotSpatial.Plugins.Updater
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

        static System.Reflection.Assembly Resolver(object sender, ResolveEventArgs args)
        {
            System.Reflection.Assembly a1 = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream s = a1.GetManifestResourceStream(string.Format("{0}.Resources.{1}.dll", 
                typeof(Updater).Namespace, new System.Reflection.AssemblyName(args.Name).Name));
            byte[] block = new byte[s.Length];
            s.Read(block, 0, block.Length);
            System.Reflection.Assembly a2 = System.Reflection.Assembly.Load(block);
            return a2;
        }
    }
}
