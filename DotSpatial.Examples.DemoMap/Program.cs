// ****************************************************************************
// Product Name: TestViewer.exe
// Description:  A very basic demonstration of the controls.
// ****************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific
// language governing rights and
// limitations under the License.
//
// The Initial Developer of this Original Code is Ted Dunsford. Created during
// refactoring 2010.
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