// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel.Composition;
using System.Windows.Forms;

namespace DemoMap
{
    /// <summary>
    /// This is the main window of the DemoMap program.
    /// </summary>
    public partial class MainForm : Form
    {
        [Export("Shell", typeof(ContainerControl))]
        private static ContainerControl shell;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            if (DesignMode) return;
            shell = this;
            appManager.LoadExtensions();
        }
    }
}