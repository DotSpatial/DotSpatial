// ****************************************************************************
// Product Name: DemoMap.exe
// Description:  A very basic demonstration of the controls.
// ****************************************************************************

using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace DemoMap
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [DebuggerNonUserCode]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}