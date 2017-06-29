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
using System.Globalization;
using System.Text;
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
        #region Fields

        private bool _changesApplied;
        private IMapFrame _mapFrame;
        private ProjectionInfo _projection;

        #endregion

        /// <summary>
        /// use the mapFrame with this dialog
        /// </summary>
        /// <param name="mapFrame"></param>
        public MapFrameProjectionDialog(IMapFrame mapFrame)
        {
            InitializeComponent();
            lnkSpatialReference.Links[0].LinkData = "http://spatialreference.org/";
            tcMain.SelectTab(0);
            MapFrame = mapFrame;
            ChangesApplied = true;
        }

        /// <summary>
        /// Gets or sets the map frame for this dialog
        /// </summary>
        public IMapFrame MapFrame
        {
            get { return _mapFrame; }
            set
            {
                if (_mapFrame == value) return;
                _mapFrame = value;
                var projection = new ProjectionInfo();
                if (_mapFrame != null)
                {
                    projection.CopyProperties(_mapFrame.Projection);
                }
                Projection = projection;
            }
        }

        /// <summary>
        /// Gets or sets projection
        /// </summary>
        public ProjectionInfo Projection
        {
            get { return _projection; }
            set
            {
                if (_projection == value) return;
                _projection = value;

                txtEsriString.Text = _projection.ToEsriString();
                txtProj4String.Text = _projection.ToProj4String();
                txtName.Text = _projection.ToString();
                txtProjectionType.Text = _projection.IsLatLon ? "Geographic" : "Projected ";

                txtAuthority.Text = !string.IsNullOrEmpty(_projection.Authority) ? _projection.Authority : "Not specified";
                txtAuthorityCode.Text = _projection.AuthorityCode > 0 ? _projection.AuthorityCode.ToString(CultureInfo.InvariantCulture) : "Not specified";
            }
        }

        private bool ChangesApplied
        {
            get { return _changesApplied; }
            set
            {
                if (_changesApplied == value) return;
                _changesApplied = value;
                btnOk.Enabled = btnApply.Enabled = !_changesApplied;
            }
        }

        private void ApplyChanges()
        {
            if (ChangesApplied) return;
            var ignoredLayers = new StringBuilder();
            _mapFrame.ReprojectMapFrame(_projection, layer => ignoredLayers.AppendLine(layer.LegendText));
            if (ignoredLayers.Length > 0)
            {
                MessageBox.Show(this, "These layers was skipped:" + Environment.NewLine + ignoredLayers,
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            _mapFrame.ResetExtents();
            ChangesApplied = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ApplyChanges();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ApplyChanges();
        }

        private void lnkSpatialReference_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Display the appropriate link based on the value of the  
            // LinkData property of the Link object. 
            var target = e.Link.LinkData as string;

            // If the value looks like a URL, navigate to it. 
            if (null != target && (target.StartsWith("www.") ||
                                   target.StartsWith("http:")))
            {
                System.Diagnostics.Process.Start(target);
            }
        }

        private void btnChangeToSelected_Click(object sender, EventArgs e)
        {
            using (var pf = new ProjectionSelectDialog())
            {
                pf.SelectedCoordinateSystem = Projection;
                pf.ChangesApplied += PfOnChangesApplied;
                pf.ShowDialog(this);
                pf.ChangesApplied -= PfOnChangesApplied;
            }
        }

        private void PfOnChangesApplied(object sender, EventArgs eventArgs)
        {
            var pf = (ProjectionSelectDialog) sender;
            Projection = pf.SelectedCoordinateSystem;
            ChangesApplied = false;
        }
    }
}