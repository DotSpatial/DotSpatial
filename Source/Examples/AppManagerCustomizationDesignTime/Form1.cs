// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System.ComponentModel.Composition;
using System.Windows.Forms;

namespace DotSpatial.Examples.AppManagerCustomizationDesignTime
{
    /// <summary>
    /// A form to show the map.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Is needed to load extensions.
        /// </summary>
        [Export("Shell", typeof(ContainerControl))]
#pragma warning disable IDE0052 // Ungelesene private Member entfernen
        private static ContainerControl Shell;
#pragma warning restore IDE0052 // Ungelesene private Member entfernen

        /// <summary>
        /// Initializes a new Form1.
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            if (DesignMode)
            {
                return;
            }

            // The following 2 lines are required to load extensions
            Shell = this;
            appManager.LoadExtensions();

            appManager.UpdateProgress("Control is Ready"); // Show some status message
        }
    }
}
