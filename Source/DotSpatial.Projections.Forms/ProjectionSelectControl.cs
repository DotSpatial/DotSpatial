﻿// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Projections.Forms
{
    /// <summary>
    /// ProjectionSelectControl can be used to choose a coordinate system from KnownCoordinateSystems.
    /// </summary>
    public partial class ProjectionSelectControl : UserControl
    {
        #region Fields

        private bool _disableEvents;

        private ProjectionInfo _selectedCoordinateSystem;
        private bool _userChanges;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectionSelectControl"/> class.
        /// </summary>
        public ProjectionSelectControl()
        {
            InitializeComponent();

            chbEsri.Checked = true;
            Load += (sender, args) =>
            {
                if (DesignMode) return;

                if (SelectedCoordinateSystem == null)
                {
                    radProjected.Checked = true;
                }
            };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the currently chosen coordinate system.
        /// </summary>
        [Browsable(false)]
        public ProjectionInfo SelectedCoordinateSystem
        {
            get
            {
                return _selectedCoordinateSystem;
            }

            set
            {
                if (_selectedCoordinateSystem == value) return;

                _selectedCoordinateSystem = value;
                if (_selectedCoordinateSystem == null)
                {
                    nudEpsgCode.Value = 0;
                    tbEsriProj4.Text = null;
                    return;
                }

                nudEpsgCode.Value = _selectedCoordinateSystem.AuthorityCode;
                tbEsriProj4.Text = chbEsri.Checked ? _selectedCoordinateSystem.ToEsriString() : _selectedCoordinateSystem.ToProj4String();
                if (!_userChanges)
                {
                    // try to found Category for SelectedCoordinateSystem
                    var prc = GetProjectionCategory(_selectedCoordinateSystem);
                    if (prc == null)
                    {
                        cmbMinorCategory.Text = cmbMajorCategory.Text = ProjectionStrings.NotFoundInPredefined;
                        return;
                    }

                    _disableEvents = true;
                    radProjected.Checked = !_selectedCoordinateSystem.IsLatLon;
                    radGeographic.Checked = _selectedCoordinateSystem.IsLatLon;
                    LoadMajorCategories(prc.CategoryName);
                    LoadMinorCategories(prc.ProjectionFieldName);
                    _disableEvents = false;
                }
            }
        }

        private ICoordinateSystemCategoryHolder CurrentCategoryHolder => radProjected.Checked ? (ICoordinateSystemCategoryHolder)KnownCoordinateSystems.Projected : KnownCoordinateSystems.Geographic;

        #endregion

        #region Methods

        private static ProjectionCategory GetProjectionCategory(ProjectionInfo projectionInfo)
        {
            var holder = projectionInfo.IsLatLon ? (ICoordinateSystemCategoryHolder)KnownCoordinateSystems.Geographic : KnownCoordinateSystems.Projected;
            var selectedAsStr = projectionInfo.ToString();
            var selectedEsri = projectionInfo.ToEsriString();
            foreach (var name in holder.Names)
            {
                var cat = holder.GetCategory(name);
                foreach (var projName in cat.Names)
                {
                    var proj = cat.GetProjection(projName);
                    if (proj.ToString() == selectedAsStr && proj.ToEsriString() == selectedEsri)
                    {
                        return new ProjectionCategory
                               {
                                   CategoryName = name,
                                   ProjectionFieldName = projName
                               };
                    }
                }
            }

            return null;
        }

        private void BtnFromEpsgCodeClick(object sender, EventArgs e)
        {
            ProjectionInfo proj;
            try
            {
                proj = ProjectionInfo.FromEpsgCode((int)nudEpsgCode.Value);
            }
            catch (Exception)
            {
                MessageBox.Show(this, ProjectionStrings.UnknownEpsgCode, ProjectionStrings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                proj = null;
            }

            if (proj != null)
            {
                SelectedCoordinateSystem = proj;
            }
        }

        private void BtnUseEsriClick(object sender, EventArgs e)
        {
            ProjectionInfo proj;
            try
            {
                proj = chbEsri.Checked ? ProjectionInfo.FromEsriString(tbEsriProj4.Text) : ProjectionInfo.FromProj4String(tbEsriProj4.Text);
            }
            catch (Exception)
            {
                MessageBox.Show(this, ProjectionStrings.UnknownEsriOrProj4String, ProjectionStrings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                proj = null;
            }

            if (proj != null)
            {
                SelectedCoordinateSystem = proj;
            }
        }

        private void ChbEsriCheckedChanged(object sender, EventArgs e)
        {
            if (SelectedCoordinateSystem == null) return;

            tbEsriProj4.Text = chbEsri.Checked ? SelectedCoordinateSystem.ToEsriString() : SelectedCoordinateSystem.ToProj4String();
        }

        private void CmbMajorCategorySelectedIndexChanged(object sender, EventArgs e)
        {
            if (_disableEvents) return;

            LoadMinorCategories();
        }

        private void CmbMinorCategorySelectedIndexChanged(object sender, EventArgs e)
        {
            if (_disableEvents) return;

            var c = CurrentCategoryHolder.GetCategory((string)cmbMajorCategory.SelectedItem);
            if (c == null) return;

            _userChanges = true;
            SelectedCoordinateSystem = c.GetProjection((string)cmbMinorCategory.SelectedItem);
            _userChanges = false;
        }

        private void LoadMajorCategories(string selectedName = null)
        {
            cmbMajorCategory.SuspendLayout();
            cmbMajorCategory.Items.Clear();
            foreach (var name in CurrentCategoryHolder.Names)
            {
                cmbMajorCategory.Items.Add(name);
            }

            if (selectedName != null)
            {
                cmbMajorCategory.SelectedItem = selectedName;
            }
            else
            {
                cmbMajorCategory.SelectedIndex = 0;
            }

            cmbMajorCategory.ResumeLayout();
        }

        private void LoadMinorCategories(string selectedName = null)
        {
            var c = CurrentCategoryHolder.GetCategory((string)cmbMajorCategory.SelectedItem);
            if (c == null) return;

            cmbMinorCategory.SuspendLayout();
            cmbMinorCategory.Items.Clear();
            foreach (var name in c.Names)
            {
                cmbMinorCategory.Items.Add(name);
            }

            if (selectedName != null)
            {
                cmbMinorCategory.SelectedItem = selectedName;
            }
            else
            {
                cmbMinorCategory.SelectedIndex = 0;
            }

            cmbMinorCategory.ResumeLayout();
        }

        private void RadProjectedCheckedChanged(object sender, EventArgs e)
        {
            if (_disableEvents) return;

            LoadMajorCategories();
        }

        #endregion

        #region Classes

        private class ProjectionCategory
        {
            #region Properties

            public string CategoryName { get; set; }

            public string ProjectionFieldName { get; set; }

            #endregion
        }

        #endregion
    }
}