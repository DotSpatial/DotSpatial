// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// A menu strip designed to work along with the modeler.
    /// </summary>
    [ToolboxItem(false)]
    public partial class ModelerMenuStrip : MenuStrip
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelerMenuStrip"/> class.
        /// </summary>
        public ModelerMenuStrip()
        {
            InitializeComponent();
        }
    }
}