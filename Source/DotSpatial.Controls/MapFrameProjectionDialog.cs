// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Diagnostics;
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

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MapFrameProjectionDialog"/> class.
        /// </summary>
        /// <param name="mapFrame">Map frame for this dialog.</param>
        public MapFrameProjectionDialog(IMapFrame mapFrame)
        {
            InitializeComponent();
            lnkSpatialReference.Links[0].LinkData = "http://spatialreference.org/";
            tcMain.SelectTab(0);
            MapFrame = mapFrame;
            ChangesApplied = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the map frame for this dialog.
        /// </summary>
        public IMapFrame MapFrame
        {
            get
            {
                return _mapFrame;
            }

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
        /// Gets or sets projection.
        /// </summary>
        public ProjectionInfo Projection
        {
            get
            {
                return _projection;
            }

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
            get
            {
                return _changesApplied;
            }

            set
            {
                if (_changesApplied == value) return;
                _changesApplied = value;
                btnOk.Enabled = btnApply.Enabled = !_changesApplied;
            }
        }

        #endregion

        #region Methods

        private void ApplyChanges()
        {
            if (ChangesApplied) return;
            var ignoredLayers = new StringBuilder();
            _mapFrame.ReprojectMapFrame(_projection, layer => ignoredLayers.AppendLine(layer.LegendText));
            if (ignoredLayers.Length > 0)
            {
                MessageBox.Show(this, string.Format(MessageStrings.MapFrameProjectionDialog_LayersWereSkipped, ignoredLayers), MessageStrings.Warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            _mapFrame.ResetExtents();
            ChangesApplied = true;
        }

        private void BtnApplyClick(object sender, EventArgs e)
        {
            ApplyChanges();
        }

        private void BtnChangeToSelectedClick(object sender, EventArgs e)
        {
            using (var pf = new ProjectionSelectDialog())
            {
                pf.SelectedCoordinateSystem = Projection;
                pf.ChangesApplied += PfOnChangesApplied;
                pf.ShowDialog(this);
                pf.ChangesApplied -= PfOnChangesApplied;
            }
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            ApplyChanges();
        }

        private void LnkSpatialReferenceLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Display the appropriate link based on the value of the LinkData property of the Link object.
            var target = e.Link.LinkData as string;

            // If the value looks like a URL, navigate to it.
            if (target != null && (target.StartsWith("www.") || target.StartsWith("http:")))
            {
                Process.Start(target);
            }
        }

        private void PfOnChangesApplied(object sender, EventArgs eventArgs)
        {
            var pf = (ProjectionSelectDialog)sender;
            Projection = pf.SelectedCoordinateSystem;
            ChangesApplied = false;
        }

        #endregion
    }
}