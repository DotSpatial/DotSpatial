using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DotSpatial.Positioning;

namespace Diagnostics
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}