// ********************************************************************************************************
// Product Name: DotSpatial.Plugins.ShapeEditor.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/10/2009 5:10:05 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Plugins.ShapeEditor
{
    /// <summary>
    /// A Dialog that displays options for feature type when creating a new feature.
    /// </summary>
    public partial class FeatureTypeDialog : Form
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureTypeDialog"/> class.
        /// </summary>
        public FeatureTypeDialog()
        {
            InitializeComponent();
            _cmbFeatureType.SelectedIndex = 0;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Coordinate type for this dialog.
        /// </summary>
        public CoordinateType CoordinateType
        {
            get
            {
                if (_chkZ.Checked)
                {
                    return CoordinateType.Z;
                }

                if (_chkM.Checked)
                {
                    return CoordinateType.M;
                }

                return CoordinateType.Regular;
            }
        }

        /// <summary>
        /// Gets the feature type chosen by this dialog.
        /// </summary>
        public FeatureType FeatureType
        {
            get
            {
                switch (_cmbFeatureType.SelectedIndex)
                {
                    case 0:
                        return FeatureType.Point;
                    case 1:
                        return FeatureType.Line;
                    case 2:
                        return FeatureType.Polygon;
                    case 3:
                        return FeatureType.MultiPoint;
                }

                return FeatureType.Unspecified;
            }
        }

        /// <summary>
        /// Gets the filename which should be used to save the layer to file.
        /// </summary>
        public string Filename => _tbFilename.Text.Trim();

        #endregion

        #region Methods

        /// <summary>
        /// Opens the SafeFileDialog and copys the selected filename to _tbFilename.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void BtnSelectFilenameClick(object sender, EventArgs e)
        {
            if (_sfdFilename.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(_sfdFilename.FileName))
            {
                _tbFilename.Text = _sfdFilename.FileName;
            }
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        private void OkButtonClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_tbFilename.Text) && !_tbFilename.Text.ToLower().EndsWith(".shp"))
                _tbFilename.Text += @".shp";
            Close();
        }

        #endregion
    }
}