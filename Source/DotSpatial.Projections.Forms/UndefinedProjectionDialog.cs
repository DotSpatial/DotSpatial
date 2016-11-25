// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/25/2010 2:32:14 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Windows.Forms;
using DotSpatial.Projections.Forms.Properties;

namespace DotSpatial.Projections.Forms
{
    /// <summary>
    /// A Dialog for choosing how to define a projection.
    /// </summary>
    public partial class UndefinedProjectionDialog : Form
    {
        private ProjectionInfo _mapProjection;
        private string _originalString;
        private ProjectionInfo _selectedProjection;
        private string _layerName;

        /// <summary>
        /// Used to remember the windows header text.
        /// </summary>
        private string _originalFormText;

        /// <summary>
        /// Indicates whether _originalFormText should be updated when the windows header text gets changed.
        /// </summary>
        private bool _ignoreTextChanged;

        /// <summary>
        /// Initializes a new instance of the Undefined Projection Dialog
        /// </summary>
        public UndefinedProjectionDialog()
        {
            InitializeComponent();
            SelectedCoordinateSystem = KnownCoordinateSystems.Geographic.World.WGS1984;
            MapProjection = KnownCoordinateSystems.Geographic.World.WGS1984;
        }

        /// <summary>
        /// Gets or sets a value that indicates that the newly selected Projection should be used.  If this
        /// is false, then this layer should be drawn unmodified.
        /// </summary>
        public bool UseProjection
        {
            get { return radDoNothing.Checked; }
        }

        /// <summary>
        /// Gets or sets the Projection chosen to be the final defined projection.
        /// </summary>
        public ProjectionInfo SelectedCoordinateSystem
        {
            get
            {
                return _selectedProjection;
            }
            set
            {
                _selectedProjection = value;
                lblSelectedTransform.Text = _selectedProjection.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the current MapProjection.
        /// </summary>
        public ProjectionInfo MapProjection
        {
            get
            {
                return _mapProjection;
            }
            set
            {
                _mapProjection = value;
                if (value != null) lblMapProjection.Text = value.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the original projection string that could not be identified.
        /// </summary>
        public string OriginalString
        {
            get
            {
                return _originalString;
            }
            set
            {
                _originalString = value;
                lblOriginal.Text = _originalString == null ? Resources.Nothing : value;
            }
        }

        /// <summary>
        /// Gets or sets whether the option chosen here should be used for the rest of the session.
        /// </summary>
        public bool AlwaysUse
        {
            get { return chkAlways.Checked; }
            set { chkAlways.Checked = value; }
        }

        /// <summary>
        /// Gets or sets the layername of the layer that could not be identified.
        /// </summary>
        public string LayerName
        {
            get
            {
                return _layerName;
            }
            set
            {
                if (value == _layerName) return;
                _layerName = value;

                _ignoreTextChanged = true;
                Text = string.IsNullOrWhiteSpace(value) ? _originalFormText : _originalFormText + " - " + value;
                _ignoreTextChanged = false;
            }
        }

        /// <summary>
        /// Changes the _originalFormText to Text, if _ignoreTextChanged is not set.
        /// </summary>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (!_ignoreTextChanged)
            {
                _originalFormText = Text;
            }
        }

        /// <summary>
        /// Gets the action selected from the dialog.
        /// </summary>
        public UndefinedProjectionAction Action
        {
            get
            {
                var result = UndefinedProjectionAction.Nothing;
                if (radLatLong.Checked)
                {
                    result = UndefinedProjectionAction.WGS84;
                }
                if (radMapFrame.Checked)
                {
                    result = UndefinedProjectionAction.Map;
                }
                if (radSelectedTransform.Checked)
                {
                    result = UndefinedProjectionAction.Chosen;
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the result of the dialog.
        /// </summary>
        public ProjectionInfo Result
        {
            get
            {
                ProjectionInfo result = null;
                if (radLatLong.Checked)
                {
                    result = KnownCoordinateSystems.Geographic.World.WGS1984;
                }
                if (radMapFrame.Checked)
                {
                    result = _mapProjection;
                }
                if (radSelectedTransform.Checked)
                {
                    result = _selectedProjection;
                }
                return result;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            using (var dialog = new ProjectionSelectDialog {SelectedCoordinateSystem = SelectedCoordinateSystem})
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                SelectedCoordinateSystem = dialog.SelectedCoordinateSystem;
            }
        }
    }
}