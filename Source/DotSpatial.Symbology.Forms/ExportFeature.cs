// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The core assembly for the DotSpatial 6.0 distribution.
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/5/2009 12:49:10 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// ExportFeature
    /// </summary>
    public partial class ExportFeature : Form
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportFeature"/> class.
        /// </summary>
        public ExportFeature()
        {
            InitializeComponent();
            cmbFeatureSpecification.Items.Add(SymbologyFormsMessageStrings.ExportFeature_FeaturesAll);
            cmbFeatureSpecification.Items.Add(SymbologyFormsMessageStrings.ExportFeature_FeaturesSelected);
            cmbFeatureSpecification.Items.Add(SymbologyFormsMessageStrings.ExportFeature_FeaturesExtent);
            cmbFeatureSpecification.SelectedIndex = 0;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the zero based integer index.
        /// </summary>
        public int FeaturesIndex => cmbFeatureSpecification.SelectedIndex;

        /// <summary>
        /// Gets or sets the string fileName.  Setting this will not actually use this value,
        /// but will make up a new value based on the entered value.
        /// </summary>
        public string Filename
        {
            get
            {
                return txtOutput.Text;
            }

            set
            {
                string baseFile = Path.GetDirectoryName(value) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(value);
                int i = 1;
                string outFile = value;
                string ext = Path.GetExtension(value);

                while (File.Exists(outFile))
                {
                    outFile = baseFile + i + ext;
                    i++;
                }

                txtOutput.Text = outFile;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Occurs when closing, and throws up a message box preventing "ok" with an invalid path.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel) return;
            List<string> result = DataManager.DefaultDataManager.GetSupportedExtensions(DataManager.DefaultDataManager.VectorWriteFilter);
            string ext = Path.GetExtension(txtOutput.Text);

            if (!result.Contains(ext))
            {
                MessageBox.Show(SymbologyFormsMessageStrings.ExportFeature_ExtensionNotSupported, SymbologyFormsMessageStrings.ExportFeature_InvalidFileFormat, MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }

            base.OnClosing(e);
        }

        private void BtnBrowseClick(object sender, EventArgs e)
        {
            using (SaveFileDialog ofd = new SaveFileDialog { Filter = DataManager.DefaultDataManager.VectorWriteFilter })
            {
                if (ofd.ShowDialog() != DialogResult.OK) return;
                txtOutput.Text = ofd.FileName;
            }
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}