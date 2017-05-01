// ********************************************************************************************************
// Product Name: DotSpatial.Tools.ModelerForm
// Description:  A form which contains the modeler component
//
// ********************************************************************************************************
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Apr, 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

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