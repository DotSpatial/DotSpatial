// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.IO;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A form used in Brian's toolkit code.
    /// </summary>
    public partial class ModelerForm : Form
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelerForm"/> class.
        /// </summary>
        public ModelerForm()
        {
            InitializeComponent();

            _modeler.ModelFilenameChanged += ModelerModelFilenameChanged;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the forms modeler.
        /// </summary>
        public Modeler Modeler => _modeler;

        #endregion

        #region Methods

        private void ModelerModelFilenameChanged(object sender, EventArgs e)
        {
            Text = string.Format(MessageStrings.ModelerForm_DotSpatialModelerFileName, Path.GetFileNameWithoutExtension(_modeler.ModelFilename));
        }

        #endregion
    }
}