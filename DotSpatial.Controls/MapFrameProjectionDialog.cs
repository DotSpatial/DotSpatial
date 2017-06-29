// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Jiri Kadlec. Created 2/26/2011 11:03:10 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// ------------------|------------|---------------------------------------------------------------
//        Name       |    Date    |                       Comments
// ------------------|------------|---------------------------------------------------------------
// ********************************************************************************************************

using System;
using System.Windows.Forms;
using DotSpatial.Projections;
using DotSpatial.Projections.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A dialog for changing the projection of the map frame.
    /// </summary>
    public partial class MapFrameProjectionDialog : Form
    {
        private bool _changesApplied;

        /// <summary>
        /// This is necessary because we want to change the projection of the MapFrame
        /// </summary>
        private IMapFrame _mapFrame;

        private ProjectionInfo _projection;

        /// <summary>
        /// use the mapFrame with this dialog
        /// </summary>
        /// <param name="mapFrame"></param>
        public MapFrameProjectionDialog(IMapFrame mapFrame)
        {
            InitializeComponent();
            _mapFrame = mapFrame;
            _projection = new ProjectionInfo();
            _projection.CopyProperties(_mapFrame.Projection);
            UpdateProjectionStrings();
        }

        /// <summary>
        /// Gets or sets the map frame for this dialog
        /// </summary>
        public IMapFrame MapFrame
        {
            get { return _mapFrame; }
            set
            {
                _mapFrame = value;
                UpdateProjectionStrings();
            }
        }

        private void UpdateProjectionStrings()
        {
            txtEsriString.Text = _projection.ToEsriString();
            txtProj4String.Text = _projection.ToProj4String();
        }

        private void ApplyChanges()
        {
            MapFrameProjectionHelper.ReprojectMapFrame(_mapFrame, _projection.ToEsriString());
            _mapFrame.ResetExtents();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!_changesApplied) ApplyChanges();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ApplyChanges();
            _changesApplied = true;
        }

        private void btnChangeProjection_Click(object sender, EventArgs e)
        {
            ProjectionSelectDialog projDialog = new ProjectionSelectDialog();
            projDialog.SelectedCoordinateSystem = _projection;
            if (projDialog.ShowDialog() == DialogResult.OK)
            {
                _projection = projDialog.SelectedCoordinateSystem;
                UpdateProjectionStrings();
                _changesApplied = false;
            }
        }
    }
}